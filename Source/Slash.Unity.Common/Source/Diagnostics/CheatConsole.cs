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

        private readonly Rect dragRect = new Rect(0, 0, 10000, 20);

        private string cheat = string.Empty;

        private Game game;

        private GameBehaviour gameBehaviour;

        private bool showConsole;

        private Rect windowRect = new Rect(10, 10, 300, 200);

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
                    this.game.EventManager.QueueEvent(FrameworkEventType.Cheat, this.cheat);
                }
            }
            else
            {
                GUILayout.Label("No running game.");
            }
        }

        private void OnGUI()
        {
            // Draw button to show/hide cheat console.
            if (GUI.Button(this.ButtonRect, "$"))
            {
                this.showConsole = !this.showConsole;
            }

            if (this.showConsole)
            {
                this.windowRect = GUI.Window(0, this.windowRect, this.DrawCheatConsole, "Cheat Console");
            }
        }

        private void OnGameChanged(Game newGame, Game oldGame)
        {
            this.game = newGame;
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