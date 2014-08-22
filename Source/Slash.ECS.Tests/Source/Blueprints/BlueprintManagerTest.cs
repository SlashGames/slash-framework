// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintManagerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests.Blueprints
{
    using NUnit.Framework;

    using Slash.ECS.Blueprints;
    using Slash.Tests.Utils;

    public class BlueprintManagerTest
    {
        #region Fields

        private Blueprint blueprint;

        private string blueprintId = "TestBlueprint";

        private BlueprintManager blueprintManager;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.blueprintManager = new BlueprintManager();
            this.blueprint = new Blueprint();
            this.blueprintManager.AddBlueprint(this.blueprintId, this.blueprint);
        }

        [Test]
        public void TestSerialization()
        {
            SerializationTestUtils.TestXmlSerialization(this.blueprintManager);
        }

        #endregion
    }
}