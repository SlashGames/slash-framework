// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessManagerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests
{
    using NUnit.Framework;

    using Slash.Application.Games;
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.ECS.Processes;

    public class ProcessManagerTest
    {
        #region Fields

        private Game game;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.game = new Game();
            this.game.StartGame();
        }

        [Test]
        public void TestActiveWhenStarted()
        {
            // Arrange.
            WaitProcess process = new WaitProcess(3.0f);

            // Act.
            this.game.ProcessManager.AddProcess(process);

            // Assert.
            Assert.IsTrue(process.Active);
        }

        [Test]
        public void TestAliveWhenStarted()
        {
            // Arrange.
            TestProcess process = new TestProcess();

            // Assert.
            Assert.IsFalse(process.Dead);
        }

        [Test]
        public void TestDeadAfterKilled()
        {
            // Arrange.
            TestProcess process = new TestProcess();

            // Act.
            process.Kill();

            // Assert.
            Assert.IsTrue(process.Dead);
        }

        [Test]
        public void TestDontStartNextProcessWhileAlive()
        {
            // Arrange.
            TestProcess first = new TestProcess();
            TestProcess second = new TestProcess();
            first.Next = second;

            // Act.
            this.game.ProcessManager.AddProcess(first);
            this.game.Update(1.0f);

            // Assert.
            Assert.IsTrue(first.Active);
            Assert.IsFalse(second.Active);
        }

        [Test]
        public void TestDontUpdateDeadProcess()
        {
            // Arrange.
            WaitProcess process = new WaitProcess(3.0f);
            process.Kill();
            this.game.ProcessManager.AddProcess(process);

            // Act.
            this.game.Update(1.0f);

            // Assert.
            Assert.AreEqual(0.0f, process.TimeElapsed);
        }

        [Test]
        public void TestDontUpdatePausedProcess()
        {
            // Arrange.
            WaitProcess process = new WaitProcess(3.0f);
            process.TogglePause();
            this.game.ProcessManager.AddProcess(process);

            // Act.
            this.game.Update(1.0f);

            // Assert.
            Assert.AreEqual(0.0f, process.TimeElapsed);
        }

        [Test]
        public void TestInactiveBeforeStarted()
        {
            // Arrange.
            WaitProcess process = new WaitProcess(3.0f);

            // Assert.
            Assert.IsFalse(process.Active);
        }

        [Test]
        public void TestInitializedWhenStarted()
        {
            // Arrange.
            TestProcess process = new TestProcess();

            // Act.
            this.game.ProcessManager.AddProcess(process);

            // Assert.
            Assert.IsTrue(process.Initialized);
        }

        [Test]
        public void TestNotPausedWhenStarted()
        {
            // Arrange.
            TestProcess process = new TestProcess();

            // Assert.
            Assert.IsFalse(process.Paused);
        }

        [Test]
        public void TestProcessThen()
        {
            // Arrange.
            TestProcess first = new TestProcess();
            TestProcess second = new TestProcess();
            TestProcess third = new TestProcess();

            // Act.
            first.Then(second).Then(third);

            // Assert.
            Assert.AreEqual(second, first.Next);
            Assert.AreEqual(third, second.Next);
        }

        [Test]
        public void TestStartNextProcess()
        {
            // Arrange.
            TestProcess first = new TestProcess();
            TestProcess second = new TestProcess();
            first.Next = second;
            first.Kill();

            // Act.
            this.game.ProcessManager.AddProcess(first);
            this.game.Update(1.0f);

            // Assert.
            Assert.IsTrue(first.Dead);
            Assert.IsTrue(second.Active);
        }

        [Test]
        public void TestTogglePause()
        {
            // Arrange.
            TestProcess process = new TestProcess();

            // Act.
            process.TogglePause();

            // Assert.
            Assert.IsTrue(process.Paused);

            // Act.
            process.TogglePause();

            // Assert.
            Assert.IsFalse(process.Paused);
        }

        [Test]
        public void TestUninitializedBeforeStarted()
        {
            // Arrange.
            TestProcess process = new TestProcess();

            // Assert.
            Assert.IsFalse(process.Initialized);
        }

        [Test]
        public void TestUpdateProcess()
        {
            // Arrange.
            WaitProcess process = new WaitProcess(3.0f);
            this.game.ProcessManager.AddProcess(process);

            // Act.
            this.game.Update(1.0f);

            // Assert.
            Assert.AreEqual(1.0f, process.TimeElapsed);
        }

        [Test]
        public void TestWaitProcessAliveBeforeExpired()
        {
            // Arrange.
            WaitProcess process = new WaitProcess(3.0f);
            this.game.ProcessManager.AddProcess(process);

            // Act.
            this.game.Update(1.0f);

            // Assert.
            Assert.IsFalse(process.Dead);
        }

        [Test]
        public void TestWaitProcessDeadAfterExpired()
        {
            // Arrange.
            WaitProcess process = new WaitProcess(3.0f);
            this.game.ProcessManager.AddProcess(process);

            // Act.
            this.game.Update(3.0f);

            // Assert.
            Assert.IsTrue(process.Dead);
        }

        #endregion

        private class TestProcess : GameProcess
        {
            #region Public Properties

            public bool Initialized { get; private set; }

            #endregion

            #region Public Methods and Operators

            public override void InitProcess(EntityManager entityManager, EventManager eventManager)
            {
                base.InitProcess(entityManager, eventManager);

                this.Initialized = true;
            }

            #endregion
        }
    }
}