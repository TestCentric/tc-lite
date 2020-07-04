// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// NaNConstraint tests that the actual value is a double or float NaN
    /// </summary>
    public class NaNConstraint : Constraint<double>
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

        /// <summary>
        /// Test that the actual value is an NaN
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        public override ConstraintResult ApplyTo(double actual)
        {
            return new ConstraintResult(this, actual, double.IsNaN(actual));
        }

        public override ConstraintResult ApplyTo<T>(T actual)
        {
            return new ConstraintResult(this, actual,
                actual is double && double.IsNaN((double)(object)actual) ||
                actual is float && float.IsNaN((float)(object)actual));
        }
    }
}