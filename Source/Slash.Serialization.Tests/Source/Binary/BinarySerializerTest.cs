// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinarySerializerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Tests.Source.Binary
{
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    using Slash.Serialization.Binary;

    public class BinarySerializerTest
    {
        #region Fields

        private BinarySerializer binarySerializer;

        private MemoryStream memoryStream;

        #endregion

        #region Enums

        private enum TestEnum
        {
            Blue,

            Red,

            Green
        }

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public void SetUp()
        {
            this.binarySerializer = new BinarySerializer();
            this.memoryStream = new MemoryStream();
        }

        [Test]
        public void TestSerializeBool()
        {
            const bool B = true;
            this.AssertSerializable(B);
        }

        [Test]
        public void TestSerializeByte()
        {
            const byte B = 42;
            this.AssertSerializable(B);
        }

        [Test]
        public void TestSerializeChar()
        {
            const char C = '.';
            this.AssertSerializable(C);
        }

        [Test]
        public void TestSerializeDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("22335", "Hamburg");
            dictionary.Add("24118", "Kiel");

            this.AssertSerializable(dictionary);
        }

        [Test]
        public void TestSerializeDouble()
        {
            const double D = 42;
            this.AssertSerializable(D);
        }

        [Test]
        public void TestSerializeEnum()
        {
            const TestEnum EnumValue = TestEnum.Green;
            this.AssertSerializable(EnumValue);
        }

        [Test]
        public void TestSerializeFloat()
        {
            const float F = 42f;
            this.AssertSerializable(F);
        }

        [Test]
        public void TestSerializeInt()
        {
            const int I = 42;
            this.AssertSerializable(I);
        }

        [Test]
        public void TestSerializeLong()
        {
            const long L = 42L;
            this.AssertSerializable(L);
        }

        [Test]
        public void TestSerializeReflection()
        {
            TestClass o = new TestClass { Color = TestEnum.Red, Name = "test" };
            this.AssertSerializable(o);
        }

        [Test]
        public void TestSerializeSByte()
        {
            const sbyte B = 42;
            this.AssertSerializable(B);
        }

        [Test]
        public void TestSerializeShort()
        {
            const short S = 42;
            this.AssertSerializable(S);
        }

        [Test]
        public void TestSerializeString()
        {
            const string S = "Test";
            this.AssertSerializable(S);
        }

        [Test]
        public void TestSerializeStringList()
        {
            List<string> stringList = new List<string> { "first", "second" };
            this.AssertSerializable(stringList);
        }

        [Test]
        public void TestSerializeUInt()
        {
            const uint I = 42;
            this.AssertSerializable(I);
        }

        [Test]
        public void TestSerializeULong()
        {
            const ulong L = 42;
            this.AssertSerializable(L);
        }

        [Test]
        public void TestSerializeUShort()
        {
            const ushort S = 42;
            this.AssertSerializable(S);
        }

        [Test]
        public void TestSerializeValueWithType()
        {
            ValueWithType valueWithType = new ValueWithType("test");
            this.AssertSerializable(valueWithType);
        }

        #endregion

        #region Methods

        private void AssertSerializable<T>(T o)
        {
            this.binarySerializer.Serialize(this.memoryStream, o);
            this.memoryStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(o, this.binarySerializer.Deserialize<T>(this.memoryStream));
        }

        #endregion

        private class TestClass
        {
            #region Fields

            public TestEnum Color;

            #endregion

            #region Public Properties

            public string Name { get; set; }

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
                unchecked
                {
                    return ((int)this.Color * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                }
            }

            #endregion

            #region Methods

            protected bool Equals(TestClass other)
            {
                return this.Color == other.Color && string.Equals(this.Name, other.Name);
            }

            #endregion
        }
    }
}