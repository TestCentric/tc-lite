// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// ThrowsNothingConstraint tests that a delegate does not
    /// throw an exception.
    /// </summary>
    public class ThrowsNothingConstraint : Constraint
    {
        private Exception _caughtException;

        public override string Description => "No exception to be thrown";

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. Overridden in ThrowsNothingConstraint to write 
        /// information about the exception that was actually caught.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteLine($"<{_caughtException.GetType().FullName}> ({_caughtException.Message})");
            writer.WriteLine(_caughtException.StackTrace);
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True if no exception is thrown, otherwise false</returns>
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            Guard.ArgumentIsRequiredType<Delegate>(actual, nameof(actual));

            _caughtException = ExceptionInterceptor.Intercept(actual);

            return new ConstraintResult(this, _caughtException, _caughtException == null);
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True if no exception is thrown, otherwise false</returns>
        public override ConstraintResult ApplyTo(object actual)
        {
            Guard.ArgumentIsRequiredType<Delegate>(actual, nameof(actual));

            _caughtException = ExceptionInterceptor.Intercept(actual);

            return new ConstraintResult(this, _caughtException, _caughtException == null);
        }

        /// <summary>
        /// Applies the constraint to an ActualValueDelegate that returns 
        /// the value to be tested. The default implementation simply evaluates 
        /// the delegate but derived classes may override it to provide for 
        /// delayed processing.
        /// </summary>
        /// <param name="del">An ActualValueDelegate</param>
        /// <returns>A ConstraintResult</returns>
        public override ConstraintResult ApplyTo<TActual>(ActualValueDelegate<TActual> del)
        {
            return ApplyTo((Delegate)del);
        }
    }
}
