using System.IO;
using UnityEditor;
using UnityEngine;

namespace Slash.Unity.Editor.Common.ScriptableObjects
{
    public static class ScriptableObjectUtils
    {
        /// <summary>
        ///     This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        /// <param name="defaultName">Default name of new asset. If not set it is called 'New TypeName.asset'.</param>
        /// <returns>Returns the asset which was created.</returns>
        public static T CreateAsset<T>(string defaultName = null) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(path), "");
            }

            if (string.IsNullOrEmpty(defaultName))
            {
                defaultName = "New " + typeof(T).Name;
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + defaultName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            return asset;
        }
    }
}