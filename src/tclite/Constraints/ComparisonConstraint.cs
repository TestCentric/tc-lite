// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Constraints
{
    /// <summary>
    /// Abstract base class for constraints that compare values to
    /// determine if one is greater than, equal to or less than
    /// the other. This class supplies the Using modifiers.
    /// </summary>
    public abstract class ComparisonConstraint<TExpected> : ExpectedValueConstraint<TExpected>
    {
        /// <summary>
        /// ComparisonAdapter to be used in making the comparison
        /// </summary>
        protected ComparisonAdapter Comparer { get; set; } = ComparisonAdapter.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ComparisonConstraint"/> class.
        /// </summary>
        public ComparisonConstraint(TExpected arg) : base(arg) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ComparisonConstraint"/> class.
        /// </summary>
        //public ComparisonConstraint(object arg1, object arg2) : base(arg1, arg2) { }

        /// <summary>
        /// Modifies the constraint to use an IComparer and returns self
        /// </summary>
        public ComparisonConstraint<TExpected> Using(IComparer comparer)
        {
            Comparer = ComparisonAdapter.For(comparer);
            return this;
        }

        /// <summary>
        /// Modifies the constraint to use an IComparer&lt;T&gt; and returns self
        /// </summary>
        public ComparisonConstraint<TExpected> Using<T>(IComparer<T> comparer)
        {
            Comparer = ComparisonAdapter.For(comparer);
            return this;
        }

        /// <summary>
        /// Modifies the constraint to use a Comparison&lt;T&gt; and returns self
        /// </summary>
        public ComparisonConstraint<TExpected> Using<T>(Comparison<T> comparer)
        {
            Comparer = ComparisonAdapter.For(comparer);
            return this;
        }

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            if (!(actual is TExpected))
                Guard.ArgumentValid(
                    Numerics.IsNumericType(actual) && Numerics.IsNumericType(typeof(TExpected)),
                    "Comparisons between objects of different types require both to be numeric types.",
                    nameof(actual));
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value   
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new ConstraintResult(this, actual, Matches(actual));
        }

        /// <summary>
        /// Protected function overridden by derived class to actually perform the comparison
        /// </summary>
        protected abstract bool Matches<T>(T actual);
    }
}
