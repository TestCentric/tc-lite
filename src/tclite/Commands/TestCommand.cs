// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Internal;
using TCLite.Framework.Internal.Tests;
using TCLite.Framework.Internal.Results;

namespace TCLite.Framework.Commands
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
        private Test test;

        /// <summary>
        /// Construct a TestCommand for a test.
        /// </summary>
        /// <param name="test">The test to be executed</param>
        public TestCommand(Test test)
        {
            this.test = test;
        }

        #region ITestCommandMembers

        /// <summary>
        /// Gets the test associated with this command.
        /// </summary>
        public Test Test
        {
            get { return test; }
        }

        /// <summary>
        /// Runs the test in a specified context, returning a TestResult.
        /// </summary>
        /// <param name="context">The TestExecutionContext to be used for running the test.</param>
        /// <returns>A TestResult</returns>
        public abstract TestResult Execute(TestExecutionContext context);

        #endregion
    }
}
