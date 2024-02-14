// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Reflection;

namespace TCLite.Internal
{
    public class TestMethodTests
    {
        private TestMethod _testMethod;

        [TestCase]
        public void TestType()
        {
            CreateTestMethod();
            Assert.That(_testMethod.TestType, Is.EqualTo("TestMethod"));
        }

        [TestCase]
        public void MethodName()
        {
            CreateTestMethod();
            Assert.That(_testMethod.MethodName, Is.EqualTo(nameof(MyTestMethod)));
        }

        private void MyTestMethod()
        {

        }

        private void CreateTestMethod()
        {
            var method = GetType().GetMethod(nameof(MyTestMethod), BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method);
            _testMethod = new TestMethod(method, null);
         
            Assert.IsNotNull(_testMethod);
        }
    }
}