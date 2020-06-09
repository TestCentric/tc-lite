// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Threading;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.Framework.Tests;

namespace TCLite.Framework.Commands
{
    /// <summary>
    /// TestMethodCommand is the lowest level concrete command
    /// used to run actual test cases.
    /// </summary>
    public class TestMethodCommand : TestCommand
    {
        private const string TaskWaitMethod = "Wait";
        private const string TaskResultProperty = "Result";
        private const string SystemAggregateException = "System.AggregateException";
        private const string InnerExceptionsProperty = "InnerExceptions";
        private const BindingFlags TaskResultPropertyBindingFlags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;
        private readonly TestMethod testMethod;
        private readonly object[] arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestMethodCommand"/> class.
        /// </summary>
        /// <param name="testMethod">The test.</param>
        public TestMethodCommand(TestMethod testMethod) : base(testMethod)
        {
            this.testMethod = testMethod;
            this.arguments = testMethod.Arguments;
        }

        /// <summary>
        /// Runs the test, saving a TestResult in the execution context, as
        /// well as returning it. If the test has an expected result, it
        /// is asserts on that value. Since failed tests and errors throw
        /// an exception, this command must be wrapped in an outer command,
        /// will handle that exception and records the failure. This role
        /// is usually played by the SetUpTearDown command.
        /// </summary>
        /// <param name="context">The execution context</param>
        public override TestResult Execute(TestExecutionContext context)
        {
            if (!testMethod.Method.IsStatic)
                context.TestObject = Reflect.Construct(testMethod.FixtureType);

            try
            {
                object result = RunTestMethod(context);

                if (testMethod.HasExpectedResult)
                    TCLite.Framework.Assert.AreEqual(testMethod.ExpectedResult, result);

                context.CurrentResult.SetResult(ResultState.Success);
            }
            catch(Exception ex)
            {
                if (ex is ThreadAbortException)
                    Thread.ResetAbort();

                context.CurrentResult.RecordException(ex);
            }

            // TODO: Set assert count here?
            //context.CurrentResult.AssertCount = context.AssertCount;
            return context.CurrentResult;
        }

        private object RunTestMethod(TestExecutionContext context)
        {
#if NYI
            if (MethodHelper.IsAsyncMethod(testMethod.Method))
                return RunAsyncTestMethod(context);
            //{
            //    if (testMethod.Method.ReturnType == typeof(void))
            //        return RunAsyncVoidTestMethod(context);
            //    else
            //        return RunAsyncTaskTestMethod(context);
            //}
            else
#endif
                return RunNonAsyncTestMethod(context);
        }

#if NYI
        private object RunAsyncTestMethod(TestExecutionContext context)
        {
            using (AsyncInvocationRegion region = AsyncInvocationRegion.Create(testMethod.Method))
            {
                object result = Reflect.InvokeMethod(testMethod.Method, context.TestObject, arguments);

                try
                {
                    return region.WaitForPendingOperationsToComplete(result);
                }
                catch (Exception e)
                {
                    throw new NUnitException("Rethrown", e);
                }
            }
        }
#endif

        private object RunNonAsyncTestMethod(TestExecutionContext context)
        {
            return Reflect.InvokeMethod(testMethod.Method, context.TestObject, arguments);
        }

#if NYI
        private object RunAsyncVoidTestMethod(TestExecutionContext context)
        {
            var previousContext = SynchronizationContext.Current;
            var currentContext = new AsyncSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(currentContext);

            try
            {
                object result = Reflect.InvokeMethod(testMethod.Method, context.TestObject, arguments);

                currentContext.WaitForOperationCompleted();

                if (currentContext.Exceptions.Count > 0)
                    throw new NUnitException("Rethrown", currentContext.Exceptions[0]);

                return result;
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(previousContext);
            }
        }

        private object RunAsyncTaskTestMethod(TestExecutionContext context)
        {
            try
            {
                object task = Reflect.InvokeMethod(testMethod.Method, context.TestObject, arguments);

                Reflect.InvokeMethod(testMethod.Method.ReturnType.GetMethod(TaskWaitMethod, new Type[0]), task);
                PropertyInfo resultProperty = testMethod.Method.ReturnType.GetProperty(TaskResultProperty, TaskResultPropertyBindingFlags);

                return resultProperty != null ? resultProperty.GetValue(task, null) : null;
            }
            catch (NUnitException e)
            {
                if (e.InnerException != null &&
                    e.InnerException.GetType().FullName.Equals(SystemAggregateException))
                {
                    IList<Exception> inner = (IList<Exception>)e.InnerException.GetType()
                        .GetProperty(InnerExceptionsProperty).GetValue(e.InnerException, null);

                    throw new NUnitException("Rethrown", inner[0]);
                }

                throw;
            }
        }
#endif
    }
}