// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompoundEntitiesTests.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests.Systems
{
    using NUnit.Framework;

    using Slash.Application.Systems;
    using Slash.ECS.Components;
    using Slash.ECS.Events;

    public class CompoundEntitiesTests
    {
        #region Public Methods and Operators

        [Test]
        public void TestEntityRemovedWhenRemovingWholeEntity()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            var compoundEntities = new CompoundEntities<TestCompound>(entityManager);
            var eventCount = 0;
            compoundEntities.EntityRemoved += (id, entity) => { ++eventCount; };

            // Add entity with required components.
            var entityId = entityManager.CreateEntity();
            entityManager.InitEntity(
                entityId,
                new IEntityComponent[] { new TestCompoundComponentA(), new TestCompoundComponentB() });

            // Now remove entity.
            entityManager.RemoveEntity(entityId);
            entityManager.CleanUpEntities();

            Assert.AreEqual(1, eventCount);
        }

        [Test]
        public void TestInitialize()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            new CompoundEntities<TestCompound>(entityManager);
        }

        [Test]
        public void TestInitializeWithComponentField()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            new CompoundEntities<TestCompoundWithField>(entityManager);
        }

        [Test]
        public void TestInvalidEntityAdded()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            var compoundEntities = new CompoundEntities<TestCompound>(entityManager);
            var entityAdded = false;
            compoundEntities.EntityAdded += (id, entity) => { entityAdded = true; };

            // Just add one of the necessary components.
            var entityId = entityManager.CreateEntity();
            entityManager.InitEntity(entityId, new IEntityComponent[] { new TestCompoundComponentA() });

            Assert.IsFalse(entityAdded);
        }

        [Test]
        public void TestValidEntityAdded()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            var compoundEntities = new CompoundEntities<TestCompound>(entityManager);
            var entityAdded = false;
            compoundEntities.EntityAdded += (id, entity) => { entityAdded = true; };

            // Add entity with correct components.
            var entityId = entityManager.CreateEntity();
            entityManager.InitEntity(
                entityId,
                new IEntityComponent[] { new TestCompoundComponentA(), new TestCompoundComponentB() });

            Assert.IsTrue(entityAdded);
        }

        [Test]
        public void TestValidEntityAddedWithComponentField()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            var compoundEntities = new CompoundEntities<TestCompoundWithField>(entityManager);
            var entityAdded = false;
            compoundEntities.EntityAdded += (id, entity) => { entityAdded = true; };

            // Add entity with correct components.
            var entityId = entityManager.CreateEntity();
            entityManager.InitEntity(entityId, new IEntityComponent[] { new TestCompoundComponentA() });

            Assert.IsTrue(entityAdded);
        }

        [Test]
        public void TestValidEntityWithOptionalCompAdded()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            var compoundEntities = new CompoundEntities<TestCompoundWithOneOptionalComp>(entityManager);
            var entityAdded = false;
            compoundEntities.EntityAdded += (id, entity) => { entityAdded = true; };

            // Just add one of the components which is the necessary one.
            var entityId = entityManager.CreateEntity();
            entityManager.InitEntity(entityId, new IEntityComponent[] { new TestCompoundComponentA() });

            Assert.IsTrue(entityAdded);
        }

        [Test]
        public void TestValidEntityWithOptionalCompAddedTriggerEventOnlyOnce()
        {
            var entityManager = new EntityManager(new EventManager());

            // Create compound entities.
            var compoundEntities = new CompoundEntities<TestCompoundWithOneOptionalComp>(entityManager);
            var entityAddedEvent = 0;
            compoundEntities.EntityAdded += (id, entity) => { ++entityAddedEvent; };

            // Add required and optional component.
            var entityId = entityManager.CreateEntity();
            entityManager.InitEntity(
                entityId,
                new IEntityComponent[] { new TestCompoundComponentA(), new TestCompoundComponentB() });

            Assert.AreEqual(1, entityAddedEvent);
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

        private class TestCompoundWithField
        {
            #region Fields

            [CompoundComponent]
            public TestCompoundComponentA ComponentA;

            #endregion
        }
    }
}