// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
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
        public AndConstraint(Constraint left, Constraint right) : base(left, right) { }

        public override string Description => $"{Left.Description} and {Right.Description}";

        /// <summary>
        /// Apply both member constraints to an actual value, succeeding 
        /// succeeding only if both of them succeed.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <returns>True if the constraints both succeeded</returns>
        // public override ConstraintResult ApplyTo<TActual>(TActual actual)
        // {
        //     var leftResult = Left.ApplyTo(actual);
        //     var rightResult = leftResult.IsSuccess
        //         ? Right.ApplyTo(actual)
        //         : new ConstraintResult(Right, actual);

        //     return new AndConstraintResult(this, actual, leftResult, rightResult);
        // }

        /// <summary>
        /// Apply both member constraints to an actual value, succeeding 
        /// succeeding only if both of them succeed.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <returns>True if the constraints both succeeded</returns>
        public override ConstraintResult ApplyTo(object actual)
        {
            var leftResult = Left.ApplyTo(actual);
            var rightResult = leftResult.IsSuccess
                ? Right.ApplyTo(actual)
                : new ConstraintResult(Right, actual);

            return new AndConstraintResult(this, actual, leftResult, rightResult);
        }

        #region Nested Result Class

        class AndConstraintResult : ConstraintResult
        {
            private readonly ConstraintResult leftResult;
            private readonly ConstraintResult rightResult;

            public AndConstraintResult(AndConstraint constraint, object actual, ConstraintResult leftResult, ConstraintResult rightResult)
                : base(constraint, actual, leftResult.IsSuccess && rightResult.IsSuccess)
            {
                this.leftResult = leftResult;
                this.rightResult = rightResult;
            }

            /// <summary>
            /// Write the actual value for a failing constraint test to a
            /// MessageWriter. The default implementation simply writes
            /// the raw value of actual, leaving it to the writer to
            /// perform any formatting.
            /// </summary>
            /// <param name="writer">The writer on which the actual value is displayed</param>
            public override void WriteActualValueTo(MessageWriter writer)
            {
                if (this.IsSuccess)
                    base.WriteActualValueTo(writer);
                else if (!leftResult.IsSuccess)
                    leftResult.WriteActualValueTo(writer);
                else
                    rightResult.WriteActualValueTo(writer);
            }

            public override void WriteAdditionalLinesTo(MessageWriter writer)
            {
                if (this.IsSuccess)
                    base.WriteAdditionalLinesTo(writer);
                else if (!leftResult.IsSuccess)
                    leftResult.WriteAdditionalLinesTo(writer);
                else
                    rightResult.WriteAdditionalLinesTo(writer);
            }
        }

        #endregion
    }
}