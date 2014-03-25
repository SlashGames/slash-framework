// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorUtilsTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Tests.Inspector.Utils
{
    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;
    using Slash.GameBase.Inspector.Utils;

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
                    this.testGame, this.inspectorType, attributeTable);
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
                        testInspectorType = InspectorUtils.CreateFromAttributeTable<TestInspectorType>(
                            this.testGame, this.inspectorType, null);
                    });

            Assert.NotNull(testInspectorType);
        }

        [Test]
        public void TestInitFromNullAttributeTable()
        {
            TestInspectorType testInspectorType = new TestInspectorType();
            Assert.DoesNotThrow(() => InspectorUtils.InitFromAttributeTable(this.testGame, testInspectorType, null));
        }

        [Test]
        public void TestInitFromNullAttributeTableAndInspectorType()
        {
            TestInspectorType testInspectorType = new TestInspectorType();
            Assert.DoesNotThrow(
                () => InspectorUtils.InitFromAttributeTable(this.testGame, this.inspectorType, testInspectorType, null));
        }

        #endregion

        [InspectorType]
        public class TestInspectorType
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
}