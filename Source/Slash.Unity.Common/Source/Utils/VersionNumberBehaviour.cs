// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionNumberBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    public class VersionNumberBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Path to file which contains version string.
        /// </summary>
        public string VersionFilePath = "Misc/Version";

        #endregion

        #region Public Properties

        public string Version { get; private set; }

        #endregion

        #region Methods

        private void Awake()
        {
            var versionAsset = (TextAsset)Resources.Load(this.VersionFilePath);
            this.Version = versionAsset.text;

            Debug.Log(string.Format("Game Version {0}", this.Version));
        }

        #endregion
    }
}