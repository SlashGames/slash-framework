// -----------------------------------------------------------------------
// <copyright file="ComponentManagerTest.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.Tests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the ComponentManagerTest class.
    /// </summary>
    [TestFixture]
    public class ComponentManagerTest
    {
        #region Constants and Fields

        /// <summary>
        /// Test game to run unit tests on.
        /// </summary>
        private Game game;

        /// <summary>
        /// Test component manager to run unit tests on.
        /// </summary>
        private ComponentManager componentManager;

        /// <summary>
        /// Test component to run unit tests on.
        /// </summary>
        private TestComponent testComponent;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup for the tests of the ComponentManagerTest class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.componentManager = new ComponentManager(this.game);
            this.testComponent = new TestComponent();
        }

        /// <summary>
        /// Tests adding a component to the component manager.
        /// </summary>
        [Test]
        public void TestAddComponent()
        {
            this.componentManager.AddComponent(0L, this.testComponent);
            Assert.AreEqual(this.testComponent, this.componentManager.GetComponent(0L));
        }

        /// <summary>
        /// Tests adding null as a component to the component manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullComponent()
        {
            this.componentManager.AddComponent(0L, null);
        }

        /// <summary>
        /// Tests adding a component twice to the component manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddComponentTwice()
        {
            this.componentManager.AddComponent(0L, this.testComponent);
            this.componentManager.AddComponent(0L, this.testComponent);
        }

        /// <summary>
        /// Tests removing a component from the component manager.
        /// </summary>
        [Test]
        public void TestRemoveComponent()
        {
            this.componentManager.AddComponent(0L, this.testComponent);
            Assert.IsTrue(this.componentManager.RemoveComponent(0L));
            Assert.IsFalse(this.componentManager.RemoveComponent(0L));
            Assert.IsNull(this.componentManager.GetComponent(0L));
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
