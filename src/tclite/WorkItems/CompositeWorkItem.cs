// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.Framework.Tests;

namespace TCLite.Framework.WorkItems
{
    /// <summary>
    /// A CompositeWorkItem represents a test suite and
    /// encapsulates the execution of the suite as well
    /// as all its child tests.
    /// </summary>
    public class CompositeWorkItem : WorkItem
    {
        private TestSuite _suite;
        private ITestFilter _childFilter;
        private System.Collections.Generic.Queue<WorkItem> _children = new System.Collections.Generic.Queue<WorkItem>();
        private CountdownEvent _childTestCountdown;

        /// <summary>
        /// Construct a CompositeWorkItem for executing a test suite
        /// using a filter to select child tests.
        /// </summary>
        /// <param name="suite">The TestSuite to be executed</param>
        /// <param name="childFilter">A filter used to select child tests</param>
        public CompositeWorkItem(TestSuite suite, ITestFilter childFilter)
            : base(suite)
        {
            _suite = suite;
            _childFilter = childFilter;
        }

        /// <summary>
        /// Method that actually performs the work. Overridden
        /// in CompositeWorkItem to do setup, run all child
        /// items and then do teardown.
        /// </summary>
        protected override void PerformWork()
        {
            if (_suite.HasChildren)
                foreach (Test test in _suite.Tests)
                    if (_childFilter.Pass(test))
                        if (test.IsSuite)
                            _children.Enqueue(new CompositeWorkItem(test as TestSuite, _childFilter));
                        else
                            _children.Enqueue(new SimpleWorkItem(test as TestMethod));

            switch (Test.RunState)
            {
                default:
                case RunState.Runnable:
                case RunState.Explicit:
                    // Assume success, since the result will be inconclusive
                    // if there is no setup method to run or if the
                    // context initialization fails.
                    Result.SetResult(ResultState.Success);

                    if (_children.Count > 0)
                        switch (Result.ResultState.Status)
                        {
                            case TestStatus.Passed:
                                RunChildren();
                                return;
                                // Just return: completion event will take care
                                // of TestFixtureTearDown when all tests are done.

                            case TestStatus.Skipped:
                            case TestStatus.Inconclusive:
                            case TestStatus.Failed:
                                SkipChildren();
                                break;
                        }

                    break;

                case RunState.Skipped:
                    SkipFixture(ResultState.Skipped, GetSkipReason(), null);
                    break;

                case RunState.Ignored:
                    SkipFixture(ResultState.Ignored, GetSkipReason(), null);
                    break;

                case RunState.NotRunnable:
                    SkipFixture(ResultState.NotRunnable, GetSkipReason(), GetProviderStackTrace());
                    break;
            }
   
            // Fall through in case no child tests were run.
            // Otherwise, this is done in the completion event.
            WorkItemComplete();
        }

        #region Helper Methods

        private void RunChildren()
        {
            _childTestCountdown = new CountdownEvent(_children.Count);

            while (_children.Count > 0)
            {
                WorkItem child = (WorkItem)_children.Dequeue();
                child.Completed += new EventHandler(OnChildCompleted);
                child.Execute(this.Context);
            }
        }

        private void SkipFixture(ResultState resultState, string message, string stackTrace)
        {
            Result.SetResult(resultState, message, stackTrace);
            SkipChildren();
        }

        private void SkipChildren()
        {
            while (_children.Count > 0)
            {
                WorkItem child = (WorkItem)_children.Dequeue();
                Test test = child.Test;
                TestResult result = test.MakeTestResult();
                if (Result.ResultState.Status == TestStatus.Failed)
                    result.SetResult(ResultState.Failure, "TestFixtureSetUp Failed");
                else
                    result.SetResult(Result.ResultState, Result.Message);
                Result.AddResult(result);
            }
        }


        private string GetSkipReason()
        {
            return (string)Test.Properties.Get(PropertyNames.SkipReason);
        }

        private string GetProviderStackTrace()
        {
            return (string)Test.Properties.Get(PropertyNames.ProviderStackTrace);
        }
        
        private object _completionLock = new object();

        private void OnChildCompleted(object sender, EventArgs e)
        {
            lock (_completionLock)
            {
                WorkItem childTask = sender as WorkItem;
                if (childTask != null)
                {
                    childTask.Completed -= new EventHandler(OnChildCompleted);
                    Result.AddResult(childTask.Result);
                    _childTestCountdown.Signal();

                    if (_childTestCountdown.CurrentCount == 0)
                    {
                        WorkItemComplete();
                    }
                }
            }
        }

        #endregion
    }
}
