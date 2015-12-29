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

        /// <summary>
        ///   Test component to run unit tests on.
        /// </summary>
        private TestEntityComponent testEntityComponent;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Setup for the tests of the ComponentManagerTest class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.componentManager = new ComponentManager();
            this.testEntityComponent = new TestEntityComponent();
        }

        /// <summary>
        ///   Tests adding a component to the component manager.
        /// </summary>
        [Test]
        public void TestAddComponent()
        {
            this.componentManager.AddComponent(0, this.testEntityComponent);
            Assert.AreEqual(this.testEntityComponent, this.componentManager.GetComponent(0));
        }

        /// <summary>
        ///   Tests adding a component twice to the component manager.
        /// </summary>
        [Test]
        public void TestAddComponentTwice()
        {
            this.componentManager.AddComponent(0, this.testEntityComponent);
            Assert.Throws<InvalidOperationException>(() => this.componentManager.AddComponent(0, this.testEntityComponent));
        }

        /// <summary>
        ///   Tests adding null as a component to the component manager.
        /// </summary>
        [Test]
        public void TestAddNullComponent()
        {
            Assert.Throws<ArgumentNullException>(() => this.componentManager.AddComponent(0, null));
        }

        /// <summary>
        ///   Tests removing a component from the component manager.
        /// </summary>
        [Test]
        public void TestRemoveComponent()
        {
            this.componentManager.AddComponent(0, this.testEntityComponent);
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