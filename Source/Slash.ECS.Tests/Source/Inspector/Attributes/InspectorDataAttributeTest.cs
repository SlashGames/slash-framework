// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorDataAttributeTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests.Inspector.Attributes
{
    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Inspector.Attributes;
    using Slash.ECS.Inspector.Data;
    using Slash.ECS.Inspector.Utils;

    public class InspectorDataAttributeTest
    {
        #region Fields

        private InspectorType dataInspectorType;

        private InspectorType parentInspectorType;

        private Game testGame;

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.parentInspectorType = InspectorType.GetInspectorType(typeof(TestDataParent));
            this.dataInspectorType = InspectorType.GetInspectorType(typeof(TestData));
            this.testGame = new Game();
        }

        [Test]
        public void TestDeserializationFromAttributeTable()
        {
            IAttributeTable attributeTable = new AttributeTable();
            const string TestValueString1 = "Test1";
            const string TestValueString2 = "Test2";
            attributeTable.SetValue(TestData.AttributeString1, TestValueString1);
            attributeTable.SetValue(TestData.AttributeString2, TestValueString2);
            IAttributeTable parentAttributeTable = new AttributeTable();
            parentAttributeTable.SetValue(TestDataParent.AttributeTestData, attributeTable);

            TestDataParent testDataParent = InspectorUtils.CreateFromAttributeTable<TestDataParent>(
                this.testGame.EntityManager, this.parentInspectorType, parentAttributeTable);
            Assert.AreEqual(testDataParent.TestData.String1, TestValueString1);
            Assert.AreEqual(testDataParent.TestData.String2, TestValueString2);
        }

        #endregion
    }

    [InspectorType]
    public class TestDataParent
    {
        #region Constants

        public const string AttributeTestData = "TestInspectorType";

        #endregion

        #region Public Properties

        [InspectorData(AttributeTestData)]
        public TestData TestData { get; set; }

        #endregion
    }

    [InspectorType]
    public class TestData
    {
        #region Constants

        public const string AttributeString1 = "String1";

        public const string AttributeString2 = "String2";

        #endregion

        #region Public Properties

        [InspectorString(AttributeString1)]
        public string String1 { get; set; }

        [InspectorString(AttributeString2)]
        public string String2 { get; set; }

        #endregion
    }
}