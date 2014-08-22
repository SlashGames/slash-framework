// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckTextureAssets.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.MenuItems.Util
{
    using System;
    using System.IO;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    public static class CheckTextureAssets
    {
        #region Constants

        public const int MaxTextureSize = 2048;

        #endregion

        #region Public Methods and Operators

        [MenuItem("Slash Games/Util/Check Texture Assets")]
        public static void CheckTextures()
        {
            var assetsPaths =
                Directory.GetFiles(Application.dataPath, "*", SearchOption.AllDirectories)
                         .Where(path => !path.EndsWith(".meta"));
            var assetCount = assetsPaths.Count();
            var processedAssets = 0;

            foreach (var assetPath in assetsPaths)
            {
                // Update progress bar.
                EditorUtility.DisplayProgressBar(
                    "Checking texture assets", assetPath, (float)processedAssets / assetCount);

                // Check texture size.
                var relativeAssetPath = assetPath.Substring(assetPath.IndexOf("Assets", StringComparison.Ordinal));
                var texture = (Texture2D)AssetDatabase.LoadAssetAtPath(relativeAssetPath, typeof(Texture2D));

                if (texture != null)
                {
                    if (texture.width > MaxTextureSize)
                    {
                        Debug.LogWarning(
                            string.Format(
                                "Texture {0} exceeds maximum width of {1}. It won't be visible on some mobile devices.",
                                texture,
                                MaxTextureSize));
                    }

                    if (texture.height > MaxTextureSize)
                    {
                        Debug.LogWarning(
                            string.Format(
                                "Texture {0} exceeds maximum height of {1}. It won't be visible on some mobile devices.",
                                texture,
                                MaxTextureSize));
                    }
                }

                processedAssets++;
            }

            EditorUtility.ClearProgressBar();
        }

        #endregion
    }
}