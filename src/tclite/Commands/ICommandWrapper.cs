// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Commands
{
    /// <summary>
    /// ICommandWrapper is implemented by attributes and other
    /// objects able to wrap a TestCommand with another command.
    /// </summary>
    public interface ICommandWrapper
    {
        /// <summary>
        /// Wrap another command and return the new command.
        /// </summary>
        /// <param name="command">The command to be decorated</param>
        /// <returns>The wrapped command</returns>
        TestCommand Wrap(TestCommand command);
    }
}
