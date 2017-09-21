// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowRoot.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Scenes
{
    using System;

    using UnityEngine;

    public class WindowRoot : MonoBehaviour
    {
        #region Fields

        public string WindowId;

        #endregion

        #region Events

        public event Action<WindowRoot> WindowDestroyed;

        #endregion

        #region Properties

        public WindowManager.Window Window { get; set; }

        #endregion

        #region Methods

        protected void OnDestroy()
        {
            Debug.Log(string.Format("Window root {0} is being destroyed, closing window.", this.WindowId));
            this.OnWindowDestroyed();
        }

        private void OnWindowDestroyed()
        {
            var handler = this.WindowDestroyed;
            if (handler != null)
            {
                handler(this);
            }
        }

        #endregion
    }
}