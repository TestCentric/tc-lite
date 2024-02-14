// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Interfaces;

namespace TCLite.Commands
{
    /// <summary>
    /// Abstract base for commands that wrap another command,
    /// which is referred to as the inner command.
    /// </summary>
    public abstract class DelegatingTestCommand : TestCommand
    {
        protected TestCommand _innerCommand;

        /// <summary>
        /// Construct a DelegatingTestCommand
        /// </summary>
        /// <param name="innerCommand">The command wrapped by this one</param>
        protected DelegatingTestCommand(TestCommand innerCommand)
            : base(innerCommand.Test)
        {
            _innerCommand = innerCommand;
        }
    }
}