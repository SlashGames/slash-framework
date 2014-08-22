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
        private BlueprintManager blueprintManager;

        private string blueprintId = "TestBlueprint";

        private Blueprint blueprint;

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
    }
}