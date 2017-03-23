namespace Slash.Unity.Editor.Common.Utils
{
    using Slash.Unity.Common.Utils;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            if (property != null)
            {
                property.intValue = EditorGUI.LayerField(position, property.intValue);
            }
            EditorGUI.EndProperty();
        }
    }
}