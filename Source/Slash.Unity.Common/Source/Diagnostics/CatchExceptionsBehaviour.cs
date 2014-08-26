// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatchExceptionsBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Diagnostics
{
    using UnityEngine;

    /// <summary>
    ///   Debug behaviour to catch unhandled exceptions and show them with a modal dialog.
    /// </summary>
    public class CatchExceptionsBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Rectangle showing caught exceptions.
        /// </summary>
        public Rect DialogRect = new Rect(100, 100, 400, 300);

        private Vector2 dialogScrollPosition = Vector2.zero;

        private string logString;

        private string stackTrace;

        #endregion

        #region Methods

        private void DrawModalDialog(int id)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            this.dialogScrollPosition = GUILayout.BeginScrollView(this.dialogScrollPosition);
            GUILayout.Label(this.logString);
            GUILayout.Label("Stack Trace:");
            GUILayout.Label(this.stackTrace);

            if (GUILayout.Button("Ok"))
            {
                this.logString = null;
                this.stackTrace = null;
            }
            GUILayout.EndScrollView();
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Exception)
            {
                return;
            }

            this.logString = logString;
            this.stackTrace = stackTrace;
            this.dialogScrollPosition = Vector2.zero;
        }

        private void OnDisable()
        {
            Application.RegisterLogCallback(null);
        }

        private void OnEnable()
        {
            Application.RegisterLogCallback(this.HandleLog);
        }

        private void OnGUI()
        {
            if (this.logString != null)
            {
                this.DialogRect = GUI.ModalWindow(0, this.DialogRect, this.DrawModalDialog, "Uncaught exception");
            }
        }

        #endregion
    }
}