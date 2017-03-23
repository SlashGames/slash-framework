// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowMediator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Slash.Unity.StrangeIoC.Windows.Signals;

namespace Slash.Unity.StrangeIoC.Windows.Mediators
{
    using strange.extensions.mediation.impl;

    using Slash.Unity.Common.Scenes;

    public class WindowMediator<TView> : Mediator
        where TView : View
    {
        #region Properties

        [Inject]
        public TView View { get; set; }

        [Inject]
        public CloseWindowSignal CloseWindowSignal { get; set; }

        public WindowManager.Window Window
        {
            get
            {
                var windowRoot = this.View.GetComponent<WindowRoot>();
                return windowRoot != null ? windowRoot.Window : null;
            }
        }

        #endregion

        #region Methods

        protected void CloseWindow()
        {
            CloseWindowSignal.Dispatch(this.Window);
        }

        #endregion
    }
}