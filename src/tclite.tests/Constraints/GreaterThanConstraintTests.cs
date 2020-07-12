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
    public class GreaterThanConstraintTests : ComparisonConstraintTest<IComparable>
    {
        protected override Constraint Constraint => new GreaterThanConstraint<int>(5);
        protected override string ExpectedDescription => "greater than 5";
        protected override string ExpectedRepresentation => "<greaterthan 5>";

        protected override IComparable[] SuccessData => new IComparable[] { 6, 5.001 };

        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(4, "4"),
            new TestCaseData(5, "5")
        };

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