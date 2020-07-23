// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Attributes
{
    /// <summary>
    /// Summary description for CategoryAttributeTests.
    /// </summary>
    [TestFixture, Category("CategoryOnFixture")]
    public class CategoryAttributeTests
    {
        Test _currentTest;
        IList _categories;

        public CategoryAttributeTests()
        {
            _currentTest = TestExecutionContext.CurrentContext.CurrentTest;
            _categories = _currentTest.Properties["Category"];
            string msg = "Category was not visible in constructor";

            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnFixture"), msg);
            if (_currentTest.Name == "CategoryOnTestMethod")
                Assert.That(_categories, Contains.Item.EqualTo("CategoryOnMethod"), msg);
        }

        [TestCase]
        public void CategoryOnFixture()
        {
            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnFixture"));
        }

        [TestCase, Category("CategoryOnMethod")]
        public void CategoryOnTestMethod()
        {
            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnMethod"));
        }

        [TestCase, Critical]
        public void CanDeriveFromCategoryAttribute()
        {
            Assert.That(_categories, Contains.Item.EqualTo("Critical"));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Category("CategoryOnMethod")]
        public void CategoryOnTestMethod_MultipleTestCases(int n)
        {
            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnMethod"), $"Case {n}");
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
        public class CriticalAttribute : CategoryAttribute { }
    }
}