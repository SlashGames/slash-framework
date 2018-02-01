namespace Slash.Unity.Editor.Common.UI
{
    using Slash.Unity.Common.UI;
    using UnityEditor;
    using UnityEditor.UI;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(NonDrawingGraphic), false)]
    public class NonDrawingGraphicEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            EditorGUILayout.PropertyField(this.m_Script);

            // skipping AppearanceControlsGUI
            this.RaycastControlsGUI();
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}