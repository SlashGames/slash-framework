// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Ext.Windows
{
    using Slash.Unity.Common.Scenes;
    using Slash.Unity.DataBind.Core.Data;

    public class WindowContext : Context
    {
        #region Properties

        /// <summary>
        ///   Window which has this context.
        /// </summary>
        public WindowManager.Window Window { get; set; }

        #endregion

        #region Methods

        protected void Close(object returnValue = null)
        {
            WindowManagerBehaviour.Instance.CloseWindow(this.Window, returnValue);
        }

        #endregion
    }
}