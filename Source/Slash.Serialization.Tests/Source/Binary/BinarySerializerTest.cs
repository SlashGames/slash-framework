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
    using Slash.Serialization.Xml;

    public class BinarySerializerTest
    {
        #region Fields

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
            this.memoryStream = new MemoryStream();
        }

        [Test]
        public void TestSerializeArray()
        {
            string[] array = new[] { "first", "second" };
            this.AssertSerializable(array);
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
        public void TestSerializeDerivedDictionary()
        {
            SerializableDictionary<string, string> dictionary = new SerializableDictionary<string, string>();
            dictionary.Add("22335", "Hamburg");
            dictionary.Add("24118", "Kiel");

            this.AssertSerializable(dictionary);
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
        public void TestSerializeNull()
        {
            TestClass o = new TestClass(TestEnum.Red, string.Empty);
            this.AssertSerializable(o);
        }

        [Test]
        public void TestSerializeReflection()
        {
            TestClass o = new TestClass(TestEnum.Red, "test");
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
            BinarySerializer binarySerializer = new BinarySerializer(this.memoryStream);
            binarySerializer.Serialize(o);

            this.memoryStream.Seek(0, SeekOrigin.Begin);

            BinaryDeserializer binaryDeserializer = new BinaryDeserializer(this.memoryStream);
            Assert.AreEqual(o, binaryDeserializer.Deserialize<T>());
        }

        #endregion

        private class TestClass
        {
            #region Fields

            [SerializeMember]
            private readonly TestEnum color;

            #endregion

            #region Constructors and Destructors

            public TestClass()
            {
            }

            public TestClass(TestEnum color, string name)
            {
                this.color = color;
                this.Name = name;
            }

            #endregion

            #region Public Properties

            [SerializeMember]
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
                    return ((int)this.color * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                }
            }

            public override string ToString()
            {
                return string.Format("Color: {0}, Name: {1}", this.color, this.Name);
            }

            #endregion

            #region Methods

            protected bool Equals(TestClass other)
            {
                return this.color == other.color && string.Equals(this.Name, other.Name);
            }

            #endregion
        }
    }
}