// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorUtilsTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests.Inspector.Utils
{
    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Configurations;
    using Slash.ECS.Inspector.Attributes;
    using Slash.ECS.Inspector.Data;
    using Slash.ECS.Inspector.Utils;

    public class InspectorUtilsTest
    {
        #region Fields

        private InspectorType inspectorType;

        private Game testGame;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.inspectorType = InspectorType.GetInspectorType(typeof(TestInspectorType));
            this.testGame = new Game();
        }

        [Test]
        public void TestCreateFromAttributeTable()
        {
            IAttributeTable attributeTable = new AttributeTable();
            const string TestValueString1 = "Test1";
            const string TestValueString2 = "Test2";
            attributeTable.SetValue(TestInspectorType.AttributeString1, TestValueString1);
            attributeTable.SetValue(TestInspectorType.AttributeString2, TestValueString2);

            TestInspectorType testInspectorType =
                InspectorUtils.CreateFromAttributeTable<TestInspectorType>(
                    this.testGame.EntityManager,
                    this.inspectorType,
                    attributeTable);
            Assert.AreEqual(testInspectorType.String1, TestValueString1);
            Assert.AreEqual(testInspectorType.String2, TestValueString2);
        }

        [Test]
        public void TestCreateFromNullAttributeTable()
        {
            TestInspectorType testInspectorType = null;
            Assert.DoesNotThrow(
                () =>
                {
                    testInspectorType =
                        InspectorUtils.CreateFromAttributeTable<TestInspectorType>(
                            this.testGame.EntityManager,
                            this.inspectorType,
                            null);
                });

            Assert.NotNull(testInspectorType);
        }

        [Test]
        public void TestDeinitRemovesChildEntity()
        {
            var entityManager = this.testGame.EntityManager;
            const string TestBlueprintId = "TestBlueprint";
            BlueprintManager blueprintManager;
            this.testGame.BlueprintManager = blueprintManager = new BlueprintManager();
            blueprintManager.AddBlueprint(TestBlueprintId, new Blueprint());

            TestInspectorTypeWithEntityProperty testType = new TestInspectorTypeWithEntityProperty();
            var testInspectorType = InspectorType.GetInspectorType(typeof(TestInspectorTypeWithEntityProperty));
            IAttributeTable configuration = new AttributeTable();
            EntityConfiguration childConfiguration = new EntityConfiguration { BlueprintId = TestBlueprintId };
            configuration.SetValue(TestInspectorTypeWithEntityProperty.AttributeMember1, childConfiguration);

            // Init.
            InspectorUtils.InitFromAttributeTable(entityManager, testInspectorType, testType, configuration);

            // Check that child entity was created.
            Assert.AreNotEqual(0, testType.EntityMember);

            // Deinit.
            InspectorUtils.Deinit(entityManager, testInspectorType, testType);

            // Check that child entity was removed.
            Assert.IsTrue(entityManager.EntityIsBeingRemoved(testType.EntityMember));
        }

        [Test]
        public void TestInitFromNullAttributeTable()
        {
            TestInspectorType testInspectorType = new TestInspectorType();
            Assert.DoesNotThrow(
                () => InspectorUtils.InitFromAttributeTable(this.testGame.EntityManager, testInspectorType, null));
        }

        [Test]
        public void TestInitFromNullAttributeTableAndInspectorType()
        {
            TestInspectorType testInspectorType = new TestInspectorType();
            Assert.DoesNotThrow(
                () =>
                    InspectorUtils.InitFromAttributeTable(
                        this.testGame.EntityManager,
                        this.inspectorType,
                        testInspectorType,
                        null));
        }

        #endregion

        [InspectorType]
        public class TestInspectorTypeWithEntityProperty
        {
            #region Constants

            public const string AttributeMember1 = "Member1";

            #endregion

            #region Properties

            [InspectorEntity(AttributeMember1)]
            public int EntityMember { get; set; }

            #endregion
        }

        [InspectorType]
        public class TestInspectorType
        {
            #region Constants

            public const string AttributeString1 = "String1";

            public const string AttributeString2 = "String2";

            #endregion

            #region Properties

            [InspectorString(AttributeString1)]
            public string String1 { get; set; }

            [InspectorString(AttributeString2)]
            public string String2 { get; set; }

            #endregion
        }
    }
}