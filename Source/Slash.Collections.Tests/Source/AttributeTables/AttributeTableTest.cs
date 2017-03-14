// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeTableTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Tests.Source.AttributeTables
{
    using System;
    using System.Collections.Generic;

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
            var attributeTable = new AttributeTable { { "one", 1 } };
            SerializationTestUtils.TestXmlSerialization(attributeTable);
        }

        [Test]
        public void TestSerializationNullValue()
        {
            var attributeTable = new AttributeTable { { "null", null } };
            SerializationTestUtils.TestXmlSerialization(attributeTable);
        }

        [Test]
        public void TestSerializationListInt()
        {
            var attributeTable = new AttributeTable { { "ListInt", new List<int> { 1, 3, 2 } } };
            SerializationTestUtils.TestXmlSerialization(attributeTable);
        }

        public struct Vector2
        {
            public float X;

            public float Y;

            /// <inheritdoc />
            public bool Equals(Vector2 other)
            {
                return this.X.Equals(other.X) && this.Y.Equals(other.Y);
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                return obj is Vector2 && this.Equals((Vector2)obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                unchecked
                {
                    return (this.X.GetHashCode() * 397) ^ this.Y.GetHashCode();
                }
            }
        }

        [Test]
        public void TestSerializationListVector2()
        {
            var attributeTable = new AttributeTable
            {
                {
                    "ListVector2",
                    new List<Vector2>
                    {
                        new Vector2 { X = 1, Y = 2 },
                        new Vector2 { X = 321, Y = 11 },
                        new Vector2 { X = 52.23f, Y = 2323.23f }
                    }
                }
            };
            SerializationTestUtils.TestXmlSerialization(attributeTable);
        }

        // NOTE: Enable when dictionary serialization works out-of-the-box
        //[Test]
        //public void TestSerializationDictionaryStringFloat()
        //{
        //    var attributeTable = new AttributeTable
        //    {
        //        {
        //            "DictionaryStringFloat",
        //            new Dictionary<string, float>
        //            {
        //                { "One", 1.0f },
        //                { "Pi", (float)Math.PI },
        //                { "Answer To Everything", 42 }
        //            }
        //        }
        //    };
        //    SerializationTestUtils.TestXmlSerialization(attributeTable);
        //}

        #endregion
    }
}