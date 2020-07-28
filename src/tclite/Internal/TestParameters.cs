// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Interfaces;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// TestCaseParameters encapsulates method arguments and
    /// other selected parameters needed for constructing
    /// a parameterized test case.
    /// </summary>
    public class TestParameters : ITestData, IApplyToTest
    {
        /// <summary>
        /// Construct an empty parameter set, which
        /// defaults to being Runnable.
        /// </summary>
        public TestParameters() : this(new object[0]) { }

        /// <summary>
        /// Construct a parameter set with a list of arguments
        /// </summary>
        /// <param name="args"></param>
        public TestParameters(object[] args)
        {
            OriginalArguments = args;

            // Copy args in case they are changed
            var numargs = OriginalArguments.Length;
            Arguments = new object[numargs];
            if (numargs > 0)
                Array.Copy(OriginalArguments, Arguments, numargs);
        }

        /// <summary>
        /// Construct from an object implementing ITestCaseData
        /// </summary>
        public TestParameters(ITestData data)
            : this(data.Arguments)
        {
            TestName = data.TestName;
            RunState = data.RunState;

            foreach (string key in data.Properties.Keys)
                this.Properties[key] = data.Properties[key];
        }

        /// <summary>
        /// Construct a non-runnable ParameterSet, specifying
        /// the provider exception that made it invalid.
        /// </summary>
        public TestParameters(Exception exception)
        {
            this.RunState = RunState.NotRunnable;
            this.Properties.Set(PropertyNames.SkipReason, ExceptionHelper.BuildMessage(exception));
            this.Properties.Set(PropertyNames.ProviderStackTrace, ExceptionHelper.BuildStackTrace(exception));
        }

        /// <summary>
        /// The RunState for this set of parameters.
        /// </summary>
        public RunState RunState { get; set; } = RunState.Runnable;

        /// <summary>
        /// The arguments to be used in running the test,
        /// which must match the method signature.
        /// </summary>
        public object[] Arguments { get; internal set; }

        /// <summary>
        /// The original arguments provided by the user,
        /// used for display purposes.
        /// </summary>
        public object[] OriginalArguments { get; private set; }

        /// <summary>
        /// A name to be used for this test case in lieu
        /// of the standard generated name containing
        /// the argument list.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Gets the property dictionary for this test
        /// </summary>
        public IPropertyBag Properties { get; } = new PropertyBag();

        #region IApplyToTest Members

        /// <summary>
        /// Applies ParameterSet values to the test itself.
        /// </summary>
        /// <param name="test">A test.</param>
        public void ApplyToTest(ITest test)
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
