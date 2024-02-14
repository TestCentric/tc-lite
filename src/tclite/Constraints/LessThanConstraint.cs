// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Constraints
{
    /// <summary>
    /// Tests whether a value is less than the value supplied to its constructor
    /// </summary>
    public class LessThanConstraint<TExpected> : ComparisonConstraint<TExpected>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LessThanConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public LessThanConstraint(TExpected expected) : base(expected) { }

        public override string Description => $"less than {ExpectedValue}";

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches<T>(T actual)
        {
            if (ExpectedValue == null || actual == null)
                throw new ArgumentException("Cannot compare using a null reference");

            return Comparer.Compare(actual, ExpectedValue) < 0;
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than the suppled argument
        /// </summary>
        public LessThanConstraint<T> LessThan<T>(T expected)
        {
            return (LessThanConstraint<T>)Append(new LessThanConstraint<T>(expected));
        }

        /// <summary>
        /// Returns a constraint that tests for a negative value
        /// </summary>
        public LessThanConstraint<int> Negative
        {
            get { return (LessThanConstraint<int>)this.Append(new LessThanConstraint<int>(0)); }
        }
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than the suppled argument
        /// </summary>
        public static LessThanConstraint<T> LessThan<T>(T expected)
        {
            return new LessThanConstraint<T>(expected);
        }

        /// <summary>
        /// Returns a constraint that tests for a negative value
        /// </summary>
        public static LessThanConstraint<int> Negative
        {
            get { return new LessThanConstraint<int>(0); }
        }
    }
}
