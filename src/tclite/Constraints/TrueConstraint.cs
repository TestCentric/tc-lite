// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// TrueConstraint tests that the actual value is true
    /// </summary>
    public class TrueConstraint : Constraint<bool>
    {
        public override string Description => "True";

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<bool>(actual, nameof(actual));
        }

        public override ConstraintResult ApplyTo(bool actual)
        {
            return new ConstraintResult(this, actual, true.Equals(actual));
        }

        public override ConstraintResult ApplyTo<T>(T actual)
        {
            return new ConstraintResult(this, actual, true.Equals(actual));
        }
    }
}