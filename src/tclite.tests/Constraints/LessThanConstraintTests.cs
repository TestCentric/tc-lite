// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Constraints.Tests
{
    [TestFixture]
    public class LessThanConstraintTests : ComparisonConstraintTest<IComparable>
    {
        protected override Constraint Constraint => new LessThanConstraint<int>(5);
        protected override string ExpectedDescription => "less than 5";
        protected override string ExpectedRepresentation => "<lessthan 5>";

        static IComparable[] SuccessData => new IComparable[] { 4, 4.999 };

        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(6, "6"),
            new TestCaseData(5, "5")
        };

        [TestCase]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(42);
            ClassWithIComparable actual = new ClassWithIComparable(0);
            Assert.That(actual, Is.LessThan(expected));
        }

        [TestCase]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(42);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(0);
            Assert.That(actual, Is.LessThan(expected));
        }
    }
}