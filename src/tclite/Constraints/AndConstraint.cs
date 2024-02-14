// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// AndConstraint succeeds only if both members succeed.
    /// </summary>
    public class AndConstraint : BinaryConstraint
    {
        /// <summary>
        /// Create an AndConstraint from two other constraints
        /// </summary>
        /// <param name="left">The first constraint</param>
        /// <param name="right">The second constraint</param>
        public AndConstraint(IConstraint left, IConstraint right) : base(left, right) { }

        public override string Description => $"{Left.Description} and {Right.Description}";

        /// <summary>
        /// Apply both member constraints to an actual value, succeeding 
        /// succeeding only if both of them succeed.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <returns>True if the constraints both succeeded</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            var leftResult = Left.ApplyTo(actual);
            var rightResult = leftResult.IsSuccess
                ? Right.ApplyTo(actual)
                : new ConstraintResult(Right, actual);

            return new AndConstraintResult(this, actual, leftResult, rightResult);
        }
    }
}