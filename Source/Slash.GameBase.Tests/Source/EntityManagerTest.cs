// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityManagerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Tests
{
    using System;

    using NUnit.Framework;

    using Slash.Collections.AttributeTables;

    /// <summary>
    ///   Unit tests for the EntityManager class.
    /// </summary>
    [TestFixture]
    public class EntityManagerTest
    {
        #region Fields

        /// <summary>
        ///   Test entity manager to run unit tests on.
        /// </summary>
        private EntityManager entityManager;

        /// <summary>
        ///   Test game to run unit tests on.
        /// </summary>
        private Game game;

        /// <summary>
        ///   Test component to run unit tests on.
        /// </summary>
        private TestEntityComponent testEntityComponent;

        private int testEntityId;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Setup for the tests of the EntityManager class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.entityManager = new EntityManager(this.game);
            this.testEntityComponent = new TestEntityComponent();
        }

        /// <summary>
        ///   Tests accessing an entity with an id that exceed the total number
        ///   of entities added to the manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAccessEntityWithExceedingId()
        {
            this.entityManager.GetComponent(42, typeof(TestEntityComponent));
        }

        /// <summary>
        ///   Tests accessing an entity with a negative id.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAccessEntityWithNegativeId()
        {
            this.entityManager.GetComponent(-1, typeof(TestEntityComponent));
        }

        /// <summary>
        ///   Tests accessing an entity after is has been removed from the
        ///   manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAccessRemovedEntity()
        {
            this.testEntityId = this.entityManager.CreateEntity();
            this.entityManager.RemoveEntity(this.testEntityId);
            this.entityManager.CleanUpEntities();
            this.entityManager.GetComponent(this.testEntityId, typeof(TestEntityComponent));
        }

        /// <summary>
        ///   Tests adding a component to an entity controlled by the entity
        ///   manager.
        /// </summary>
        [Test]
        public void TestAddComponent()
        {
            int entityId = this.entityManager.CreateEntity();
            this.entityManager.AddComponent(entityId, this.testEntityComponent);
            Assert.AreEqual(
                this.testEntityComponent, this.entityManager.GetComponent(entityId, typeof(TestEntityComponent)));
        }

        /// <summary>
        ///   Tests creating entities and checks the total number of entities
        ///   after.
        /// </summary>
        [Test]
        public void TestCreateEntity()
        {
            Assert.AreEqual(this.entityManager.CreateEntity(), 1);
            Assert.AreEqual(this.entityManager.CreateEntity(), 2);
            Assert.AreEqual(this.entityManager.EntityCount, 2);
        }

        /// <summary>
        ///   Tests adding and removing an entity and checking whether the
        ///   entity is alive before and after.
        /// </summary>
        [Test]
        public void TestEntityIsAlive()
        {
            int entityId = this.entityManager.CreateEntity();
            Assert.AreEqual(true, this.entityManager.EntityIsAlive(entityId));

            this.entityManager.RemoveEntity(entityId);
            this.entityManager.CleanUpEntities();
            Assert.AreEqual(false, this.entityManager.EntityIsAlive(entityId));
        }

        /// <summary>
        ///   Tests getting a component attached to an entity controlled by the
        ///   entity manager before it has been added.
        /// </summary>
        [Test]
        public void TestGetComponentBeforeAdding()
        {
            this.testEntityId = this.entityManager.CreateEntity();
            IEntityComponent entityComponent = this.entityManager.GetComponent(
                this.testEntityId, typeof(TestEntityComponent));
            Assert.IsNull(entityComponent);
        }

        /// <summary>
        ///   Tests getting a component of type null attached to an entity
        ///   controlled by the entity manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetComponentWithoutType()
        {
            int entityId = this.entityManager.CreateEntity();
            this.entityManager.AddComponent(entityId, this.testEntityComponent);
            this.entityManager.GetComponent(entityId, null);
        }

        /// <summary>
        ///   Tests removing a component from an entity controlled by the entity
        ///   manager.
        /// </summary>
        [Test]
        public void TestRemoveComponent()
        {
            int entityId = this.entityManager.CreateEntity();
            this.entityManager.AddComponent(entityId, this.testEntityComponent);
            Assert.IsTrue(this.entityManager.RemoveComponent(entityId, typeof(TestEntityComponent)));
            Assert.IsFalse(this.entityManager.RemoveComponent(entityId, typeof(TestEntityComponent)));
            Assert.IsNull(this.entityManager.GetComponent(entityId, typeof(TestEntityComponent)));
        }

        /// <summary>
        ///   Tests removing a component from an entity controlled by the entity
        ///   manager before it has been added.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveComponentBeforeAdding()
        {
            this.entityManager.CreateEntity();
            this.entityManager.RemoveComponent(0, typeof(TestEntityComponent));
        }

        /// <summary>
        ///   Tests removing a component of type null from an entity controlled
        ///   by the entity manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveComponentWithoutType()
        {
            int entityId = this.entityManager.CreateEntity();
            this.entityManager.AddComponent(entityId, this.testEntityComponent);
            this.entityManager.RemoveComponent(entityId, null);
        }

        /// <summary>
        ///   Tests removing an entity and checks the total number of entities
        ///   after.
        /// </summary>
        [Test]
        public void TestRemoveEntity()
        {
            int entityId = this.entityManager.CreateEntity();
            this.entityManager.RemoveEntity(entityId);
            this.entityManager.CleanUpEntities();

            Assert.AreEqual(this.entityManager.EntityCount, 0);
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
    }
}