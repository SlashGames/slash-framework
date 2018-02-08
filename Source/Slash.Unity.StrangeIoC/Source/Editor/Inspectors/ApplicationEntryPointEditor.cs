namespace Slash.Unity.StrangeIoC.Editor.Inspectors
{
    using System.Collections.Generic;
    using System.Linq;
    using Slash.Unity.StrangeIoC.Initialization;
    using Slash.Unity.StrangeIoC.Modules;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ApplicationEntryPoint))]
    public class ApplicationEntryPointEditor : Editor
    {
        private readonly Dictionary<ModuleContext, bool> foldouts = new Dictionary<ModuleContext, bool>();

        /// <inheritdoc />
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var applicationEntryPoint = this.target as ApplicationEntryPoint;
            if (applicationEntryPoint == null)
            {
                return;
            }

            // Show module information.
            var moduleContext = applicationEntryPoint.context as ModuleContext;
            if (moduleContext != null)
            {
                this.DrawModuleFoldout(moduleContext);
            }
        }

        private void DrawModuleFoldout(ModuleContext moduleContext)
        {
            var label = new GUIContent(moduleContext.Name);

            if (moduleContext.SubModules == null || !moduleContext.SubModules.Any())
            {
                EditorGUILayout.LabelField(label);
            }
            else
            {
                bool foldout;
                this.foldouts.TryGetValue(moduleContext, out foldout);
                foldout = EditorGUILayout.Foldout(foldout,
                    label);
                this.foldouts[moduleContext] = foldout;
                if (foldout)
                {
                    foreach (var subModule in moduleContext.SubModules)
                    {
                        ++EditorGUI.indentLevel;
                        this.DrawModuleFoldout(subModule);
                        --EditorGUI.indentLevel;
                    }
                }
            }
        }
    }
}