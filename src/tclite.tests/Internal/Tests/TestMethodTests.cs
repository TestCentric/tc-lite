// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Reflection;
using NUnit.Framework;

namespace TCLite.Framework.Tests
{
    public class TestMethodTests
    {
        private TestMethod _testMethod;

        [NUnit.Framework.SetUp]
        public void CreateTestMethod()
        {
            var method = GetType().GetMethod(nameof(MyTestMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);
            _testMethod = new TestMethod(method, null);
         
            Assert.NotNull(_testMethod);
        }

        [NUnit.Framework.Test]
        public void TestType()
        {
            Assert.That(_testMethod.TestType, Is.EqualTo("TestMethod"));
        }

        [NUnit.Framework.Test]
        public void MethodName()
        {
            Assert.That(_testMethod.MethodName, Is.EqualTo(nameof(MyTestMethod)));
        }

        private void MyTestMethod()
        {

        }
    }
}