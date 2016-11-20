// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemManagerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests
{
    using System;

    using NUnit.Framework;

    using Slash.ECS.Systems;

    /// <summary>
    ///   Unit tests for the SystemManager class.
    /// </summary>
    [TestFixture]
    public class SystemManagerTest
    {
        #region Fields

        /// <summary>
        ///   Test game to run unit tests on.
        /// </summary>
        private Game game;

        /// <summary>
        ///   Test system to run unit tests on.
        /// </summary>
        private TestSystem system;

        /// <summary>
        ///   Test system manager to run unit tests on.
        /// </summary>
        private SystemManager systemManager;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Setup for the tests of the SystemManager class.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.systemManager = new SystemManager(this.game);
            this.system = new TestSystem();
        }

        /// <summary>
        ///   Tests adding null as system to the system manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullSystem()
        {
            this.systemManager.AddSystem(null);
        }

        /// <summary>
        ///   Tests adding a system to the system manager.
        /// </summary>
        [Test]
        public void TestAddSystem()
        {
            this.systemManager.AddSystem(this.system);
            Assert.AreEqual(this.system, this.systemManager.GetSystem(typeof(TestSystem)));
        }

        /// <summary>
        ///   Tests adding a system twice to the system manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddSystemTwice()
        {
            this.systemManager.AddSystem(this.system);
            this.systemManager.AddSystem(this.system);
        }

        /// <summary>
        ///   Tests fetching a system from the system manager before it has been
        ///   added.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetSystemBeforeAdding()
        {
            this.systemManager.GetSystem(typeof(TestSystem));
        }

        /// <summary>
        ///   Tests fetching a system of type null from the system manager.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetSystemWithoutType()
        {
            this.systemManager.GetSystem(null);
        }

        /// <summary>
        ///   Tests updating a system after it has been added to the system manager.
        /// </summary>
        [Test]
        public void TestUpdateSystems()
        {
            this.systemManager.AddSystem(this.system);
            this.systemManager.Update(1.0f);

            Assert.IsTrue(this.system.Updated);
        }

        #endregion

        /// <summary>
        ///   Test implementation of a game system. Remebers whether it has
        ///   been updated any time, or not.
        /// </summary>
        private class TestSystem : GameSystem
        {
            #region Public Properties

            /// <summary>
            ///   Whether this system has been updated any time.
            /// </summary>
            public bool Updated { get; private set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///   Ticks this system.
            /// </summary>
            /// <param name="dt">
            ///   Time passed since the last tick, in seconds.
            /// </param>
            public override void Update(float dt)
            {
                this.Updated = true;
            }

            #endregion
        }
    }
}