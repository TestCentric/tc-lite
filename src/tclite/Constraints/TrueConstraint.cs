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

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Guard.ArgumentIsRequiredType<bool>(actual, nameof(actual));
            return new ConstraintResult(this, actual, true.Equals(actual));
        }

        public override ConstraintResult ApplyTo(object actual)
        {
            Guard.ArgumentIsRequiredType<bool>(actual, nameof(actual));
            return new ConstraintResult(this, actual, true.Equals(actual));
        }
    }
}