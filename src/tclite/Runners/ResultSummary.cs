// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;

namespace TCLite.Runners
{
    /// <summary>
    /// Helper class used to summarize the result of a test run
    /// </summary>
    public class ResultSummary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultSummary"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public ResultSummary(ITestResult result)
        {
            ResultState = result.ResultState;
            //StartTime = result.StartTime;
            //EndTime = result.Endtime;
            Duration = result.Duration.Seconds;

            Visit(result);
        }

        public ResultState ResultState { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double Duration { get; }

        /// <summary>
        /// Gets the test count.
        /// </summary>
        /// <value>The test count.</value>
        public int TestCount { get; private set; }

        /// <summary>
        /// Gets the count of passed tests
        /// </summary>
        public int PassCount { get; private set; }

        /// <summary>
        /// Total failed cases
        /// </summary>
        public int FailedCount { get; private set; }

        /// <summary>
        /// Total cases resulting in a warning
        /// </summary>
        public int WarningCount { get; private set; }

        /// <summary>
        /// Gets the error count.
        /// </summary>
        public int ErrorCount { get; private set; }

        /// <summary>
        /// Gets the failure count.
        /// </summary>
        public int FailureCount { get; private set; }

        /// <summary>
        /// Gets the not run count.
        /// </summary>
        public int NotRunCount { get; private set; }

        /// <summary>
        /// Gets the ignore count
        /// </summary>
        public int IgnoreCount { get; private set; }

        /// <summary>
        /// Gets the skip count
        /// </summary>
        public int SkipCount { get; private set; }

        /// <summary>
        /// Gets the invalid count
        /// </summary>
        public int InvalidCount { get; private set; }

        /// <summary>
        /// Gets the count of inconclusive results
        /// </summary>
        public int InconclusiveCount { get; private set; }

        private void Visit(ITestResult result)
        {
            if (result.Test.IsSuite)
            {
                foreach (ITestResult r in result.Children)
                    Visit(r);
            }
            else
            {
                TestCount++;

                switch (result.ResultState.Status)
                {
                    case TestStatus.Passed:
                        PassCount++;
                        break;
                    case TestStatus.Skipped:
                        if (result.ResultState == ResultState.Ignored)
                            IgnoreCount++;
                        else if (result.ResultState == ResultState.Skipped)
                            SkipCount++;
                        else if (result.ResultState == ResultState.NotRunnable)
                            InvalidCount++;
                        NotRunCount++;
                        break;
                    case TestStatus.Failed:
                        FailedCount++;
                        if (result.ResultState == ResultState.Failure)
                            FailureCount++;
                        else
                            ErrorCount++;
                        break;
                    case TestStatus.Warning:
                            WarningCount++;
                            break;
                    case TestStatus.Inconclusive:
                        InconclusiveCount++;
                        break;
                }

                return;
            }
        }
    }
}
