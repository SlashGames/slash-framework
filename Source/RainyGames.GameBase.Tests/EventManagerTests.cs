// -----------------------------------------------------------------------
// <copyright file="EventManagerTests.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the EventManager class.
    /// </summary>
    [TestFixture]
    public class EventManagerTests
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

        /// <summary>
        /// Test system to run unit tests on.
        /// </summary>
        private TestSystem system;

        /// <summary>
        /// Test component to run unit tests on.
        /// </summary>
        private TestComponent component;

        /// <summary>
        /// Whether the last event triggered correctly and the unit test has
        /// passed, or not.
        /// </summary>
        private bool testPassed;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup for the tests of the EventManager class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.player = new Player(this.game);
            this.system = new TestSystem();
            this.component = new TestComponent();
            this.testPassed = false;
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on creating a new entity.
        /// </summary>
        [Test]
        public void TestEntityCreatedEvent()
        {
            this.game.EventManager.EntityCreated += new EventManager.EntityDelegate(this.OnEntityCreated);
            this.game.EntityManager.CreateEntity();
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing an entity.
        /// </summary>
        [Test]
        public void TestEntityRemovedEvent()
        {
            this.game.EventManager.EntityRemoved += new EventManager.EntityDelegate(this.OnEntityRemoved);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.RemoveEntity(0L);
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new player.
        /// </summary>
        [Test]
        public void TestPlayerAddedEvent()
        {
            this.game.EventManager.PlayerAdded += new EventManager.PlayerDelegate(this.OnPlayerAdded);
            this.game.AddPlayer(this.player);
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing a player.
        /// </summary>
        [Test]
        public void TestPlayerRemovedEvent()
        {
            this.game.EventManager.PlayerRemoved += new EventManager.PlayerDelegate(this.OnPlayerRemoved);
            this.game.AddPlayer(this.player);
            this.game.RemovePlayer(this.player);
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on starting the game.
        /// </summary>
        [Test]
        public void TestGameStartedEvent()
        {
            this.game.EventManager.GameStarted += new EventManager.GameDelegate(this.OnGameStarted);
            this.game.StartGame();
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on pausing the game.
        /// </summary>
        [Test]
        public void TestGamePausedEvent()
        {
            this.game.EventManager.GamePaused += new EventManager.GameDelegate(this.OnGamePaused);
            this.game.StartGame();
            this.game.PauseGame();
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on resuming the game.
        /// </summary>
        [Test]
        public void TestGameResumedEvent()
        {
            this.game.EventManager.GameResumed += new EventManager.GameDelegate(this.OnGameResumed);
            this.game.StartGame();
            this.game.PauseGame();
            this.game.ResumeGame();
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new system.
        /// </summary>
        [Test]
        public void TestSystemAddedEvent()
        {
            this.game.EventManager.SystemAdded += new EventManager.SystemDelegate(this.OnSystemAdded);
            this.game.SystemManager.AddSystem(this.system);
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new component to an entity.
        /// </summary>
        [Test]
        public void TestComponentAddedEvent()
        {
            this.game.EventManager.ComponentAdded += new EventManager.ComponentDelegate(this.OnComponentAdded);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(0L, this.component);
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing a component from an entity.
        /// </summary>
        [Test]
        public void TestComponentRemovedEvent()
        {
            this.game.EventManager.ComponentRemoved += new EventManager.ComponentDelegate(this.OnComponentRemoved);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(0L, this.component);
            this.game.EntityManager.RemoveComponent(0L, typeof(TestComponent));
            Assert.IsTrue(this.testPassed);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when a new entity has been created.
        /// </summary>
        /// <param name="entityId">
        /// Id of the new entity.
        /// </param>
        private void OnEntityCreated(long entityId)
        {
            this.testPassed = entityId == 0;
        }

        /// <summary>
        /// Called when an entity has been removed.
        /// </summary>
        /// <param name="entityId">
        /// Id of the removed entity.
        /// </param>
        private void OnEntityRemoved(long entityId)
        {
            this.testPassed = entityId == 0;
        }

        /// <summary>
        /// Called when a new player has been added.
        /// </summary>
        /// <param name="player">
        /// Player that has been added.
        /// </param>
        private void OnPlayerAdded(Player player)
        {
            this.testPassed = this.player.Equals(player);
        }

        /// <summary>
        /// Called when a player has been removed.
        /// </summary>
        /// <param name="player">
        /// Player that has been removed.
        /// </param>
        private void OnPlayerRemoved(Player player)
        {
            this.testPassed = this.player.Equals(player);
        }

        /// <summary>
        /// Called when the game starts.
        /// </summary>
        private void OnGameStarted()
        {
            this.testPassed = true;
        }

        /// <summary>
        /// Called when the game has been paused.
        /// </summary>
        private void OnGamePaused()
        {
            this.testPassed = true;
        }

        /// <summary>
        /// Called when the game has been resumed.
        /// </summary>
        private void OnGameResumed()
        {
            this.testPassed = true;
        }

        /// <summary>
        /// Called when a new system has been added.
        /// </summary>
        /// <param name="system">
        /// System that has been added.
        /// </param>
        private void OnSystemAdded(ISystem system)
        {
            this.testPassed = this.system.Equals(system);
        }

        /// <summary>
        /// Called when a new component has been added.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity the component has been added to.
        /// </param>
        /// <param name="component">
        /// Component that has been added.
        /// </param>
        private void OnComponentAdded(long entityId, IComponent component)
        {
            this.testPassed = entityId == 0 && this.component.Equals(component);
        }

        /// <summary>
        /// Called when a component has been removed.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity the component has been removed from.
        /// </param>
        /// <param name="component">
        /// Component that has been removed.
        /// </param>
        private void OnComponentRemoved(long entityId, IComponent component)
        {
            this.testPassed = entityId == 0 && this.component.Equals(component);
        }

        #endregion

        /// <summary>
        /// Test implementation of a game system.
        /// </summary>
        private class TestSystem : ISystem
        {
            /// <summary>
            /// Ticks this system.
            /// </summary>
            /// <param name="dt">
            /// Time passed since the last tick, in seconds.
            /// </param>
            public void Update(float dt)
            {
            }
        }

        /// <summary>
        /// Test implementation of a game component.
        /// </summary>
        private class TestComponent : IComponent
        {
        }
    }
}
