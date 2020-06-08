// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Threading;
using TCLite.Framework.Api;
using TCLite.Framework.Internal.Tests;
using TCLite.Framework.Internal.Results;

namespace TCLite.Framework.Internal.WorkItems
{
    /// <summary>
    /// A WorkItem may be an individual test case, a fixture or
    /// a higher level grouping of tests. All WorkItems inherit
    /// from the abstract WorkItem class, which uses the template
    /// pattern to allow derived classes to perform work in
    /// whatever way is needed.
    /// </summary>
    public abstract class WorkItem
    {
        // The current state of the WorkItem
        private WorkItemState _state;

        // The test this WorkItem represents
        private Test _test;

        /// <summary>
        /// The result of running the test
        /// </summary>
        protected TestResult testResult;

        // The execution context used by this work item
        private TestExecutionContext _context;

        #region Constructor

        /// <summary>
        /// Construct a WorkItem for a particular test.
        /// </summary>
        /// <param name="test">The test that the WorkItem will run</param>
        public WorkItem(Test test)
        {
            _test = test;
            testResult = test.MakeTestResult();
            _state = WorkItemState.Ready;
        }

        #endregion

        #region Properties and Events

        /// <summary>
        /// Event triggered when the item is complete
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// Gets the current state of the WorkItem
        /// </summary>
        public WorkItemState State
        {
            get { return _state; }
        }

        /// <summary>
        /// The test being executed by the work item
        /// </summary>
        public Test Test
        {
            get { return _test; }
        }

        /// <summary>
        /// The execution context
        /// </summary>
        protected TestExecutionContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// The test result
        /// </summary>
        public TestResult Result
        {
            get { return testResult; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Execute the current work item, including any
        /// child work items.
        /// </summary>
        public virtual void Execute(TestExecutionContext context)
        {
            _context = new TestExecutionContext(context);

            // Timeout set at a higher level
            int timeout = _context.TestCaseTimeout;

            // Timeout set on this test
            if (Test.Properties.ContainsKey(PropertyNames.Timeout))
                timeout = (int)Test.Properties.Get(PropertyNames.Timeout);

            if (Test is TestMethod && timeout > 0)
                RunTestWithTimeout(timeout);
            else
                RunTest();
        }

        private void RunTestWithTimeout(int timeout)
        {
            Thread thread = new Thread(new ThreadStart(RunTest));

            thread.Start();

            if (timeout <= 0)
                timeout = Timeout.Infinite;

            thread.Join(timeout);

            if (thread.IsAlive)
            {
                ThreadUtility.Kill(thread);

                // NOTE: Without the use of Join, there is a race condition here.
                // The thread sets the result to Cancelled and our code below sets
                // it to Failure. In order for the result to be shown as a failure,
                // we need to ensure that the following code executes after the
                // thread has terminated. There is a risk here: the test code might
                // refuse to terminate. However, it's more important to deal with
                // the normal rather than a pathological case.
                thread.Join();

                Result.SetResult(ResultState.Failure,
                    string.Format("Test exceeded Timeout value of {0}ms", timeout));

                WorkItemComplete();
            }
        }

        private void RunTest()
        {
            _context.CurrentTest = this.Test;
            _context.CurrentResult = this.Result;
            _context.Listener.TestStarted(this.Test);
            _context.StartTime = DateTime.Now;

            TestExecutionContext.SetCurrentContext(_context);

            long startTicks = Stopwatch.GetTimestamp();

            try
            {
                PerformWork();
            }
            finally
            {
                long tickCount = Stopwatch.GetTimestamp() - startTicks;
                double seconds = (double)tickCount / Stopwatch.Frequency;
                Result.Duration = TimeSpan.FromSeconds(seconds);

                Result.AssertCount = _context.AssertCount;

                _context.Listener.TestFinished(Result);

                _context = _context.Restore();
                _context.AssertCount += Result.AssertCount;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Method that performs actually performs the work. It should
        /// set the State to WorkItemState.Complete when done.
        /// </summary>
        protected abstract void PerformWork();

        /// <summary>
        /// Method called by the derived class when all work is complete
        /// </summary>
        protected void WorkItemComplete()
        {
            _state = WorkItemState.Complete;
            if (Completed != null)
                Completed(this, EventArgs.Empty);
        }

        #endregion
    }
}
