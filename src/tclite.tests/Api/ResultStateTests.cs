// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using NUnit.Framework;

namespace TCLite.Framework.Api
{
    [NUnit.Framework.TestFixture]
    public class ResultStateTests
    {
        [NUnit.Framework.Test]
        public void ConstructWithOneArgument([Values]TestStatus status)
        {
            ResultState resultState = new ResultState(status);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(string.Empty, resultState.Label);
        }

        [NUnit.Framework.TestCase(TestStatus.Failed, "Error")]
        [NUnit.Framework.TestCase(TestStatus.Failed, "Invalid")]
        [NUnit.Framework.TestCase(TestStatus.Skipped, "Ignored")]
        [NUnit.Framework.TestCase(TestStatus.Skipped, "Explicit")]
        public void ConstructWithTwoArguments(TestStatus status, string label)
        {
            ResultState resultState = new ResultState(status, label);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(label, resultState.Label);
        }

        [NUnit.Framework.Test]
        public void ConstructorWithNullLabel([Values]TestStatus status)
        {
            ResultState resultState = new ResultState(status, null);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(string.Empty, resultState.Label);
        }

        [NUnit.Framework.Test]
        public void ConstructWithEmptyLabel([Values]TestStatus status)
        {
            ResultState resultState = new ResultState(status, string.Empty);

            Assert.AreEqual(status, resultState.Status);
            Assert.AreEqual(string.Empty, resultState.Label);
        }

        [NUnit.Framework.TestCase(TestStatus.Skipped, null, "Skipped")]
        [NUnit.Framework.TestCase(TestStatus.Passed, "", "Passed")]
        [NUnit.Framework.TestCase(TestStatus.Failed, "Error", "Failed:Error")]
        [NUnit.Framework.TestCase(TestStatus.Skipped, "Ignored", "Skipped:Ignored")]
        public void ToStringTest(TestStatus status, string label, string expected)
        {
            ResultState resultState = new ResultState(status, label);

            Assert.AreEqual(expected, resultState.ToString());
        }

        #region EqualityTests

        [NUnit.Framework.Test]
        public void TestEquality_StatusSpecified()
        {
            Assert.AreEqual(new ResultState(TestStatus.Failed), new ResultState(TestStatus.Failed));
        }

        [NUnit.Framework.Test]
        public void TestEquality_StatusAndLabelSpecified()
        {
            Assert.AreEqual(new ResultState(TestStatus.Skipped, "Ignored"), new ResultState(TestStatus.Skipped, "Ignored"));
        }

        [NUnit.Framework.Test]
        public void TestEquality_StatusDiffers()
        {
            Assert.AreNotEqual(new ResultState(TestStatus.Passed), new ResultState(TestStatus.Failed));
        }

        [NUnit.Framework.Test]
        public void TestEquality_LabelDiffers()
        {
            Assert.AreNotEqual(new ResultState(TestStatus.Failed, "Error"), new ResultState(TestStatus.Failed));
        }

        [NUnit.Framework.Test]
        public void TestEquality_WrongType()
        {
            var rs = new ResultState(TestStatus.Passed);
            var s = "123";

            Assert.AreNotEqual(rs, s);
            Assert.AreNotEqual(s, rs);
            Assert.False(rs.Equals(s));
        }

        [NUnit.Framework.Test]
        public void TestEquality_Null()
        {
            var rs = new ResultState(TestStatus.Passed);
            Assert.AreNotEqual(null, rs);
            Assert.AreNotEqual(rs, null);
            Assert.False(rs.Equals(null));
        }

        #endregion

        #region Test Static Fields with standard ResultStates

        [NUnit.Framework.Test]
        public void Inconclusive_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Inconclusive;

            Assert.AreEqual(TestStatus.Inconclusive, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void NotRunnable_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.NotRunnable;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual("Invalid", resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void Skipped_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Skipped;

            Assert.AreEqual(TestStatus.Skipped, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void Ignored_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Ignored;

            Assert.AreEqual(TestStatus.Skipped, resultState.Status, "Status not correct.");
            Assert.AreEqual("Ignored", resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void Success_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Success;

            Assert.AreEqual(TestStatus.Passed, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void Warning_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Warning;

            Assert.AreEqual(TestStatus.Warning, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void Failure_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Failure;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual(string.Empty, resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void Error_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Error;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual("Error", resultState.Label, "Label not correct.");
        }

        [NUnit.Framework.Test]
        public void Cancelled_ReturnsResultStateWithPropertiesCorrectlySet()
        {
            ResultState resultState = ResultState.Cancelled;

            Assert.AreEqual(TestStatus.Failed, resultState.Status, "Status not correct.");
            Assert.AreEqual("Cancelled", resultState.Label, "Label not correct.");
        }

        #endregion
    }
}