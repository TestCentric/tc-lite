// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// Marks an assembly, test fixture or test method as being ignored. Ignored tests result in a warning message when the tests are run.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class|AttributeTargets.Assembly, AllowMultiple=false, Inherited=false)]
    public class IgnoreAttribute : TCLiteAttribute, IApplyToTest
    {
        private readonly string _reason;

        /// <summary>
        /// Constructs the attribute giving a reason for ignoring the test
        /// </summary>
        /// <param name="reason">The reason for ignoring the test</param>
        public IgnoreAttribute(string reason)
        {
            _reason = reason;
        }

        #region IApplyToTest members

        /// <summary>
        /// Modifies a test by marking it as Ignored.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(ITest test)
        {
            if (test.RunState != RunState.NotRunnable)
            {
                test.RunState = RunState.Ignored;
                test.Properties.Set(PropertyNames.SkipReason, _reason);
            }
        }

        #endregion
    }
}
