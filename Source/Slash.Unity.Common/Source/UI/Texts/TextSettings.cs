namespace Slash.Unity.Common.UI.Texts
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    static class TextSettingsEditor
    {
        [MenuItem("Assets/Create/Text Settings")]
        public static void CreateTextSettings()
        {
            var asset = ScriptableObject.CreateInstance<TextSettings>();
            ProjectWindowUtil.CreateAsset(asset, "New TextSettings.asset");
            AssetDatabase.SaveAssets();
        }
    }

#endif

    public class TextSettings : ScriptableObject
    {
        public Font Font;

        public FontStyle FontStyle;

        public int FontSize;

        public int LineSpacing;

        public bool RichText;

        public TextAnchor Alignment;

        public HorizontalWrapMode HorizontalOverflow;

        public VerticalWrapMode VerticalOverflow;

        public bool BestFit;

        public int BestFitMinSize;

        public int BestFitMaxSize;

        public Color Color;

        public Material Material;

        public TextEffectSettings Outline;

        public TextEffectSettings Shadow;
    }
}