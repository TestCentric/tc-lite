// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// NullConstraint tests that the actual value is null
    /// </summary>
    public class NullConstraint : Constraint
    {
        public override string Description => "null";       

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public override ConstraintResult ApplyTo<T>(T actual)
        {
            return new ConstraintResult(this, actual, actual == null);
        }
    }
}