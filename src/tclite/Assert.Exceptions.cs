// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Constraints;

namespace TCLite.Framework
{
    public abstract partial class Assert
    {

        /// <summary>
        /// Verifies that a delegate throws a particular exception when called.
        /// </summary>
        /// <param name="expression">A constraint to be satisfied by the exception</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Throws(IResolveConstraint expression, TestDelegate code, string message=null, params object[] args)
        {
            Exception caughtException = null;

#if NYI
            if (AsyncInvocationRegion.IsAsyncOperation(code))
            {
                using (AsyncInvocationRegion region = AsyncInvocationRegion.Create(code))
                {
                    code();

                    try
                    {
                        region.WaitForPendingOperationsToComplete(null);
                    }
                    catch (Exception e)
                    {
                        caughtException = e;
                    }
                }
            }
            else
#endif
                try
            {
                code();
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.That(caughtException, expression, message, args);

            return caughtException;
        }

        /// <summary>
        /// Verifies that a delegate throws a particular exception when called.
        /// </summary>
        /// <param name="expectedExceptionType">The exception Type expected</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Throws(Type expectedExceptionType, TestDelegate code, string message=null, params object[] args)
        {
            return Throws(new ExceptionTypeConstraint(expectedExceptionType), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws a particular exception when called.
        /// </summary>
        /// <typeparam name="T">Type of the expected exception</typeparam>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static T Throws<T>(TestDelegate code, string message=null, params object[] args) where T : Exception
        {
            return (T)Throws(typeof(T), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws an exception when called
        /// and returns it.
        /// </summary>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Catch(TestDelegate code, string message=null, params object[] args)
        {
            return Throws(new InstanceOfTypeConstraint(typeof(Exception)), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws an exception of a certain Type
        /// or one derived from it when called and returns it.
        /// </summary>
        /// <param name="expectedExceptionType">The expected Exception Type</param>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static Exception Catch(Type expectedExceptionType, TestDelegate code, string message=null, params object[] args)
        {
            return Throws(new InstanceOfTypeConstraint(expectedExceptionType), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate throws an exception of a certain Type
        /// or one derived from it when called and returns it.
        /// </summary>
        /// <typeparam name="T">The expected Exception Type</typeparam>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static T Catch<T>(TestDelegate code, string message=null, params object[] args) where T : System.Exception
        {
            return (T)Throws(new InstanceOfTypeConstraint(typeof(T)), code, message, args);
        }

        /// <summary>
        /// Verifies that a delegate does not throw an exception
        /// </summary>
        /// <param name="code">A TestDelegate</param>
        /// <param name="message">The message that will be displayed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        public static void DoesNotThrow(TestDelegate code, string message=null, params object[] args)
        {
            Assert.That(code, new ThrowsNothingConstraint(), message, args);
        }
    }
}
