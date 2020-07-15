// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Linq;
using TCLite.Framework.Api;
using TCLite.Framework.Constraints;
using TCLite.Framework.Internal;
using TCLite.TestUtilities;

#if TASK_PARALLEL_LIBRARY_API
using System.Threading.Tasks;
#endif

#if NET40
using Task = System.Threading.Tasks.TaskEx;
#endif

namespace TCLite.Framework.Assertions
{
    [TestFixture]
    public class WarningTests
    {
        #region Passing Tests

        [TestCase]
        public void WarnUnless_Passes_Boolean()
        {
            Warn.Unless(2 + 2 == 4);
        }

        [TestCase]
        public void WarnIf_Passes_Boolean()
        {
            Warn.If(2 + 2 != 4);
        }

        [TestCase]
        public void WarnUnless_Passes_BooleanWithMessage()
        {
            Warn.Unless(2 + 2 == 4, "Not Equal");
        }

        [TestCase]
        public void WarnIf_Passes_BooleanWithMessage()
        {
            Warn.If(2 + 2 != 4, "Not Equal");
        }

        [TestCase]
        public void WarnUnless_Passes_BooleanWithMessageAndArgs()
        {
            Warn.Unless(2 + 2 == 4, "Not Equal to {0}", 4);
        }

        [TestCase]
        public void WarnIf_Passes_BooleanWithMessageAndArgs()
        {
            Warn.If(2 + 2 != 4, "Not Equal to {0}", 4);
        }

        [TestCase]
        public void WarnUnless_Passes_BooleanWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.Unless(2 + 2 == 4, getExceptionMessage);
        }

