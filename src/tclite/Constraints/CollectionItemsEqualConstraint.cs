// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Constraints
{
    /// <summary>
    /// CollectionItemsEqualConstraint is the abstract base class for all
    /// collection constraints that apply some notion of item equality
    /// as a part of their operation.
    /// </summary>
    public abstract class CollectionItemsEqualConstraint : CollectionConstraint
    {
        private readonly TCLiteEqualityComparer _comparer = new TCLiteEqualityComparer();

        /// <summary>
        /// Construct an empty CollectionConstraint
        /// </summary>
        protected CollectionItemsEqualConstraint() { }

        /// <summary>
        /// Construct a CollectionConstraint
        /// </summary>
        /// <param name="arg"></param>
        protected CollectionItemsEqualConstraint(object arg) : base(arg) { }

        /// <summary>
        /// Get a flag indicating whether the user requested us to ignore case.
        /// </summary>
        protected bool IgnoringCase => _comparer.IgnoreCase;

        /// <summary>
        /// Get a flag indicating whether any external comparers are in use.
        /// </summary>
        protected bool UsingExternalComparer => _comparer.ExternalComparers.Count > 0;

        #region Modifiers

        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public CollectionItemsEqualConstraint IgnoreCase
        {
            get
            {
                _comparer.IgnoreCase = true;
                return this;
            }
        }

        /// <summary>
        /// Flag the constraint to use the supplied EqualityAdapter.
        /// NOTE: For internal use only.
        /// </summary>
        /// <param name="adapter">The EqualityAdapter to use.</param>
        /// <returns>Self.</returns>
        internal CollectionItemsEqualConstraint Using(EqualityAdapter adapter)
        {
            _comparer.ExternalComparers.Add(adapter);
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using(IComparer comparer)
        {
            return Using(EqualityAdapter.For(comparer));
        }

        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using<T>(IComparer<T> comparer)
        {
            return Using(EqualityAdapter.For(comparer));
        }

        /// <summary>
        /// Flag the constraint to use the supplied Comparison object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using<T>(Comparison<T> comparer)
        {
            return Using(EqualityAdapter.For(comparer));
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using(IEqualityComparer comparer)
        {
            return Using(EqualityAdapter.For(comparer));
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public CollectionItemsEqualConstraint Using<T>(IEqualityComparer<T> comparer)
        {
            return Using(EqualityAdapter.For(comparer));
        }

        #endregion

        /// <summary>
        /// Compares two collection members for equality
        /// </summary>
        protected bool ItemsEqual(object x, object y)
        {
            Tolerance tolerance = Tolerance.Exact;
            return _comparer.AreEqual(x, y, ref tolerance);
        }

        /// <summary>
        /// Return a new CollectionTally for use in making tests
        /// </summary>
        /// <param name="c">The collection to be included in the tally</param>
        protected CollectionTally Tally(IEnumerable c)
        {
            return new CollectionTally(_comparer, c);
        }
    }
}
