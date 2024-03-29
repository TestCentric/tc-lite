// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Constraints
{
    /// <summary>
    /// CollectionConstraint is the abstract base class for
    /// constraints that operate on collections.
    /// </summary>
    public abstract class CollectionConstraint : ConditionConstraint<IEnumerable>
    {
        /// <summary>
        /// Construct an empty CollectionConstraint
        /// </summary>
        protected CollectionConstraint() { }

        /// <summary>
        /// Construct a CollectionConstraint
        /// </summary>
        /// <param name="arg"></param>
        protected CollectionConstraint(object arg) : base(arg) { }

        /// <summary>
        /// Determines whether the specified enumerable is empty.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// <c>true</c> if the specified enumerable is empty; otherwise, <c>false</c>.
        /// </returns>
        protected static bool IsEmpty(IEnumerable enumerable)
        {
            ICollection collection = enumerable as ICollection;
            if (collection != null)
                return collection.Count == 0;

            foreach (object o in enumerable)
                return false;

            return true;
        }

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<IEnumerable>(actual, nameof(actual));
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            var enumerable = actual as IEnumerable;

            return new ConstraintResult(this, enumerable, Match(enumerable));
        }

        /// <summary>
        /// Protected method to be implemented by derived classes
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected abstract bool Match(IEnumerable collection);
    }
}