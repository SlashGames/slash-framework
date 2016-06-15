// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowContextManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Ext.Windows
{
    using Slash.Unity.Common.Scenes;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;

    public class WindowContextManager
    {
        #region Fields

        private WindowManager windowManager;

        #endregion

        #region Public Methods and Operators

        public void Deinit()
        {
            if (this.windowManager != null)
            {
                this.windowManager.WindowOpened -= SetupWindowContext;
                this.windowManager = null;
            }
        }

        public void Init(WindowManager windowManager)
        {
            this.windowManager = windowManager;

            if (this.windowManager != null)
            {
                this.windowManager.WindowOpened += SetupWindowContext;
            }
        }

        #endregion

        #region Methods

        private static void SetupWindowContext(WindowManager.Window window)
        {
            // Check if window has data context.
            var context = window.Context as Context;
            if (context == null)
            {
                return;
            }

            // Set context in context holder.
            var contextHolder = window.Root != null ? window.Root.GetComponent<ContextHolder>() : null;
            if (contextHolder != null)
            {
                contextHolder.Context = context;
            }

            // Set window reference if window context.
            var windowContext = context as WindowContext;
            if (windowContext != null)
            {
                windowContext.Window = window;
            }
        }

        #endregion
    }
}