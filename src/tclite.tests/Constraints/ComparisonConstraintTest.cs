// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints
{
    #region ComparisonConstraintTest

    public abstract class ComparisonConstraintTest : ConstraintTestBase<object>
    {
        private ComparisonConstraint ComparisonConstraint => Constraint as ComparisonConstraint;

        protected override TestCaseData[] InvalidData => new TestCaseData[]
        { 
            new TestCaseData(null, typeof(ArgumentException)),
            new TestCaseData("xxx", typeof(ArgumentException))
        };

        [Test]
        public void UsesProvidedIComparer()
        {
            SimpleObjectComparer comparer = new SimpleObjectComparer();
            ComparisonConstraint.Using(comparer).ApplyTo(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [Test]
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

        [Test]
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

        [Test]
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