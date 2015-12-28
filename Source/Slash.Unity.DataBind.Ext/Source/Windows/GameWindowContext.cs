// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameWindowContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Ext.Windows
{
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.Unity.Common.ECS;

    using UnityEngine;

    public class GameWindowContext : WindowContext
    {
        #region Fields

        private readonly GameBehaviour game;

        #endregion

        #region Constructors and Destructors

        public GameWindowContext()
        {
            this.game = Object.FindObjectOfType<GameBehaviour>();
        }

        #endregion

        #region Properties

        protected EntityManager EntityManager
        {
            get
            {
                return this.game != null && this.game.Game != null ? this.game.Game.EntityManager : null;
            }
        }

        protected EventManager EventManager
        {
            get
            {
                return this.game != null && this.game.Game != null ? this.game.Game.EventManager : null;
            }
        }

        #endregion
    }
}