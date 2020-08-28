// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Threading;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Commands
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
        private readonly TestMethod _testMethod;
        private readonly object[] _arguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestMethodCommand"/> class.
        /// </summary>
        /// <param name="testMethod">The test.</param>
        public TestMethodCommand(TestMethod testMethod) : base(testMethod)
        {
            _testMethod = testMethod;
            _arguments = testMethod.Arguments;
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
            switch (Test.RunState)
            {
                default:
                case RunState.Runnable:
                    RunTestMethod(context);
                    break;
                case RunState.Skipped:
                    context.CurrentResult.SetResult(ResultState.Skipped, GetSkipReason());
                    break;
                case RunState.Ignored:
                    context.CurrentResult.SetResult(ResultState.Ignored, GetSkipReason());
                    break;
                case RunState.NotRunnable:
                    context.CurrentResult.SetResult(ResultState.NotRunnable, GetSkipReason(), GetProviderStackTrace());
                    break;
            }

            if (context.CurrentResult.AssertionResults.Count > 0)
                context.CurrentResult.RecordTestCompletion();

            context.CurrentResult.AssertCount = context.AssertCount;
            return context.CurrentResult;
        }

        private string GetSkipReason()
        {
            return (string)Test.Properties.Get(PropertyNames.SkipReason);
        }

        private string GetProviderStackTrace()
        {
            return (string)Test.Properties.Get(PropertyNames.ProviderStackTrace);
        }

        private void RunTestMethod(TestExecutionContext context)
        {
            try
            {
#if NYI // async
                object result = 
                    MethodHelper.IsAsyncMethod(testMethod.Method))
                        ? RunAsyncTestMethod(context)
                        : RunNonAsyncTestMethod(context);
#else
                object result = RunNonAsyncTestMethod(context);
#endif
                if (_testMethod.HasExpectedResult)
                    Assert.AreEqual(_testMethod.ExpectedResult, result);

                context.CurrentResult.SetResult(ResultState.Success);
            }
            catch (Exception ex)
            {
                if (ex is ThreadAbortException)
                    Thread.ResetAbort();

                context.CurrentResult.RecordException(ex);
            }
        }

#if NYI // async
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
            return Reflect.InvokeMethod(_testMethod.Method, context.TestObject, _arguments);
        }

#if NYI // async
        private object RunAsyncVoidTestMethod(ITestExecutionContext context)
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