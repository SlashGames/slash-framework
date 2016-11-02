using Slash.Unity.Common.UI.Texts;
using UnityEditor;
using UnityEngine;

namespace Slash.Unity.Editor.Common.UI.Texts
{
    public static class TextSettingsEditor
    {
        [MenuItem("Assets/Create/Text Settings")]
        public static void CreateTextSettings()
        {
            var asset = ScriptableObject.CreateInstance<TextSettings>();
            ProjectWindowUtil.CreateAsset(asset, "New TextSettings.asset");
            AssetDatabase.SaveAssets();
        }
    }
}