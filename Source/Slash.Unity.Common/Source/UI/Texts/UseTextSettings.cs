using UnityEngine;
using UnityEngine.UI;

namespace Slash.Unity.Common.UI.Texts
{
    public class UseTextSettings : MonoBehaviour
    {
        public bool IgnoreAlignment;

        public bool IgnoreColor;

        public bool IgnoreFontSize;

        public TextSettings Settings;

        public Text Target;

        public void ApplySettings()
        {
            if (this.Settings == null || this.Target == null)
            {
                return;
            }

            // Character settings.
            this.Target.font = this.Settings.Font;
            this.Target.fontStyle = this.Settings.FontStyle;

            if (!this.IgnoreFontSize)
            {
                this.Target.fontSize = this.Settings.FontSize;
            }

            this.Target.lineSpacing = this.Settings.LineSpacing;
            this.Target.supportRichText = this.Settings.RichText;

            // Paragraph settings.
            if (!this.IgnoreAlignment)
            {
                this.Target.alignment = this.Settings.Alignment;
            }
            this.Target.horizontalOverflow = this.Settings.HorizontalOverflow;
            this.Target.verticalOverflow = this.Settings.VerticalOverflow;

            if (!this.IgnoreFontSize)
            {
                this.Target.resizeTextForBestFit = this.Settings.BestFit;
                this.Target.resizeTextMinSize = this.Settings.BestFitMinSize;
                this.Target.resizeTextMaxSize = this.Settings.BestFitMaxSize;
            }

            // Other settings.
            if (!this.IgnoreColor)
            {
                this.Target.color = this.Settings.Color;
            }
            this.Target.material = this.Settings.Material;

            // Use shadow if required.
            SetupEffect<Shadow>(this.Target.gameObject, this.Settings.Shadow);

            // Use outline if required.
            SetupEffect<Outline>(this.Target.gameObject, this.Settings.Outline);
        }

        [ContextMenu("Apply")]
        private void ApplySettingsInEditor()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this.Target, "Apply Text Settings");
#endif

            this.ApplySettings();
        }

        private void Awake()
        {
            this.ApplySettings();
        }

        private void Reset()
        {
            if (this.Target == null)
            {
                this.Target = this.GetComponent<Text>();
            }

            this.ApplySettings();
        }

        private static void SetupEffect<TEffect>(GameObject gameObject, TextEffectSettings effectSettings)
            where TEffect : Shadow
        {
            if (effectSettings.UseEffect)
            {
                var effect = gameObject.GetComponent<TEffect>() ?? gameObject.AddComponent<TEffect>();
                effect.effectColor = effectSettings.Color;
                effect.effectDistance = effectSettings.Distance;
            }
        }
    }
}