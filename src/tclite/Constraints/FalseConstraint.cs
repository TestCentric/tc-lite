// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// FalseConstraint tests that the actual value is false
    /// </summary>
    public class FalseConstraint : Constraint<bool>
    {
        public override string Description => "False";

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<bool>(actual, nameof(actual));
        }

        public override ConstraintResult ApplyTo(bool actual)
        {
            return new ConstraintResult(this, actual, false.Equals(actual));
        }

        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new ConstraintResult(this, actual, false.Equals(actual));
        }
    }
}