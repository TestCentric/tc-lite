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
        protected ComparisonConstraint _comparisonConstraint;

        [Test]
        public void UsesProvidedIComparer()
        {
            SimpleObjectComparer comparer = new SimpleObjectComparer();
            _comparisonConstraint.Using(comparer).ApplyTo(0);
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [Test]
        public void UsesProvidedComparerOfT()
        {
            MyComparer<int> comparer = new MyComparer<int>();
            _comparisonConstraint.Using(comparer).ApplyTo(0);
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
            _comparisonConstraint.Using(new Comparison<int>(comparer.Compare)).ApplyTo(0);
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
            _comparisonConstraint.Using(comparer).ApplyTo(0);
        }
#endif
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