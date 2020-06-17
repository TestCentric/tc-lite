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
            private readonly object caughtException;

            public ExceptionTypeConstraintResult(ExceptionTypeConstraint constraint, object caughtException, Type type, bool matches)
                : base(constraint, type, matches)
            {
                this.caughtException = caughtException;
            }

            public override void WriteActualValueTo(MessageWriter writer)
            {
                if (this.Status == ConstraintStatus.Failure)
                {
                    Exception ex = caughtException as Exception;

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
    
        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. Overridden to write additional information 
        /// in the case of an Exception.
        /// </summary>
        /// <param name="writer">The MessageWriter to use</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            Exception ex = ActualValue as Exception;
            base.WriteActualValueTo(writer);

            if (ex != null)
            {
                writer.WriteLine(" ({0})", ex.Message);
                writer.Write(ex.StackTrace);
            }
        }
    }
}

