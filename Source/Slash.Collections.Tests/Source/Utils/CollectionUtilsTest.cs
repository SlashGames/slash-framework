// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionUtilsTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Tests.Source.Utils
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Slash.Collections.Utils;

    public class CollectionUtilsTest
    {
        #region Public Methods and Operators

        [Test]
        public void TestArrayToString()
        {
            string[] testArray = { "One", "Two", "Three" };
            string toString = CollectionUtils.ToString(testArray);
            Assert.AreEqual("[One, Two, Three]", toString);
        }

        [Test]
        public void TestNestedArrayToString()
        {
            object[] testArray = { "One", new[] { "Two", "Three" } };
            string toString = CollectionUtils.ToString(testArray);
            Assert.AreEqual("[One, [Two, Three]]", toString);
        }

        [Test]
        public void TestDictionaryToString()
        {
            Dictionary<int, object> testDictionary = new Dictionary<int, object>()
            {
                { 1, "One" },
                { 2, new [] { "Two", "Three" } }
            };
            string toString = CollectionUtils.ToString(testDictionary);
            Assert.AreEqual("[[1, One], [2, [Two, Three]]]", toString);
        }

        [Test]
        public void TestStringToString()
        {
            string[] testArray = { "Data" };
            string toString = CollectionUtils.ToString(testArray);
            Assert.AreEqual("[Data]", toString);
        }

        #endregion
    }
}