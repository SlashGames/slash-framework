// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheatConsole.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Diagnostics
{
    using Slash.GameBase;
    using Slash.GameBase.Events;
    using Slash.Unity.Common.Core;

    using UnityEngine;

    public class CheatConsole : MonoBehaviour
    {
        #region Fields

        public Rect ButtonRect = new Rect(0, 0, 10, 10);

        /// <summary>
        ///   Reference to a game specific cheat behaviour which implements a
        ///   "DrawCheats" method to render the controls to execute cheats.
        /// </summary>
        public GameCheatBehaviour GameSpecific;

        public string[] QuickCheats;

        /// <summary>
        ///   Height of UI.
        /// </summary>
        public float UIHeight = 600;

        /// <summary>
        ///   Width of UI.
        /// </summary>
        public float UIWidth = 800;

        /// <summary>
        ///   Indicates if a butotn should be shown to enable/disable cheat console.
        /// </summary>
        public bool UseButton = true;

        public Rect WindowRect = new Rect(10, 10, 300, 400);

        private readonly Rect dragRect = new Rect(0, 0, 10000, 20);

        private string cheat = string.Empty;

        private Game game;

        private GameBehaviour gameBehaviour;

        private Vector2 scrollPositionGameSpecific;

        private bool showConsole = true;

        private Vector2 scrollPositionQuickCheats;

        #endregion

        #region Public Methods and Operators

        public void DisableConsole()
        {
            this.showConsole = false;

            // Disable collider if available (blocks input for other UI).
            if (this.collider != null)
            {
                this.collider.enabled = false;
            }
            if (this.collider2D != null)
            {
                this.collider2D.enabled = false;
            }
        }

        public void EnableConsole()
        {
            this.showConsole = true;

            // Enable collider if available (blocks input for other UI).
            if (this.collider != null)
            {
                this.collider.enabled = true;
            }
            if (this.collider2D != null)
            {
                this.collider2D.enabled = true;
            }
        }

        public void Start()
        {
            this.DisableConsole();
        }

        #endregion

        #region Methods

        private void DrawCheatConsole(int id)
        {
            GUI.DragWindow(this.dragRect);

            if (this.game != null)
            {
                GUILayout.Label("Cheat: ");
                this.cheat = GUILayout.TextField(this.cheat);

                if (!string.IsNullOrEmpty(this.cheat) && GUILayout.Button("Submit"))
                {
                    this.game.EventManager.QueueEvent(FrameworkEvent.Cheat, this.cheat);
                }

                GUILayout.Label("Quick Cheats:");
                this.scrollPositionQuickCheats = GUILayout.BeginScrollView(this.scrollPositionQuickCheats);
                foreach (var quickCheat in this.QuickCheats)
                {
                    if (GUILayout.Button(quickCheat))
                    {
                        this.game.EventManager.QueueEvent(FrameworkEvent.Cheat, quickCheat);
                    }
                }
                GUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("No running game.");
            }

            // Draw game specific cheats.
            if (this.GameSpecific != null)
            {
                GUILayout.Label("Game Specific: ");
                this.scrollPositionGameSpecific = GUILayout.BeginScrollView(this.scrollPositionGameSpecific);
                this.GameSpecific.DrawCheats();
                GUILayout.EndScrollView();
            }
        }

        private void OnGUI()
        {
            // Draw button to show/hide cheat console.
            if (this.UseButton)
            {
                var currentMatrix = this.ScaleGUI();
                if (GUI.Button(this.ButtonRect, "$"))
                {
                    this.showConsole = !this.showConsole;
                }
                GUI.matrix = currentMatrix;
            }

            if (this.showConsole)
            {
                var currentMatrix = this.ScaleGUI();
                this.WindowRect = GUI.Window(0, this.WindowRect, this.DrawCheatConsole, "Cheat Console");
                GUI.matrix = currentMatrix;
            }
        }

        private void OnGameChanged(Game newGame, Game oldGame)
        {
            this.game = newGame;
        }

        private Matrix4x4 ScaleGUI()
        {
            // Scale window.
            var horizRatio = Screen.width / this.UIWidth;
            var vertRatio = Screen.height / this.UIHeight;
            var currentMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(horizRatio, vertRatio, 1));
            return currentMatrix;
        }

        private void Update()
        {
            if (this.showConsole)
            {
                GameBehaviour newGameBehaviour = (GameBehaviour)FindObjectOfType(typeof(GameBehaviour));
                if (this.gameBehaviour != newGameBehaviour)
                {
                    if (this.gameBehaviour != null)
                    {
                        this.gameBehaviour.GameChanged -= this.OnGameChanged;
                    }
                    this.gameBehaviour = newGameBehaviour;

                    if (this.gameBehaviour != null)
                    {
                        this.gameBehaviour.GameChanged += this.OnGameChanged;
                    }

                    this.game = this.gameBehaviour != null ? this.gameBehaviour.Game : null;
                }
            }
        }

        #endregion
    }
}