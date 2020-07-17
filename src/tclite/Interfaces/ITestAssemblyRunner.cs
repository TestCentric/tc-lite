// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;

namespace TCLite.Framework.Interfaces
{
    /// <summary>
    /// The ITestAssemblyRunner interface is implemented by classes
    /// that are able to execute a suite of tests loaded
    /// from an assembly.
    /// </summary>
    public interface ITestAssemblyRunner
    {
        #region Properties

        /// <summary>
        /// Gets the tree of loaded tests, or null if
        /// no tests have been loaded.
        /// </summary>
        ITest LoadedTest { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the tests found in an Assembly, returning an 
        /// indication of whether or not the load succeeded.
        /// </summary>
        /// <param name="assembly">The assembly to load</param>
        /// <param name="settings">Dictionary of settings to use in loading the test</param>
        /// <returns>True if the load was successful</returns>
        bool Load(Assembly assembly, IDictionary<string,object> settings);

        ///// <summary>
        ///// Count Test Cases using a filter
        ///// </summary>
        ///// <param name="filter">The filter to apply</param>
        ///// <returns>The number of test cases found</returns>
        //int CountTestCases(TestFilter filter);

        /// <summary>
        /// Run selected tests and return a test result. The test is run synchronously,
        /// and the listener interface is notified as it progresses.
        /// </summary>
        /// <param name="listener">Interface to receive ITestListener notifications.</param>
        /// <param name="filter">A test filter used to select tests to be run</param>
        ITestResult Run(ITestListener listener, ITestFilter filter);

        #endregion
    }
}
