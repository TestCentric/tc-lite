// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Commands;
using TCLite.Internal;

namespace TCLite.WorkItems
{
    /// <summary>
    /// A SimpleWorkItem represents a single test case and is
    /// marked as completed immediately upon execution. This
    /// class is also used for skipped or ignored test suites.
    /// </summary>
    public class SimpleWorkItem : WorkItem
    {
        private TestCommand _command;

        /// <summary>
        /// Construct a simple work item for a test.
        /// </summary>
        /// <param name="test">The test to be executed</param>
        public SimpleWorkItem(TestMethod test) : base(test)
        {
            _command = test.MakeTestCommand();
        }

        /// <summary>
        /// Method that performs actually performs the work.
        /// </summary>
        protected override void PerformWork()
        {
            try
            {
                Result = _command.Execute(Context);
            }
            finally
            {
                WorkItemComplete();
            }
        }

    }
}
