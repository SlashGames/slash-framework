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

    using UnityEngine;

    public class WindowContextManager : MonoBehaviour
    {
        #region Fields

        public WindowManager WindowManager;

        #endregion

        #region Methods

        protected void Awake()
        {
            if (this.WindowManager == null)
            {
                this.WindowManager = FindObjectOfType<WindowManager>();
            }
        }

        protected void OnDisable()
        {
            if (this.WindowManager != null)
            {
                this.WindowManager.WindowOpened -= this.OnWindowOpened;
            }
        }

        protected void OnEnable()
        {
            if (this.WindowManager != null)
            {
                this.WindowManager.WindowOpened += this.OnWindowOpened;
            }
        }

        private void OnWindowOpened(WindowManager.Window window)
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