// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Tests whether a value is greater than or equal to the value supplied to its constructor
    /// </summary>
    public class GreaterThanOrEqualConstraint : ComparisonConstraint
    {
        /// <summary>
        /// The value against which a comparison is to be made
        /// </summary>
        private object ExpectedValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GreaterThanOrEqualConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public GreaterThanOrEqualConstraint(IComparable expected)
            : base(expected)
        {
            ExpectedValue = expected;
        }

        public override string Description => $"greater than or equal to {ExpectedValue}";

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches(IComparable actual)
        {
            if (ExpectedValue == null || actual == null)
                throw new ArgumentException("Cannot compare using a null reference");

            return Comparer.Compare(actual, ExpectedValue) >= 0;
        }
    }
}