        [TestCase]
        public void WarnIf_Passes_BooleanWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.If(2 + 2 != 4, getExceptionMessage);
        }

        [TestCase]
        public void WarnUnless_Passes_BooleanLambda()
        {
            Warn.Unless(() => 2 + 2 == 4);
        }

        [TestCase]
        public void WarnIf_Passes_BooleanLambda()
        {
            Warn.If(() => 2 + 2 != 4);
        }

        [TestCase]
        public void WarnUnless_Passes_BooleanLambdaWithMessage()
        {
            Warn.Unless(() => 2 + 2 == 4, "Not Equal");
        }

        [TestCase]
        public void WarnIf_Passes_BooleanLambdaWithMessage()
        {
            Warn.If(() => 2 + 2 != 4, "Not Equal");
        }

        [TestCase]
        public void WarnUnless_Passes_BooleanLambdaWithMessageAndArgs()
        {
            Warn.Unless(() => 2 + 2 == 4, "Not Equal to {0}", 4);
        }

        [TestCase]
        public void WarnIf_Passes_BooleanLambdaWithMessageAndArgs()
        {
            Warn.If(() => 2 + 2 != 4, "Not Equal to {0}", 4);
        }

        [TestCase]
        public void WarnUnless_Passes_BooleanLambdaWithWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.Unless(() => 2 + 2 == 4, getExceptionMessage);
        }

        [TestCase]
        public void WarnIf_Passes_BooleanLambdaWithWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.If(() => 2 + 2 != 4, getExceptionMessage);
        }

        [TestCase]
        public void WarnUnless_Passes_ActualAndConstraint()
        {
            Warn.Unless(2 + 2, Is.EqualTo(4));
        }

        [TestCase]
        public void WarnIf_Passes_ActualAndConstraint()
        {
            Warn.If(2 + 2, Is.Not.EqualTo(4));
        }

        [TestCase]
        public void WarnUnless_Passes_ActualAndConstraintWithMessage()
        {
            Warn.Unless(2 + 2, Is.EqualTo(4), "Should be 4");
        }

        [TestCase]
        public void WarnIf_Passes_ActualAndConstraintWithMessage()
        {
            Warn.If(2 + 2, Is.Not.EqualTo(4), "Should be 4");
        }

        [TestCase]
        public void WarnUnless_Passes_ActualAndConstraintWithMessageAndArgs()
        {
            Warn.Unless(2 + 2, Is.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase]
        public void WarnIf_Passes_ActualAndConstraintWithMessageAndArgs()
        {
            Warn.If(2 + 2, Is.Not.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase]
        public void WarnUnless_Passes_ActualAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.Unless(2 + 2, Is.EqualTo(4), getExceptionMessage);
        }

        [TestCase]
        public void WarnIf_Passes_ActualAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.If(2 + 2, Is.Not.EqualTo(4), getExceptionMessage);
        }

        [TestCase]
        public void WarnUnless_Passes_ActualLambdaAndConstraint()
        {
            Warn.Unless(() => 2 + 2, Is.EqualTo(4));
        }

        [TestCase]
        public void WarnIf_Passes_ActualLambdaAndConstraint()
        {
            Warn.If(() => 2 + 2, Is.Not.EqualTo(4));
        }

        [TestCase]
        public void WarnUnless_Passes_ActualLambdaAndConstraintWithMessage()
        {
            Warn.Unless(() => 2 + 2, Is.EqualTo(4), "Should be 4");
        }

        [TestCase]
        public void WarnIf_Passes_ActualLambdaAndConstraintWithMessage()
        {
            Warn.If(() => 2 + 2, Is.Not.EqualTo(4), "Should be 4");
        }

        [TestCase]
        public void WarnUnless_Passes_ActualLambdaAndConstraintWithMessageAndArgs()
        {
            Warn.Unless(() => 2 + 2, Is.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase]
        public void WarnIf_Passes_ActualLambdaAndConstraintWithMessageAndArgs()
        {
            Warn.If(() => 2 + 2, Is.Not.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase]
        public void WarnUnless_Passes_ActualLambdaAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.Unless(() => 2 + 2, Is.EqualTo(4), getExceptionMessage);
        }

        [TestCase]
        public void WarnIf_Passes_ActualLambdaAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.If(() => 2 + 2, Is.Not.EqualTo(4), getExceptionMessage);
        }

        [TestCase]
        public void WarnUnless_Passes_DelegateAndConstraint()
        {
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4));
        }

        [TestCase]
        public void WarnIf_Passes_DelegateAndConstraint()
        {
            Warn.If(new ActualValueDelegate<int>(ReturnsFour), Is.Not.EqualTo(4));
        }

        [TestCase]
        public void WarnUnless_Passes_DelegateAndConstraintWithMessage()
        {
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4), "Message");
        }

        [TestCase]
        public void WarnIf_Passes_DelegateAndConstraintWithMessage()
        {
            Warn.If(new ActualValueDelegate<int>(ReturnsFour), Is.Not.EqualTo(4), "Message");
        }

        [TestCase]
        public void WarnUnless_Passes_DelegateAndConstraintWithMessageAndArgs()
        {
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase]
        public void WarnIf_Passes_DelegateAndConstraintWithMessageAndArgs()
        {
            Warn.If(new ActualValueDelegate<int>(ReturnsFour), Is.Not.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase]
        public void WarnUnless_Passes_DelegateAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4), getExceptionMessage);
        }

        [TestCase]
        public void WarnIf_Passes_DelegateAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => string.Format("Not Equal to {0}", 4);
            Warn.If(new ActualValueDelegate<int>(ReturnsFour), Is.Not.EqualTo(4), getExceptionMessage);
        }

#if TASK_PARALLEL_LIBRARY_API
        [TestCase]
        public void WarnUnless_Passes_Async()
        {
            Warn.Unless(async () => await One(), Is.EqualTo(1));
        }

        [TestCase]
        public void WarnIf_Passes_Async()
        {
            Warn.If(async () => await One(), Is.Not.EqualTo(1));
        }
