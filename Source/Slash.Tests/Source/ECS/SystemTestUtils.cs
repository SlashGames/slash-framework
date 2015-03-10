// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemTestUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tests.ECS
{
    using System;

    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.ECS;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.ECS.Systems;

    public static class SystemTestUtils
    {
        #region Public Methods and Operators

        public static Game CreateGameWithSystem<T>() where T : ISystem, new()
        {
            return CreateGameWithSystem<T>(new AttributeTable());
        }

        public static Game CreateGameWithSystem<T>(IAttributeTable configuration) where T : ISystem, new()
        {
            var game = new Game { BlueprintManager = new BlueprintManager(), AddSystemsViaReflection = false };
            game.AddSystem<T>();

            // Write logs to console.
            game.Log.InfoLogged += Console.WriteLine;
            game.Log.WarningLogged += Console.WriteLine;
            game.Log.ErrorLogged += Console.WriteLine;

            // Start game.
            game.StartGame(configuration);
            game.Update(0);

            return game;
        }

        public static EventManager CreateSystem<T>() where T : ISystem, new()
        {
            EventManager eventManager;
            CreateSystem<T>(out eventManager);
            return eventManager;
        }

        public static void CreateSystem<T>(out EventManager eventManager) where T : ISystem, new()
        {
            CreateSystem<T>(out eventManager, new AttributeTable());
        }

        public static void CreateSystem<T>(out EventManager eventManager, out EntityManager entityManager)
            where T : ISystem, new()
        {
            CreateSystem<T>(out eventManager, out entityManager, new AttributeTable());
        }

        public static void CreateSystem<T>(out EventManager eventManager, IAttributeTable configuration)
            where T : ISystem, new()
        {
            EntityManager tmp;
            CreateSystem<T>(out eventManager, out tmp, configuration);
        }

        public static void CreateSystem<T>(
            out EventManager eventManager, out EntityManager entityManager, IAttributeTable configuration)
            where T : ISystem, new()
        {
            var game = CreateGameWithSystem<T>(configuration);

            eventManager = game.EventManager;
            entityManager = game.EntityManager;
        }

        public static void ExpectEventAfter(
            EventManager eventManager,
            GameEvent triggerEvent,
            object expectedEvent,
            Func<GameEvent, bool> checkEvent = null)
        {
            ExpectEventAfter(true, eventManager, triggerEvent, expectedEvent, checkEvent);
        }

        public static void ExpectEventAfter(
            Game game, float dt, object expectedEvent, Func<GameEvent, bool> checkEvent = null)
        {
            ExpectEventAfter(true, game, dt, expectedEvent, checkEvent);
        }

        public static void NotExpectEventAfter(
            Game game, float dt, object expectedEvent, Func<GameEvent, bool> checkEvent = null)
        {
            ExpectEventAfter(false, game, dt, expectedEvent, checkEvent);
        }

        public static void NotExpectEventAfter(
            EventManager eventManager,
            GameEvent triggerEvent,
            object expectedEvent,
            Func<GameEvent, bool> checkEvent = null)
        {
            ExpectEventAfter(false, eventManager, triggerEvent, expectedEvent, checkEvent);
        }

        #endregion

        #region Methods

        private static void AssertEventOccured(bool expected, bool eventOccured, object expectedEvent)
        {
            if (eventOccured != expected)
            {
                Assert.Fail(
                    expected
                        ? "Expected event {0}, but that event wasn't raised."
                        : "Didn't expect event {0}, but that event was raised.",
                    expectedEvent);
            }
        }

        private static void ExpectEventAfter(
            bool expected, Game game, float dt, object expectedEvent, Func<GameEvent, bool> checkEvent = null)
        {
            // Register for event.
            bool eventOccured = false;
            game.EventManager.RegisterListener(
                expectedEvent,
                e =>
                    {
                        if (checkEvent != null && !checkEvent(e))
                        {
                            return;
                        }
                        eventOccured = true;
                    });

            // Trigger event.
            game.Update(dt);

            // Check if event occured.
            AssertEventOccured(expected, eventOccured, expectedEvent);
        }

        private static void ExpectEventAfter(
            bool expected,
            EventManager eventManager,
            GameEvent triggerEvent,
            object expectedEvent,
            Func<GameEvent, bool> checkEvent = null)
        {
            // Register for event.
            bool eventOccured = false;
            eventManager.RegisterListener(
                expectedEvent,
                e =>
                    {
                        if (checkEvent != null && !checkEvent(e))
                        {
                            return;
                        }
                        eventOccured = true;
                    });

            // Trigger event.
            eventManager.QueueEvent(triggerEvent);

            // Process events.
            eventManager.ProcessEvents();

            // Check if event occured.
            AssertEventOccured(expected, eventOccured, expectedEvent);
        }

        #endregion
    }
}