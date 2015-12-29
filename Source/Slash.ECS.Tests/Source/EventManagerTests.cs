// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventManagerTests.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Slash.Application.Games;
    using Slash.Application.Systems;
    using Slash.Collections.AttributeTables;
    using Slash.Collections.Utils;
    using Slash.ECS.Components;
    using Slash.ECS.Events;

    /// <summary>
    ///   Unit tests for the EventManager class.
    /// </summary>
    [TestFixture]
    public class EventManagerTests
    {
        #region Fields

        /// <summary>
        ///   Test component to run unit tests on.
        /// </summary>
        private TestEntityComponent entityComponent;

        /// <summary>
        ///   Test game to run unit tests on.
        /// </summary>
        private Game game;

        /// <summary>
        ///   Test system to run unit tests on.
        /// </summary>
        private TestSystem system;

        private int testEntityId;

        /// <summary>
        ///   Whether the last event triggered correctly and the unit test has
        ///   passed, or not.
        /// </summary>
        private bool testPassed;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Setup for the tests of the EventManager class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.system = new TestSystem();
            this.entityComponent = new TestEntityComponent();
            this.testPassed = false;
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on adding a new component to an entity.
        /// </summary>
        [Test]
        public void TestComponentAddedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEvent.ComponentAdded, this.OnComponentAdded);
            this.testEntityId = this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(this.testEntityId, this.entityComponent);
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on removing a component from an entity.
        /// </summary>
        [Test]
        public void TestComponentRemovedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEvent.ComponentRemoved, this.OnComponentRemoved);
            this.testEntityId = this.game.EntityManager.CreateEntity();
            this.game.EntityManager.AddComponent(this.testEntityId, this.entityComponent);
            this.game.EntityManager.RemoveComponent(this.testEntityId, typeof(TestEntityComponent));
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests that delayed events are correctly dispatched when calling ProcessEvents while processing.
        /// </summary>
        [Test]
        public void TestDelayedEventsDispatchedWhenProcessEventsWhileProcessing()
        {
            const int TestEvent1 = 1;
            const int TestEvent2 = 2;
            const int TestEvent3 = 3;
            const float dt = 0.5f;
            this.game.EventManager.RegisterListener(
                TestEvent1,
                ev =>
                    {
                        this.game.EventManager.QueueEvent(TestEvent3);
                        this.game.EventManager.ProcessEvents(dt);
                    });
            List<object> events = new List<object>();
            this.game.EventManager.RegisterListener(ev => events.Add(ev.EventType));

            // Queue two events.
            this.game.EventManager.QueueEvent(TestEvent1);
            this.game.EventManager.FireDelayed(dt * 1.5f, TestEvent2);

            // Process events.
            this.game.EventManager.ProcessEvents(dt);

            // Check order.
            Assert.IsTrue(
                CollectionUtils.SequenceEqual(events, new List<object> { TestEvent1, TestEvent3, TestEvent2 }));
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on creating a new entity.
        /// </summary>
        [Test]
        public void TestEntityCreatedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEvent.EntityCreated, this.OnEntityCreated);
            this.testEntityId = this.game.EntityManager.CreateEntity();
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on removing an entity.
        /// </summary>
        [Test]
        public void TestEntityRemovedEvent()
        {
            this.game.EventManager.RegisterListener(FrameworkEvent.EntityRemoved, this.OnEntityRemoved);
            this.testEntityId = this.game.EntityManager.CreateEntity();
            this.game.EntityManager.RemoveEntity(this.testEntityId);
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on pausing the game.
        /// </summary>
        [Test]
        public void TestGamePausedEvent()
        {
            this.game.EventManager.RegisterListener(ApplicationGameEvent.GamePaused, this.OnGamePaused);
            this.game.StartGame(null);
            this.game.PauseGame();
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on resuming the game.
        /// </summary>
        [Test]
        public void TestGameResumedEvent()
        {
            this.game.EventManager.RegisterListener(ApplicationGameEvent.GameResumed, this.OnGameResumed);
            this.game.StartGame(null);
            this.game.PauseGame();
            this.game.ResumeGame();
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on starting the game.
        /// </summary>
        [Test]
        public void TestGameStartedEvent()
        {
            this.game.EventManager.RegisterListener(ApplicationGameEvent.GameStarted, this.OnGameStarted);
            this.game.StartGame(null);
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests whether the we can listen to all events.
        /// </summary>
        [Test]
        public void TestListenToAllEvents()
        {
            object testEvent = new object();
            this.game.EventManager.RegisterListener(delegate { this.testPassed = true; });
            this.game.EventManager.QueueEvent(new GameEvent(testEvent));
            this.CheckTestPassed();
        }

        /// <summary>
        ///   Tests that event order is maintained even if ProcessEvents is called while processing events.
        /// </summary>
        [Test]
        public void TestProcessEventsWhileProcessing()
        {
            const int TestEvent1 = 1;
            const int TestEvent2 = 2;
            const int TestEvent3 = 3;
            this.game.EventManager.RegisterListener(
                TestEvent1,
                ev =>
                    {
                        this.game.EventManager.QueueEvent(TestEvent3);
                        this.game.EventManager.ProcessEvents();
                    });
            List<object> events = new List<object>();
            this.game.EventManager.RegisterListener(ev => events.Add(ev.EventType));

            // Queue two events.
            this.game.EventManager.QueueEvent(TestEvent1);
            this.game.EventManager.QueueEvent(TestEvent2);

            // Process events.
            this.game.EventManager.ProcessEvents();

            // Check order.
            Assert.IsTrue(
                CollectionUtils.SequenceEqual(events, new List<object> { TestEvent1, TestEvent2, TestEvent3 }));
        }

        /// <summary>
        ///   Tests removing a listener for a specific event.
        /// </summary>
        [Test]
        public void TestRemoveListener()
        {
            this.game.EventManager.RegisterListener(FrameworkEvent.EntityCreated, this.OnEntityCreated);
            this.game.EventManager.RemoveListener(FrameworkEvent.EntityCreated, this.OnEntityCreated);
            this.game.EntityManager.CreateEntity();
            this.game.EventManager.ProcessEvents();
            Assert.IsFalse(this.testPassed);
        }

        /// <summary>
        ///   Tests whether the appropriate event is fired on adding a new system.
        /// </summary>
        [Test]
        public void TestSystemAddedEvent()
        {
            this.game.EventManager.RegisterListener(SystemGameEvent.SystemAdded, this.OnSystemAdded);
            this.game.SystemManager.AddSystem(this.system);
            this.CheckTestPassed();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Processes all occurred events and checks whether last unit test
        ///   has passed.
        /// </summary>
        private void CheckTestPassed()
        {
            this.game.EventManager.ProcessEvents();
            Assert.IsTrue(this.testPassed);
        }

        /// <summary>
        ///   Called when a new component has been added.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnComponentAdded(GameEvent e)
        {
            EntityComponentData data = (EntityComponentData)e.EventData;
            this.testPassed = data.EntityId == this.testEntityId && this.entityComponent.Equals(data.Component);
        }

        /// <summary>
        ///   Called when a component has been removed.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnComponentRemoved(GameEvent e)
        {
            EntityComponentData data = (EntityComponentData)e.EventData;
            this.testPassed = data.EntityId == this.testEntityId && this.entityComponent.Equals(data.Component);
        }

        /// <summary>
        ///   Called when a new entity has been created.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnEntityCreated(GameEvent e)
        {
            int entityId = (int)e.EventData;
            this.testPassed = entityId == this.testEntityId;
        }

        /// <summary>
        ///   Called when an entity has been removed.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnEntityRemoved(GameEvent e)
        {
            int entityId = (int)e.EventData;
            this.testPassed = entityId == this.testEntityId;
        }

        /// <summary>
        ///   Called when the game has been paused.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnGamePaused(GameEvent e)
        {
            this.testPassed = true;
        }

        /// <summary>
        ///   Called when the game has been resumed.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnGameResumed(GameEvent e)
        {
            this.testPassed = true;
        }

        /// <summary>
        ///   Called when the game starts.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnGameStarted(GameEvent e)
        {
            this.testPassed = true;
        }

        /// <summary>
        ///   Called when a new system has been added.
        /// </summary>
        /// <param name="e"> Event that has occurred within the framework. </param>
        private void OnSystemAdded(GameEvent e)
        {
            this.testPassed = this.system.Equals(e.EventData);
        }

        #endregion

        /// <summary>
        ///   Test implementation of a game component.
        /// </summary>
        private class TestEntityComponent : IEntityComponent
        {
            #region Public Methods and Operators

            /// <summary>
            ///   Initializes this component.
            /// </summary>
            /// <param name="attributeTable"> This parameter is ignored. </param>
            public void InitComponent(IAttributeTable attributeTable)
            {
            }

            #endregion
        }

        /// <summary>
        ///   Test implementation of a game system.
        /// </summary>
        private class TestSystem : GameSystem
        {
        }
    }
}