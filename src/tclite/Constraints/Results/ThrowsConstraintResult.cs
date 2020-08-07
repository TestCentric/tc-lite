// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;

namespace TCLite.Constraints
{
    public sealed class ThrowsConstraintResult : ConstraintResult
    {
        private readonly ConstraintResult baseResult;

        public ThrowsConstraintResult(ThrowsConstraint constraint,
            Exception caughtException,
            ConstraintResult baseResult)
            : base(constraint, caughtException)
        {
            if (caughtException != null && baseResult.IsSuccess)
                Status = ConstraintStatus.Success;
            else
                Status = ConstraintStatus.Failure;

            this.baseResult = baseResult;
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. This override only handles the special message
        /// used when an exception is expected but none is thrown.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            if (ActualValue == null)
                writer.Write("no exception thrown");
            else
                baseResult.WriteActualValueTo(writer);
        }
    }
}
