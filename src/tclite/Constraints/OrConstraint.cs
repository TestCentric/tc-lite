// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// OrConstraint succeeds if either member succeeds{{
    /// </summary>
    public class OrConstraint : BinaryConstraint
    {
        /// <summary>
        /// Create an OrConstraint from two other constraints
        /// </summary>
        /// <param name="left">The first constraint</param>
        /// <param name="right">The second constraint</param>
        public OrConstraint(IConstraint left, IConstraint right) : base(left, right) { }

        public override string  Description => $"{Left.Description} or {Right.Description}";

        /// <summary>
        /// Apply the member constraints to an actual value, succeeding 
        /// succeeding as soon as one of them succeeds.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <returns>True if either constraint succeeded</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            bool hasSucceeded = Left.ApplyTo(actual).IsSuccess || Right.ApplyTo(actual).IsSuccess;
            return new ConstraintResult(this, actual, hasSucceeded);
        }
    }
}