namespace Slash.Unity.Editor.Common.UI.Texts
{
    using System;
    using System.Linq;

    using Slash.Unity.Common.UI.Texts;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///   Property Drawer for <see cref="TextSettings" />.
    /// </summary>
    [CustomPropertyDrawer(typeof(TextSettings))]
    public class TextSettingsPropertyDrawer : PropertyDrawer
    {
        #region Constants

        private const float LineHeight = 16f;

        private const float LineSpacing = 2f;

        private static GUIContent[] availableTextSettingGUIContents;

        private static TextSettings[] availableTextSettings;

        private static string[] availableTextSettingsAssetGUIDs;

        #endregion

        #region Public Methods and Operators

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return LineHeight + LineSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var textSettingsAssetGUIDs = AssetDatabase.FindAssets("t:TextSettings");
            if (availableTextSettings == null || !availableTextSettingsAssetGUIDs.SequenceEqual(textSettingsAssetGUIDs))
            {
                availableTextSettingsAssetGUIDs = textSettingsAssetGUIDs;
                var assetPaths = availableTextSettingsAssetGUIDs.Select<string, string>(AssetDatabase.GUIDToAssetPath).ToList();
                availableTextSettings =
                    assetPaths.Select<string, TextSettings>(AssetDatabase.LoadAssetAtPath<TextSettings>).ToArray();
                availableTextSettingGUIContents =
                    new[] { new GUIContent("None") }.Union(
                        availableTextSettings.Select(textSettings => new GUIContent(textSettings.name))).ToArray();
            }

            position.height = LineHeight;

            // Drop down for available text settings.
            var currentSettingsIndex = Array.IndexOf(availableTextSettings, (TextSettings)property.objectReferenceValue);

            var selectedSettingsIndex = EditorGUI.Popup(
                position,
                label,
                currentSettingsIndex != -1 ? currentSettingsIndex + 1 : 0,
                availableTextSettingGUIContents) - 1;
            if (selectedSettingsIndex != currentSettingsIndex)
            {
                var newTextSettings = selectedSettingsIndex >= 0
                    ? availableTextSettings[selectedSettingsIndex]
                    : null;
                property.objectReferenceValue = newTextSettings;
            }

            position.y += LineHeight + LineSpacing;
        }

        #endregion
    }
}