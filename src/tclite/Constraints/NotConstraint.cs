// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// NotConstraint negates the effect of some other constraint
    /// </summary>
    public class NotConstraint : PrefixConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotConstraint"/> class.
        /// </summary>
        /// <param name="baseConstraint">The base constraint to be negated.</param>
        public NotConstraint(IConstraint baseConstraint)
            : base(baseConstraint)
        {
            DescriptionPrefix = "not";
        }

        public override void ValidateActualValue(object actual)
        {
            BaseConstraint.ValidateActualValue(actual);
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for if the base constraint fails, false if it succeeds</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            var baseResult = BaseConstraint.ApplyTo(actual);
            return new ConstraintResult(this, baseResult.ActualValue, !baseResult.IsSuccess);
        }
    }
}