// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TCLite.Internal;

namespace TCLite.Attributes
{
    /// <summary>
    /// Summary description for CategoryAttributeTests.
    /// </summary>
    [TestFixture, Category("CategoryOnFixture")]
    public class CategoryAttributeTests
    {
        IEnumerable<string> _categories = TestContext.CurrentTest.Categories;

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

        [TestCase, Critical]
        public void CanUseCustomCategory()
        {
            Assert.That(_categories, Contains.Item.EqualTo("Critical"));
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
        public class CriticalAttribute : CategoryAttribute { }
    }
}
