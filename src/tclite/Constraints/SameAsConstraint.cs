// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// SameAsConstraint tests whether an object is identical to
    /// the object passed to its constructor
    /// </summary>
    public class SameAsConstraint<TExpected> : ExpectedValueConstraint<TExpected> where TExpected : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SameAsConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected object.</param>
        public SameAsConstraint(TExpected expected) : base(expected) { }

        public override string Description => $"Same as {ExpectedValue}";

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            bool hasSucceeded = ReferenceEquals(ExpectedValue, actual);

            return new ConstraintResult(this, actual, hasSucceeded);
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests that two references are the same object
        /// </summary>
        public SameAsConstraint<T> SameAs<T>(T expected) where T : class
        {
            return (SameAsConstraint<T>)Append(new SameAsConstraint<T>(expected));
        }
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a constraint that tests that two references are the same object
        /// </summary>
        public static SameAsConstraint<T> SameAs<T>(T expected) where T : class
        {
            return new SameAsConstraint<T>(expected);
        }
    }
}
