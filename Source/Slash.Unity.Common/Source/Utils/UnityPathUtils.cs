// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityPathUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using System.IO;

    /// <summary>
    ///   Utility methods for handling URIs in Unity.
    /// </summary>
    public static class UnityPathUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Combines two strings into a path. Considers that Unity asset/resource paths are not
        ///   accepting backslashes ('\'), but only slashes ('/').
        ///   See http://www.createdbyx.com/createdbyx/post/2013/03/18/Unity-101-Tip-57-%E2%80%93-ResourceLoadLoadAll-Gotchas.aspx
        ///   for more details.
        /// </summary>
        /// <param name="path1">The first path to combine.</param>
        /// <param name="path2">The second path to combine.</param>
        /// <returns>The combined paths. If one of the specified paths is a zero-length string, this method returns the other path.</returns>
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2).Replace('\\', '/');
        }

        #endregion
    }
}