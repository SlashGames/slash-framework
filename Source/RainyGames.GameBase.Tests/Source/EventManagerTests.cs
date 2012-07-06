// -----------------------------------------------------------------------
// <copyright file="EventManagerTests.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.Tests
{
    using NUnit.Framework;
    using RainyGames.Collections.AttributeTables;
    using RainyGames.GameBase.EventArgs;

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
            this.game.EventManager.RegisterListener(FrameworkEventType.EntityCreated, this.OnEntityCreated);
            this.game.EntityManager.CreateEntity();
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing an entity.
        /// </summary>
        [Test]
        public void TestEntityRemovedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.EntityRemoved, this.OnEntityRemoved);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.RemoveEntity(0);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new player.
        /// </summary>
        [Test]
        public void TestPlayerAddedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.PlayerAdded, this.OnPlayerAdded);
            this.game.AddPlayer(this.player);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing a player.
        /// </summary>
        [Test]
        public void TestPlayerRemovedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.PlayerRemoved, this.OnPlayerRemoved);
            this.game.AddPlayer(this.player);
            this.game.RemovePlayer(this.player);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on starting the game.
        /// </summary>
        [Test]
        public void TestGameStartedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.GameStarted, this.OnGameStarted);
            this.game.StartGame();
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on pausing the game.
        /// </summary>
        [Test]
        public void TestGamePausedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.GamePaused, this.OnGamePaused);
            this.game.StartGame();
            this.game.PauseGame();
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on resuming the game.
        /// </summary>
        [Test]
        public void TestGameResumedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.GameResumed, this.OnGameResumed);
            this.game.StartGame();
            this.game.PauseGame();
            this.game.ResumeGame();
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new system.
        /// </summary>
        [Test]
        public void TestSystemAddedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.SystemAdded, this.OnSystemAdded);
            this.game.SystemManager.AddSystem(this.system);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the we can listen to all events.
        /// </summary>
        [Test]
        public void TestListenToAllEvents()
        {
            object testEvent = new object();
            this.game.EventManager.RegisterListener(delegate { this.testPassed = true; });
            this.game.EventManager.QueueEvent(new Event(testEvent));
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests removing a listener for a specific event.
        /// </summary>
        [Test]
        public void TestRemoveListener()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.EntityCreated, this.OnEntityCreated);
            this.game.EventManager.RemoveListener(FrameworkEventType.EntityCreated, this.OnEntityCreated);
            this.game.EntityManager.CreateEntity();
            this.game.EventManager.ProcessEvents();
            Assert.IsFalse(this.testPassed);
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new component to an entity.
        /// </summary>
        [Test]
        public void TestComponentAddedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.ComponentAdded, this.OnComponentAdded);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(0, this.component);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing a component from an entity.
        /// </summary>
        [Test]
        public void TestComponentRemovedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEventType.ComponentRemoved, this.OnComponentRemoved);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(0, this.component);
            this.game.EntityManager.RemoveComponent(0, typeof(TestComponent));
            this.CheckTestPassed();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when a new entity has been created.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnEntityCreated(Event e)
        {
            int entityId = (int)e.EventData;
            this.testPassed = entityId == 0;
        }

        /// <summary>
        /// Called when an entity has been removed.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnEntityRemoved(Event e)
        {
            int entityId = (int)e.EventData;
            this.testPassed = entityId == 0;
        }

        /// <summary>
        /// Called when a new player has been added.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnPlayerAdded(Event e)
        {
            Player player = (Player)e.EventData;
            this.testPassed = this.player.Equals(player);
        }

        /// <summary>
        /// Called when a player has been removed.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnPlayerRemoved(Event e)
        {
            Player player = (Player)e.EventData;
            this.testPassed = this.player.Equals(player);
        }

        /// <summary>
        /// Called when the game starts.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnGameStarted(Event e)
        {
            this.testPassed = true;
        }

        /// <summary>
        /// Called when the game has been paused.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnGamePaused(Event e)
        {
            this.testPassed = true;
        }

        /// <summary>
        /// Called when the game has been resumed.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnGameResumed(Event e)
        {
            this.testPassed = true;
        }

        /// <summary>
        /// Called when a new system has been added.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnSystemAdded(Event e)
        {
            this.testPassed = this.system.Equals(e.EventData);
        }

        /// <summary>
        /// Called when a new component has been added.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnComponentAdded(Event e)
        {
            ComponentEventArgs eventArgs = (ComponentEventArgs)e.EventData;
            this.testPassed = eventArgs.EntityId == 0 && this.component.Equals(eventArgs.Component);
        }

        /// <summary>
        /// Called when a component has been removed.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        private void OnComponentRemoved(Event e)
        {
            ComponentEventArgs eventArgs = (ComponentEventArgs)e.EventData;
            this.testPassed = eventArgs.EntityId == 0 && this.component.Equals(eventArgs.Component);
        }

        /// <summary>
        /// Processes all occurred events and checks whether last unit test
        /// has passed.
        /// </summary>
        private void CheckTestPassed()
        {
            this.game.EventManager.ProcessEvents();
            Assert.IsTrue(this.testPassed);
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
            /// <summary>
            /// Initializes this component.
            /// </summary>
            /// <param name="attributeTable">This parameter is ignored.</param>
            public void InitComponent(IAttributeTable attributeTable)
            {
            }
        }
    }
}
