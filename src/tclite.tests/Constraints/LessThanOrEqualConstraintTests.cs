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
    public class LessThanOrEqualConstraintTests : ComparisonConstraintTest
    {
        public LessThanOrEqualConstraintTests()
        {
            _constraint = comparisonConstraint = new LessThanOrEqualConstraint(5);
            _expectedDescription = "less than or equal to 5";
            _expectedRepresentation = "<lessthanorequal 5>";
        }

        internal object[] SuccessData = new object[] { 4, 5 };

        internal object[] FailureData = new object[] { new object[] { 6, "6" } };

        internal object[] InvalidData = new object[] { null, "xxx" };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(42);
            ClassWithIComparable actual = new ClassWithIComparable(0);
            Assert.That(actual, Is.LessThanOrEqualTo(expected));
        }

        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(42);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(0);
            Assert.That(actual, Is.LessThanOrEqualTo(expected));
        }
    }
}