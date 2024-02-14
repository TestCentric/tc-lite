// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.WorkItems
{
    /// <summary>
    /// The current state of a work item
    /// </summary>
    internal enum WorkItemState
    {
        /// <summary>
        /// Ready to run or continue
        /// </summary>
        Ready,

        /// <summary>
        /// Waiting for a dependency to complete
        /// </summary>
        Waiting,

        /// <summary>
        /// Complete
        /// </summary>
        Complete
    }
}
