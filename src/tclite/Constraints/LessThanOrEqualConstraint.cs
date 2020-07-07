// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public LessThanOrEqualConstraint<T> LessThanOrEqualTo<T>(T expected)
        {
            return (LessThanOrEqualConstraint<T>)Append(new LessThanOrEqualConstraint<T>(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public LessThanOrEqualConstraint<T> AtMost<T>(T expected)
        {
            return (LessThanOrEqualConstraint<T>)Append(new LessThanOrEqualConstraint<T>(expected));
        }
    }

    /// <summary>
    /// Tests whether a value is less than or equal to the value supplied to its constructor
    /// </summary>
    public class LessThanOrEqualConstraint<TExpected> : ComparisonConstraint<TExpected>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LessThanOrEqualConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public LessThanOrEqualConstraint(TExpected expected) : base(expected) { }

        public override string Description => $"less than or equal to {ExpectedValue}";

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches(IComparable actual)
        {
            if (ExpectedValue == null || actual == null)
                throw new ArgumentException("Cannot compare using a null reference");

            return Comparer.Compare(actual, ExpectedValue) <= 0;
        }
    }
}
