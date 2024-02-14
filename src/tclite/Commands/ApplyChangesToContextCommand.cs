// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Commands
{
    /// <summary>
    /// ContextSettingsCommand applies specified changes to the
    /// TestExecutionContext prior to running a test. No special
    /// action is needed after the test runs, since the prior
    /// context will be restored automatically.
    /// </summary>
    class ApplyToContextCommand : BeforeTestCommand
    {
        private IApplyToContext _change;

        public ApplyToContextCommand(TestCommand innerCommand, IApplyToContext change)
            : base(innerCommand)
        {
            _change = change;
        }

        protected override void BeforeTest(TestExecutionContext context)
        {
            _change.ApplyToContext(context);
        }
    }
}
