// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests
{
    using NUnit.Framework;

    /// <summary>
    ///   Unit tests for the Game class.
    /// </summary>
    [TestFixture]
    public class GameTest
    {
        #region Fields

        /// <summary>
        ///   Test game to run unit tests on.
        /// </summary>
        private Game game;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Setup for the tests of the Game class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
        }

        /// <summary>
        ///   Tests starting, pausing and resuming the game.
        /// </summary>
        [Test]
        public void TestGameRunning()
        {
            Assert.IsFalse(this.game.Running);
            this.game.StartGame(null);
            Assert.IsTrue(this.game.Running);
            this.game.PauseGame();
            Assert.IsFalse(this.game.Running);
            this.game.ResumeGame();
            Assert.IsTrue(this.game.Running);
        }

        /// <summary>
        ///   Tests tracking the elapsed time of the game while
        ///   ticking it after it has been started, paused and resumed.
        /// </summary>
        [Test]
        public void TestTimeElapsed()
        {
            Assert.AreEqual(this.game.TimeElapsed, 0);
            this.game.Update(1.0f);
            Assert.AreEqual(this.game.TimeElapsed, 0);
            this.game.StartGame(null);
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