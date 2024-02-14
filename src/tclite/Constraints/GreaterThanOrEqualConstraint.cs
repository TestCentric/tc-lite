// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Constraints
{
    /// <summary>
    /// Tests whether a value is greater than or equal to the value supplied to its constructor
    /// </summary>
    public class GreaterThanOrEqualConstraint<TExpected> : ComparisonConstraint<TExpected>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:GreaterThanOrEqualConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public GreaterThanOrEqualConstraint(TExpected expected) : base(expected) { }

        public override string Description => $"greater than or equal to {ExpectedValue}";

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches<T>(T actual)
        {
            if (ExpectedValue == null || actual == null)
                throw new ArgumentException("Cannot compare using a null reference");

            return Comparer.Compare(actual, ExpectedValue) >= 0;
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public GreaterThanOrEqualConstraint<T> GreaterThanOrEqualTo<T>(T expected)
        {
            return (GreaterThanOrEqualConstraint<T>)this.Append(new GreaterThanOrEqualConstraint<T>(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public GreaterThanOrEqualConstraint<T> AtLeast<T>(T expected)
        {
            return (GreaterThanOrEqualConstraint<T>)this.Append(new GreaterThanOrEqualConstraint<T>(expected));
        }
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public static GreaterThanOrEqualConstraint<T> GreaterThanOrEqualTo<T>(T expected)
        {
            return new GreaterThanOrEqualConstraint<T>(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public static GreaterThanOrEqualConstraint<T> AtLeast<T>(T expected)
        {
            return new GreaterThanOrEqualConstraint<T>(expected);
        }
    }
}
