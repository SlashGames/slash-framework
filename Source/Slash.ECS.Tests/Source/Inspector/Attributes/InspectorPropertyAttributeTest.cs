// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorPropertyAttributeTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Tests.Inspector.Attributes
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Slash.ECS.Inspector.Attributes;
    using Slash.ECS.Inspector.Data;

    public class InspectorPropertyAttributeTest
    {
        #region Public Methods and Operators

        [Test]
        public void TestAddingDefaultItemToEmptyIntList()
        {
            var inspectorType = InspectorType.GetInspectorType(typeof(TestType));
            this.TestAddingDefaultItemToEmptyList(inspectorType, TestType.AttributeMember2);
        }

        [Test]
        public void TestAddingDefaultItemToEmptyStringList()
        {
            var inspectorType = InspectorType.GetInspectorType(typeof(TestType));
            this.TestAddingDefaultItemToEmptyList(inspectorType, TestType.AttributeMember1);
        }

        #endregion

        #region Methods

        private void TestAddingDefaultItemToEmptyList(InspectorType inspectorType, string propertyName)
        {
            InspectorPropertyAttribute testAttribute =
                inspectorType.Properties.First(property => property.Name == propertyName);

            // Create list.
            var list = testAttribute.GetEmptyList();
            list.Add(testAttribute.DefaultListItem);
        }

        #endregion

        [InspectorType]
        public class TestType
        {
            #region Constants

            public const string AttributeMember1 = "Member1";

            public const string AttributeMember2 = "Member2";

            #endregion

            #region Properties

            [InspectorInt(AttributeMember2)]
            public List<int> IntList { get; set; }

            [InspectorString(AttributeMember1)]
            public List<string> StringList { get; set; }

            #endregion
        }
    }
}