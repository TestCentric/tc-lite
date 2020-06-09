// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Framework.Internal;
using TCLite.Framework.Tests;

namespace TCLite.Framework.Commands
{
    /// <summary>
    /// ContextSettingsCommand applies specified changes to the
    /// TestExecutionContext prior to running a test. No special
    /// action is needed after the test runs, since the prior
    /// context will be restored automatically.
    /// </summary>
    class ApplyChangesToContextCommand : DelegatingTestCommand
    {
        private IApplyToContext[] _changes;

        public ApplyChangesToContextCommand(TestCommand innerCommand, IApplyToContext[] changes)
            : base(innerCommand)
        {
            _changes = changes;
        }

        public override TestResult Execute(TestExecutionContext context)
        {
            try
            {
                foreach (IApplyToContext change in _changes)
                    change.ApplyToContext(context);

                context.CurrentResult = innerCommand.Execute(context);
            }
            catch (Exception ex)
            {
#if !NETCF && !SILVERLIGHT
                if (ex is ThreadAbortException)
                    Thread.ResetAbort();
#endif
                context.CurrentResult.RecordException(ex);
            }

            return context.CurrentResult;
        }
    }
}
