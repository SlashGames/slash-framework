// -----------------------------------------------------------------------
// <copyright file="EntityManagerTest.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.Tests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the EntityManager class.
    /// </summary>
    [TestFixture]
    public class EntityManagerTest
    {
        #region Constants and Fields

        /// <summary>
        /// Test game to run unit tests on.
        /// </summary>
        private Game game;

        /// <summary>
        /// Test entity manager to run unit tests on.
        /// </summary>
        private EntityManager entityManager;

        /// <summary>
        /// Test component to run unit tests on.
        /// </summary>
        private TestComponent testComponent;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup for the tests of the EntityManager class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.entityManager = new EntityManager(this.game);
            this.testComponent = new TestComponent();
        }

        /// <summary>
        /// Tests creating entities and checks the total number of entities
        /// after.
        /// </summary>
        [Test]
        public void TestCreateEntity()
        {
            Assert.AreEqual(this.entityManager.CreateEntity(), 0L);
            Assert.AreEqual(this.entityManager.CreateEntity(), 1L);
            Assert.AreEqual(this.entityManager.EntityCount, 2L);
        }

        /// <summary>
        /// Tests removing an entity and checks the total number of entities
        /// after.
        /// </summary>
        [Test]
        public void TestRemoveEntity()
        {
            this.entityManager.CreateEntity();
            this.entityManager.RemoveEntity(0L);
            this.entityManager.CleanUpEntities();

            Assert.AreEqual(this.entityManager.EntityCount, 0L);
        }

        /// <summary>
        /// Tests adding and removing an entity and checking whether the
        /// entity is alive before and after.
        /// </summary>
        [Test]
        public void TestEntityIsAlive()
        {
            this.entityManager.CreateEntity();
            Assert.AreEqual(true, this.entityManager.EntityIsAlive(0L));

            this.entityManager.RemoveEntity(0L);
            this.entityManager.CleanUpEntities();
            Assert.AreEqual(false, this.entityManager.EntityIsAlive(0L));
        }

        /// <summary>
        /// Tests adding a component to an entity controlled by the entity
        /// manager.
        /// </summary>
        [Test]
        public void TestAddComponent()
        {
            this.entityManager.CreateEntity();
            this.entityManager.AddComponent(0L, this.testComponent);
            Assert.AreEqual(this.testComponent, this.entityManager.GetComponent(0L, typeof(TestComponent)));
        }

        /// <summary>
        /// Tests removing a component from an entity controlled by the entity
        /// manager.
        /// </summary>
        [Test]
        public void TestRemoveComponent()
        {
            this.entityManager.CreateEntity();
            this.entityManager.AddComponent(0L, this.testComponent);
            Assert.IsTrue(this.entityManager.RemoveComponent(0L, typeof(TestComponent)));
            Assert.IsFalse(this.entityManager.RemoveComponent(0L, typeof(TestComponent)));
            Assert.IsNull(this.entityManager.GetComponent(0L, typeof(TestComponent)));
        }

        /// <summary>
        /// Tests removing a component of type null from an entity controlled
        /// by the entity manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveComponentWithoutType()
        {
            this.entityManager.CreateEntity();
            this.entityManager.AddComponent(0L, this.testComponent);
            this.entityManager.RemoveComponent(0L, null);
        }

        /// <summary>
        /// Tests removing a component from an entity controlled by the entity
        /// manager before it has been added.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveComponentBeforeAdding()
        {
            this.entityManager.CreateEntity();
            this.entityManager.RemoveComponent(0L, typeof(TestComponent));
        }

        /// <summary>
        /// Tests getting a component of type null attached to an entity
        /// controlled by the entity manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetComponentWithoutType()
        {
            this.entityManager.CreateEntity();
            this.entityManager.AddComponent(0L, this.testComponent);
            this.entityManager.GetComponent(0L, null);
        }

        /// <summary>
        /// Tests getting a component attached to an entity controlled by the
        /// entity manager before it has been added.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetComponentBeforeAdding()
        {
            this.entityManager.CreateEntity();
            this.entityManager.GetComponent(0L, typeof(TestComponent));
        }

        /// <summary>
        /// Tests accessing an entity with a negative id.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAccessEntityWithNegativeId()
        {
            this.entityManager.GetComponent(-1L, typeof(TestComponent));
        }

        /// <summary>
        /// Tests accessing an entity with an id that exceed the total number
        /// of entities added to the manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAccessEntityWithExceedingId()
        {
            this.entityManager.GetComponent(42L, typeof(TestComponent));
        }

        /// <summary>
        /// Tests accessing an entity after is has been removed from the
        /// manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAccessRemovedEntity()
        {
            this.entityManager.CreateEntity();
            this.entityManager.RemoveEntity(0L);
            this.entityManager.GetComponent(0L, typeof(TestComponent));
        }

        #endregion

        /// <summary>
        /// Test implementation of a game component.
        /// </summary>
        private class TestComponent : IComponent
        {
        }
    }
}
