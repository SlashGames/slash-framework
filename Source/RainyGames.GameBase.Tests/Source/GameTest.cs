// -----------------------------------------------------------------------
// <copyright file="GameTest.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the Game class.
    /// </summary>
    [TestFixture]
    public class GameTest
    {
        #region Constants and Fields

        /// <summary>
        /// Test game to run unit tests on.
        /// </summary>
        private Game game;

        /// <summary>
        /// Test player to run unit tests on.
        /// </summary>
        private Player player;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup for the tests of the Game class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.player = new Player(this.game, 0);
        }

        /// <summary>
        /// Tests adding a player to the game.
        /// </summary>
        [Test]
        public void TestAddPlayer()
        {
            this.game.AddPlayer(this.player);
            Assert.AreEqual(this.player, this.game.GetPlayer(0));
        }

        /// <summary>
        /// Tests removing a player from the game.
        /// </summary>
        [Test]
        public void TestRemovePlayer()
        {
            this.game.AddPlayer(this.player);
            Assert.IsTrue(this.game.RemovePlayer(this.player));
            Assert.IsFalse(this.game.RemovePlayer(this.player));
            Assert.IsNull(this.game.GetPlayer(0));
        }

        /// <summary>
        /// Tests starting, pausing and resuming the game.
        /// </summary>
        [Test]
        public void TestGameRunning()
        {
            Assert.IsFalse(this.game.Running);
            this.game.StartGame();
            Assert.IsTrue(this.game.Running);
            this.game.PauseGame();
            Assert.IsFalse(this.game.Running);
            this.game.ResumeGame();
            Assert.IsTrue(this.game.Running);
        }

        /// <summary>
        /// Tests tracking the elapsed time of the game while
        /// ticking it after it has been started, paused and resumed.
        /// </summary>
        [Test]
        public void TestTimeElapsed()
        {
            Assert.AreEqual(this.game.TimeElapsed, 0);
            this.game.Update(1.0f);
            Assert.AreEqual(this.game.TimeElapsed, 0);
            this.game.StartGame();
            this.game.Update(1.0f);
            Assert.AreEqual(this.game.TimeElapsed, 1.0f);
            this.game.PauseGame();
            this.game.Update(1.0f);
            Assert.AreEqual(this.game.TimeElapsed, 1.0f);
            this.game.ResumeGame();
            this.game.Update(1.0f);
            Assert.AreEqual(this.game.TimeElapsed, 2.0f);
        }

        #endregion
    }
}
