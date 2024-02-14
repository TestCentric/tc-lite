// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Commands
{
    /// <summary>
    /// BeforeTestCommand is a DelegatingTestCommand that performs some
    /// specific action before the inner command is run.
    /// </summary>
    public abstract class BeforeTestCommand : DelegatingTestCommand
    {
        /// <summary>
        /// Construct a BeforeCommand
        /// </summary>
        public BeforeTestCommand(TestCommand innerCommand) : base(innerCommand) { }

        /// <summary>
        /// Execute the command
        /// </summary>
        public override TestResult Execute(TestExecutionContext context)
        {
            BeforeTest(context);

            return context.CurrentResult = _innerCommand.Execute(context);
        }

        /// <summary>
        /// Action to perform before the inner command.
        /// </summary>
        protected abstract void BeforeTest(TestExecutionContext context);
    }
}
