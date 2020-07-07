// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests for False
        /// </summary>
        public FalseConstraint False
        {
            get { return (FalseConstraint)this.Append(new FalseConstraint()); }
        }
    }

    /// <summary>
    /// FalseConstraint tests that the actual value is false
    /// </summary>
    public class FalseConstraint : ConditionConstraint<bool>
    {
        public override string Description => "False";

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<bool>(actual, nameof(actual));
        }

        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new ConstraintResult(this, actual, false.Equals(actual));
        }
    }
}