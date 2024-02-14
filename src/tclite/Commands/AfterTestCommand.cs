// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using TCLite.Internal;

namespace TCLite.Commands
{
    /// <summary>
    /// AfterCommand is a DelegatingTestCommand that performs some
    /// specific action after the inner command is run.
    /// </summary>
    public abstract class AfterTestCommand : DelegatingTestCommand
    {
        /// <summary>
        /// Construct an AfterCommand
        /// </summary>
        public AfterTestCommand(TestCommand innerCommand) : base(innerCommand) { }

        /// <summary>
        /// Execute the command
        /// </summary>
        public override TestResult Execute(TestExecutionContext context)
        {
            Guard.OperationValid(AfterTest != null, "AfterTest was not set by the derived class constructor");
            
            _innerCommand.Execute(context);

            AfterTest(context);

            return context.CurrentResult;
        }

        /// <summary>
        /// Set this to perform action after the inner command.
        /// </summary>
        protected Action<TestExecutionContext> AfterTest;
    }
}
