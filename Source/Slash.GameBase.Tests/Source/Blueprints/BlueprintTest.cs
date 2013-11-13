// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Tests.Blueprints
{
    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Blueprints;
    using Slash.GameBase.Components;
    using Slash.Tests.Utils;

    public class BlueprintTest
    {
        #region Constants

        private const string AttributeKey = "AttributeKey";

        private const int AttributeValue = 123;

        #endregion

        #region Fields

        private Blueprint blueprint;

        private string parentId = "ParentBlueprint";

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.blueprint = new Blueprint { ParentId = this.parentId };
        }

        [Test]
        public void TestComponentTypesSerialization()
        {
            this.blueprint.ComponentTypes.Add(typeof(TestComponent));
            SerializationTestUtils.TestXmlSerialization(this.blueprint);
        }

        [Test]
        public void TestAttributeTableSerialization()
        {
            this.blueprint.AttributeTable.Add(AttributeKey, AttributeValue);
            SerializationTestUtils.TestXmlSerialization(this.blueprint);
        }

        #endregion
    }

    public class TestComponent : IEntityComponent
    {
        #region Public Methods and Operators

        public void InitComponent(IAttributeTable attributeTable)
        {
        }

        #endregion
    }
}