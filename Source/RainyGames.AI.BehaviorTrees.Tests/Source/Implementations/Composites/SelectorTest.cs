// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorTest.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.AI.BehaviorTrees.Tests.Implementations.Composites
{
    using NUnit.Framework;

    using RainyGames.AI.BehaviorTrees.Editor;
    using RainyGames.AI.BehaviorTrees.Implementations.Composites;

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