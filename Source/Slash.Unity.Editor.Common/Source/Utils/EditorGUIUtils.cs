namespace Slash.Unity.Editor.Common.Utils
{
    using UnityEditor;

    using UnityEngine;

    public static class EditorGUIUtils
    {
        public static string FolderField(GUIContent label, string path)
        {
            EditorGUILayout.BeginHorizontal();
            path = EditorGUILayout.TextField(label, path);
            if (GUILayout.Button("Select"))
            {
                path = EditorUtility.OpenFolderPanel("Select " + label.text + "...", path, path);
            }
            EditorGUILayout.EndHorizontal();
            return path;
        }
    }
}