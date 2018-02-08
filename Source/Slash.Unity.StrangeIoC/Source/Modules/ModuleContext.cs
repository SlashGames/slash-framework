namespace Slash.Unity.StrangeIoC.Modules
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using strange.extensions.command.api;
    using strange.extensions.command.impl;
    using strange.extensions.context.api;
    using strange.extensions.context.impl;
    using strange.extensions.dispatcher.api;
    using strange.extensions.dispatcher.eventdispatcher.api;
    using strange.extensions.dispatcher.eventdispatcher.impl;
    using strange.extensions.implicitBind.api;
    using strange.extensions.implicitBind.impl;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;
    using strange.extensions.sequencer.api;
    using strange.extensions.sequencer.impl;
    using strange.framework.api;
    using strange.framework.impl;
    using Slash.Unity.StrangeIoC.Configs;
    using Slash.Unity.StrangeIoC.Coroutines;
    using Slash.Unity.StrangeIoC.Mediation;
    using Slash.Unity.StrangeIoC.Modules.Commands;
    using Slash.Unity.StrangeIoC.Modules.Signals;
    using Slash.Unity.StrangeIoC.Modules.Utils;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Object = UnityEngine.Object;

    public class ModuleContext : CrossContext
    {
        /// <summary>
        ///     Registered bridges.
        /// </summary>
        private readonly List<Type> bridgeTypes;

        /// <summary>
        ///     Registered modules.
        /// </summary>
        private readonly List<Module> modules;

        /// <summary>
        ///     A list of Views Awake before the Context is fully set up.
        /// </summary>
        protected ISemiBinding ViewCache = new SemiBinding();

        /// <summary>
        ///     Indicates if context is started.
        /// </summary>
        private bool isStarted;

        /// <summary>
        ///     Root of module view.
        /// </summary>
        private ModuleView moduleView;

        /// <summary>
        ///     Indicates that the scene of the module is currently loading.
        /// </summary>
        private bool sceneIsLoading;

        /// <inheritdoc />
        public ModuleContext()
        {
            this.modules = new List<Module>();
            this.bridgeTypes = new List<Type>();
        }

        /// A Binder that maps Events to Commands
        public ICommandBinder CommandBinder { get; set; }

        /// A Binder that serves as the Event bus for the Context
        public IEventDispatcher Dispatcher { get; set; }

        //Interprets implicit bindings
        public IImplicitBinder ImplicitBinder { get; set; }

        /// <summary>
        ///     Installer for module.
        /// </summary>
        public IModuleInstaller Installer { get; set; }

        /// <summary>
        ///     Indicates if module is already launched.
        /// </summary>
        public bool IsLaunched { get; private set; }

        /// <summary>
        ///     Indicates if verbose logging is enabled (for debugging).
        /// </summary>
        public bool IsLoggingEnabled { get; set; }

        /// <summary>
        ///     Indicates if module is ready to be launched.
        /// </summary>
        public bool IsReadyToLaunch
        {
            get
            {
                // Check if module view is set.
                if (this.moduleView == null)
                {
                    return false;
                }

                // Check if scene is loaded.
                if (this.sceneIsLoading)
                {
                    return false;
                }

                // Check if sub modules are ready to launch.
                foreach (var module in this.modules)
                {
                    if (!module.Context.IsReadyToLaunch)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// A Binder that maps Views to Mediators
        public IMediationBinder MediationBinder { get; set; }

        public string Name
        {
            get { return this.Installer != null ? this.Installer.GetType().Name : "No Config"; }
        }

        /// A Binder that maps Events to Sequences
        public ISequencer Sequencer { get; set; }

        /// <summary>
        ///     Sub modules of this module.
        /// </summary>
        public IEnumerable<ModuleContext> SubModules
        {
            get { return this.modules.Select(module => module.Context); }
        }

        /// <summary>
        ///     Adds a brige to the module.
        /// </summary>
        /// <param name="bridgeType">Type of bridge to add.</param>
        public void AddBridge(Type bridgeType)
        {
            if (this.bridgeTypes.Contains(bridgeType))
            {
                throw new ArgumentException("Can't add bridge of type '{0}', it's already added.", "bridgeType");
            }

            this.bridgeTypes.Add(bridgeType);

            // Bind and fire up bridge if already launched.
            if (this.IsLaunched)
            {
                this.injectionBinder.Bind(bridgeType).ToSingleton();
                this.injectionBinder.GetInstance(bridgeType);
            }
        }

        public void AddSubModule(Type moduleType)
        {
            // Check if (old) module config or new module type.
            var strangeConfigType = typeof(StrangeConfig);
            if (strangeConfigType.IsAssignableFrom(moduleType))
            {
                // Create module config on module view game object.
                this.AddSubModule((StrangeConfig) this.moduleView.gameObject.AddComponent(moduleType));
            }
            else
            {
                this.AddSubModule((StrangeModule) Activator.CreateInstance(moduleType));
            }
        }

        public void AddSubModule(IModuleInstaller subModuleInstaller)
        {
            var moduleType = subModuleInstaller.GetType();

            // Check if module already exist.
            var moduleBinding = this.injectionBinder.GetBinding<ModuleContext>(moduleType);
            ModuleContext moduleContext;
            if (moduleBinding == null)
            {
                // Create context for module.
                moduleContext = new ModuleContext {Installer = subModuleInstaller};
                moduleContext.Init(this.moduleView);
                this.injectionBinder.Bind<ModuleContext>().ToValue(moduleContext).ToName(moduleType).CrossContext();

                // If module is already started, start sub module immediately. Otherwise it will started when the module is started.
                if (this.isStarted)
                {
                    moduleContext.Start();
                }

                if (this.IsLaunched)
                {
                    if (moduleContext.IsReadyToLaunch)
                    {
                        moduleContext.Launch();
                    }
                    else
                    {
                        this.moduleView.StartCoroutine(LaunchContextWhenReady(moduleContext));
                    }
                }

                // Add sub module and context.
                var module = new Module {Type = moduleType, Context = moduleContext};
                this.AddContext(module.Context);
                this.modules.Add(module);
            }
            else
            {
                // Setup already created module context.
                moduleContext = (ModuleContext) moduleBinding.value;
            }

            // Setup module with settings from new installer.
            var settings = subModuleInstaller.SetupSettings;
            if (settings != null)
            {
                var setupModuleSignal = moduleContext.injectionBinder.GetInstance<SetupModuleSignal>();
                setupModuleSignal.Dispatch(settings);
            }
        }

        /// <inheritdoc />
        public override void AddView(object viewObject)
        {
            var view = viewObject as IView;
            if (view == null)
            {
                Debug.LogError("View object " + viewObject + " doesn't implement IView interface");
                return;
            }

            if (this.IsLoggingEnabled)
            {
                Debug.LogFormat(
                    view as Object,
                    "Adding view '{0}' to context '{1}'",
                    view.GetType().Name,
                    this.Installer != null ? this.Installer.GetType().Name : "App");
            }

            if (this.MediationBinder != null)
            {
                this.MediationBinder.Trigger(MediationEvent.AWAKE, view);
            }
            else
            {
                this.CacheView(view as MonoBehaviour);
            }
        }

        /// <inheritdoc />
        public override object GetComponent<T>()
        {
            return this.GetComponent<T>(null);
        }

        /// <inheritdoc />
        public override object GetComponent<T>(object name)
        {
            var binding = this.injectionBinder.GetBinding<T>(name);
            if (binding != null)
            {
                return this.injectionBinder.GetInstance<T>(name);
            }

            return null;
        }

        public void Init(ModuleView parentModuleView)
        {
            //If firstContext was unloaded, the contextView will be null. Assign the new context as firstContext.
            if (firstContext == null || firstContext.GetContextView() == null)
            {
                firstContext = this;
            }
            else
            {
                firstContext.AddContext(this);
            }

            this.addCoreComponents();
            this.autoStartup = false;

            // Use parent module view.
            this.SetModuleView(parentModuleView);

            if (this.Installer != null)
            {
                // Add bridges.
                foreach (var bridgeType in this.Installer.Bridges)
                {
                    if (bridgeType != null)
                    {
                        this.AddBridge(bridgeType);
                    }
                }

                // Add submodules from installer.
                this.InitSubModules();

                // Setup module view.
                if (!string.IsNullOrEmpty(this.Installer.SceneName))
                {
                    // Module view is in a scene.
                    this.sceneIsLoading = true;
                    parentModuleView.StartCoroutine(LoadViewFromScene(this.Installer.SceneName,
                        rootGameObjects =>
                        {
                            foreach (var rootGameObject in rootGameObjects)
                            {
                                // Setup context views in scene to have a reference to the context there.
                                var sceneContextView = rootGameObject.GetComponent<ContextView>() ?? rootGameObject.AddComponent<SceneContextView>();
                                sceneContextView.context = this;

                                // Add sub modules from scene.
                                foreach (var subModuleConfig in ModuleUtils.FindModuleConfigs(rootGameObject))
                                {
                                    this.AddSubModule(subModuleConfig);
                                }
                            }

                            this.sceneIsLoading = false;
                        }
                    ));
                }
            }
        }

        /// <inheritdoc />
        public override void Launch()
        {
            // Make sure context is not started twice.
            if (this.IsLaunched)
            {
                Debug.LogError("Context already launched", this.moduleView);
                return;
            }

            base.Launch();

            // Launch sub-modules.
            foreach (var module in this.modules)
            {
                if (!module.Context.IsLaunched)
                {
                    module.Context.Launch();
                }
            }

            // Fire up bridges.
            foreach (var bridgeType in this.bridgeTypes)
            {
                this.injectionBinder.GetInstance(bridgeType);
            }

            //It's possible for views to fire their Awake before bindings. This catches any early risers and attaches their Mediators.
            this.MediateViewCache();

            this.MediationBinder.Trigger(MediationEvent.AWAKE, this.moduleView);

            var launchedSignal = this.injectionBinder.GetInstance<ModuleLaunchedSignal>();

            this.IsLaunched = true;
            launchedSignal.Dispatch();

            this.Dispatcher.Dispatch(ContextEvent.START);
        }

        /// <inheritdoc />
        public override void OnRemove()
        {
            base.OnRemove();

            // Remove sub modules.
            foreach (var module in this.modules)
            {
                this.RemoveContext(module.Context);
            }

            this.modules.Clear();

            if (this.Installer != null)
            {
                // Unmap bindings.
                this.Installer.UnmapCrossContextBindings(this.injectionBinder.CrossContextBinder);

                // Remove view.
                if (!string.IsNullOrEmpty(this.Installer.SceneName))
                {
                    UnloadViewScene(this.Installer.SceneName);
                }
            }

            this.CommandBinder.OnRemove();
        }

        /// <summary>
        ///     Removes a bridge from the module.
        /// </summary>
        /// <param name="bridgeType">Type of bridge to remove.</param>
        public void RemoveBridge(Type bridgeType)
        {
            if (!this.bridgeTypes.Remove(bridgeType))
            {
                throw new ArgumentException("Can't remove bridge of type '{0}', doesn't exist.", "bridgeType");
            }

            if (this.IsLaunched)
            {
                var bridge = (StrangeBridge) this.injectionBinder.GetInstance(bridgeType);
                if (bridge != null)
                {
                    // Note: This should be supported by StrangeIoC, but isn't.
                    bridge.OnDeconstruct();
                }

                this.injectionBinder.Unbind(bridgeType);
            }
        }

        public void RemoveSubModule(Type moduleConfigType)
        {
            var module = this.modules.FirstOrDefault(existingModule => existingModule.Type == moduleConfigType);
            if (module == null)
            {
                Debug.LogErrorFormat(
                    "No module of type '{0}' exists in context '{1}'",
                    moduleConfigType.Name,
                    this.Name);
                return;
            }

            // Remove sub context.
            this.RemoveContext(module.Context);

            this.modules.Remove(module);
        }

        /// <inheritdoc />
        public override void RemoveView(object view)
        {
            this.MediationBinder.Trigger(MediationEvent.DESTROYED, view as IView);
        }

        /// <summary>
        ///     Sets the root of the module view.
        /// </summary>
        /// <param name="newModuleView">Root of the module view.</param>
        public void SetModuleView(ModuleView newModuleView)
        {
            if (this.moduleView != null)
            {
                // Remove injection.
                this.injectionBinder.Unbind<GameObject>(ContextKeys.CONTEXT_VIEW);
                this.injectionBinder.Unbind<ICoroutineRunner>();
            }

            this.moduleView = newModuleView;
            this.SetContextView(this.moduleView);

            if (this.moduleView != null)
            {
                // Set injection.
                this.injectionBinder.Bind<GameObject>()
                    .ToValue(this.moduleView.gameObject)
                    .ToName(ContextKeys.CONTEXT_VIEW);
                this.injectionBinder.Bind<ICoroutineRunner>()
                    .ToValue(new MonoBehaviourCoroutineRunner(this.moduleView));
            }
        }

        /// <inheritdoc />
        public override IContext Start()
        {
            // Make sure context is not started twice.
            if (this.isStarted)
            {
                Debug.LogError("Context already started", this.moduleView);
                return this;
            }

            // Start sub modules.
            foreach (var module in this.modules)
            {
                if (!module.Context.isStarted)
                {
                    module.Context.Start();
                }
            }

            base.Start();

            this.isStarted = true;

            return this;
        }

        /// <inheritdoc />
        protected override void addCoreComponents()
        {
            base.addCoreComponents();

            this.injectionBinder.Bind<IInstanceProvider>().Bind<IInjectionBinder>().ToValue(this.injectionBinder);
            this.injectionBinder.Bind<IContext>().ToValue(this).ToName(ContextKeys.CONTEXT);
            this.injectionBinder.Bind<ModuleContext>().ToValue(this).ToName(ContextKeys.CONTEXT);
            this.injectionBinder.Bind<ModuleContext>().ToValue(this);
            this.injectionBinder.Bind<ICommandBinder>().To<EventCommandBinder>().ToSingleton();

            //This binding is for local dispatchers
            this.injectionBinder.Bind<IEventDispatcher>().To<EventDispatcher>();

            //This binding is for the common system bus
            this.injectionBinder.Bind<IEventDispatcher>()
                .To<EventDispatcher>()
                .ToSingleton()
                .ToName(ContextKeys.CONTEXT_DISPATCHER);
            this.injectionBinder.Bind<IMediationBinder>().To<ModuleMediationBinder>().ToSingleton();
            this.injectionBinder.Bind<ISequencer>().To<EventSequencer>().ToSingleton();
            this.injectionBinder.Bind<IImplicitBinder>().To<ImplicitBinder>().ToSingleton();

            // Enable signals.
            this.injectionBinder.Unbind<ICommandBinder>();
            this.injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
            this.injectionBinder.Bind<SetupModuleSignal>().ToSingleton();
            this.injectionBinder.Bind<ModuleSetupSignal>().ToSingleton();
        }

        /// <summary>
        ///     Caches early-riser Views.
        ///     If a View is on stage at startup, it's possible for that
        ///     View to be Awake before this Context has finished initing.
        ///     `cacheView()` maintains a list of such 'early-risers'
        ///     until the Context is ready to mediate them.
        /// </summary>
        /// <param name="view"></param>
        protected virtual void CacheView(MonoBehaviour view)
        {
            if (this.ViewCache.constraint.Equals(BindingConstraintType.ONE))
            {
                this.ViewCache.constraint = BindingConstraintType.MANY;
            }

            this.ViewCache.Add(view);
        }

        /// <inheritdoc />
        protected override void instantiateCoreComponents()
        {
            base.instantiateCoreComponents();

            this.CommandBinder = this.injectionBinder.GetInstance<ICommandBinder>();

            this.Dispatcher = this.injectionBinder.GetInstance<IEventDispatcher>(ContextKeys.CONTEXT_DISPATCHER);
            this.MediationBinder = this.injectionBinder.GetInstance<IMediationBinder>();
            this.Sequencer = this.injectionBinder.GetInstance<ISequencer>();
            this.ImplicitBinder = this.injectionBinder.GetInstance<IImplicitBinder>();

            ((ITriggerProvider) this.Dispatcher).AddTriggerable(this.CommandBinder as ITriggerable);
            ((ITriggerProvider) this.Dispatcher).AddTriggerable(this.Sequencer as ITriggerable);
        }

        /// <inheritdoc />
        protected override void mapBindings()
        {
            base.mapBindings();

            // Module lifecycle.
            this.injectionBinder.Bind<ModuleLaunchedSignal>().ToSingleton();

            // Sub-Module actions.
            this.CommandBinder.Bind<LoadConfigSignal>().To<LoadConfigCommand>();
            this.CommandBinder.Bind<LoadModuleSignal>().To<LoadModuleCommand>();
            this.CommandBinder.Bind<LoadModuleByTypeSignal>().To<LoadModuleByTypeCommand>();
            this.CommandBinder.Bind<UnloadModuleSignal>().To<UnloadModuleCommand>();

            if (this.Installer != null)
            {
                // Map bindings for module.
                this.Installer.MapBindings(this.injectionBinder);
                this.Installer.MapBindings(this.CommandBinder);
                this.Installer.MapBindings(this.MediationBinder);

                // TODO: Allow definition of sub modules in module installer.
                //// Map bindings for features.
                //if (this.Config.Features != null)
                //{
                //    foreach (var configFeature in this.Config.Features)
                //    {
                //        configFeature.MapBindings(this.injectionBinder);
                //        configFeature.MapBindings(this.CommandBinder);
                //        configFeature.MapBindings(this.MediationBinder);

                //        // Load scene for feature and bind this module.
                //        if (!string.IsNullOrEmpty(configFeature.SceneName))
                //        {
                //            this.Config.StartCoroutine(LoadViewFromScene(configFeature.SceneName,
                //                sceneModuleView => sceneModuleView.context = this));
                //        }
                //    }
                //}
            }

            // Inject bridges.
            foreach (var bridgeType in this.bridgeTypes)
            {
                this.injectionBinder.Bind(bridgeType).ToSingleton();
            }
        }

        protected virtual void MediateViewCache()
        {
            if (this.MediationBinder == null)
            {
                throw new ContextException(
                    "MVCSContext cannot mediate views without a mediationBinder",
                    ContextExceptionType.NO_MEDIATION_BINDER);
            }

            var values = this.ViewCache.value as object[];
            if (values == null)
            {
                return;
            }

            var aa = values.Length;
            for (var a = 0; a < aa; a++)
            {
                var viewObject = values[a];
                var view = viewObject as IView;
                if (view == null)
                {
                    Debug.LogError("View object " + viewObject + " doesn't implement IView interface");
                    continue;
                }

                this.MediationBinder.Trigger(MediationEvent.AWAKE, view);
            }

            this.ViewCache = new SemiBinding();
        }

        private void InitSubModules()
        {
            // Add sub modules.
            foreach (var subModule in this.Installer.SubModules)
            {
                if (subModule != null)
                {
                    this.AddSubModule(subModule);
                }
            }

            foreach (var subModuleType in this.Installer.SubModuleTypes)
            {
                if (subModuleType != null)
                {
                    this.AddSubModule(subModuleType);
                }
            }
        }

        private static IEnumerator LaunchContextWhenReady(ModuleContext domainContext)
        {
            while (!domainContext.IsReadyToLaunch)
            {
                yield return null;
            }

            domainContext.Launch();
        }

        private static IEnumerator LoadViewFromScene(string sceneName, Action<IEnumerable<GameObject>> onSceneLoaded)
        {
            // Check if scene is loaded (e.g. in editor).
            Scene? scene = null;
            for (var index = 0; index < SceneManager.sceneCount; index++)
            {
                var loadedScene = SceneManager.GetSceneAt(index);
                if (loadedScene.name == sceneName)
                {
                    scene = loadedScene;
                    break;
                }
            }

            // Only load if not already loaded.
            if (!scene.HasValue)
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                scene = SceneManager.GetSceneByName(sceneName);
            }
            else if (!scene.Value.isLoaded)
            {
                yield return new WaitUntil(() => scene.Value.isLoaded);
            }

            var rootGameObjects = scene.Value.GetRootGameObjects();

            var handler = onSceneLoaded;
            if (handler != null)
            {
                onSceneLoaded(rootGameObjects);
            }
        }

        private static void UnloadViewScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        private class Module
        {
            /// <summary>
            ///     Context of this module.
            /// </summary>
            public ModuleContext Context { get; set; }

            /// <summary>
            ///     Module type.
            /// </summary>
            public Type Type { get; set; }
        }
    }
}