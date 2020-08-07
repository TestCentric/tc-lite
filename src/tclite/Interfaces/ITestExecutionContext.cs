// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Constraints;
using TCLite.Internal;

namespace TCLite.Interfaces
{
    public interface ITestExecutionContext
    {
        /// <summary>
        /// Gets or sets the current test
        /// </summary>
        Test CurrentTest { get; }

        /// <summary>
        /// The time the current test started execution
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// Gets or sets the current test result
        /// </summary>
        TestResult CurrentResult { get; set; }

        /// <summary>
        /// The current test object - that is the user fixture
        /// object on which tests are being executed.
        /// </summary>
        object TestObject { get; }

        /// <summary>
        /// Get or set the working directory
        /// </summary>
        string WorkDirectory { get; }

        /// <summary>
        /// Get or set indicator that run should stop on the first error
        /// </summary>
        bool StopOnError { get; }

        Tolerance DefaultFloatingPointTolerance { get; set; }

        /// <summary>
        /// Gets the RandomGenerator specific to this Test
        /// </summary>
        Randomizer RandomGenerator { get; }

        /// <summary>
        /// Gets the assert count.
        /// </summary>
        /// <value>The assert count.</value>
        int AssertCount { get; }

        /// <summary>
        /// Gets or sets the test case timeout value
        /// </summary>
        int TestCaseTimeout { get; set; }

        /// <summary>
        /// Saves or restores the CurrentCulture
        /// </summary>
        CultureInfo CurrentCulture { get; set; }

        /// <summary>
        /// Saves or restores the CurrentUICulture
        /// </summary>
        CultureInfo CurrentUICulture { get; set; }

    }
}