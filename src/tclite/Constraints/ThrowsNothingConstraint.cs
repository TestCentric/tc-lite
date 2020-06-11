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

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True if no exception is thrown, otherwise false</returns>
        public override bool Matches<TActual>(TActual actual)
        {
            _caughtException = ExceptionInterceptor.Intercept(actual);

            return _caughtException == null;
        }

        public override bool Matches<TActual>(ActualValueDelegate<TActual> del)
        {
            return Matches(new GenericInvocationDescriptor<TActual>(del));
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.Write(string.Format("No Exception to be thrown"));
        }

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
    }
}