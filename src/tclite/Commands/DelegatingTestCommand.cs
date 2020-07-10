// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Api;

namespace TCLite.Framework.Commands
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