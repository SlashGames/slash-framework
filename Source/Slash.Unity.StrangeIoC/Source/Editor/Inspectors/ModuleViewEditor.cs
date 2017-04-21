namespace Slash.Unity.StrangeIoC.Editor.Inspectors
{
    using Slash.Unity.StrangeIoC.Modules;

    using UnityEditor;

    [CustomEditor(typeof(ModuleView))]
    public class ModuleViewEditor : Editor
    {
        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var moduleView = this.target as ModuleView;
            if (moduleView == null)
            {
                return;
            }

            // Show module information.
            var moduleContext = moduleView.context as ModuleContext;
            if (moduleContext != null)
            {
                EditorGUILayout.TextField("Module", moduleContext.Name);
            }
        }
    }
}