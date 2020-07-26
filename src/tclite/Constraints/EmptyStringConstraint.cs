// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EmptyStringConstraint tests whether a string is empty.
    /// </summary>
    public class EmptyStringConstraint : ConditionConstraint<string>
    {
        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return "<empty>"; }
        }

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentOfType<string>(actual, nameof(actual));
        }

        /// <summary>
        /// Applies the constraint to an actual value, of the same type as
        /// the constraint expected value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new ConstraintResult(this, actual, actual as string == string.Empty);
        }
    }
}