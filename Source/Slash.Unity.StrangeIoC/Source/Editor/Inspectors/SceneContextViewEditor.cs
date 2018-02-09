namespace Slash.Unity.StrangeIoC.Editor.Inspectors
{
    using Slash.Unity.StrangeIoC.Modules;

    using UnityEditor;

    [CustomEditor(typeof(SceneContextView))]
    public class SceneContextViewEditor : Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var contextView = this.target as SceneContextView;
            if (contextView == null)
            {
                return;
            }

            // Show module information.
            var moduleContext = contextView.context as ModuleContext;
            if (moduleContext != null)
            {
                EditorGUILayout.TextField("Module", moduleContext.Name);
            }
        }
    }
}