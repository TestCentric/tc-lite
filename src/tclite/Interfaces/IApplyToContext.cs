// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Interfaces
{
    /// <summary>
    /// The IApplyToContext interface is implemented by attributes
    /// that want to make changes to the execution context before
    /// a test is run.
    /// </summary>
    public interface IApplyToContext
    {
        /// <summary>
        /// Apply changes to the execution context
        /// </summary>
        /// <param name="context">The execution context</param>
        void ApplyToContext(ITestExecutionContext context);
    }
}
