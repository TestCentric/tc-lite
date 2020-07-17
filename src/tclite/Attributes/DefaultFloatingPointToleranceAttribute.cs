// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Constraints;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Sets the tolerance used by default when checking the equality of floating point values
    /// within the test assembly, fixture or method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DefaultFloatingPointToleranceAttribute : TCLiteAttribute, IApplyToContext
    {
        private readonly Tolerance _tolerance;

        /// <summary>
        /// Construct specifying an amount
        /// </summary>
        /// <param name="amount"></param>
        public DefaultFloatingPointToleranceAttribute(double amount)
        {
            _tolerance = new Tolerance(amount);
        }

        #region IApplyToContext Members

        /// <summary>
        /// Apply changes to the TestExecutionContext
        /// </summary>
        /// <param name="context">The TestExecutionContext</param>
        public void ApplyToContext(ITestExecutionContext context)
        {
            context.DefaultFloatingPointTolerance = _tolerance;
        }

        #endregion
    }
}
