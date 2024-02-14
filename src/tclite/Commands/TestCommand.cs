// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Commands
{
    /// <summary>
    /// TestCommand is the abstract base class for all test commands
    /// in the framework. A TestCommand represents a single stage in
    /// the execution of a test, e.g.: SetUp/TearDown, checking for
    /// Timeout, verifying the returned result from a method, etc.
    /// 
    /// TestCommands may decorate other test commands so that the
    /// execution of a lower-level command is nested within that
    /// of a higher level command. All nested commands are executed
    /// synchronously, as a single unit. Scheduling test execution
    /// on separate threads is handled at a higher level, using the
    /// task dispatcher.
    /// </summary>
    public abstract class TestCommand
    {
        /// <summary>
        /// Construct a TestCommand for a test.
        /// </summary>
        /// <param name="test">The test to be executed</param>
        public TestCommand(Test test)
        {
            Test = test;
        }

        #region ITestCommandMembers

        /// <summary>
        /// Gets the test associated with this command.
        /// </summary>
        public Test Test { get; }

        /// <summary>
        /// Runs the test in a specified context, returning a TestResult.
        /// </summary>
        /// <param name="context">The TestExecutionContext to be used for running the test.</param>
        /// <returns>A TestResult</returns>
        public abstract TestResult Execute(TestExecutionContext context);

        #endregion
    }
}
