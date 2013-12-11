// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeCollectionSerializationTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Tests.Source.Xml
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;

    using NUnit.Framework;

    using Slash.Collections.Utils;
    using Slash.Reflection.Utils;
    using Slash.Tests.Utils;

    public class TypeCollectionSerializationTest
    {
        #region Public Methods and Operators

        [Test]
        public void TestSerialization()
        {
            TestClass testClass = new TestClass { Types = new List<Type> { typeof(int), typeof(string) } };
            SerializationTestUtils.TestXmlSerialization(testClass);
        }

        #endregion

        public class TestClass
        {
            #region Public Properties

            [XmlIgnore]
            public List<Type> Types { get; set; }

            [XmlArray("Types")]
            [XmlArrayItem("Type")]
            [Browsable(false)]
            [EditorBrowsable(EditorBrowsableState.Never)]
            public string[] TypesString
            {
                get
                {
                    return this.Types == null ? null : this.Types.Select(type => type.AssemblyQualifiedName).ToArray();
                }
                set
                {
                    this.Types = value.Select(ReflectionUtils.FindType).ToList();
                }
            }

            #endregion

            #region Public Methods and Operators

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }
                if (obj.GetType() != this.GetType())
                {
                    return false;
                }
                return this.Equals((TestClass)obj);
            }

            public override int GetHashCode()
            {
                return (this.Types != null ? this.Types.GetHashCode() : 0);
            }

            public override string ToString()
            {
                return string.Format("Types: {0}", CollectionUtils.ToString(this.Types));
            }

            #endregion

            #region Methods

            protected bool Equals(TestClass other)
            {
                return CollectionUtils.SequenceEqual(this.Types, other.Types);
            }

            #endregion
        }
    }
}