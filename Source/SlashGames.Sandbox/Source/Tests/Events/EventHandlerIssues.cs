// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventHandlerIssues.cs" company="Slash Games ">
//   Copyright (c) Slash Games . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.Sandbox.Tests.Events
{
    using System;

    using NUnit.Framework;

    public class DummyEventProvider
    {
        #region Constructors and Destructors

        public DummyEventProvider()
        {
            ++InstanceCount;
        }

        ~DummyEventProvider()
        {
            --InstanceCount;
        }

        #endregion

        #region Delegates

        public delegate void TestEventDelegate();

        #endregion

        #region Public Events

        public event TestEventDelegate TestEvent;

        #endregion

        #region Public Properties

        public static int InstanceCount { get; set; }

        #endregion

        #region Public Methods and Operators

        public void OnTestEvent()
        {
            TestEventDelegate handler = this.TestEvent;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }

    public class DummyEventHandler
    {
        #region Static Fields

        public static int InstanceCount;

        #endregion

        #region Constructors and Destructors

        public DummyEventHandler()
        {
            ++InstanceCount;
        }

        ~DummyEventHandler()
        {
            --InstanceCount;
        }

        #endregion

        #region Public Methods and Operators

        public void OnTestEvent()
        {
        }

        #endregion
    }

    public class EventHandlerIssues
    {
        #region Public Methods and Operators

        [Test]
        public void MemoryLeakWhenNotRemovingEventHandler()
        {
            DummyEventProvider eventProvider = new DummyEventProvider();
            DummyEventHandler eventHandler = new DummyEventHandler();

            // Register for event.
            eventProvider.TestEvent += eventHandler.OnTestEvent;

            // Don't unregister from event.

            // Release object.
            eventHandler = null;

            // Event handler is not garbage collected.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(1, DummyEventHandler.InstanceCount);

            // Release provider.
            eventProvider = null;

            // Event handler is garbage collected.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(0, DummyEventHandler.InstanceCount);
        }

        [Test]
        public void NoEventProviderMemoryLeakWhenNotRemovingEventHandler()
        {
            DummyEventProvider eventProvider = new DummyEventProvider();
            DummyEventHandler eventHandler = new DummyEventHandler();

            // Register for event.
            eventProvider.TestEvent += eventHandler.OnTestEvent;

            // Don't unregister from event.

            // Release object.
            eventHandler = null;

            // Release provider.
            eventProvider = null;

            // Event handler is not garbage collected.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(0, DummyEventHandler.InstanceCount);
            Assert.AreEqual(0, DummyEventProvider.InstanceCount);
        }

        [Test]
        public void NoMemoryLeakWhenRemovingEventHandler()
        {
            DummyEventProvider eventProvider = new DummyEventProvider();
            DummyEventHandler eventHandler = new DummyEventHandler();

            // Register for event.
            eventProvider.TestEvent += eventHandler.OnTestEvent;

            // Unregister from event.
            eventProvider.TestEvent -= eventHandler.OnTestEvent;

            // Release object.
            eventHandler = null;

            // Event handler is not garbage collected.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Assert.AreEqual(0, DummyEventHandler.InstanceCount);
        }

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion
    }
}