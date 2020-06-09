// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Specialized;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

// TODO: Remove conditional code
namespace TCLite.Framework
{
    /// <summary>
    /// The TestCaseData class represents a set of arguments
    /// and other parameter info to be used for a parameterized
    /// test case. It provides a number of instance modifiers
    /// for use in initializing the test case.
    /// 
    /// Note: Instance modifiers are getters that return
    /// the same instance after modifying it's state.
    /// </summary>
    public class TestCaseData : ITestCaseData
    {

        #region Instance Fields

        /// <summary>
        /// The argument list to be provided to the test
        /// </summary>
        private object[] arguments;

        /// <summary>
        /// The expected result to be returned
        /// </summary>
        private object expectedResult;

        /// <summary>
        /// A dictionary of properties, used to add information
        /// to tests without requiring the class to change.
        /// </summary>
        private IPropertyBag properties;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestCaseData"/> class.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public TestCaseData(params object[] args)
        {
			this.RunState = RunState.Runnable;

			if (args == null)
                this.arguments = new object[] { null };
            else
                this.arguments = args;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestCaseData"/> class.
        /// </summary>
        /// <param name="arg">The argument.</param>
        public TestCaseData(object arg)
        {
			this.RunState = RunState.Runnable;
            this.arguments = new object[] { arg };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestCaseData"/> class.
        /// </summary>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        public TestCaseData(object arg1, object arg2)
        {
			this.RunState = RunState.Runnable;
            this.arguments = new object[] { arg1, arg2 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TestCaseData"/> class.
        /// </summary>
        /// <param name="arg1">The first argument.</param>
        /// <param name="arg2">The second argument.</param>
        /// <param name="arg3">The third argument.</param>
        public TestCaseData(object arg1, object arg2, object arg3)
        {
			this.RunState = RunState.Runnable;
            this.arguments = new object[] { arg1, arg2, arg3 };
        }

        #endregion

        #region ITestCaseData Members

        /// <summary>
        /// Gets the argument list to be provided to the test
        /// </summary>
        public object[] Arguments
        {
            get { return arguments; }
        }

        /// <summary>
        /// Gets the expected result
        /// </summary>
        public object ExpectedResult
        {
            get { return expectedResult; }
			set
			{
				expectedResult = value;
				HasExpectedResult = true;
			}
        }

        private bool hasExpectedResult;
        /// <summary>
        /// Returns true if the expected result has been set
        /// </summary>
        public bool HasExpectedResult 
        {
            get { return hasExpectedResult; }
            set { hasExpectedResult = value; }
        }

        private string testName;
        /// <summary>
        /// Gets the name to be used for the test
        /// </summary>
        public string TestName 
        {
            get { return testName; }
            set { testName = value; }
        }

        private RunState runState;
        /// <summary>
		/// Gets the RunState for this test case.
		/// </summary>
		public RunState RunState 
        {
            get { return runState; }
            set { runState = value; }
        }

        /// <summary>
        /// Gets the property dictionary for this test
        /// </summary>
        public IPropertyBag Properties
        {
            get
            {
                if (properties == null)
                    properties = new TCLite.Framework.Internal.PropertyBag();

                return properties;
            }
        }

        #endregion

        #region Fluent Instance Modifiers

        /// <summary>
        /// Sets the expected result for the test
        /// </summary>
        /// <param name="result">The expected result</param>
        /// <returns>A modified TestCaseData</returns>
        public TestCaseData Returns(object result)
        {
            this.ExpectedResult = result;
            return this;
        }

        /// <summary>
        /// Sets the name of the test case
        /// </summary>
        /// <returns>The modified TestCaseData instance</returns>
        public TestCaseData SetName(string name)
        {
            this.TestName = name;
            return this;
        }

        /// <summary>
        /// Sets the description for the test case
        /// being constructed.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The modified TestCaseData instance.</returns>
        public TestCaseData SetDescription(string description)
        {
            this.Properties.Set(PropertyNames.Description, description);
            return this;
        }

        /// <summary>
        /// Applies a category to the test
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public TestCaseData SetCategory(string category)
        {
            this.Properties.Add(PropertyNames.Category, category);
            return this;
        }

        /// <summary>
        /// Applies a named property to the test
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public TestCaseData SetProperty(string propName, string propValue)
        {
            this.Properties.Add(propName, propValue);
            return this;
        }

        /// <summary>
        /// Applies a named property to the test
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public TestCaseData SetProperty(string propName, int propValue)
        {
            this.Properties.Add(propName, propValue);
            return this;
        }

        /// <summary>
        /// Applies a named property to the test
        /// </summary>
        /// <param name="propName"></param>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public TestCaseData SetProperty(string propName, double propValue)
        {
            this.Properties.Add(propName, propValue);
            return this;
        }

        /// <summary>
        /// Ignores this TestCase.
        /// </summary>
        /// <returns></returns>
        public TestCaseData Ignore()
        {
            this.RunState = RunState.Ignored;
            return this;
        }
		
		/// <summary>
		/// Marks the test case as explicit.
		/// </summary>
		public TestCaseData Explicit()	{
			this.RunState = RunState.Explicit;
			return this;
		}

		/// <summary>
		/// Marks the test case as explicit, specifying the reason.
		/// </summary>
		public TestCaseData Explicit(string reason)
		{
			this.RunState = RunState.Explicit;
            this.Properties.Set(PropertyNames.SkipReason, reason);
			return this;
		}

        /// <summary>
        /// Ignores this TestCase, specifying the reason.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <returns></returns>
        public TestCaseData Ignore(string reason)
        {
            this.RunState = RunState.Ignored;
            this.Properties.Set(PropertyNames.SkipReason, reason);
            return this;
        }

        #endregion
    }
}
