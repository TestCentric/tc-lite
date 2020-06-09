// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// CollectionItemsEqualConstraint is the abstract base class for all
    /// collection constraints that apply some notion of item equality
    /// as a part of their operation.
    /// </summary>
    public abstract class CollectionItemsEqualConstraint : CollectionConstraint
    {
        private readonly NUnitEqualityComparer comparer = NUnitEqualityComparer.Default;

        /// <summary>
        /// Construct an empty CollectionConstraint
        /// </summary>
        protected CollectionItemsEqualConstraint() { }

        /// <summary>
        /// Construct a CollectionConstraint
        /// </summary>
        /// <param name="arg"></param>
        protected CollectionItemsEqualConstraint(object arg) : base(arg) { }

        #region Modifiers

        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public CollectionItemsEqualConstraint IgnoreCase
        {
            get
            {
                comparer.IgnoreCase = true;
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
            this.comparer.ExternalComparers.Add(adapter);
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
            Tolerance tolerance = Tolerance.Zero;
            return comparer.AreEqual(x, y, ref tolerance);
        }

#if NYI
        /// <summary>
        /// Return a new CollectionTally for use in making tests
        /// </summary>
        /// <param name="c">The collection to be included in the tally</param>
        protected CollectionTally Tally(IEnumerable c)
        {
            return new CollectionTally(comparer, c);
        }
#endif
    }
}
