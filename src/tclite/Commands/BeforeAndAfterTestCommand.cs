// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Internal;

namespace TCLite.Commands
{
    /// <summary>
    /// TestActionCommand handles a single ITestAction applied
    /// to a test. It runs the BeforeTest method, then runs the
    /// test and finally runs the AfterTest method.
    /// </summary>
    public abstract class BeforeAndAfterTestCommand : DelegatingTestCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestActionCommand"/> class.
        /// </summary>
        /// <param name="innerCommand">The inner command.</param>
        public BeforeAndAfterTestCommand(TestCommand innerCommand) : base(innerCommand) { }

        /// <summary>
        /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
        /// </summary>
        /// <param name="context">The context in which the test should run.</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            BeforeTest(context);

            _innerCommand.Execute(context);

            AfterTest(context);

            return context.CurrentResult;
        }

        /// <summary>
        /// Perform the before test action
        /// </summary>
        protected abstract void BeforeTest(TestExecutionContext context);

        /// <summary>
        /// Perform the after test action
        /// </summary>
        protected abstract void AfterTest(TestExecutionContext context1);
    }
}
