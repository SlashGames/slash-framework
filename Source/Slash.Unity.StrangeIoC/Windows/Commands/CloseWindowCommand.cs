// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseWindowCommand.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Commands
{
    using strange.extensions.command.impl;

    using Slash.Unity.Common.Scenes;

    using UnityEngine;

    public class CloseWindowCommand : EventCommand
    {
        #region Properties

        [Inject]
        public WindowManager WindowManager { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void Execute()
        {
            var window = this.evt.data as WindowManager.Window;
            if (window == null)
            {
                Debug.LogWarning("No window provided to close");
                return;
            }

            this.WindowManager.CloseWindow(window, null);
        }

        #endregion
    }
}