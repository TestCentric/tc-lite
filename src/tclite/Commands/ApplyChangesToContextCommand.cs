// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Commands
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
