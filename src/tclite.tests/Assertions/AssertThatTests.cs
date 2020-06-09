// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Threading.Tasks;
using TCLite.Framework.Constraints;
using TCLite.Framework.Internal;
// using TCLite.TestData;
// using TCLite.TestUtilities;

namespace TCLite.Framework.Assertions
{
    [TestFixture]
    public class AssertThatTests : AssertionTestBase
    {
        [Test]
        public void AssertionPasses_Boolean()
        {
            Assert.That(2 + 2 == 4);
        }

        [Test]
        public void AssertionPasses_BooleanWithMessage()
        {
            Assert.That(2 + 2 == 4, "Not Equal");
        }

        [Test]
        public void AssertionPasses_BooleanWithMessageAndArgs()
        {
            Assert.That(2 + 2 == 4, "Not Equal to {0}", 4);
        }

        [Test]
        public void AssertionPasses_ActualAndConstraint()
        {
            Assert.That(2 + 2, Is.EqualTo(4));
        }

        [Test]
        public void AssertionPasses_ActualAndConstraintWithMessage()
        {
            Assert.That(2 + 2, Is.EqualTo(4), "Should be 4");
        }

        [Test]
        public void AssertionPasses_ActualAndConstraintWithMessageAndArgs()
        {
            Assert.That(2 + 2, Is.EqualTo(4), "Should be {0}", 4);
        }

        [Test]
        public void AssertionPasses_ReferenceAndConstraint()
        {
            bool value = true;
            Assert.That(ref value, Is.True);
        }

        [Test]
        public void AssertionPasses_ReferenceAndConstraintWithMessage()
        {
            bool value = true;
            Assert.That(ref value, Is.True, "Message");
        }

        [Test]
        public void AssertionPasses_ReferenceAndConstraintWithMessageAndArgs()
        {
            bool value = true;
            Assert.That(ref value, Is.True, "Message", 42);
        }

        [Test]
        public void AssertionPasses_DelegateAndConstraint()
        {
            Assert.That(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4));
        }

        private int ReturnsFour()
        {
            return 4;
        }

        [Test]
        public void FailureThrowsAssertionException_Boolean()
        {
            ThrowsAssertionException(() => Assert.That(2 + 2 == 5));
        }

        [Test]
        public void FailureThrowsAssertionException_BooleanWithMessage()
        {
            ThrowsAssertionException(() => Assert.That(2 + 2 == 5, "message"),
                "  message" + NL +
                "  Expected: True"  + NL +
                "  But was:  False" + NL);
        }

        [Test]
        public void FailureThrowsAssertionException_ActualAndConstraint()
        {
            ThrowsAssertionException(() => Assert.That(2 + 2, Is.EqualTo(5)));
        }

        [Test,]
        public void FailureThrowsAssertionException_ActualAndConstraintWithMessage()
        {
            ThrowsAssertionException(() => Assert.That(2 + 2, Is.EqualTo(5), "Error"),
                "  Error" + NL +
                "  Expected: 5" + NL +
                "  But was:  4" + NL);
        }

        [Test]
        public void FailureThrowsAssertionException_ReferenceAndConstraint()
        {
            bool value = false;
            ThrowsAssertionException(() => Assert.That(ref value, Is.True));
        }

        [Test]
        public void FailureThrowsAssertionException_ReferenceAndConstraintWithMessage()
        {
            bool value = false;
            ThrowsAssertionException(() => Assert.That(ref value, Is.True, "message"),
                "  message" + NL +
                "  Expected: True" + NL +
                "  But was:  False" + NL);
        }

        [Test]
        public void FailureThrowsAssertionException_DelegateAndConstraint()
        {
            ThrowsAssertionException(() => Assert.That(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4)));
        }

        [Test]
        public void FailureThrowsAssertionException_DelegateAndConstraintWithMessage()
        {
            ThrowsAssertionException(() => Assert.That(new ActualValueDelegate<int>(ReturnsFive), Is.EqualTo(4), "Error"),
                "  Error" + NL +
                "  Expected: 4" + NL +
                "  But was:  5" + NL);
        }

#if NYI
        [Test]
        public void AssertionsAreCountedCorrectly()
        {
            TestResult result = TestBuilder.RunTestFixture(typeof(AssertCountFixture));

            int totalCount = 0;
            foreach (TestResult childResult in result.Children)
            {
                int expectedCount = childResult.Name == "ThreeAsserts" ? 3 : 1;
                Assert.That(childResult.AssertCount, Is.EqualTo(expectedCount), "Bad count for {0}", childResult.Name);
                totalCount += expectedCount;
            }

            Assert.That(result.AssertCount, Is.EqualTo(totalCount), "Fixture count is not correct");
        }
#endif

        private int ReturnsFive()
        {
            return 5;
        }

#if NYI
        [Test]
        public void AssertThatSuccess()
        {
            Assert.That(async () => await One(), Is.EqualTo(1));
        }

        [Test]
        public void AssertThatFailure()
        {
            var exception = Assert.Throws<AssertionException>(() =>
                Assert.That(async () => await One(), Is.EqualTo(2)));
        }

        [Test]
        public void AssertThatErrorTask()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Assert.That(async () => await ThrowExceptionTask(), Is.EqualTo(1)));

            Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionTask"));
        }

        [Test]
        public void AssertThatErrorGenericTask()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Assert.That(async () => await ThrowExceptionGenericTask(), Is.EqualTo(1)));

            Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionGenericTask"));
        }

        [Test]
        public void AssertThatErrorVoid()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Assert.That(async () => { await ThrowExceptionGenericTask(); }, Is.EqualTo(1)));

            Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionGenericTask"));
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

        private static async Task ThrowExceptionTask()
        {
            await One();
            throw new InvalidOperationException();
        }
#endif
    }
}
