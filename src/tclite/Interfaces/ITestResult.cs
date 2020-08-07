// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Interfaces
{
    /// <summary>
    /// The ITestResult interface represents the result of a test.
    /// </summary>
    public interface ITestResult : IXmlNodeBuilder
    {
        /// <summary>
        /// Gets the ResultState of the test result, which 
        /// indicates the success or failure of the test.
        /// </summary>
        ResultState ResultState { get; }

        /// <summary>
        /// Gets the display name of the test result
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the full name of the test result
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the elapsed time for running the test
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets the message associated with a test
        /// failure or with not running the test
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets any stacktrace associated with an
        /// error or failure. Not available in
        /// the Compact Framework 1.0.
        /// </summary>
        string StackTrace { get; }

        /// <summary>
        /// Gets the number of asserts executed
        /// when running the test and all its children.
        /// </summary>
        int AssertCount { get; }


        /// <summary>
        /// Gets the number of test cases that failed
        /// when running the test and all its children.
        /// </summary>
        int FailCount { get; }

        /// <summary>
        /// Gets the number of test cases that passed
        /// when running the test and all its children.
        /// </summary>
        int PassCount { get; }

        /// <summary>
        /// Gets the number of test cases that were skipped
        /// when running the test and all its children.
        /// </summary>
        int SkipCount { get; }

        /// <summary>
        /// Gets the number of test cases that were inconclusive
        /// when running the test and all its children.
        /// </summary>
        int InconclusiveCount { get; }

        /// <summary>
        /// Gets the number of test cases that resulted in warnings.
        /// </summary>
        int WarningCount { get; }

        /// <summary>
        /// Indicates whether this result has any child results.
        /// Accessing HasChildren should not force creation of the
        /// Children collection in classes implementing this interface.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Gets the the collection of child results.
        /// </summary>
        System.Collections.Generic.IList<ITestResult> Children { get; }

        /// <summary>
        /// Gets the Test to which this result applies.
        /// </summary>
        ITest Test { get; }
    }
}
