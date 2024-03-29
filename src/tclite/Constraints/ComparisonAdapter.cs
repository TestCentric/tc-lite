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
    /// ComparisonAdapter class centralizes all comparisons of
    /// values in NUnit, adapting to the use of any provided
    /// IComparer, IComparer&lt;T&gt; or Comparison&lt;T&gt;
    /// </summary>
    public abstract class ComparisonAdapter
    {
        /// <summary>
        /// Gets the default ComparisonAdapter, which wraps an
        /// NUnitComparer object.
        /// </summary>
        public static ComparisonAdapter Default
        {
            get { return new DefaultComparisonAdapter(); }
        }

        /// <summary>
        /// Returns a ComparisonAdapter that wraps an IComparer
        /// </summary>
        public static ComparisonAdapter For(IComparer comparer)
        {
            return new ComparerAdapter(comparer);
        }

        /// <summary>
        /// Returns a ComparisonAdapter that wraps an IComparer&lt;T&gt;
        /// </summary>
        public static ComparisonAdapter For<T>(IComparer<T> comparer)
        {
            return new ComparerAdapter<T>(comparer);
        }

        /// <summary>
        /// Returns a ComparisonAdapter that wraps a Comparison&lt;T&gt;
        /// </summary>
        public static ComparisonAdapter For<T>(Comparison<T> comparer)
        {
            return new ComparisonAdapterForComparison<T>(comparer);
        }

        /// <summary>
        /// Compares two objects
        /// </summary>
        public abstract int Compare(object expected, object actual);

        class DefaultComparisonAdapter : ComparerAdapter
        {
            /// <summary>
            /// Construct a default ComparisonAdapter
            /// </summary>
            public DefaultComparisonAdapter() : base( TCLiteComparer.Default ) { }
        }

        class ComparerAdapter : ComparisonAdapter
        {
            private readonly IComparer _comparer;

            /// <summary>
            /// Construct a ComparisonAdapter for an IComparer
            /// </summary>
            public ComparerAdapter(IComparer comparer)
            {
                _comparer = comparer;
            }

            /// <summary>
            /// Compares two objects
            /// </summary>
            /// <param name="expected"></param>
            /// <param name="actual"></param>
            /// <returns></returns>
            public override int Compare(object expected, object actual)
            {
                return _comparer.Compare(expected, actual);
            }
        }

        /// <summary>
        /// ComparisonAdapter&lt;T&gt; extends ComparisonAdapter and
        /// allows use of an IComparer&lt;T&gt; or Comparison&lt;T&gt;
        /// to actually perform the comparison.
        /// </summary>
        class ComparerAdapter<T> : ComparisonAdapter
        {
            private readonly IComparer<T> _comparer;

            /// <summary>
            /// Construct a ComparisonAdapter for an IComparer&lt;T&gt;
            /// </summary>
            public ComparerAdapter(IComparer<T> comparer)
            {
                _comparer = comparer;
            }

            /// <summary>
            /// Compare a Type T to an object
            /// </summary>
            public override int Compare(object expected, object actual)
            {
                if (!typeof(T).IsAssignableFrom(expected.GetType()))
                    throw new ArgumentException("Cannot compare " + expected.ToString());

                if (!typeof(T).IsAssignableFrom(actual.GetType()))
                    throw new ArgumentException("Cannot compare to " + actual.ToString());

                return _comparer.Compare((T)expected, (T)actual);
            }
        }

        class ComparisonAdapterForComparison<T> : ComparisonAdapter
        {
            private readonly Comparison<T> _comparison;

            /// <summary>
            /// Construct a ComparisonAdapter for a Comparison&lt;T&gt;
            /// </summary>
            public ComparisonAdapterForComparison(Comparison<T> comparer)
            {
                _comparison = comparer;
            }

            /// <summary>
            /// Compare a Type T to an object
            /// </summary>
            public override int Compare(object expected, object actual)
            {
                if (!typeof(T).IsAssignableFrom(expected.GetType()))
                    throw new ArgumentException("Cannot compare " + expected.ToString());

                if (!typeof(T).IsAssignableFrom(actual.GetType()))
                    throw new ArgumentException("Cannot compare to " + actual.ToString());

                return _comparison.Invoke((T)expected, (T)actual);
            }
        }
    }
}
