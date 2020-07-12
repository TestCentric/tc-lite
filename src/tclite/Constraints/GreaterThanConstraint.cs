// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Tests whether a value is greater than the value supplied to its constructor
    /// </summary>
    public class GreaterThanConstraint<TExpected> : ComparisonConstraint<TExpected>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:GreaterThanConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public GreaterThanConstraint(TExpected expected) : base(expected) { }

        public override string Description => $"greater than {ExpectedValue}";

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches<T>(T actual)
        {
            if (ExpectedValue == null || actual == null)
                throw new ArgumentException("Cannot compare using a null reference");

            return Comparer.Compare(actual, ExpectedValue) > 0;
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public GreaterThanConstraint<T> GreaterThan<T>(T expected)
        {
            return (GreaterThanConstraint<T>)Append(new GreaterThanConstraint<T>(expected));
        }
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public static GreaterThanConstraint<T> GreaterThan<T>(T expected)
        {
            return new GreaterThanConstraint<T>(expected);
        }
    }
}
