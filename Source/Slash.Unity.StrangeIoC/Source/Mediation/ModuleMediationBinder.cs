namespace Slash.Unity.StrangeIoC.Mediation
{
    using System;
    using strange.extensions.mediation.api;
    using strange.extensions.mediation.impl;
    using strange.framework.api;
    using Slash.Unity.StrangeIoC.Modules;
    using UnityEngine;

    public class ModuleMediationBinder : MediationBinder
    {
        private readonly ModuleContext module;

        /// <inheritdoc />
        public ModuleMediationBinder(ModuleContext module)
        {
            this.module = module;
        }

        public override IBinding GetBinding(object key, object name)
        {
            var binding = base.GetBinding(key, name);
            if (binding == null)
            {
                // Check sub modules.
                foreach (var subModule in this.module.SubModules)
                {
                    binding = subModule.MediationBinder.GetBinding(key, name);
                    if (binding != null)
                    {
                        break;
                    }
                }
            }

            return binding;
        }

        /// <inheritdoc />
        protected override void mapView(IView view, IMediationBinding binding)
        {
            if (binding == null)
            {
                return;
            }

            var viewType = view.GetType();
            var mediatorTypes = (object[]) binding.value;
            var viewGameObject = ((MonoBehaviour) view).gameObject;
            foreach (Type mediatorType in mediatorTypes)
            {
                if (mediatorType == viewType)
                {
                    throw new MediationException(
                        viewType + "mapped to itself. The result would be a stack overflow.",
                        MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
                }

                // Only add mediator if not yet existent on view game object.
                var mediator = viewGameObject.GetComponent(mediatorType);
                if (mediator == null)
                {
                    mediator = viewGameObject.AddComponent(mediatorType);
                    if (mediator == null)
                    {
                        throw new MediationException(
                            "The view: " + viewType + " is mapped to mediator: " + mediatorType +
                            ". AddComponent resulted in null, which probably means " +
                            mediatorType.Name +
                            " is not a MonoBehaviour.", MediationExceptionType.NULL_MEDIATOR);
                    }
                }

                var mediatorInterface = mediator as IMediator;
                var registerMediator = false;
                if (mediatorInterface != null && mediatorInterface.contextView == null)
                {
                    mediatorInterface.PreRegister();
                    registerMediator = true;
                }

                // Inject mediator.
                this.injectionBinder.Bind(viewType).ToValue(view).ToInject(false);
                this.injectionBinder.injector.Inject(mediator);
                this.injectionBinder.Unbind(viewType);

                if (mediatorInterface != null && registerMediator)
                {
                    mediatorInterface.OnRegister();
                }
            }
        }
    }
}