// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;
using TCLite.Framework.Builders;

namespace TCLite.Framework.Extensibility
{
    class TestCaseProviders : ITestCaseProvider
    {
        private List<ITestCaseProvider> Extensions = new List<ITestCaseProvider>();

        public TestCaseProviders()
        {
            this.Extensions.Add(new DataAttributeTestCaseProvider());
            //this.Extensions.Add(new CombinatorialTestCaseProvider());
        }

        #region ITestCaseProvider Members

        /// <summary>
        /// Determine whether any test cases are available for a parameterized method.
        /// </summary>
        /// <param name="method">A MethodInfo representing a parameterized test</param>
        /// <returns>True if any cases are available, otherwise false.</returns>
        public bool HasTestCasesFor(MethodInfo method)
        {
            foreach (ITestCaseProvider provider in Extensions)
                if (provider.HasTestCasesFor(method))
                    return true;

            return false;
        }

        /// <summary>
        /// Return an enumeration providing test cases for use in
        /// running a parameterized test.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            List<ITestCaseData> testcases = new List<ITestCaseData>();

            foreach (ITestCaseProvider provider in Extensions)
                try
                {
                    if (provider.HasTestCasesFor(method))
                        foreach (ITestCaseData testcase in provider.GetTestCasesFor(method))
                            testcases.Add(testcase);
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    testcases.Add(new ParameterSet(ex.InnerException));
                }
                catch (System.Exception ex)
                {
                    testcases.Add(new ParameterSet(ex));
                }

            return testcases;
        }

        #endregion
    }
}
