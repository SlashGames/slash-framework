// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintManagerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.GameBase.Tests.Blueprints
{
    using System.IO;
    using System.Xml.Serialization;

    using NUnit.Framework;

    using Slash.GameBase.Blueprints;
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
            this.blueprintManager.AddBlueprint(blueprintId, blueprint);
        }

        [Test]
        public void TestSerialization()
        {
            SerializationTestUtils.TestXmlSerialization(this.blueprintManager);
        }
    }
}