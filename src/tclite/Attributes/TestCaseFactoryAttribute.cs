// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
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
    public class TestCaseFactoryAttribute : TCLiteAttribute, ITestCaseSource, IImplyFixture
    {
        /// <summary>
        /// Construct with a Type
        /// </summary>
        /// <param name="factoryType">The type that will provide data</param>
        public TestCaseFactoryAttribute(Type factoryType)
        {
            Guard.ArgumentValid(
                typeof(ITestCaseSource).IsAssignableFrom(factoryType),
                $"Type {factoryType.Name} is not a test case factory", nameof(factoryType));
            FactoryType = factoryType;
        }

        /// <summary>
        /// The Type of the factory being used
        /// </summary>
        public Type FactoryType { get; }

        /// <summary>
        /// The factory itself
        /// </summary>
        /// <value></value>
        public ITestCaseSource Factory
        {
            get
            {
                if (_factory == null)
                    _factory = (ITestCaseSource)Reflect.Construct(FactoryType);

                return _factory;
            }
        }
        private ITestCaseSource _factory;

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

            if (Factory != null)
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
