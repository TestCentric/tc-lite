// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Interfaces
{
    /// <summary>
    /// The ITestCaseData interface is implemented by a class
    /// that is able to return complete testcases for use by
    /// a parameterized test method.
    /// </summary>
    public interface ITestCaseData : ITestData
    {
        /// <summary>
        /// Gets the expected result of the test case
        /// </summary>
        object ExpectedResult { get; }

        /// <summary>
        /// Returns true if an expected result has been set
        /// </summary>
        bool HasExpectedResult { get; }
    }
}
