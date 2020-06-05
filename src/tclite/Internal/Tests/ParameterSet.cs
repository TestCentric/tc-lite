// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Tests
{
    /// <summary>
    /// ParameterSet encapsulates method arguments and
    /// other selected parameters needed for constructing
    /// a parameterized test case.
    /// </summary>
    public class ParameterSet : ITestCaseData, IApplyToTest
    {
        private object[] _arguments;
        private object _expectedResult;

        /// <summary>
        /// Construct an empty parameter set, which
        /// defaults to being Runnable.
        /// </summary>
        public ParameterSet()
        {
        }

        /// <summary>
        /// Construct a ParameterSet from an object implementing ITestCaseData
        /// </summary>
        /// <param name="data"></param>
        public ParameterSet(ITestCaseData data)
        {
            this.TestName = data.TestName;
            this.RunState = data.RunState;
            this.Arguments = data.Arguments;
            
			if (data.HasExpectedResult)
                this.ExpectedResult = data.ExpectedResult;
			
            foreach (string key in data.Properties.Keys)
                this.Properties[key] = data.Properties[key];
        }

#if NYI
        /// <summary>
        /// Construct a non-runnable ParameterSet, specifying
        /// the provider exception that made it invalid.
        /// </summary>
        public ParameterSet(Exception exception)
        {
            this.RunState = RunState.NotRunnable;
            this.Properties.Set(PropertyNames.SkipReason, ExceptionHelper.BuildMessage(exception));
            this.Properties.Set(PropertyNames.ProviderStackTrace, ExceptionHelper.BuildStackTrace(exception));
        }
#endif

        /// <summary>
        /// The RunState for this set of parameters.
        /// </summary>
        public RunState RunState { get; set; } = RunState.Runnable;

        /// <summary>
        /// The arguments to be used in running the test,
        /// which must match the method signature.
        /// </summary>
        public object[] Arguments
        {
            get { return _arguments; }
            set 
            { 
                _arguments = value;

                if (OriginalArguments == null)
                    OriginalArguments = value;
            }
        }

        /// <summary>
        /// The original arguments provided by the user,
        /// used for display purposes.
        /// </summary>
        public object[] OriginalArguments { get; private set; }

        /// <summary>
        /// The expected result of the test, which
        /// must match the method return type.
        /// </summary>
        public object ExpectedResult
        {
            get { return _expectedResult; }
            set
            {
                _expectedResult = value;
                HasExpectedResult = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an expected result was specified.
        /// </summary>
        public bool HasExpectedResult { get; private set; }

        /// <summary>
        /// A name to be used for this test case in lieu
        /// of the standard generated name containing
        /// the argument list.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Gets the property dictionary for this test
        /// </summary>
        //public IPropertyBag Properties { get; } = new PropertyBag();
        public IPropertyBag Properties => throw new NotImplementedException();

        #region IApplyToTest Members

        /// <summary>
        /// Applies ParameterSet values to the test itself.
        /// </summary>
        /// <param name="test">A test.</param>
        public void ApplyToTest(Test test)
        {
            if (this.RunState != RunState.Runnable)
				test.RunState = this.RunState;

            foreach (string key in Properties.Keys)
                foreach (object value in Properties[key])
                    test.Properties.Add(key, value);
        }

        #endregion
    }
}
