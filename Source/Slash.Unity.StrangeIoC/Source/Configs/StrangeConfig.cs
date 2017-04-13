// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrangeConfig.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Configs
{
    using System.Collections;

    using strange.extensions.command.api;
    using strange.extensions.context.impl;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;
    using strange.framework.api;

    using Slash.Unity.StrangeIoC.Modules;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    public abstract class StrangeConfig : MonoBehaviour
    {
        /// <summary>
        ///   Scene to load for this module.
        /// </summary>
        protected string SceneName { get; set; }

        /// <summary>
        ///   Maps bindings to the injection binder.
        /// </summary>
        /// <param name="injectionBinder">Binder to map to.</param>
        public virtual void MapBindings(IInjectionBinder injectionBinder)
        {
        }

        /// <summary>
        ///   Maps bindings to the command binder.
        /// </summary>
        /// <param name="commandBinder">Binder to map to.</param>
        public virtual void MapBindings(ICommandBinder commandBinder)
        {
        }

        /// <summary>
        ///   Maps bindings to the mediation binder.
        /// </summary>
        /// <param name="mediationBinder">Binder to map to.</param>
        public virtual void MapBindings(IMediationBinder mediationBinder)
        {
        }

        /// <summary>
        ///   Sets up the view for this module.
        /// </summary>
        public virtual void SetupView(ModuleContext context)
        {
            if (!string.IsNullOrEmpty(this.SceneName))
            {
                this.StartCoroutine(LoadAndSetupScene(this.SceneName, context));
            }
            else
            {
                var contextView = this.gameObject.GetComponent<ContextView>()
                                  ?? this.gameObject.AddComponent<ContextView>();

                contextView.context = context;
                context.SetContextView(contextView);
            }
        }

        private static IEnumerator LoadAndSetupScene(string sceneName, ModuleContext context)
        {
            var scene = SceneManager.GetSceneByName(sceneName);

            // Only load if not already loaded (e.g. in editor)
            if (!scene.isLoaded)
            {
                yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                scene = SceneManager.GetSceneByName(sceneName);
            }

            var rootGameObjects = scene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                var contextView = rootGameObject.GetComponent<ContextView>()
                                  ?? rootGameObject.AddComponent<ContextView>();
                contextView.context = context;
                context.SetContextView(contextView);
            }
        }
    }
}