// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;
using TCLite.Framework.Api;

namespace TCLite.Framework.Extensibility
{
    /// <summary>
    /// The ITestCaseProvider interface is used by extensions
    /// that provide data for parameterized tests, along with
    /// certain flags and other indicators used in the test.
    /// </summary>
    public interface ITestCaseProvider
    {
        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        bool HasTestCasesFor(MethodInfo method);

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a paramterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        System.Collections.Generic.IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method);
    }
}
