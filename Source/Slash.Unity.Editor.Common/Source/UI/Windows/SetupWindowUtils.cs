// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetupWindowUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.UI.Windows
{
    using Slash.Unity.Common.Scenes;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public static class SetupWindowUtils
    {
        [MenuItem("Slash Games/UI/Windows/Create Window")]
        public static void CreateWindow()
        {
            EditorWindow.GetWindow<WindowSetupEditorWindow>(true, "Create New Window");
        }

        public static GameObject CreateWindow(string windowId)
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Create window root.
            var root = new GameObject(windowId);

            var canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var canvasScaler = root.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.matchWidthOrHeight = 0.5f;
            root.AddComponent<GraphicRaycaster>();

            var windowRoot = root.AddComponent<WindowRoot>();
            windowRoot.WindowId = windowId;

            // Create UI node.
            var ui = new GameObject("UI");
            var uiRect = ui.AddComponent<RectTransform>();
            ui.transform.SetParent(root.transform, false);
            uiRect.anchorMin = Vector2.zero;
            uiRect.anchorMax = Vector2.one;
            uiRect.sizeDelta = Vector2.zero;
            uiRect.anchoredPosition = Vector2.zero;

            // Create window placeholder.
            var windowPanel = new GameObject("Window");
            windowPanel.transform.SetParent(ui.transform, false);
            var windowRect = windowPanel.AddComponent<RectTransform>();
            windowRect.anchorMin = Vector2.zero;
            windowRect.anchorMax = Vector2.one;
            windowRect.sizeDelta = new Vector2(-40, -40);
            windowPanel.AddComponent<Image>();

            // Create Logic node.
            var logic = new GameObject("Logic");
            logic.transform.SetParent(root.transform, false);

            // Create Debug node.
            var debug = new GameObject("Debug");
            debug.transform.SetParent(root.transform, false);

            var cameraGameObject = new GameObject("Camera");
            cameraGameObject.AddComponent<Camera>();
            cameraGameObject.transform.SetParent(debug.transform, false);

            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.transform.SetParent(debug.transform, false);

            return root;
        }
    }
}