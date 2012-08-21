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

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup for the tests of the Game class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
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
