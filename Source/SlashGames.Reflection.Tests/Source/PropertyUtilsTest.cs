// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyUtilsTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Tests
{
    using System.Reflection;

    using NUnit.Framework;

    public class PropertyUtilsTest
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
        public void TestGetPropertyInfo()
        {
            PropertyInfo propertyInfo = PropertyUtils<TestClass>.GetPropertyInfo(obj => obj.TestProperty);
            Assert.NotNull(propertyInfo);
        }

        #endregion

        public class TestClass
        {
            #region Public Properties

            public float TestProperty { get; set; }

            #endregion
        }
    }
}