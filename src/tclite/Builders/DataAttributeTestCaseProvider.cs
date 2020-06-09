// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Extensibility;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Builders
{
    /// <summary>
    /// DataAttributeTestCaseProvider provides data for methods
    /// annotated with any DataAttribute. For correct operation,
    /// any new or custom Attributes must implement one of the 
    /// following interfaces:
    ///    ITestCaseData
    ///    ITestCaseSource
    /// </summary>
    public class DataAttributeTestCaseProvider : ITestCaseProvider
    {
        #region ITestCaseProvider Members

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            return method.IsDefined(typeof(DataAttribute), false);
        }

        /// <summary>
        /// Return an IEnumerable providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            List<ITestCaseData> testCases = new List<ITestCaseData>();

            foreach (DataAttribute attr in method.GetCustomAttributes(typeof(DataAttribute), false))
            {
                ITestCaseSource source = attr as ITestCaseSource;
                if (source != null)
                {
                    // TODO: Create a class to handle exceptions for NUnitLite
                    foreach (ITestCaseData testCase in ((ITestCaseSource)attr).GetTestCasesFor(method))
                        testCases.Add(testCase);
                    continue;
                }
            }

            return testCases;
        }
        #endregion
    }
}
