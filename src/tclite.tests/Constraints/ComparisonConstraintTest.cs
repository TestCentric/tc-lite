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

    public abstract class ComparisonConstraintTest : ConstraintTestBaseWithArgumentException
    {
        protected ComparisonConstraint comparisonConstraint;

        [Test]
        public void UsesProvidedIComparer()
        {
            SimpleObjectComparer comparer = new SimpleObjectComparer();
            comparisonConstraint.Using(comparer).Matches(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [Test]
        public void UsesProvidedComparerOfT()
        {
            MyComparer<int> comparer = new MyComparer<int>();
            comparisonConstraint.Using(comparer).Matches(0);
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
            comparisonConstraint.Using(new Comparison<int>(comparer.Compare)).Matches(0);
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

#if !NETCF_2_0
        [Test]
        public void UsesProvidedLambda()
        {
            Comparison<int> comparer = (x, y) => x.CompareTo(y);
            comparisonConstraint.Using(comparer).Matches(0);
        }
#endif
    }

    #endregion

    #region Comparison Test Classes

    class ClassWithIComparable : IComparable
    {
        private int val;

        public ClassWithIComparable(int val)
        {
            this.val = val;
        }

        public int CompareTo(object x)
        {
            ClassWithIComparable other = x as ClassWithIComparable;
            if (x is ClassWithIComparable)
                return val.CompareTo(other.val);

            throw new ArgumentException();
        }
    }

    class ClassWithIComparableOfT : IComparable<ClassWithIComparableOfT>
    {
        private int val;

        public ClassWithIComparableOfT(int val)
        {
            this.val = val;
        }

        public int CompareTo(ClassWithIComparableOfT other)
        {
            return val.CompareTo(other.val);
        }
    }

    #endregion
}