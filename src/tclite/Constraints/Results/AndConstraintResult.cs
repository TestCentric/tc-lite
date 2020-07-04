// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
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
}