// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Commands
{
    /// <summary>
    /// TODO: Documentation needed for class
    /// </summary>
    public class SkipCommand : TestCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkipCommand"/> class.
        /// </summary>
        /// <param name="test">The test being skipped.</param>
        public SkipCommand(Test test) : base(test)
        {
        }

        /// <summary>
        /// Overridden to simply set the CurrentResult to the
        /// appropriate Skipped state.
        /// </summary>
        /// <param name="context">The execution context for the test</param>
        /// <returns>A TestResult</returns>
        public override TestResult Execute(TestExecutionContext context)
        {
            //TestResult testResult = this.Test.MakeTestResult();
            TestResult testResult = context.CurrentResult;

            switch (Test.RunState)
            {
                default:
                case RunState.Skipped:
                    testResult.SetResult(ResultState.Skipped, GetSkipReason());
                    break;
                case RunState.Ignored:
                    testResult.SetResult(ResultState.Ignored, GetSkipReason());
                    break;
                case RunState.NotRunnable:
                    testResult.SetResult(ResultState.NotRunnable, GetSkipReason(), GetProviderStackTrace());
                    break;
            }

            return testResult;
        }

        private string GetSkipReason()
        {
            return (string)Test.Properties.Get(PropertyNames.SkipReason);
        }

        private string GetProviderStackTrace()
        {
            return (string)Test.Properties.Get(PropertyNames.ProviderStackTrace);
        }
    }
}