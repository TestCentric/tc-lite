// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Constraints
{
    [TestFixture]
    public class LessThanOrEqualConstraintTests : ComparisonConstraintTest<IComparable>
    {
        protected override Constraint Constraint => new LessThanOrEqualConstraint<int>(5);
        protected override string ExpectedDescription => "less than or equal to 5";
        protected override string ExpectedRepresentation => "<lessthanorequal 5>";

        static IComparable[] SuccessData => new IComparable[] { 4, 5 };

        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(6, "6")
        };

        [TestCase]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(42);
            ClassWithIComparable actual = new ClassWithIComparable(0);
            Assert.That(actual, Is.LessThanOrEqualTo(expected));
        }

        [TestCase]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(42);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(0);
            Assert.That(actual, Is.LessThanOrEqualTo(expected));
        }
    }
}