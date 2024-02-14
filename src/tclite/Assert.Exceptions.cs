// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using TCLite.Constraints;
using TCLite.Internal;

namespace TCLite
{
    public abstract partial class Assert
    {

        /// <summary>
        /// Asserts that a delegate throws an exception, which
        /// satisfies a provided <c>Constraint</c>. If no exception
        /// is thrown or if the constraint is not satisfied, then
        /// a <see cref="TCLite.AssertionException"/> is thrown, ending
        /// the test and reporting it as a Failure.
        /// </summary>
        /// <param name="expression">A constraint to be satisfied by the exception</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        /// <returns>The exception that was thrown</returns>
        /// <remarks>
        /// On success, this assertion returns the exception that
        /// was thrown. This allows making further assertions on the
        /// properties of the exception.
        /// </remarks>
        public static Exception Throws(IResolveConstraint expression, TestDelegate code, string message=null, params object[] args)
        {
            Exception caughtException = null;

            using (new TestExecutionContext.IsolatedContext())
            {
                try
                {
                    code();
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }
            }

            Assert.That(caughtException, expression, message, args);

            return caughtException;
        }

        /// <summary>
        /// Asserts that a delegate throws an exception of the Type
        /// specified. If no exception is thrown or if it is of the
        /// wrong Type, a <see cref="TCLite.AssertionException"/> is thrown,
        /// ending the test and reporting it as a Failure.
        /// </summary>
        /// <param name="expectedExceptionType">The exception Type expected</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        /// <returns>The exception that was thrown</returns>
        /// <remarks>
        /// On success, this assertion returns the exception that
        /// was thrown. This allows making further assertions on the
        /// properties of the exception.
        /// </remarks>
        public static Exception Throws(Type expectedExceptionType, TestDelegate code, string message=null, params object[] args)
        {
            return Throws(new ExceptionTypeConstraint(expectedExceptionType), code, message, args);
        }

        /// <summary>
        /// Asserts that a delegate throws an exception of the Type
        /// specified. If no exception is thrown or if it is of the
        /// wrong Type, a <see cref="TCLite.AssertionException"/> is thrown,
        /// ending the test and reporting it as a Failure.
        /// </summary>
        /// <typeparam name="T">Type of the expected exception</typeparam>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        /// <returns>The exception that was thrown</returns>
        /// <remarks>
        /// On success, this assertion returns the exception that
        /// was thrown. This allows making further assertions on the
        /// properties of the exception.
        /// </remarks>
        public static T Throws<T>(TestDelegate code, string message=null, params object[] args) where T : Exception
        {
            return (T)Throws(typeof(T), code, message, args);
        }

        /// <summary>
        /// Asserts that a delegate does not throw an exception.
        /// If any exception is thrown, it is caught and a 
        /// <see cref="TCLite.AssertionException"/> is thrown,
        /// ending the test and reporting it as a Failure.
        /// </summary>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        /// <remarks>
        /// This assertion is useful in converting an Error result
        /// due to an unexpected exception into a Failure.
        /// </remarks>
        public static void DoesNotThrow(TestDelegate code, string message=null, params object[] args)
        {
            Assert.That(code, new ThrowsNothingConstraint(), message, args);
        }
    }
}
