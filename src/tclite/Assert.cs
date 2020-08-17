// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.ComponentModel;
using TCLite.Constraints;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// Delegate used by tests that execute code and
    /// capture any thrown exception.
    /// </summary>
    public delegate void TestDelegate();

    /// <summary>
    /// The Assert class contains a collection of static methods that
    /// implement the assertions used in TC-Lite.
    /// </summary>
    /// <remarks>
    /// We don't actually want any instances of this object, but some people
    /// like to inherit from it to add other static methods. Hence, the
    /// use of abstract in the declaration. 
    /// </remarks>
    public abstract partial class Assert
    {
        /// <summary>
        /// The Equals method is overridden to throw a <see cref="System.InvalidOperationException"/>.
        /// This is done to make sure there is no mistake by calling it when
        /// <see cref="Assert.AreEqual(object, object)"/> is actually intended.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b)
        {
            throw new InvalidOperationException("Assert.Equals should not be used for Assertions");
        }

        /// <summary>
        /// The ReferenceEquals method is overridden to throw a <see cref="InvalidOperationException"/>.
        /// This ensures that there there is no mistake in calling this function rather than
        /// <see cref="Assert.AreSame(object, object)"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object a, object b)
        {
            throw new InvalidOperationException("Assert.ReferenceEquals should not be used for Assertions");
        }

        /// <summary>
        /// Throws a <see cref="TCLite.SuccessException"/> with the message
        /// and arguments provided, ending the running test immediately
        /// and reporting it as successful.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Pass(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

#if NYI // Assert.Multiple
            // If we are in a multiple assert block, this is an error
            if (TestExecutionContext.CurrentContext.MultipleAssertLevel > 0)
                throw new Exception("Assert.Pass may not be used in a multiple assertion block.");
#endif

            throw new SuccessException(message);
        }

        /// <summary>
        /// Throws a <see cref="TCLite.AssertionException"/> with the message
        /// and arguments provided, ending the running test immediately
        /// and reporting it as a Failure.
        /// </summary>
        /// <remarks>
        /// The <c>Assert.Fail</c> method allows you to immediately end the test, recording 
        /// it as failed. Use of <c>Assert.Fail</c> allows you to generate failure messages
        /// for tests that are not encapsulated by the other Assert methods. 
        /// It is also useful in developing your own project-specific assertions.
        /// </remarks>
        /// <param name="message">The failure message</param>
        /// <param name="args">Arguments used format the message</param>
        static public void Fail(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            ReportFailure(message);
        }

        /// <summary>
        /// Records a warning message and continues running the test.
        /// If the test eventually completes without failing, it will
        /// be reported with a Warning result.
        /// </summary>
        /// <param name="message">The warning message</param>
        /// <param name="args">Arguments used to format the message</param>
        static public void Warn(string message = null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            IssueWarning(message);
        }

        /// <summary>
        /// Throws a <see cref="TCLite.IgnoreException"/> with the message
        /// and arguments provided, ending the running test immediately
        /// and reporting it as Ignored.
        /// </summary>
        /// <param name="message">A message indicating why the test is being ignored</param>
        /// <param name="args">Arguments used to format the message</param>
        static public void Ignore(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

#if NYI // Assert.Multiple
            // If we are in a multiple assert block, this is an error
            if (TestExecutionContext.CurrentContext.MultipleAssertLevel > 0)
                throw new Exception("Assert.Ignore may not be used in a multiple assertion block.");
#endif

            throw new IgnoreException(message);
        }

        /// <summary>
        /// Throws a <see cref="TCLite.InconclusiveException"/> with the message
        /// and arguments provided, ending the running test immediately. The test
        /// is reported as Inconclusive unless Warnings were previously recorded.
        /// </summary>
        /// <param name="message">A message indicating why the test was inconclusive.</param>
        /// <param name="args">Arguments used to format the message.</param>
        static public void Inconclusive(string message = null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

# if NYI // Assert.Multiple
            // If we are in a multiple assert block, this is an error
            if (TestExecutionContext.CurrentContext.MultipleAssertLevel > 0)
                throw new Exception("Assert.Inconclusive may not be used in a multiple assertion block.");
#endif

            throw new InconclusiveException(message);
        }

        #region Helper Methods

        private static void ReportFailure(ConstraintResult result, string message = null, params object[] args)
        {
            MessageWriter writer = new TextMessageWriter(message, args);
            result.WriteMessageTo(writer);

            ReportFailure(writer.ToString());
        }

        private static void ReportFailure(string message)
        {
            // Record the failure in an <assertion> element
            var result = TestExecutionContext.CurrentContext.CurrentResult;
            result.RecordAssertion(AssertionStatus.Failed, message, GetStackTrace());
            result.RecordTestCompletion();

#if NYI // Assert.Multiple
            // If we are outside any multiple assert block, then throw
            if (TestExecutionContext.CurrentContext.MultipleAssertLevel == 0)
                throw new AssertionException(result.Message);
#else
            throw new AssertionException(message);
#endif
        }

        private static void IssueWarning(string message)
        {
            var result = TestExecutionContext.CurrentContext.CurrentResult;
            result.RecordAssertion(AssertionStatus.Warning, message, GetStackTrace());
        }

        // System.Environment.StackTrace puts extra entries on top of the stack, at least in some environments
        private static readonly StackFilter SystemEnvironmentFilter = new StackFilter(@" System\.Environment\.");

        private static string GetStackTrace() =>
            StackFilter.DefaultFilter.Filter(SystemEnvironmentFilter.Filter(GetEnvironmentStackTraceWithoutThrowing()));

        /// <summary>
        /// If <see cref="Exception.StackTrace"/> throws, returns "SomeException was thrown by the
        /// Environment.StackTrace property." See also <see cref="ExceptionExtensions.GetStackTraceWithoutThrowing"/>.
        /// </summary>
#if !NET35
        // https://github.com/dotnet/coreclr/issues/19698 is also currently present in .NET Framework 4.7 and 4.8. A
        // race condition between threads reading the same PDB file to obtain file and line info for a stack trace
        // results in AccessViolationException when the stack trace is accessed even indirectly e.g. Exception.ToString.
        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
#endif
        private static string GetEnvironmentStackTraceWithoutThrowing()
        {
            try
            {
                return Environment.StackTrace;
            }
            catch (Exception ex)
            {
                return ex.GetType().Name + " was thrown by the Environment.StackTrace property.";
            }
        }

        private static void IncrementAssertCount()
        {
            TestExecutionContext.CurrentContext.IncrementAssertCount();
        }

        #endregion

    }
}
