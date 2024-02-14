// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
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
            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnFixture"));
        }

        [TestCase, Category("Cat1"), Category("Cat2"), Category("Cat3")]
        public void MultipleCategoryAttributes()
        {
            Assert.That(_categories, Contains.Item.EqualTo("Cat1"));
            Assert.That(_categories, Contains.Item.EqualTo("Cat2"));
            Assert.That(_categories, Contains.Item.EqualTo("Cat3"));
        }

        [TestCase, Category("Cat1,Cat2,, Cat3")]
        public void MultipleCategoriesOnSingleAttribute()
        {
            Assert.That(_categories, Contains.Item.EqualTo("Cat1"));
            Assert.That(_categories, Contains.Item.EqualTo("Cat2"));
            Assert.That(_categories, Contains.Item.EqualTo("Cat3"));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Category("CategoryOnMethod")]
        public void CategoryOnTestMethod_MultipleTestCases(int n)
        {
            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnMethod"), $"Case {n}");
            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnFixture"), $"Case {n}");
        }

        [TestCase, Critical]
        public void CanDeriveFromCategoryAttribute()
        {
            Assert.That(_categories, Contains.Item.EqualTo("Critical"));
            Assert.That(_categories, Contains.Item.EqualTo("CategoryOnFixture"));
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
        public class CriticalAttribute : CategoryAttribute { }
    }
}
