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

        [Test]
        public void TestEquality_StatusSpecified()
        {
            Assert.AreEqual(new ResultState(TestStatus.Failed), new ResultState(TestStatus.Failed));
        }

        [Test]
        public void TestEquality_StatusAndLabelSpecified()
        {
            Assert.AreEqual(new ResultState(TestStatus.Skipped, "Ignored"), new ResultState(TestStatus.Skipped, "Ignored"));
        }

        [Test]
        public void TestEquality_StatusDiffers()
        {
            Assert.AreNotEqual(new ResultState(TestStatus.Passed), new ResultState(TestStatus.Failed));
        }

        [Test]
        public void TestEquality_LabelDiffers()
        {
            Assert.AreNotEqual(new ResultState(TestStatus.Failed, "Error"), new ResultState(TestStatus.Failed));
        }

        [Test]
        public void TestEquality_WrongType()
        {
            var rs = new ResultState(TestStatus.Passed);
            var s = "123";

            Assert.AreNotEqual(rs, s);
            Assert.AreNotEqual(s, rs);
            Assert.IsFalse(rs.Equals(s));
        }

        [Test]
        public void TestEquality_Null()
        {
            var rs = new ResultState(TestStatus.Passed);
            Assert.AreNotEqual(null, rs);
            Assert.AreNotEqual(rs, null);
            Assert.IsFalse(rs.Equals(null));
        }

        #endregion

        #region Test Static Fields with standard ResultStates

        [Test]
        public void Inconclusive_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Inconclusive;

            Assert.AreEqual(TestStatus.Inconclusive, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [Test]
        public void NotRunnable_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.NotRunnable;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual("Invalid", resultState.Label, "Label not correct.");
        }

        [Test]
        public void Skipped_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Skipped;

            Assert.AreEqual(TestStatus.Skipped, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [Test]
        public void Ignored_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Ignored;

            Assert.AreEqual(TestStatus.Skipped, resultState.Status, "Status not correct.");
            Assert.AreEqual("Ignored", resultState.Label, "Label not correct.");
        }

        [Test]
        public void Success_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Success;

            Assert.AreEqual(TestStatus.Passed, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [Test]
        public void Warning_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Warning;

            Assert.AreEqual(TestStatus.Warning, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [Test]
        public void Failure_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Failure;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [Test]
        public void Error_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Error;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual("Error", resultState.Label, "Label not correct.");
        }

        [Test]
        public void Cancelled_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Cancelled;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual("Cancelled", resultState.Label, "Label not correct.");
        }

        #endregion
    }
}