// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Marks an assembly, test fixture or test method such that it will only run if explicitly 
    /// executed from the GUI, command line or included within a test filter. 
    /// The test will not be run simply because an enclosing suite is run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public sealed class ExplicitAttribute : TCLiteAttribute, IApplyToTest
    {
        private readonly string _reason;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExplicitAttribute()
        {
        }

        /// <summary>
        /// Constructor with a reason
        /// </summary>
        /// <param name="reason">The reason test is marked explicit</param>
        public ExplicitAttribute(string reason)
        {
            _reason = reason;
        }

        #region IApplyToTest members

        /// <summary>
        /// Modifies a test by marking it as explicit.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(ITest test)
        {
            if (test.RunState != RunState.NotRunnable && test.RunState != RunState.Ignored)
            {
                test.RunState = RunState.Explicit;
                if (_reason != null)
                    test.Properties.Set(PropertyNames.SkipReason, _reason);
            }
        }

        #endregion
    }
}
