// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenWindowCommand.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Commands
{
    using strange.extensions.command.impl;

    using Slash.Unity.Common.Scenes;

    public class OpenWindowCommand : EventCommand
    {
        #region Fields

        private readonly string windowId;

        #endregion

        #region Constructors and Destructors

        protected OpenWindowCommand(string windowId)
        {
            this.windowId = windowId;
        }

        #endregion

        #region Properties

        [Inject]
        public WindowManager WindowManager { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Execute()
        {
            this.OpenWindow(this.CreateWindowContext());
        }

        #endregion

        #region Methods

        protected virtual object CreateWindowContext()
        {
            return null;
        }

        protected WindowManager.Window OpenWindow(object context)
        {
            return this.WindowManager.OpenWindow(this.windowId, context, null, null);
        }

        #endregion
    }
}