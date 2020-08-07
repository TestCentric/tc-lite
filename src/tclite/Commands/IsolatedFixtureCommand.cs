// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Internal;

namespace TCLite.Commands
{
    /// <summary>
    /// IsolatedFixtureCommand creates the test fixture used to run
    /// an individual test case and disposes it if necessary.
    /// </summary>
    public class IsolatedFixtureCommand : BeforeAndAfterTestCommand
    {
        private Type _fixtureType;
        private object[] _fixtureArgs;

        public IsolatedFixtureCommand(TestCommand innerCommand, TestFixture containingFixture)
            : base(innerCommand)
        {
            _fixtureType = containingFixture.FixtureType;
            _fixtureArgs = containingFixture.Arguments;           
        }

        protected override void BeforeTest(TestExecutionContext context)
        {
            try
            {
                context.TestObject = Reflect.Construct(_fixtureType, _fixtureArgs);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                    Thread.ResetAbort();

                context.CurrentResult.RecordException(ex);
            }
        }

        protected override void AfterTest(TestExecutionContext context)
        {
            (context.TestObject as IDisposable)?.Dispose();
        }
    }
}
