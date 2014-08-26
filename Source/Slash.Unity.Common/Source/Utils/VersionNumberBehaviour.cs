// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionNumberBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Provides the version number specified by the text file at the
    ///   given path.
    /// </summary>
    public class VersionNumberBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Path to file which contains version string.
        /// </summary>
        public string VersionFilePath = "Misc/Version";

        #endregion

        #region Public Properties

        /// <summary>
        ///   Current version number.
        /// </summary>
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