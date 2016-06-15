namespace Slash.Unity.Editor.Common.UI.Texts
{
    using Slash.Unity.Common.UI.Texts;

    using UnityEditor;

    [CustomEditor(typeof(UseTextSettings))]
    public class UseTextSettingsEditor : Editor
    {
        #region Public Methods and Operators

        public override void OnInspectorGUI()
        {
            var useTextSettings = this.target as UseTextSettings;
            if (useTextSettings == null)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();

            base.OnInspectorGUI();
            
            if (EditorGUI.EndChangeCheck())
            {
                useTextSettings.ApplySettings();
            }
        }

        #endregion
    }
}