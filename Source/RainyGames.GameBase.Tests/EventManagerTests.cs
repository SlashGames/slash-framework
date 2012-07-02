// -----------------------------------------------------------------------
// <copyright file="EventManagerTests.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.Tests
{
    using NUnit.Framework;
    using RainyGames.GameBase.EventArgs;

    /// <summary>
    /// Unit tests for the EventManager class.
    /// </summary>
    [TestFixture]
    public class EventManagerTests : IEventListener
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
            this.game.EventManager.RegisterListener(this, FrameworkEventType.EntityCreated);
            this.game.EntityManager.CreateEntity();
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing an entity.
        /// </summary>
        [Test]
        public void TestEntityRemovedEvent()
        {
            this.game.EventManager.RegisterListener(this, FrameworkEventType.EntityRemoved);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.RemoveEntity(0L);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new player.
        /// </summary>
        [Test]
        public void TestPlayerAddedEvent()
        {
            this.game.EventManager.RegisterListener(this, FrameworkEventType.PlayerAdded);
            this.game.AddPlayer(this.player);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing a player.
        /// </summary>
        [Test]
        public void TestPlayerRemovedEvent()
        {
            this.game.EventManager.RegisterListener(this, FrameworkEventType.PlayerRemoved);
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
            this.game.EventManager.RegisterListener(this, FrameworkEventType.GameStarted);
            this.game.StartGame();
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on pausing the game.
        /// </summary>
        [Test]
        public void TestGamePausedEvent()
        {
            this.game.EventManager.RegisterListener(this, FrameworkEventType.GamePaused);
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
            this.game.EventManager.RegisterListener(this, FrameworkEventType.GameResumed);
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
            this.game.EventManager.RegisterListener(this, FrameworkEventType.SystemAdded);
            this.game.SystemManager.AddSystem(this.system);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on adding a new component to an entity.
        /// </summary>
        [Test]
        public void TestComponentAddedEvent()
        {
            this.game.EventManager.RegisterListener(this, FrameworkEventType.ComponentAdded);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(0L, this.component);
            this.CheckTestPassed();
        }

        /// <summary>
        /// Tests whether the appropriate event is fired on removing a component from an entity.
        /// </summary>
        [Test]
        public void TestComponentRemovedEvent()
        {
            this.game.EventManager.RegisterListener(this, FrameworkEventType.ComponentRemoved);
            this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(0L, this.component);
            this.game.EntityManager.RemoveComponent(0L, typeof(TestComponent));
            this.CheckTestPassed();
        }

        /// <summary>
        /// Notifies this unit test fixture of an Rainy Games Framework event
        /// that has occurred, checking the associated unit test.
        /// </summary>
        /// <param name="e">
        /// Event that has occurred within the framework.
        /// </param>
        public void Notify(Event e)
        {
            if (e.EventType is FrameworkEventType)
            {
                FrameworkEventType eventType = (FrameworkEventType)e.EventType;
                ComponentEventArgs eventArgs;

                switch (eventType)
                {
                    case FrameworkEventType.ComponentAdded:
                        eventArgs = (ComponentEventArgs)e.EventData;
                        this.OnComponentAdded(eventArgs.EntityId, eventArgs.Component);
                        break;

                    case FrameworkEventType.ComponentRemoved:
                        eventArgs = (ComponentEventArgs)e.EventData;
                        this.OnComponentRemoved(eventArgs.EntityId, eventArgs.Component);
                        break;

                    case FrameworkEventType.EntityCreated:
                        this.OnEntityCreated((long)e.EventData);
                        break;

                    case FrameworkEventType.EntityRemoved:
                        this.OnEntityRemoved((long)e.EventData);
                        break;

                    case FrameworkEventType.GamePaused:
                        this.OnGamePaused();
                        break;

                    case FrameworkEventType.GameResumed:
                        this.OnGameResumed();
                        break;

                    case FrameworkEventType.GameStarted:
                        this.OnGameStarted();
                        break;

                    case FrameworkEventType.PlayerAdded:
                        this.OnPlayerAdded((Player)e.EventData);
                        break;

                    case FrameworkEventType.PlayerRemoved:
                        this.OnPlayerRemoved((Player)e.EventData);
                        break;

                    case FrameworkEventType.SystemAdded:
                        this.OnSystemAdded((ISystem)e.EventData);
                        break;

                    default:
                        break;
                }
            }
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
        }
    }
}
