// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionNumberBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    [RequireComponent(typeof(GUIText))]
    public class VersionNumberBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Path to file which contains version string.
        /// </summary>
        public string VersionFilePath = "Misc/Version";

        #endregion

        #region Methods

        private void Start()
        {
            var versionAsset = (TextAsset)Resources.Load(this.VersionFilePath);
            this.guiText.text = string.Format("Version {0}", versionAsset.text);
        }

        #endregion
    }
}