// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Internal.WorkItems
{
    /// <summary>
    /// The current state of a work item
    /// </summary>
    public enum WorkItemState
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
