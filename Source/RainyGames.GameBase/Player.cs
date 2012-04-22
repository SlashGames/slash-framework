// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    using System;

    /// <summary>
    /// Player participating in a specific game. Players can be the owners of
    /// actors, i.e. the units they build or the projectiles they fired.
    /// </summary>
    public class Player
    {
        #region Constants and Fields

        /// <summary>
        /// Game this player participates in.
        /// </summary>
        private Game game;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new player to participate in the passed game,
        /// assigning a default index and name.
        /// </summary>
        /// <param name="game">
        /// Game the new player will participate in.
        /// </param>
        public Player(Game game)
            : this(game, 0)
        {
        }

        /// <summary>
        /// Constructs a new player with the specified index to participate in
        /// the passed game, assigning a default name.
        /// </summary>
        /// <param name="game">
        /// Game the new player will participate in.
        /// </param>
        /// <param name="index">
        /// Index of the player in the game.
        /// </param>
        public Player(Game game, int index)
            : this(game, index, "Player " + index)
        {
        }

        /// <summary>
        /// Constructs a new player with the specified index and name to
        /// participate in the passed game.
        /// </summary>
        /// <param name="game">
        /// Game the new player will participate in.
        /// </param>
        /// <param name="index">
        /// Index of the player in the game.
        /// </param>
        /// <param name="name">
        /// Name of the new player.
        /// </param>
        public Player(Game game, int index, string name)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;
            this.PlayerIndex = index;
            this.PlayerName = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Game this player participates in.
        /// </summary>
        public Game Game
        {
            get { return this.game; }
        }

        /// <summary>
        /// Index of this player in the game he or she is participating in.
        /// </summary>
        public int PlayerIndex { get; set; }

        /// <summary>
        /// Name of this player.
        /// </summary>
        public string PlayerName { get; set; }

        #endregion
    }
}
