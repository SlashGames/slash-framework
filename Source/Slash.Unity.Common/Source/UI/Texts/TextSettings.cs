namespace Slash.Unity.Common.UI.Texts
{
    using UnityEngine;

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