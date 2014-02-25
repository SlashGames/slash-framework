// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinarySerializerTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Tests.Source.Binary
{
    using System.IO;

    using NUnit.Framework;

    using Slash.Serialization.Binary;

    public class BinarySerializerTest
    {
        #region Fields

        private BinarySerializer binarySerializer;

        private MemoryStream memoryStream;

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
        public void TestSerializeDouble()
        {
            const double D = 42;
            this.AssertSerializable(D);
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

        #endregion

        #region Methods

        private void AssertSerializable<T>(T o)
        {
            this.binarySerializer.Serialize(this.memoryStream, o);
            this.memoryStream.Seek(0, SeekOrigin.Begin);
            Assert.AreEqual(o, this.binarySerializer.Deserialize<T>(this.memoryStream));
        }

        #endregion
    }
}