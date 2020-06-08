// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Commands
{
    /// <summary>
    /// ICommandDecorator is implemented by attributes and other
    /// objects able to decorate a TestCommand, usually by wrapping
    /// it with an outer command.
    /// </summary>
    public interface ICommandDecorator
    {
        /// <summary>
        /// The stage of command execution to which this decorator applies.
        /// </summary>
        CommandStage Stage { get; }

        /// <summary>
        /// The priority of this decorator as compared to other decorators
        /// in the same Stage. Lower values are applied first.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Decorate a command, usually by wrapping it with another
        /// command, and return the decorated command.
        /// </summary>
        /// <param name="command">The command to be decorated</param>
        /// <returns>The decorated command</returns>
        TestCommand Decorate(TestCommand command);
    }
}
