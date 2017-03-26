namespace Slash.Unity.Editor.Common.Inspectors.Loading
{
    using System.IO;
    using System.Linq;
    using Slash.Unity.Common.Loading;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    [CustomEditor(typeof(LoadSceneBehaviour))]
    public class LoadSceneBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var loadSceneBehaviour = (LoadSceneBehaviour) this.target;

            if (!string.IsNullOrEmpty(loadSceneBehaviour.SceneName) && GUILayout.Button("Open Additive"))
            {
                var buildScene =
                    EditorBuildSettings.scenes.FirstOrDefault(
                        scene => Path.GetFileNameWithoutExtension(scene.path) == loadSceneBehaviour.SceneName);
                if (buildScene != null)
                {
                    var openScene = EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Additive);
#if UNITY_5_4_OR_NEWER
                    var sceneRoot = openScene.GetRootGameObjects().FirstOrDefault();
                    if (sceneRoot != null)
                    {
                        EditorGUIUtility.PingObject(sceneRoot);
                    }
#endif
                }
                else
                {
                    Debug.LogWarningFormat("Scene name '{0}' not found in scenes of build settings",
                        loadSceneBehaviour.SceneName);
                }
            }
        }
    }
}