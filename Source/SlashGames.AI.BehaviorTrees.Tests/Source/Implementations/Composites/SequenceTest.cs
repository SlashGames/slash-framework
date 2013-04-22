// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SequenceTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.AI.BehaviorTrees.Tests.Implementations.Composites
{
    using NUnit.Framework;

    using SlashGames.AI.BehaviorTrees.Editor;
    using SlashGames.AI.BehaviorTrees.Implementations.Composites;

    public class SequenceTest
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
            TaskDescription taskDescription = TaskDescription.Generate<Sequence>();
            Assert.IsNotNull(taskDescription);
        }

        #endregion
    }
}