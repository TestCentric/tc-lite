﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Constraints
{
    /// <summary>
    /// EqualityAdapter class handles all equality comparisons
    /// that use an IEqualityComparer, IEqualityComparer&lt;T&gt;
    /// or a ComparisonAdapter.
    /// </summary>
    internal abstract class EqualityAdapter
    {
        /// <summary>
        /// Compares two objects, returning true if they are equal
        /// </summary>
        public abstract bool AreEqual(object x, object y);

        /// <summary>
        /// Returns true if the two objects can be compared by this adapter.
        /// The base adapter cannot handle IEnumerables except for strings.
        /// </summary>
        public virtual bool CanCompare(object x, object y)
        {
            if (x is string && y is string)
                return true;

            if (x is IEnumerable || y is IEnumerable)
                return false;

            return true;
        }

        #region Nested IComparer Adapter

        /// <summary>
        /// Returns an EqualityAdapter that wraps an IComparer.
        /// </summary>
        public static EqualityAdapter For(IComparer comparer)
        {
            return new ComparerAdapter(comparer);
        }

        /// <summary>
        /// EqualityAdapter that wraps an IComparer.
        /// </summary>
        class ComparerAdapter : EqualityAdapter
        {
            private IComparer comparer;

            public ComparerAdapter(IComparer comparer)
            {
                this.comparer = comparer;
            }

            public override bool AreEqual(object x, object y)
            {
                return comparer.Compare(x, y) == 0;
            }
        }

        #endregion

        #region Nested IEqualityComparer Adapter

        /// <summary>
        /// Returns an EqualityAdapter that wraps an IEqualityComparer.
        /// </summary>
        public static EqualityAdapter For(IEqualityComparer comparer)
        {
            return new EqualityComparerAdapter(comparer);
        }

        class EqualityComparerAdapter : EqualityAdapter
        {
            private IEqualityComparer comparer;

            public EqualityComparerAdapter(IEqualityComparer comparer)
            {
                this.comparer = comparer;
            }

            public override bool AreEqual(object x, object y)
            {
                return comparer.Equals(x, y);
            }
        }

        #endregion

        #region Nested GenericEqualityAdapter<T>

        abstract class GenericEqualityAdapter<T> : EqualityAdapter
        {
            /// <summary>
            /// Returns true if the two objects can be compared by this adapter.
            /// Generic adapter requires objects of the specified type.
            /// </summary>
            public override bool CanCompare(object x, object y)
            {
                return typeof(T).IsAssignableFrom(x.GetType())
                    && typeof(T).IsAssignableFrom(y.GetType());
            }

            protected void ThrowIfNotCompatible(object x, object y)
            {
                if (!typeof(T).IsAssignableFrom(x.GetType()))
                    throw new ArgumentException("Cannot compare " + x.ToString());

                if (!typeof(T).IsAssignableFrom(y.GetType()))
                    throw new ArgumentException("Cannot compare " + y.ToString());
            }
        }

        #endregion

        #region Nested IEqualityComparer<T> Adapter
        
        /// <summary>
        /// Returns an EqualityAdapter that wraps an IEqualityComparer&lt;T&gt;.
        /// </summary>
        public static EqualityAdapter For<T>(IEqualityComparer<T> comparer)
        {
            return new EqualityComparerAdapter<T>(comparer);
        }

        class EqualityComparerAdapter<T> : GenericEqualityAdapter<T>
        {
            private IEqualityComparer<T> comparer;

            public EqualityComparerAdapter(IEqualityComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public override bool AreEqual(object x, object y)
            {
                ThrowIfNotCompatible(x, y);
                return comparer.Equals((T)x, (T)y);
            }
        }

        #endregion

        #region Nested IComparer<T> Adapter

        /// <summary>
        /// Returns an EqualityAdapter that wraps an IComparer&lt;T&gt;.
        /// </summary>
        public static EqualityAdapter For<T>(IComparer<T> comparer)
        {
            return new ComparerAdapter<T>(comparer);
        }

        /// <summary>
        /// EqualityAdapter that wraps an IComparer.
        /// </summary>
        class ComparerAdapter<T> : GenericEqualityAdapter<T>
        {
            private IComparer<T> comparer;

            public ComparerAdapter(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public override bool AreEqual(object x, object y)
            {
                ThrowIfNotCompatible(x, y);
                return comparer.Compare((T)x, (T)y) == 0;
            }
        }

        #endregion

        #region Nested Comparison<T> Adapter

        /// <summary>
        /// Returns an EqualityAdapter that wraps a Comparison&lt;T&gt;.
        /// </summary>
        public static EqualityAdapter For<T>(Comparison<T> comparer)
        {
            return new ComparisonAdapter<T>(comparer);
        }

        class ComparisonAdapter<T> : GenericEqualityAdapter<T>
        {
            private Comparison<T> comparer;

            public ComparisonAdapter(Comparison<T> comparer)
            {
                this.comparer = comparer;
            }

            public override bool AreEqual(object x, object y)
            {
                ThrowIfNotCompatible(x, y);
                return comparer.Invoke((T)x, (T)y) == 0;
            }
        }

        #endregion
    }

    /// <summary>
    /// EqualityAdapterList represents a list of EqualityAdapters
    /// in a common class across platforms.
    /// </summary>
    class EqualityAdapterList : System.Collections.Generic.List<EqualityAdapter> { }
}
