// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;
using TCLite.Framework.Tests;

namespace TCLite.Framework.Commands
{
    /// <summary>
    /// IsolatedFixtureCommand creates the test fixture used to run
    /// an individual test case and disposes it if necessary.
    /// </summary>
    public class IsolatedFixtureCommand : BeforeAndAfterTestCommand
    {
        public IsolatedFixtureCommand(TestCommand innerCommand) : base(innerCommand) { }

        protected override void BeforeTest(TestExecutionContext context)
        {
            context.TestObject = Reflect.Construct(Test.FixtureType);
        }

        protected override void AfterTest(TestExecutionContext context)
        {
            (context.TestObject as IDisposable)?.Dispose();
        }
    }
}
