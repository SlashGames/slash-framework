// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionUtilsTest.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    using Slash.Reflection.Utils;
    using Slash.SystemExt.Utils;

    public class ReflectionUtilsTest
    {
        #region Public Methods and Operators

        [Test]
        public void TestFindTypeAssemblyQualifiedName()
        {
            Type type = typeof(int);

            this.TestFindType(type.AssemblyQualifiedName, type);
        }

        [Test]
        public void TestFindTypeAssemblyQualifiedNameOtherAssembly()
        {
            Type type = typeof(TestClass);

            string assemblyQualifiedName = type.AssemblyQualifiedName;

            // Replace version number to check if type is found anyway.
            assemblyQualifiedName = Regex.Replace(assemblyQualifiedName, @"Version=\d+.\d+.\d+.\d+", "Version=1.2.3.4");

            this.TestFindType(assemblyQualifiedName, type);
        }

        [Test]
        public void TestFindTypeFullName()
        {
            Type type = typeof(int);

            this.TestFindType(type);
        }

        [Test]
        public void TestFindTypeGenericFullName()
        {
            Type genericType = typeof(List<int>);
            this.TestFindType(genericType.FullName, genericType);
        }

        [Test]
        public void TestFindTypeGenericFullNameOtherAssembly()
        {
            Type genericType = typeof(List<TestClass>);

            string fullName = genericType.FullName;

            // Replace version number to check if type is found anyway.
            fullName = Regex.Replace(fullName, @"Version=\d+.\d+.\d+.\d+", "Version=1.2.3.4");

            this.TestFindType(fullName, genericType);
        }

        [Test]
        public void TestFindTypeGenericFullNameWithoutAssemblyInfo()
        {
            Type genericType = typeof(List<int>);

            string fullNameWithoutAssemblyInfo = genericType.FullNameWithoutAssemblyInfo();

            this.TestFindType(fullNameWithoutAssemblyInfo, genericType);
        }

        [Test]
        public void TestFindTypeUnknownAssembly()
        {
            string fullName =
                "System.Collections.Generic.List`1[[Slash.GameBase.Configurations.EntityConfiguration, Slash.GameBase, Version=1.0.5161.23825, Culture=neutral, PublicKeyToken=null]]";
            Assert.Throws<TypeLoadException>(() => ReflectionUtils.FindType(fullName));
        }

        #endregion

        #region Methods

        private void TestFindType(Type type)
        {
            this.TestFindType(type.FullName, type);
        }

        private void TestFindType(string fullName, Type expectedType)
        {
            Type foundType = ReflectionUtils.FindType(fullName);
            Assert.AreEqual(expectedType, foundType);
        }

        #endregion

        public class TestClass
        {
        }
    }
}