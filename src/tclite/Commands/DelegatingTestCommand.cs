// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Api;

namespace TCLite.Framework.Commands
{
    /// <summary>
    /// TODO: Documentation needed for class
    /// </summary>
    public abstract class DelegatingTestCommand : TestCommand
    {
        /// <summary>TODO: Documentation needed for field</summary>
        protected TestCommand innerCommand;

        /// <summary>
        /// TODO: Documentation needed for constructor
        /// </summary>
        /// <param name="innerCommand"></param>
        protected DelegatingTestCommand(TestCommand innerCommand)
            : base(innerCommand.Test)
        {
            this.innerCommand = innerCommand;
        }
    }
}