// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentManagerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests
{
    using System;

    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Components;

    /// <summary>
    ///   Unit tests for the ComponentManagerTest class.
    /// </summary>
    [TestFixture]
    public class ComponentManagerTest
    {
        #region Fields

        /// <summary>
        ///   Test component manager to run unit tests on.
        /// </summary>
        private ComponentManager componentManager;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Setup for the tests of the ComponentManagerTest class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.componentManager = new ComponentManager(typeof(TestEntityComponent));
        }

        /// <summary>
        ///   Tests adding a component to the component manager.
        /// </summary>
        [Test]
        public void TestAddComponent()
        {
            var entityComponent = this.componentManager.AddComponent(0);
            Assert.AreEqual(entityComponent, this.componentManager.GetComponent(0));
        }

        /// <summary>
        ///   Tests adding a component twice to the component manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddComponentTwice()
        {
            this.componentManager.AddComponent(0);
            this.componentManager.AddComponent(0);
        }

        /// <summary>
        ///   Tests adding null as a component to the component manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullComponent()
        {
            this.componentManager.AddComponent(0, null);
        }

        /// <summary>
        ///   Tests reusing component.
        /// </summary>
        [Test]
        public void TestCreateComponent()
        {
            // Add to entity.
            int entityIdA = 1;
            var entityComponentA = this.componentManager.AddComponent(entityIdA);

            // Remove from entity.
            bool removed = this.componentManager.RemoveComponent(entityIdA);
            Assert.IsTrue(removed);

            // Add to new entity.
            var entityComponentB = this.componentManager.CreateComponent();
            Assert.AreEqual(entityComponentA, entityComponentB);
        }

        /// <summary>
        ///   Tests removing a component from the component manager.
        /// </summary>
        [Test]
        public void TestRemoveComponent()
        {
            this.componentManager.AddComponent(0);
            Assert.IsTrue(this.componentManager.RemoveComponent(0));
            Assert.IsFalse(this.componentManager.RemoveComponent(0));
            Assert.IsNull(this.componentManager.GetComponent(0));
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
            /// <param name="attributeTable">This parameter is ignored.</param>
            public void InitComponent(IAttributeTable attributeTable)
            {
            }

            #endregion
        }
    }
}