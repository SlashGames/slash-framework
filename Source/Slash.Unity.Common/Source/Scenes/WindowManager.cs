// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Scenes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public class WindowManager : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Open windows.
        /// </summary>
        private readonly List<Window> windows = new List<Window>();

        /// <summary>
        ///   Root data context.
        /// </summary>
        public object RootContext;

        #endregion

        #region Events

        public event Action<Window> WindowOpened;

        #endregion

        #region Properties

        public static WindowManager Instance { get; private set; }

        #endregion

        #region Public Methods and Operators

        public bool CloseWindow(Window window, object returnValue)
        {
            if (!this.windows.Contains(window))
            {
                Debug.LogError("Can't close window '" + window + "', not found in open windows.", this);
                return false;
            }

            this.DestroyWindow(window, returnValue);

            // Destroy parent windows.
            window = window.ParentWindow;
            while (window != null)
            {
                this.DestroyWindow(window, null);
                window = window.ParentWindow;
            }

            return true;
        }

        /// <summary>
        ///   Returns to previous window of the window with the specified id if any available in window stack.
        /// </summary>
        public void GoBack(Window window, object returnValue)
        {
            if (!this.windows.Contains(window))
            {
                Debug.LogWarning("No window found with id " + window, this);
                return;
            }

            if (window.ParentWindow == null)
            {
                Debug.LogWarning("No parent window for window " + window.WindowId + ", can't go back.", this);
                return;
            }

            // Destroy current window.
            this.DestroyWindow(window, returnValue);

            // Show parent window.
            window.ParentWindow.Root.gameObject.SetActive(true);
        }

        public void OnWindowDestroyed(WindowRoot windowRoot)
        {
            // Remove listener.
            windowRoot.WindowDestroyed -= this.OnWindowDestroyed;

            // Check if destroyed by itself.
            Window window = this.windows.FirstOrDefault(openWindow => openWindow.Root == windowRoot);
            if (window != null)
            {
                this.windows.Remove(window);
            }
            else
            {
                Debug.LogError("No window found for destroyed window root " + windowRoot.WindowId);
            }
        }

        public Window OpenWindow(string windowId, object context, Window parentWindow, Action<object> onCloseCallback)
        {
            // Hide parent window.
            if (parentWindow != null)
            {
                parentWindow.Hide();
            }

            Window window = new Window
            {
                WindowId = windowId,
                Context = context,
                OnClose = onCloseCallback,
                ParentWindow = parentWindow
            };

            this.StartCoroutine(this.DoOpenWindow(window));

            return window;
        }

        public Window OpenWindow(string windowId, Action<object> onCloseCallback)
        {
            return this.OpenWindow(windowId, null, null, onCloseCallback);
        }

        public Window OpenWindow(string windowId)
        {
            return this.OpenWindow(windowId, null, null, null);
        }

        public Window OpenWindow(string windowId, object context)
        {
            return this.OpenWindow(windowId, context, null, null);
        }

        public Window OpenWindow(string windowId, object context, Window parentWindow)
        {
            return this.OpenWindow(windowId, context, parentWindow, null);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Multiple window manager found, please use only one.", this);
            }
        }

        private void DestroyWindow(Window window, object returnValue)
        {
            // Destroy window root.
            Destroy(window.Root.gameObject);

            // Invoke callback.
            if (window.OnClose != null)
            {
                window.OnClose(returnValue);
                window.OnClose = null;
            }
        }

        private IEnumerator DoOpenWindow(Window window)
        {
            this.windows.Add(window);

            yield return Application.LoadLevelAdditiveAsync(window.WindowId);

            // Setup window roots.
            this.SetupNewWindowRoots();

            // Notify listeners.
            this.OnWindowOpened(window);
        }

        private int GetLoadingWindowCount()
        {
            return this.windows.Count(window => !window.Loaded);
        }

        private void OnWindowOpened(Window window)
        {
            var handler = this.WindowOpened;
            if (handler != null)
            {
                handler(window);
            }
        }

        private void SetupNewWindowRoots()
        {
            // Get new window roots.
            var windowRoots =
                FindObjectsOfType<WindowRoot>()
                    .Where(existingWindowRoot => this.windows.All(window => window.Root != existingWindowRoot))
                    .ToList();

            if (windowRoots.Count < 1)
            {
                Debug.LogWarning(
                    "No window root found in loaded scene. If you were loading multiple scenes, that's fine. Otherwise, please make sure to add a WindowRoot component to the root game object of the scene.",
                    this);
                return;
            }

            if (windowRoots.Count > 1 && this.GetLoadingWindowCount() <= 1)
            {
                Debug.LogWarning("Multiple new window roots found. Use only one WindowRoot component per scene.", this);
            }

            var newWindowRoot = windowRoots.First();

            // Get matching window.
            var newWindow =
                this.windows.FirstOrDefault(
                    existingWindow => existingWindow.Root == null && existingWindow.WindowId == newWindowRoot.WindowId);
            if (newWindow == null)
            {
                Debug.LogError(
                    "No window found for loaded window root " + newWindowRoot.WindowId
                    + ". Make sure that scene name and window id in WindowRoot behaviour match.",
                    this);
                return;
            }

            // Add window.
            newWindow.Root = newWindowRoot;
            newWindow.Root.WindowDestroyed += this.OnWindowDestroyed;
            newWindow.Loaded = true;

            Debug.Log("Loaded window '" + newWindow + "' with id '" + newWindow.WindowId + "'.");
        }

        #endregion

        public class Window
        {
            #region Fields

            public WindowRoot Root;

            #endregion

            #region Properties

            public object Context { get; set; }

            public bool Loaded { get; set; }

            public Action<object> OnClose { get; set; }

            public Window ParentWindow { get; set; }

            public string WindowId { get; set; }

            #endregion

            #region Public Methods and Operators

            public void Hide()
            {
                this.Root.gameObject.SetActive(false);
            }

            public void Show()
            {
                this.Root.gameObject.SetActive(true);
            }

            #endregion
        }
    }
}