// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeTableTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Tests.Source.AttributeTables
{
    using System;

    using NUnit.Framework;

    using Slash.Collections.AttributeTables;
    using Slash.Tests.Utils;

    public class AttributeTableTest
    {
        #region Public Methods and Operators

        [Test]
        public void TestAddNullKey()
        {
            Assert.Throws<ArgumentNullException>(() => new AttributeTable { { null, 1 } });
        }

        [Test]
        public void TestSerialization()
        {
            AttributeTable attributeTable = new AttributeTable { { "one", 1 } };
            SerializationTestUtils.TestXmlSerialization(attributeTable);
        }

        [Test]
        public void TestSerializationNullValue()
        {
            AttributeTable attributeTable = new AttributeTable { { "null", null } };
            SerializationTestUtils.TestXmlSerialization(attributeTable);
        }

        #endregion
    }
}