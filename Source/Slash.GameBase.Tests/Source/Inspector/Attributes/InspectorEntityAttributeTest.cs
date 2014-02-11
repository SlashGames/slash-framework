// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorEntityAttributeTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Tests.Inspector.Attributes
{
    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Blueprints;
    using Slash.GameBase.Components;
    using Slash.GameBase.Configurations;
    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;
    using Slash.GameBase.Inspector.Utils;

    public class InspectorEntityAttributeTest
    {
        #region Constants

        private const string TestBlueprintId = "TestBlueprint";

        #endregion

        #region Fields

        private Game testGame;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.testGame = new Game();
            BlueprintManager blueprintManager = new BlueprintManager();
            Blueprint blueprint = new Blueprint();
            blueprint.ComponentTypes.Add(typeof(TestComponent));
            blueprintManager.AddBlueprint(TestBlueprintId, blueprint);
            this.testGame.BlueprintManager = blueprintManager;
        }

        [Test]
        public void TestDeserializationFromAttributeTable()
        {
            IAttributeTable attributeTable = new AttributeTable();
            IAttributeTable entityAttributeTable = new AttributeTable();
            const string TestValueString = "Test";
            entityAttributeTable.SetValue(TestComponent.AttributeTestString, TestValueString);
            EntityConfiguration entityConfiguration = new EntityConfiguration
                {
                    BlueprintId = TestBlueprintId,
                    Configuration = entityAttributeTable
                };
            attributeTable.SetValue(TestData.AttributeTestEntity, entityConfiguration);

            TestData testData = InspectorUtils.CreateFromAttributeTable<TestData>(
                this.testGame, InspectorType.GetInspectorType(typeof(TestData)), attributeTable);
            Assert.AreNotEqual(testData.TestEntity, 0);
            Assert.AreNotEqual(testData.TestEntity, -1);

            // Check entity.
            TestComponent testComponent = this.testGame.EntityManager.GetComponent<TestComponent>(testData.TestEntity);
            Assert.NotNull(testComponent);
            Assert.AreEqual(testComponent.TestString, TestValueString);
        }

        #endregion

        [InspectorType]
        private class TestComponent : IEntityComponent
        {
            #region Constants

            public const string AttributeTestString = "TestString";

            #endregion

            #region Public Properties

            [InspectorString(AttributeTestString)]
            public string TestString { get; set; }

            #endregion

            #region Public Methods and Operators

            public void InitComponent(IAttributeTable attributeTable)
            {
            }

            #endregion
        }

        [InspectorType]
        private class TestData
        {
            #region Constants

            public const string AttributeTestEntity = "TestEntity";

            #endregion

            #region Public Properties

            [InspectorEntity(AttributeTestEntity)]
            public int TestEntity { get; set; }

            #endregion
        }
    }
}