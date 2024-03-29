// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Constraints;
using TCLite.Internal;

namespace TCLite
{
    public abstract partial class Assert
    {
        /// <summary>
        /// Asserts that a condition is true. If the condition is false
        /// the method throws a <see cref="TCLite.AssertionException"/>,
        /// ending the running test and reporting it as a Failure.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.True, message, args);
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That<T>(T actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            IConstraint constraint = expression.Resolve();

            IncrementAssertCount();
            var result = constraint.ApplyTo(actual);
            if (!result.IsSuccess)
                ReportFailure(result, message, args);
        }

        /// <summary>
        /// Apply a constraint to a <see cref="TCLite.ActualValueDelegate"/>,
        /// succeeding if the constraint is satisfied and throwing an
        /// assertion exception on failure.
        /// </summary>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That<T>(ActualValueDelegate<T> del, IResolveConstraint expr, string message=null, params object[] args)
        {
            IConstraint constraint = expr.Resolve();

            IncrementAssertCount();
            var result = constraint.ApplyTo(del);
            if (!result.IsSuccess)
                ReportFailure(result, message, args);
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That<T>(ref T actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            IConstraint constraint = expression.Resolve();

            IncrementAssertCount();
            var result = constraint.ApplyTo(ref actual);
            if (!result.IsSuccess)
                ReportFailure(result, message, args);
        }
    }
}
