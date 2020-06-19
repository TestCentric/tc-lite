// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// ExceptionTypeConstraint is a special version of ExactTypeConstraint
    /// used to provided detailed info about the exception thrown in
    /// an error message.
    /// </summary>
    public class ExceptionTypeConstraint : ExactTypeConstraint
    {
        /// <summary>
        /// Constructs an ExceptionTypeConstraint
        /// </summary>
        public ExceptionTypeConstraint(Type type) : base(type) { }

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Guard.ArgumentIsRequiredType<Exception>(actual, nameof(actual));

            Type actualType = actual == null ? null : actual.GetType();

            return new ExceptionTypeConstraintResult(this, actual, actualType, this.Matches(actual));
        }

        #region Nested Result Class
        class ExceptionTypeConstraintResult : ConstraintResult
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
        #endregion
        }
}

