// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using TCLite.TestUtilities;

namespace TCLite.Constraints
{
    #region ComparisonConstraintTest

    public abstract class ComparisonConstraintTest<TExpected> : ConstraintTestBase<TExpected>
    {
        private ComparisonConstraint<int> ComparisonConstraint => Constraint as ComparisonConstraint<int>;

        static TestCaseData[] InvalidData => new TestCaseData[]
        { 
            new TestCaseData(null, typeof(ArgumentNullException)),
            new TestCaseData("xxx", typeof(ArgumentException))
        };

        [TestCase]
        public void UsesProvidedIComparer()
        {
            SimpleObjectComparer comparer = new SimpleObjectComparer();
            ComparisonConstraint.Using(comparer).ApplyTo(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [TestCase]
        public void UsesProvidedComparerOfT()
        {
            MyComparer<int> comparer = new MyComparer<int>();
            ComparisonConstraint.Using(comparer).ApplyTo(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparer<T> : IComparer<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

        [TestCase]
        public void UsesProvidedComparisonOfT()
        {
            MyComparison<int> comparer = new MyComparison<int>();
            ComparisonConstraint.Using(new Comparison<int>(comparer.Compare)).ApplyTo(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparison<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

        [TestCase]
        public void UsesProvidedLambda()
        {
            Comparison<int> comparer = (x, y) => x.CompareTo(y);
            ComparisonConstraint.Using(comparer).ApplyTo(0);
        }
    }

    #endregion

    #region Comparison Test Classes

    class ClassWithIComparable : IComparable
    {
        private int _val;

        public ClassWithIComparable(int val)
        {
            _val = val;
        }

        public int CompareTo(object x)
        {
            ClassWithIComparable other = x as ClassWithIComparable;
            if (x is ClassWithIComparable)
                return _val.CompareTo(other._val);

            throw new ArgumentException();
        }
    }

    class ClassWithIComparableOfT : IComparable<ClassWithIComparableOfT>
    {
        private int _val;

        public ClassWithIComparableOfT(int val)
        {
            _val = val;
        }

        public int CompareTo(ClassWithIComparableOfT other)
        {
            return _val.CompareTo(other._val);
        }
    }

    #endregion
}