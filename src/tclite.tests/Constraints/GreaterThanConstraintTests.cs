// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class GreaterThanConstraintTests : ComparisonConstraintTest
    {
        public GreaterThanConstraintTests()
        {
            _constraint = comparisonConstraint = new GreaterThanConstraint(5);
            _expectedDescription = "greater than 5";
            _expectedRepresentation = "<greaterthan 5>";
        }

        internal object[] SuccessData = new object[] { 6, 5.001 };

        internal object[] FailureData = new object[] { new object[] { 4, "4" }, new object[] { 5, "5" } };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(0);
            ClassWithIComparable actual = new ClassWithIComparable(42);
            Assert.That(actual, Is.GreaterThan(expected));
        }

        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(0);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(42);
            Assert.That(actual, Is.GreaterThan(expected));
        }
    }
}