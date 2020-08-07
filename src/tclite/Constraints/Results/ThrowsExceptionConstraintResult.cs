// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Internal;

namespace TCLite.Constraints
{
    public class ThrowsExceptionConstraintResult : ConstraintResult
    {
        public ThrowsExceptionConstraintResult(ThrowsExceptionConstraint constraint, Exception caughtException)
            : base(constraint, caughtException, caughtException != null) { }

        public override void WriteActualValueTo(MessageWriter writer)
        {
            if (this.Status == ConstraintStatus.Failure)
                writer.Write("no exception thrown");
            else
                base.WriteActualValueTo(writer);
        }
    }
}
