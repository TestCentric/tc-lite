// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// TestCaseSourceAttribute indicates the source to be used to
    /// provide test cases for a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TestCaseFactoryAttribute : TCLiteAttribute, ITestCaseFactory, IImplyFixture
    {
        /// <summary>
        /// Construct with a Type
        /// </summary>
        /// <param name="factoryType">The type that will provide data</param>
        public TestCaseFactoryAttribute(Type factoryType)
        {
            Guard.ArgumentNotNull(factoryType, nameof(factoryType));

            FactoryType = factoryType;
            // NOTE: If the provided Type is not an ITestCaseFactory,
            // then Factory will be set to null. We don't generate an
            // error here because handling it in the GetTestCasesFor
            // gives much better error reporting.
            Factory = Reflect.Construct(FactoryType) as ITestCaseFactory;
        }

        /// <summary>
        /// The Type of the factory being used
        /// </summary>
        public Type FactoryType { get; }

        /// <summary>
        /// The factory itself
        /// </summary>
        /// <value></value>
        public ITestCaseFactory Factory { get; }

        /// <summary>
        /// Gets or sets the category associated with this test.
        /// May be a single category or a comma-separated list.
        /// </summary>
        public string Category { get; set; }

        #region ITestCaseSource Members

        /// <summary>
        /// Returns a set of ITestCaseDataItems for use as arguments
        /// to a parameterized test method.
        /// </summary>
        /// <param name="method">The method for which data is needed.</param>
        /// <returns></returns>
        public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            List<ITestCaseData> data = new List<ITestCaseData>();

            if (Factory == null)
            {
                // This can only happen if the Type passed to the attribute
                // constructor is not an ITestCaseFactory. We generate and
                // return a single non-runnable (fake) TestCase and return
                // it so that the user knows about the error.
                var fakeTestCase = new TestCaseParameters();
                fakeTestCase.RunState = RunState.NotRunnable;
                fakeTestCase.Properties.Set(PropertyNames.SkipReason,
                    $"The Type {FactoryType.Name} is not a test case factory");
                data.Add(fakeTestCase);
            }
            else
            {
                foreach (var testCase in Factory.GetTestCasesFor(method))
                {
					var parameters = new TestCaseParameters(testCase);

                    if (this.Category != null)
                        foreach (string cat in this.Category.Split(new char[] { ',' }))
                            parameters.Properties.Add(PropertyNames.Category, cat);

                    data.Add(parameters);
                }
            }

            return data;
        }

        #endregion
    }
}
