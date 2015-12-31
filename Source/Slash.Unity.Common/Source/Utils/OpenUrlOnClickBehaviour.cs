// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenUrlOnClickBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Behaviour to open an url when receiving the OnClick event.
    /// </summary>
    public class OpenUrlOnClickBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Url to open.
        /// </summary>
        public string Url;

        #endregion

        #region Methods

        private void OnClick()
        {
            Debug.Log("Opening URL: " + this.Url);
            Application.OpenURL(this.Url);
        }

        #endregion
    }
}