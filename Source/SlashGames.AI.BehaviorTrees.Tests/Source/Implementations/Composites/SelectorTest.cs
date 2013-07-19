// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Tests.Implementations.Composites
{
    using NUnit.Framework;

    using Slash.AI.BehaviorTrees.Editor;
    using Slash.AI.BehaviorTrees.Implementations.Composites;

    public class SelectorTest
    {
        #region Public Methods and Operators

        [TestFixtureSetUp]
        public void SetUp()
        {
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestGenerateTaskDescription()
        {
            TaskDescription taskDescription = TaskDescription.Generate<Selector>();
            Assert.IsNotNull(taskDescription);
        }

        #endregion
    }
}