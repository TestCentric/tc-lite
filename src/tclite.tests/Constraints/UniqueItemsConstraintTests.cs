// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class UniqueItemsTests : ConstraintTestBase<IEnumerable>
    {
        protected override Constraint Constraint => new UniqueItemsConstraint();
        protected override string ExpectedRepresentation => "<uniqueitems>";
        protected override string ExpectedDescription => "all items unique";

        protected override IEnumerable[] SuccessData => new int[][] { new [] { 1, 3, 17, -2, 34 }, new int[0] };
        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(new int[] { 1, 3, 17, 3, 34 }, "< 1, 3, 17, 3, 34 >" )
        };

        [TestCaseSource(nameof(SuccessData))]
        public void SucceedsUsingOriginalAlgorithm(IEnumerable actual)
        {
            var constraint = Constraint as UniqueItemsConstraint;
            Assert.IsTrue(constraint.OriginalAlgorithm(actual));
        }
        [TestCaseSource(nameof(SuccessData))]
        public void SucceedsUsingFastAlgorithm(IEnumerable actual)
        {
            var constraint = Constraint as UniqueItemsConstraint;
            Assert.IsTrue(constraint.TryFastAlgorithm(actual).Value);
        }

        [TestCaseSource(nameof(FailureData))]
        public void FailsUsingOriginalAlgorithm(IEnumerable actual, string unused)
        {
            var constraint = Constraint as UniqueItemsConstraint;
            Assert.IsFalse(constraint.OriginalAlgorithm(actual));
        }
        [TestCaseSource(nameof(FailureData))]
        public void FailsUsingFastAlgorithm(IEnumerable actual, string unused)
        {
            var constraint = Constraint as UniqueItemsConstraint;
            Assert.IsFalse(constraint.TryFastAlgorithm(actual).Value);
        }

        [TestCaseSource( nameof(IgnoreCaseData) )]
        public void HonorsIgnoreCase( IEnumerable actual )
        {
            Assert.That( actual, Is.Not.Unique.IgnoreCase, $"{actual} should not be unique ignoring case");
        }

        private static readonly object[] IgnoreCaseData =
        {
            //new object[] {new SimpleObjectCollection("x", "y", "z", "Z")},
            new object[] {new[] {'A', 'B', 'C', 'c'}},
            new object[] {new[] {"a", "b", "c", "C"}}
        };

#if NYI
        static readonly IEnumerable<int> RANGE = Enumerable.Range(0, 10000);

        static readonly TestCaseData[] PerformanceData =
        {
            new TestCaseData(RANGE, false),
            new TestCaseData(new List<int>(RANGE), false),
            new TestCaseData(new List<double>(RANGE.Select(v => (double)v)), false),
            new TestCaseData(new List<string>(RANGE.Select(v => v.ToString())), false),
            new TestCaseData(new List<string>(RANGE.Select(v => v.ToString())), true)
        };

        [TestCaseSource(nameof(PerformanceData))]
        public void PerformanceTests(IEnumerable values, bool ignoreCase)
        {
            Warn.Unless(() =>
            {
                if (ignoreCase)
                    Assert.That(values, Is.Unique.IgnoreCase);
                else
                    Assert.That(values, Is.Unique);
            }, HelperConstraints.HasMaxTime(100));
        }
#endif
    }
}
