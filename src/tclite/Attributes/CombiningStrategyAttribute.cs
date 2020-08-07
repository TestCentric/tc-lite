// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Builders;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// Marks a test as using a particular CombiningStrategy to join any supplied parameter data.
    /// Since this is the default, the attribute is optional.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public abstract class CombiningStrategyAttribute : TCLiteAttribute, ITestCaseSource, IApplyToTest, IImplyFixture
    {
        private readonly TCLiteTestCaseBuilder _builder = new TCLiteTestCaseBuilder();

        private readonly ICombiningStrategy _strategy;

        /// <summary>
        /// Construct a CombiningStrategyAttribute incorporating an
        /// ICombiningStrategy and an IParameterDataProvider.
        /// </summary>
        /// <param name="strategy">Combining strategy to be used in combining data</param>
        /// <param name="provider">An IParameterDataProvider to supply data</param>
        protected CombiningStrategyAttribute(ICombiningStrategy strategy)
        {
            _strategy = strategy;
        }

        #region ITestCaseSource Members

        /// <summary>
        /// Builds any number of tests from the specified method and context.
        /// </summary>
        /// <param name="method">The MethodInfo for which tests are to be constructed.</param>
        /// <param name="suite">The suite to which the tests will be added.</param>
        public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            List<ITestCaseData> data = new List<ITestCaseData>();
            var dataProvider = new ParameterDataProvider(method);
            ParameterInfo[] parameters = method.GetParameters();

            var name = method.Name; // For Debugging
            if (parameters.Length > 0)
            {
                IEnumerable[] sources = new IEnumerable[parameters.Length];

                try
                {
                    for (int i = 0; i < parameters.Length; i++)
                        sources[i] = dataProvider.GetDataFor(parameters[i]);

                    foreach (var parms in _strategy.GetTestCases(sources))
                        data.Add(parms);
                }
                catch (Exception ex)
                {
                    var parms = new TestCaseParameters(ex);
                    data.Add(parms);
                }
            }

            return data;
        }

        #endregion

        #region IApplyToTest Members

        /// <summary>
        /// Modify the test by adding the name of the combining strategy
        /// to the properties.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(ITest test)
        {
            var joinType = _strategy.GetType().Name;
            if (joinType.EndsWith("Strategy"))
                joinType = joinType.Substring(0, joinType.Length - 8);

            test.Properties.Set(PropertyNames.JoinType, joinType);
        }

        #endregion
    }
}
