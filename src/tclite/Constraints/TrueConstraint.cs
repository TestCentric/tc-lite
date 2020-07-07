// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests for True
        /// </summary>
        public TrueConstraint True
        {
            get { return (TrueConstraint)this.Append(new TrueConstraint()); }
        }
    }

    /// <summary>
    /// TrueConstraint tests that the actual value is true
    /// </summary>
    public class TrueConstraint : ConditionConstraint<bool>
    {
        public override string Description => "True";

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<bool>(actual, nameof(actual));
        }

        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new ConstraintResult(this, actual, true.Equals(actual));
        }
    }
}