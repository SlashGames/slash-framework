namespace Slash.Unity.StrangeIoC.Views
{
    using strange.extensions.context.impl;
    using strange.extensions.mediation.api;
    using strange.extensions.mediation.impl;
    using UnityEngine;

    public class StrangeView : View
    {
        /// <summary>
        ///     If set, the view won't be attached to the first context, but instead waits for
        ///     being attached later on.
        /// </summary>
        [Tooltip(
            "If set, the view won't be attached to the first context, but instead waits for being attached later on")]
        public bool AllowLateContextAttaching;

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            this.requiresContext = !this.AllowLateContextAttaching;
        }

        /// <summary>
        ///     Recurses through Transform.parent to find the GameObject to which ContextView is attached
        ///     Has a loop limit of 100 levels.
        ///     By default, raises an Exception if no Context is found.
        /// </summary>
        /// <param name="view">View to add/remove to/from context.</param>
        /// <param name="toAdd">Indicates if the view should be added or removed.</param>
        /// <param name="finalTry">Indicates if this is the final try and the first context should be used if no other is found.</param>
        protected override void bubbleToContext(MonoBehaviour view, bool toAdd, bool finalTry)
        {
            const int loopMax = 100;

            var trans = view.transform;
            var loopLimiter = 0;
            while (trans != null && loopLimiter < loopMax)
            {
                loopLimiter++;
                var contextView = trans.GetComponent<ContextView>();
                if (contextView != null)
                {
                    if (contextView.context != null)
                    {
                        var contextViewContext = contextView.context;
                        if (toAdd)
                        {
                            contextViewContext.AddView(view);
                            this.registeredWithContext = true;
                        }
                        else
                        {
                            contextViewContext.RemoveView(view);
                        }

                        return;
                    }
                }

                trans = trans.parent;
            }

            if (this.requiresContext && finalTry)
            {
                //last ditch. If there's a Context anywhere, we'll use it!
                if (Context.firstContext != null)
                {
                    Context.firstContext.AddView(view);
                    this.registeredWithContext = true;
                    return;
                }

                var msg = loopLimiter == loopMax
                    ? "A view couldn't find a context. Loop limit reached."
                    : "A view was added with no context. Views must be added into the hierarchy of their ContextView lest all hell break loose.";
                msg += "\nView: " + view;
                throw new MediationException(msg, MediationExceptionType.NO_CONTEXT);
            }
        }
    }
}