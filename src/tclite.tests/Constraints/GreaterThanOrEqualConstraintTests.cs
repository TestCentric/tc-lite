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
    public class GreaterThanOrEqualConstraintTests : ComparisonConstraintTest<IComparable>
    {
        protected override Constraint Constraint => new GreaterThanOrEqualConstraint<int>(5);
        protected override string ExpectedDescription => "greater than or equal to 5";
        protected override string ExpectedRepresentation => "<greaterthanorequal 5>";

        static IComparable[] SuccessData => new IComparable[] { 6, 5 };
        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(4, "4")
        };

        [Test]
        public void CanCompareIComparables()
        {
            ClassWithIComparable expected = new ClassWithIComparable(0);
            ClassWithIComparable actual = new ClassWithIComparable(42);
            Assert.That(actual, Is.GreaterThanOrEqualTo(expected));
        }

        [Test]
        public void CanCompareIComparablesOfT()
        {
            ClassWithIComparableOfT expected = new ClassWithIComparableOfT(0);
            ClassWithIComparableOfT actual = new ClassWithIComparableOfT(42);
            Assert.That(actual, Is.GreaterThanOrEqualTo(expected));
        }
    }
}