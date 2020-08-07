// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;

namespace TCLite.Interfaces
{
    /// <summary>
    /// CombiningStrategy is the abstract base for classes that
    /// know how to combine values provided for individual test
    /// parameters to create a set of test cases.
    /// </summary>
    public interface ICombiningStrategy
    {
        /// <summary>
        /// Gets the test cases generated by the CombiningStrategy.
        /// </summary>
        /// <returns>The test cases.</returns>
        IEnumerable<ITestCaseData> GetTestCases(IEnumerable[] sources);
    }
}
