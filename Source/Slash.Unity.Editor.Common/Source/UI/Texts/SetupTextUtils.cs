namespace Slash.Unity.Editor.Common.UI.Texts
{
    using Slash.Unity.Common.UI.Texts;

    using UnityEditor;

    using UnityEngine;

    public static class SetupTextUtils
    {
        #region Public Methods and Operators

        [MenuItem("CONTEXT/Text/Use Text Settings")]
        public static void UseTextSettings()
        {
            var gameObject = Selection.activeGameObject;
            if (gameObject != null)
            {
                var useTextSettings = gameObject.GetComponent<UseTextSettings>();
                if (useTextSettings == null)
                {
                    useTextSettings = gameObject.AddComponent<UseTextSettings>();
                }
            }
        }

        [MenuItem("Slash Games/UI/Text Setup/Re-Apply styles")]
        public static void ReapplyStyles()
        {
            var useTextSettingsScripts = Object.FindObjectsOfType<UseTextSettings>();
            foreach (var useTextSettings in useTextSettingsScripts)
            {
                useTextSettings.ApplySettings();
            }
        }

        #endregion
    }
}