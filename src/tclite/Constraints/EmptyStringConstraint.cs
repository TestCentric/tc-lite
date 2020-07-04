// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EmptyStringConstraint tests whether a string is empty.
    /// </summary>
    public class EmptyStringConstraint : Constraint<string>
    {
        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return "<empty>"; }
        }

        /// <summary>
        /// Applies the constraint to an actual value, of the same type as
        /// the constraint expected value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public override ConstraintResult ApplyTo<T>(T actual)
        {
            Guard.ArgumentNotNullOfType<string>(actual, nameof(actual));
            return new ConstraintResult(this, actual, actual as string == string.Empty);
        }

        /// <summary>
        /// Applies the constraint to an actual value, of the same type as
        /// the constraint expected value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public override ConstraintResult ApplyTo(string actual)
        {
            return new ConstraintResult(this, actual, actual == string.Empty);
        }
    }
}