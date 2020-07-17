// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Framework.Interfaces
{
    /// <summary>
    /// The ITestCaseData interface is implemented by a class
    /// that is able to return complete testcases for use by
    /// a parameterized test method.
    /// </summary>
    public interface ITestCaseData
    {
        /// <summary>
        /// Gets the name to be used for the test
        /// </summary>
        string TestName { get; }
		
		/// <summary>
		/// Gets the RunState for this test case.
		/// </summary>
		RunState RunState { get; }

        /// <summary>
        /// Gets the argument list to be provided to the test
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// Gets the expected result of the test case
        /// </summary>
        object ExpectedResult { get; }

        /// <summary>
        /// Returns true if an expected result has been set
        /// </summary>
        bool HasExpectedResult { get; }

        /// <summary>
        /// Gets the property dictionary for the test case
        /// </summary>
        IPropertyBag Properties { get; }
    }
}
