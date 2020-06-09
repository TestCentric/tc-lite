// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using TCLite.Framework.Constraints;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    public abstract partial class Assert
    {
        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(object actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(actual))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.True, message, args);
        }

        /// <summary>
        /// Apply a constraint to a referenced value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="actual">The actual value to test</param>
        /// <param name="expression">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That(ref bool actual, IResolveConstraint expression, string message=null, params object[] args)
        {
            Constraint constraint = expression.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(ref actual))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Apply a constraint to an actual value, succeeding if the constraint
        /// is satisfied and throwing an assertion exception on failure.
        /// </summary>
        /// <param name="del">An ActualValueDelegate returning the value to be tested</param>
        /// <param name="expr">A Constraint expression to be applied</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void That<T>(ActualValueDelegate<T> del, IResolveConstraint expr, string message=null, params object[] args)
        {
            Constraint constraint = expr.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(del))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
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
            Constraint constraint = expression.Resolve();

            IncrementAssertCount();
            if (!constraint.Matches(ref actual))
            {
                MessageWriter writer = new TextMessageWriter(message, args);
                constraint.WriteMessageTo(writer);
                throw new AssertionException(writer.ToString());
            }
        }

        /// <summary>
        /// Asserts that the code represented by a delegate throws an exception
        /// that satisfies the constraint provided.
        /// </summary>
        /// <param name="code">A TestDelegate to be executed</param>
        /// <param name="constraint">A ThrowsConstraint used in the test</param>
        static public void That(TestDelegate code, IResolveConstraint constraint, string message=null, params object[] args)
        {
            Assert.That((object)code, constraint, message, args);
        }
    }
}
