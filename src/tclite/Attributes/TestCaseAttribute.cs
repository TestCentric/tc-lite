// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// TestCaseAttribute is used to mark both parameterized and
    /// non-parameterized test cases. In the case of parameterized
    /// tests, it provides the arguments to be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited=false)]
    public class TestCaseAttribute : DataParamAttribute, ITestCaseData, ITestCaseSource, IImplyFixture
    {
        /// <summary>
        /// Construct a TestCaseAttribute with a list of arguments.
        /// This constructor is not CLS-Compliant
        /// </summary>
        /// <param name="arguments"></param>
        public TestCaseAttribute(params object[] arguments)
        {
			RunState = RunState.Runnable;			
            Arguments = arguments ?? new object[] { null };
        }

        #region ICaseTestData Members

        /// <summary>
        /// Gets or sets the name of the test.
        /// </summary>
        /// <value>The name of the test.</value>
        public string TestName { get; set; }

        /// <summary>
        /// Gets the RunState of this test case.
        /// </summary>
        public RunState RunState { get; private set; }

        /// <summary>
        /// Gets the list of arguments to a test case
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// Gets the properties of  the test case
        /// </summary>
        public IPropertyBag Properties { get; } = new PropertyBag();

        /// <summary>
        /// Gets or sets the expected result.
        /// </summary>
        /// <value>The result.</value>
        public object ExpectedResult
        {
            get { return _expectedResult; }
            set 
            { 
                _expectedResult = value;
                HasExpectedResult = true;
            }
        }
        private object _expectedResult;

        /// <summary>
        /// Returns true if the expected result has been set
        /// </summary>
        public bool HasExpectedResult { get; private set; }
        
        #endregion

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return Properties.Get(PropertyNames.Description) as string; }
            set
            {
                Guard.ArgumentNotNull(value, nameof(value));
                Properties.Set(PropertyNames.Description, value);
            }
        }

        /// <summary>
        /// Gets or sets the ignored status of the test
        /// </summary>
        public bool Ignore 
		{ 
			get { return RunState == RunState.Ignored; }
			set { RunState = value ? RunState.Ignored : RunState.Runnable; } 
		}
		
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TestCaseAttribute"/> is explicit.
		/// </summary>
		/// <value>
		/// <c>true</c> if explicit; otherwise, <c>false</c>.
		/// </value>
		public bool Explicit 
		{ 
			get { return RunState == RunState.Explicit; }
			set { RunState = value ? RunState.Explicit : RunState.Runnable; }
		}

		/// <summary>
		/// Gets or sets the reason for not running the test.
		/// </summary>
		/// <value>The reason.</value>
		public string Reason 
		{ 
			get { return this.Properties.Get(PropertyNames.SkipReason) as string; }
			set { this.Properties.Set(PropertyNames.SkipReason, value); }
		}

        /// <summary>
        /// Gets or sets the ignore reason. When set to a non-null
        /// non-empty value, the test is marked as ignored.
        /// </summary>
        /// <value>The ignore reason.</value>
        public string IgnoreReason
        {
            get { return this.Reason; }
            set
            {
				RunState = RunState.Ignored;
                this.Reason = value;
            }
        }

        /// <summary>
        /// Gets and sets the category for this fixture.
        /// May be a comma-separated list of categories.
        /// </summary>
        public string Category
        {
            get { return Properties.Get(PropertyNames.Category) as string; }
            set 
			{ 
				foreach (string cat in value.Split(new char[] { ',' }) )
					Properties.Add(PropertyNames.Category, cat); 
			}
        }
 
        /// <summary>
        /// Gets a list of categories for this fixture
        /// </summary>
        public IList Categories
        {
            get { return Properties[PropertyNames.Category] as IList; }
        }
 
        #region ITestCaseSource Members

        /// <summary>
        /// Returns an collection containing a single ITestCaseData item,
        /// constructed from the arguments provided in the constructor and
        /// possibly converted to match the specified method.
        /// </summary>
        /// <param name="method">The method for which data is being provided</param>
        /// <returns></returns>
        public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            TestCaseParameters parms;

            try
            {
                ParameterInfo[] parameters = method.GetParameters();
                int argsNeeded = parameters.Length;
                int argsProvided = Arguments.Length;

                parms = new TestCaseParameters(this);

                // Special handling for params arguments
                if (argsNeeded > 0 && argsProvided >= argsNeeded - 1)
                {
                    ParameterInfo lastParameter = parameters[argsNeeded - 1];
                    Type lastParameterType = lastParameter.ParameterType;
                    Type elementType = lastParameterType.GetElementType();

                    if (lastParameterType.IsArray && lastParameter.IsDefined(typeof(ParamArrayAttribute), false))
                    {
                        if (argsProvided == argsNeeded)
                        {
                            Type lastArgumentType = parms.Arguments[argsProvided - 1].GetType();
                            if (!lastParameterType.IsAssignableFrom(lastArgumentType))
                            {
                                Array array = Array.CreateInstance(elementType, 1);
                                array.SetValue(parms.Arguments[argsProvided - 1], 0);
                                parms.Arguments[argsProvided - 1] = array;
                            }
                        }
                        else
                        {
                            object[] newArglist = new object[argsNeeded];
                            for (int i = 0; i < argsNeeded && i < argsProvided; i++)
                                newArglist[i] = parms.Arguments[i];

                            int length = argsProvided - argsNeeded + 1;
                            Array array = Array.CreateInstance(elementType, length);
                            for (int i = 0; i < length; i++)
                                array.SetValue(parms.Arguments[argsNeeded + i - 1], i);

                            newArglist[argsNeeded - 1] = array;
                            parms.Arguments = newArglist;
                            argsProvided = argsNeeded;
                        }
                    }
                }

                //if (method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(object[]))
                //    parms.Arguments = new object[]{parms.Arguments};

                // Special handling when sole argument is an object[]
                if (argsNeeded == 1 && method.GetParameters()[0].ParameterType == typeof(object[]))
                {
                    if (argsProvided > 1 ||
                        argsProvided == 1 && parms.Arguments[0]?.GetType() != typeof(object[]))
                    {
                        parms.Arguments = new object[] { parms.Arguments };
                    }
                }

                if (argsProvided == argsNeeded)
                    PerformSpecialConversions(parms.Arguments, parameters);
            }
            catch (Exception ex)
            {
                parms = new TestCaseParameters(ex);
            }
            
            return new ITestCaseData[] { parms };
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Performs several special conversions allowed by NUnit in order to
        /// permit arguments with types that cannot be used in the constructor
        /// of an Attribute such as TestCaseAttribute or to simplify their use.
        /// </summary>
        /// <param name="arglist">The arguments to be converted</param>
        /// <param name="parameters">The ParameterInfo array for the method</param>
        private static void PerformSpecialConversions(object[] arglist, ParameterInfo[] parameters)
        {
            for (int i = 0; i < arglist.Length; i++)
            {
                object arg = arglist[i];
                object convertedArg;
                Type targetType = parameters[i].ParameterType;

                if(TryConvert(arg, targetType, out convertedArg))
                    arglist[i] = convertedArg;
            }
        }
        #endregion
    }
}