#endif

        #endregion

        #region Failing Tests

        [TestCase, ExpectWarning("MESSAGE")]
        public void CallAssertWarnWithMessage()
        {
            Assert.Warn("MESSAGE");
        }

        [TestCase, ExpectWarning("MESSAGE: 2+2=4")]
        public void CallAssertWarnWithMessageAndArgs()
        {
            Assert.Warn(@"MESSAGE: {0}+{1}={2}", 2, 2, 4);
        }

        [TestCase, ExpectWarning]
        public void WarnUnless_Fails_Boolean()
        {
            Warn.Unless(2 + 2 == 5);
        }

        [TestCase, ExpectWarning]
        public void WarnIf_Fails_Boolean()
        {
            Warn.If(2 + 2 != 5);
        }

        [TestCase, ExpectWarning("message")]
        public void WarnUnless_Fails_BooleanWithMessage()
        {
            Warn.Unless(2 + 2 == 5, "message");
        }

        [TestCase, ExpectWarning("message")]
        public void WarnIf_Fails_BooleanWithMessage()
        {
            Warn.If(2 + 2 != 5, "message");
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnUnless_Fails_BooleanWithMessageAndArgs()
        {
            Warn.Unless(2 + 2 == 5, "got {0}", 5);
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnIf_Fails_BooleanWithMessageAndArgs()
        {
            Warn.If(2 + 2 != 5, "got {0}", 5);
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnUnless_Fails_BooleanWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "got 5";
            Warn.Unless(2 + 2 == 5, getExceptionMessage);
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnIf_Fails_BooleanWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "got 5";
            Warn.If(2 + 2 != 5, getExceptionMessage);
        }

        [TestCase, ExpectWarning]
        public void WarnUnless_Fails_BooleanLambda()
        {
            Warn.Unless(() => 2 + 2 == 5);
        }

        [TestCase, ExpectWarning]
        public void WarnIf_Fails_BooleanLambda()
        {
            Warn.If(() => 2 + 2 != 5);
        }

        [TestCase, ExpectWarning("message")]
        public void WarnUnless_Fails_BooleanLambdaWithMessage()
        {
            Warn.Unless(() => 2 + 2 == 5, "message");
        }

        [TestCase, ExpectWarning("message")]
        public void WarnIf_Fails_BooleanLambdaWithMessage()
        {
            Warn.If(() => 2 + 2 != 5, "message");
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnUnless_Fails_BooleanLambdaWithMessageAndArgs()
        {
            Warn.Unless(() => 2 + 2 == 5, "got {0}", 5);
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnIf_Fails_BooleanLambdaWithMessageAndArgs()
        {
            Warn.If(() => 2 + 2 != 5, "got {0}", 5);
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnUnless_Fails_BooleanLambdaWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "got 5";
            Warn.Unless(() => 2 + 2 == 5, getExceptionMessage);
        }

        [TestCase, ExpectWarning("got 5")]
        public void WarnIf_Fails_BooleanLambdaWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "got 5";
            Warn.If(() => 2 + 2 != 5, getExceptionMessage);
        }

        [TestCase, ExpectWarning]
        public void WarnUnless_Fails_ActualAndConstraint()
        {
            Warn.Unless(2 + 2, Is.EqualTo(5));
        }

        [TestCase, ExpectWarning]
        public void WarnIf_Fails_ActualAndConstraint()
        {
            Warn.If(2 + 2, Is.Not.EqualTo(5));
        }

        [TestCase, ExpectWarning("Error")]
        public void WarnUnless_Fails_ActualAndConstraintWithMessage()
        {
            Warn.Unless(2 + 2, Is.EqualTo(5), "Error");
        }

        [TestCase, ExpectWarning("Error")]
        public void WarnIf_Fails_ActualAndConstraintWithMessage()
        {
            Warn.If(2 + 2, Is.Not.EqualTo(5), "Error");
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnUnless_Fails_ActualAndConstraintWithMessageAndArgs()
        {
            Warn.Unless(2 + 2, Is.EqualTo(5), "Should be {0}", 5);
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnIf_Fails_ActualAndConstraintWithMessageAndArgs()
        {
            Warn.If(2 + 2, Is.Not.EqualTo(5), "Should be {0}", 5);
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnUnless_Fails_ActualAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "Should be 5";
            Warn.Unless(2 + 2, Is.EqualTo(5), getExceptionMessage);
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnIf_Fails_ActualAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "Should be 5";
            Warn.If(2 + 2, Is.Not.EqualTo(5), getExceptionMessage);
        }

        [TestCase, ExpectWarning]
        public void WarnUnless_Fails_ActualLambdaAndConstraint()
        {
            Warn.Unless(() => 2 + 2, Is.EqualTo(5));
        }

        [TestCase, ExpectWarning]
        public void WarnIf_Fails_ActualLambdaAndConstraint()
        {
            Warn.If(() => 2 + 2, Is.Not.EqualTo(5));
        }

        [TestCase, ExpectWarning("Error")]
        public void WarnUnless_Fails_ActualLambdaAndConstraintWithMessage()
        {
            Warn.Unless(() => 2 + 2, Is.EqualTo(5), "Error");
        }

        [TestCase, ExpectWarning("Error")]
        public void WarnIf_Fails_ActualLambdaAndConstraintWithMessage()
        {
            Warn.If(() => 2 + 2, Is.Not.EqualTo(5), "Error");
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnUnless_Fails_ActualLambdaAndConstraintWithMessageAndArgs()
        {
            Warn.Unless(() => 2 + 2, Is.EqualTo(5), "Should be {0}", 5);
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnIf_Fails_ActualLambdaAndConstraintWithMessageAndArgs()
        {
            Warn.If(() => 2 + 2, Is.Not.EqualTo(5), "Should be {0}", 5);
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnUnless_Fails_ActualLambdaAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "Should be 5";
            Warn.Unless(() => 2 + 2, Is.EqualTo(5), getExceptionMessage);
        }

        [TestCase, ExpectWarning("Should be 5")]
        public void WarnIf_Fails_ActualLambdaAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "Should be 5";
            Warn.If(() => 2 + 2, Is.Not.EqualTo(5), getExceptionMessage);
        }

        [TestCase, ExpectWarning]
        public void WarnUnless_Fails_DelegateAndConstraint()
        {
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4));
        }

        [TestCase, ExpectWarning]
        public void WarnIf_Fails_DelegateAndConstraint()
        {
            Warn.If(new ActualValueDelegate<int>(ReturnsFive), Is.Not.EqualTo(4));
        }

        [TestCase, ExpectWarning("Error")]
        public void WarnUnless_Fails_DelegateAndConstraintWithMessage()
        {
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4), "Error");
        }

        [TestCase, ExpectWarning("Error")]
        public void WarnIf_Fails_DelegateAndConstraintWithMessage()
        {
            Warn.If(new ActualValueDelegate<int>(ReturnsFive), Is.Not.EqualTo(4), "Error");
        }

        [TestCase, ExpectWarning("Should be 4")]
        public void WarnUnless_Fails_DelegateAndConstraintWithMessageAndArgs()
        {
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase, ExpectWarning("Should be 4")]
        public void WarnIf_Fails_DelegateAndConstraintWithMessageAndArgs()
        {
            Warn.If(new ActualValueDelegate<int>(ReturnsFive), Is.Not.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase, ExpectWarning("Should be 4")]
        public void WarnUnless_Fails_DelegateAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "Should be 4";
            Warn.Unless(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4), getExceptionMessage);
        }

        [TestCase, ExpectWarning("Should be 4")]
        public void WarnIf_Fails_DelegateAndConstraintWithMessageStringFunc()
        {
            Func<string> getExceptionMessage = () => "Should be 4";
            Warn.If(new ActualValueDelegate<int>(ReturnsFive), Is.Not.EqualTo(4), getExceptionMessage);
        }

#if TASK_PARALLEL_LIBRARY_API
        [TestCase]
        public void WarnUnless_Fails_Async()
        {
            Warn.Unless(async () => await One(), Is.EqualTo(2));
        }

        [TestCase]
        public void WarnIf_Fails_Async()
        {
            Warn.If(async () => await One(), Is.Not.EqualTo(2));
        }
#endif

        #endregion

        #region Warnings followed by terminating Assert

        [TestCase, ExpectFailure("Multiple failures or warnings", "First warning", "Second warning", "This fails")]
        public void TwoWarningsAndFailure()
        {
            Assert.Warn("First warning");
            Assert.Warn("Second warning");
            Assert.Fail("This fails");
        }

#if NYI // Warnings plus Ignore or Inconclusive
        [TestCase, ExpectWarning("Multiple failures or warnings", "First warning", "Second warning", "Ignore this")]
        public void TwoWarningsAndIgnore()
        {
            Assert.Warn("First warning");
            Assert.Warn("Second warning");
            Assert.Ignore("Ignore this");
        }

        [TestCase, ExpectWarning("Multiple failures or warnings", "First warning", "Second warning", "This is inconclusive")]
        public void TwoWarningsAndInconclusive()
        {
            Assert.Warn("First warning");
            Assert.Warn("Second warning");
            Assert.Inconclusive("This is inconclusive");
        }
#endif
        #endregion

#if NYI // Warnings in SetUp or TearDown
        [TestCase(typeof(WarningInSetUpPasses), nameof(WarningInSetUpPasses.WarningPassesInTest), 2, 0)]
        [TestCase(typeof(WarningInSetUpPasses), nameof(WarningInSetUpPasses.WarningFailsInTest), 2, 1)]
        [TestCase(typeof(WarningInSetUpPasses), nameof(WarningInSetUpPasses.ThreeWarningsFailInTest), 4, 3)]
        [TestCase(typeof(WarningInSetUpFails), nameof(WarningInSetUpFails.WarningPassesInTest), 2, 1)]
        [TestCase(typeof(WarningInSetUpFails), nameof(WarningInSetUpFails.WarningFailsInTest), 2, 2)]
        [TestCase(typeof(WarningInSetUpFails), nameof(WarningInSetUpFails.ThreeWarningsFailInTest), 4, 4)]
        [TestCase(typeof(WarningInTearDownPasses), nameof(WarningInTearDownPasses.WarningPassesInTest), 2, 0)]
        [TestCase(typeof(WarningInTearDownPasses), nameof(WarningInTearDownPasses.WarningFailsInTest), 2, 1)]
        [TestCase(typeof(WarningInTearDownPasses), nameof(WarningInTearDownPasses.ThreeWarningsFailInTest), 4, 3)]
        [TestCase(typeof(WarningInTearDownFails), nameof(WarningInTearDownFails.WarningPassesInTest), 2, 1)]
        [TestCase(typeof(WarningInTearDownFails), nameof(WarningInTearDownFails.WarningFailsInTest), 2, 2)]
        [TestCase(typeof(WarningInTearDownFails), nameof(WarningInTearDownFails.ThreeWarningsFailInTest), 4, 4)]
        public void WarningUsedInSetUpOrTearDown(Type fixtureType, string methodName, int expectedAsserts, int expectedWarnings)
        {
            var result = TestBuilder.RunTestCase(fixtureType, methodName);

            Assert.That(result.ResultState, Is.EqualTo(expectedWarnings == 0 ? ResultState.Success : ResultState.Warning));
            Assert.That(result.AssertCount, Is.EqualTo(expectedAsserts), "Incorrect AssertCount");
            Assert.That(result.AssertionResults.Count, Is.EqualTo(expectedWarnings), $"There should be {expectedWarnings} AssertionResults");
        }
#endif

        [TestCase]
        public void PassingAssertion_DoesNotCallExceptionStringFunc()
        {
            // Arrange
            var funcWasCalled = false;
            Func<string> getExceptionMessage = () =>
            {
                funcWasCalled = true;
                return "Func was called";
            };

            // Act
            using (new TestExecutionContext.IsolatedContext())
                Warn.Unless(0 + 1 == 1, getExceptionMessage);

            // Assert
            Assert.That(!funcWasCalled, "The getExceptionMessage function was called when it should not have been.");
        }

        [TestCase]
        public void FailingWarning_CallsExceptionStringFunc()
        {
            // Arrange
            var funcWasCalled = false;
            Func<string> getExceptionMessage = () =>
            {
                funcWasCalled = true;
                return "Func was called";
            };

            // Act
            using (new TestExecutionContext.IsolatedContext())
                Warn.Unless(1 + 1 == 1, getExceptionMessage);

            // Assert
            Assert.That(funcWasCalled, "The getExceptionMessage function was not called when it should have been.");
        }

#if TASK_PARALLEL_LIBRARY_API
        [TestCase]
        public void WarnUnless_Async_Error()
        {
#if !NET40
            var exception =
#endif
                Assert.Throws<InvalidOperationException>(() =>
                    Warn.Unless(async () => await ThrowExceptionGenericTask(), Is.EqualTo(1)));

#if !NET40
            Assert.That(exception.StackTrace, Does.Contain("ThrowExceptionGenericTask"));
#endif
        }

        [TestCase]
        public void WarnIf_Async_Error()
        {
#if !NET40
            var exception =
#endif
                Assert.Throws<InvalidOperationException>(() =>
                    Warn.If(async () => await ThrowExceptionGenericTask(), Is.Not.EqualTo(1)));

#if !NET40
            Assert.That(exception.StackTrace, Does.Contain("ThrowExceptionGenericTask"));
#endif
        }

        private static Task<int> One()
        {
            return Task.Run(() => 1);
        }

        private static async Task<int> ThrowExceptionGenericTask()
        {
            await One();
            throw new InvalidOperationException();
        }
#endif

#if NYI
        // We decided to trim ExecutionContext and below because ten lines per warning adds up
        // and makes it hard to read build logs.
        // See https://github.com/nunit/nunit/pull/2431#issuecomment-328404432.
        [TestCase(nameof(WarningFixture.WarningSynchronous), 1)]
        [TestCase(nameof(WarningFixture.WarningInThreadStart), 2)]
        [TestCase(nameof(WarningFixture.WarningInBeginInvoke), 5, ExcludePlatform = "mono", Reason = "Warning has no effect inside BeginInvoke on Mono")]
        [TestCase(nameof(WarningFixture.WarningInThreadPoolQueueUserWorkItem), 2)]
#if TASK_PARALLEL_LIBRARY_API
        [TestCase(nameof(WarningFixture.WarningInTaskRun), 4)]
        [TestCase(nameof(WarningFixture.WarningAfterAwaitTaskDelay), 5)]
#endif
        public static void StackTracesAreFiltered(string methodName, int maxLineCount)
        {
            var result = TestBuilder.RunTestCase(typeof(WarningFixture), methodName);
            if (result.FailCount != 0 && result.Message.StartsWith(typeof(PlatformNotSupportedException).FullName))
            {
                return; // BeginInvoke causes PlatformNotSupportedException on .NET Core
            }

            if (result.AssertionResults.Count != 1 || result.AssertionResults[0].Status != AssertionStatus.Warning)
            {
                Assert.Fail("Expected a single warning assertion. Message: " + result.Message);
            }

            var warningStackTrace = result.AssertionResults[0].StackTrace;
            var lines = warningStackTrace.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (maxLineCount < lines.Length)
            {
                Assert.Fail(
                    $"Expected the number of lines to be no more than {maxLineCount}, but it was {lines.Length}:" + Environment.NewLine
                    + Environment.NewLine
                    + string.Concat(lines.Select((line, i) => $" {i + 1}. {line.Trim()}" + Environment.NewLine).ToArray())
                    + "(end)");

                 // ^ Most of that is to differentiate it from the current method's stack trace
                 // reported directly underneath at the same level of indentation.
            }
        }
#endif // WIP

        #region Helper Methods

        private int ReturnsFour()
        {
            return 4;
        }

        private int ReturnsFive()
        {
            return 5;
        }

        #endregion
    }
}
