// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompoundEntitiesTests.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests.Systems
{
    using NUnit.Framework;

    using Slash.ECS.Components;
    using Slash.ECS.Source.Systems;

    public class CompoundEntitiesTests
    {
        #region Public Methods and Operators

        [Test]
        public void TestInitialize()
        {
            Game game = new Game();
            EntityManager entityManager = new EntityManager(game);
            // Create compound entities.
            new CompoundEntities<TestCompound>(entityManager);
        }

        [Test]
        public void TestInvalidEntityAdded()
        {
            Game game = new Game();
            EntityManager entityManager = new EntityManager(game);

            // Create compound entities.
            CompoundEntities<TestCompound> compoundEntities = new CompoundEntities<TestCompound>(entityManager);
            bool entityAdded = false;
            compoundEntities.EntityAdded += (id, entity) => { entityAdded = true; };

            // Just add one of the necessary components.
            var entityId = entityManager.CreateEntity();
            entityManager.AddComponent<TestCompoundComponentA>(entityId);

            Assert.IsFalse(entityAdded);
        }

        [Test]
        public void TestValidEntityAdded()
        {
            Game game = new Game();
            EntityManager entityManager = new EntityManager(game);

            // Create compound entities.
            CompoundEntities<TestCompound> compoundEntities = new CompoundEntities<TestCompound>(entityManager);
            bool entityAdded = false;
            compoundEntities.EntityAdded += (id, entity) => { entityAdded = true; };

            // Add entity with correct components.
            var entityId = entityManager.CreateEntity();
            entityManager.AddComponent<TestCompoundComponentA>(entityId);
            entityManager.AddComponent<TestCompoundComponentB>(entityId);

            Assert.IsTrue(entityAdded);
        }

        [Test]
        public void TestValidEntityWithOptionalCompAdded()
        {
            Game game = new Game();
            EntityManager entityManager = new EntityManager(game);

            // Create compound entities.
            CompoundEntities<TestCompoundWithOneOptionalComp> compoundEntities =
                new CompoundEntities<TestCompoundWithOneOptionalComp>(entityManager);
            bool entityAdded = false;
            compoundEntities.EntityAdded += (id, entity) => { entityAdded = true; };

            // Just add one of the components which is the necessary one.
            var entityId = entityManager.CreateEntity();
            entityManager.AddComponent<TestCompoundComponentA>(entityId);

            Assert.IsTrue(entityAdded);
        }

        [Test]
        public void TestValidEntityWithOptionalCompAddedTriggerEventOnlyOnce()
        {
            Game game = new Game();
            EntityManager entityManager = new EntityManager(game);

            // Create compound entities.
            CompoundEntities<TestCompoundWithOneOptionalComp> compoundEntities =
                new CompoundEntities<TestCompoundWithOneOptionalComp>(entityManager);
            int entityAddedEvent = 0;
            compoundEntities.EntityAdded += (id, entity) => { ++entityAddedEvent; };

            // Just add one of the components which is the necessary one.
            var entityId = entityManager.CreateEntity();
            entityManager.AddComponent<TestCompoundComponentA>(entityId);
            entityManager.AddComponent<TestCompoundComponentB>(entityId);

            Assert.AreEqual(1, entityAddedEvent);
        }

        [Test]
        public void TestEntityRemovedWhenRemovingWholeEntity()
        {
            Game game = new Game();
            EntityManager entityManager = new EntityManager(game);

            // Create compound entities.
            CompoundEntities<TestCompound> compoundEntities =
                new CompoundEntities<TestCompound>(entityManager);
            int eventCount = 0;
            compoundEntities.EntityRemoved += (id, entity) => { ++eventCount; };

            // Add entity with required components.
            var entityId = entityManager.CreateEntity();
            entityManager.AddComponent<TestCompoundComponentA>(entityId);
            entityManager.AddComponent<TestCompoundComponentB>(entityId);

            // Now remove entity.
            entityManager.RemoveEntity(entityId);
            entityManager.CleanUpEntities();

            Assert.AreEqual(1, eventCount);
        }

        #endregion

        private class TestCompoundComponentA : EntityComponent
        {
        }

        private class TestCompoundComponentB : EntityComponent
        {
        }

        private class TestCompound
        {
            #region Properties

            [CompoundComponent]
            public TestCompoundComponentA ComponentA { get; set; }

            [CompoundComponent]
            public TestCompoundComponentB ComponentB { get; set; }

            #endregion
        }

        private class TestCompoundWithOneOptionalComp
        {
            #region Properties

            [CompoundComponent]
            public TestCompoundComponentA ComponentA { get; set; }

            [CompoundComponent(IsOptional = true)]
            public TestCompoundComponentB ComponentB { get; set; }

            #endregion
        }
    }
}