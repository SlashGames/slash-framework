namespace Slash.Unity.StrangeIoC.Mediation
{
    using System;
    using strange.extensions.mediation.api;
    using strange.extensions.mediation.impl;
    using UnityEngine;

    public class ModuleMediationBinder : MediationBinder
    {
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
                var registerMediator = false;
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

                    registerMediator = true;
                }

                var mediatorInterface = mediator as IMediator;
                if (registerMediator && mediatorInterface != null)
                {
                    mediatorInterface.PreRegister();
                }

                // Inject mediator.
                this.injectionBinder.Bind(viewType).ToValue(view).ToInject(false);
                this.injectionBinder.injector.Inject(mediator);
                this.injectionBinder.Unbind(viewType);

                if (registerMediator && mediatorInterface != null)
                {
                    mediatorInterface.OnRegister();
                }
            }
        }
    }
}