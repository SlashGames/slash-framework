// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensionMethodsTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Tests.Source.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Slash.Collections.Extensions;
    using Slash.Collections.Utils;

    public class EnumerableExtensionMethodsTest
    {
        #region Public Methods and Operators

        [TestFixtureSetUp]
        public void SetUp()
        {
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestIsNullOrEmptyEmpty()
        {
            IEnumerable nullEnumerable = new object[] { };
            IEnumerable<object> nullEnumerableT = new List<object>();
            Assert.IsTrue(nullEnumerable.IsNullOrEmpty());
            Assert.IsTrue(nullEnumerableT.IsNullOrEmpty());
        }

        [Test]
        public void TestIsNullOrEmptyNotEmpty()
        {
            IEnumerable nullEnumerable = new[] { new object() };
            IEnumerable<object> nullEnumerableT = new List<object> { new object(), new object() };
            Assert.IsFalse(nullEnumerable.IsNullOrEmpty());
            Assert.IsFalse(nullEnumerableT.IsNullOrEmpty());
        }

        [Test]
        public void TestIsNullOrEmptyNull()
        {
            IEnumerable nullEnumerable = null;
            IEnumerable<object> nullEnumerableT = null;
            Assert.IsTrue(nullEnumerable.IsNullOrEmpty());
            Assert.IsTrue(nullEnumerableT.IsNullOrEmpty());
        }

        [Test]
        public void TestRandomSelectLessThanNumberOfSelections()
        {
            IEnumerable<object> enumerable = new List<object> { new object(), new object() };
            Assert.AreEqual(enumerable, enumerable.RandomSelect(enumerable.Count() + 1));

            IList<object> selectedItems = new List<object>();
            enumerable.RandomSelect(enumerable.Count() + 1, selectedItems.Add);
            Assert.IsTrue(CollectionUtils.SequenceEqual(enumerable, selectedItems));
        }

        [Test]
        public void TestRandomSelectNull()
        {
            IEnumerable<object> nullEnumerable = null;
            Assert.Throws(typeof(ArgumentNullException), () => nullEnumerable.RandomSelect());
            Assert.Throws(typeof(ArgumentNullException), () => nullEnumerable.RandomSelect(23));
            Assert.Throws(typeof(ArgumentNullException), () => nullEnumerable.RandomSelect(42, o => { }));
        }

        [Test]
        public void TestRandomSelectOne()
        {
            IEnumerable<object> enumerable = new List<object> { 0, 1, 2, 3 };
            Assert.DoesNotThrow(() => enumerable.RandomSelect());
            Assert.DoesNotThrow(() => enumerable.RandomSelect(1));
            Assert.DoesNotThrow(() => enumerable.RandomSelect(1, o => { }));
        }

        #endregion
    }
}