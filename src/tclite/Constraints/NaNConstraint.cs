// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests for NaN
        /// </summary>
        public NaNConstraint NaN
        {
            get { return (NaNConstraint)this.Append(new NaNConstraint()); }
        }
    }

    /// <summary>
    /// NaNConstraint tests that the actual value is a double or float NaN
    /// </summary>
    public class NaNConstraint : ConditionConstraint<double>
    {
        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return "NaN"; }
        }

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<double>(actual, nameof(actual));
        }

        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new ConstraintResult(this, actual,
                actual is double && double.IsNaN((double)(object)actual) ||
                actual is float && float.IsNaN((float)(object)actual));
        }
    }
}