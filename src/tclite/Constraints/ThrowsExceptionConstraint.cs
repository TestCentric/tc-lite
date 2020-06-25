// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// ThrowsExceptionConstraint tests that an exception has
    /// been thrown, without any further tests.
    /// </summary>
    public class ThrowsExceptionConstraint : Constraint
    {
        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return "an exception to be thrown"; }
        }

        // /// <summary>
        // /// Executes the code and returns success if an exception is thrown.
        // /// </summary>
        // /// <param name="actual">A delegate representing the code to be tested</param>
        // /// <returns>True if an exception is thrown, otherwise false</returns>
        // public override ConstraintResult ApplyTo<TActual>(TActual actual)
        // {
        //     Guard.ArgumentNotNullOfType<Delegate>(actual, nameof(actual));

        //     var exception = ExceptionInterceptor.Intercept(actual);

        //     return new ThrowsExceptionConstraintResult(this, exception);
        // }

        public override ConstraintResult ApplyTo(object actual)
        {
            Guard.ArgumentNotNullOfType<Delegate>(actual, nameof(actual));

            var exception = ExceptionInterceptor.Intercept(actual);

            return new ThrowsExceptionConstraintResult(this, exception);
        }

        /// <summary>
        /// Applies the constraint to an ActualValueDelegate that returns
        /// the value to be tested. The default implementation simply evaluates
        /// the delegate but derived classes may override it to provide for
        /// delayed processing.
        /// </summary>
        public override ConstraintResult ApplyTo<TActual>(ActualValueDelegate<TActual> del)
        {
            return ApplyTo((Delegate)del);
        }

        #region Nested Result Class

        class ThrowsExceptionConstraintResult : ConstraintResult
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

        #endregion
    }
}
