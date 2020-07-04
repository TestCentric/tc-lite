// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    public class ExceptionTypeConstraintResult : ConstraintResult
    {
        private readonly object _caughtException;

        public ExceptionTypeConstraintResult(ExceptionTypeConstraint constraint, object caughtException, Type type, bool matches)
            : base(constraint, type, matches)
        {
            this._caughtException = caughtException;
        }

        public override void WriteActualLineTo(MessageWriter writer)
        {
            if (this.Status == ConstraintStatus.Failure)
            {
                Exception ex = _caughtException as Exception;

                if (ex == null)
                {
                    base.WriteActualValueTo(writer);
                }
                else
                {
                    writer.WriteActualValue(ex);
                }
            }
        }
    }
}

