// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Api
{
    [TestFixture]
    public class ResultStateTests
    {
        [TestCase(TestStatus.Passed)]
        [TestCase(TestStatus.Failed)]
        [TestCase(TestStatus.Warning)]
        [TestCase(TestStatus.Skipped)]
        [TestCase(TestStatus.Inconclusive)]
        public void ConstructWithOneArgument(TestStatus status)
        {
            ResultState resultState = new ResultState(status);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(string.Empty, resultState.Label);
        }

        [TestCase(TestStatus.Failed, "Error")]
        [TestCase(TestStatus.Failed, "Invalid")]
        [TestCase(TestStatus.Skipped, "Ignored")]
        [TestCase(TestStatus.Skipped, "Explicit")]
        public void ConstructWithTwoArguments(TestStatus status, string label)
        {
            ResultState resultState = new ResultState(status, label);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(label, resultState.Label);
        }

        [TestCase(TestStatus.Passed)]
        [TestCase(TestStatus.Failed)]
        [TestCase(TestStatus.Warning)]
        [TestCase(TestStatus.Skipped)]
        [TestCase(TestStatus.Inconclusive)]
        public void ConstructorWithNullLabel(TestStatus status)
        {
            ResultState resultState = new ResultState(status, null);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(string.Empty, resultState.Label);
        }

        [TestCase(TestStatus.Passed)]
        [TestCase(TestStatus.Failed)]
        [TestCase(TestStatus.Warning)]
        [TestCase(TestStatus.Skipped)]
        [TestCase(TestStatus.Inconclusive)]
        public void ConstructWithEmptyLabel(TestStatus status)
        {
            ResultState resultState = new ResultState(status, string.Empty);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(string.Empty, resultState.Label);
        }

        [TestCase(TestStatus.Skipped, null, "Skipped")]
        [TestCase(TestStatus.Passed, "", "Passed")]
        [TestCase(TestStatus.Failed, "Error", "Failed:Error")]
        [TestCase(TestStatus.Skipped, "Ignored", "Skipped:Ignored")]
        public void ToStringTest(TestStatus status, string label, string expected)
        {
            ResultState resultState = new ResultState(status, label);

            Assert.AreEqual(expected, resultState.ToString());
        }

        #region EqualityTests

        private static TestCaseData[] SuccessData = new TestCaseData[]
        {
            new TestCaseData(new ResultState(TestStatus.Failed), new ResultState(TestStatus.Failed)),
            new TestCaseData(new ResultState(TestStatus.Skipped, "Ignored"), new ResultState(TestStatus.Skipped, "Ignored")),
            new TestCaseData(new ResultState(TestStatus.Failed), new ResultState(TestStatus.Failed)),
        };

        [TestCaseSource(nameof(SuccessData))]
        public void TestEquality(ResultState expected, ResultState actual)
        {
            Assert.AreEqual(expected, actual);
        }

        private static TestCaseData[] FailureData = new TestCaseData[]
        {
            new TestCaseData(new ResultState(TestStatus.Passed), new ResultState(TestStatus.Failed)),
            new TestCaseData(new ResultState(TestStatus.Failed, "Error"), new ResultState(TestStatus.Failed)),
            new TestCaseData(null, new ResultState(TestStatus.Passed)),
            new TestCaseData(new ResultState(TestStatus.Passed), null)
        };

        [TestCaseSource(nameof(FailureData))]
        public void TestInequality(ResultState expected, ResultState actual)
        {
            Assert.AreNotEqual(expected, actual);
        }

        #endregion

        #region Test Static Fields with standard ResultStates

        private static TestCaseData[] StandardResults = new TestCaseData[]
        {
            new TestCaseData(ResultState.Inconclusive, TestStatus.Inconclusive, string.Empty),
            new TestCaseData(ResultState.NotRunnable, TestStatus.Failed, "Invalid"),
            new TestCaseData(ResultState.Skipped, TestStatus.Skipped, string.Empty),
            new TestCaseData(ResultState.Ignored, TestStatus.Skipped, "Ignored"),
            new TestCaseData(ResultState.Success, TestStatus.Passed, string.Empty),
            new TestCaseData(ResultState.Warning, TestStatus.Warning, string.Empty),
            new TestCaseData(ResultState.Failure, TestStatus.Failed, string.Empty),
            new TestCaseData(ResultState.Error, TestStatus.Failed, "Error"),
            new TestCaseData(ResultState.Cancelled, TestStatus.Failed, "Cancelled"),
        };

        [TestCaseSource(nameof(StandardResults))]
        public void StandardResultTests(ResultState resultState, TestStatus status, string label)
        {
            Assert.AreEqual(status, resultState.Status, "Status not correct.");
            Assert.AreEqual(label, resultState.Label, "Label not correct.");
        }

        #endregion
    }
}