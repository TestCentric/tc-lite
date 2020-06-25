// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// FalseConstraint tests that the actual value is false
    /// </summary>
    public class FalseConstraint : Constraint
    {
        public override string Description => "False";

        // public override ConstraintResult ApplyTo<TActual>(TActual actual)
        // {
        //     Guard.ArgumentNotNullOfType<bool>(actual, nameof(actual));
        //     return new ConstraintResult(this, actual, false.Equals(actual));
        // }

        public override ConstraintResult ApplyTo(object actual)
        {
            Guard.ArgumentNotNullOfType<bool>(actual, nameof(actual));
            return new ConstraintResult(this, actual, false.Equals(actual));
        }
    }
}