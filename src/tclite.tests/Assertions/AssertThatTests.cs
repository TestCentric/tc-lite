// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.IO;
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
        [TestCase]
        public void AssertionPasses_Boolean()
        {
            Assert.That(2 + 2 == 4);
        }

        [TestCase]
        public void AssertionPasses_BooleanWithMessage()
        {
            Assert.That(2 + 2 == 4, "Not Equal");
        }

        [TestCase]
        public void AssertionPasses_BooleanWithMessageAndArgs()
        {
            Assert.That(2 + 2 == 4, "Not Equal to {0}", 4);
        }

        [TestCase]
        public void AssertionPasses_ActualAndConstraint()
        {
            Assert.That(2 + 2, Is.EqualTo(4));
        }

        [TestCase]
        public void AssertionPasses_ActualAndConstraintWithMessage()
        {
            Assert.That(2 + 2, Is.EqualTo(4), "Should be 4");
        }

        [TestCase]
        public void AssertionPasses_ActualAndConstraintWithMessageAndArgs()
        {
            Assert.That(2 + 2, Is.EqualTo(4), "Should be {0}", 4);
        }

        [TestCase]
        public void AssertionPasses_ReferenceAndConstraint()
        {
            bool value = true;
            Assert.That(ref value, Is.True);
        }

        [TestCase]
        public void AssertionPasses_ReferenceAndConstraintWithMessage()
        {
            bool value = true;
            Assert.That(ref value, Is.True, "Message");
        }

        [TestCase]
        public void AssertionPasses_ReferenceAndConstraintWithMessageAndArgs()
        {
            bool value = true;
            Assert.That(ref value, Is.True, "Message", 42);
        }

        [TestCase]
        public void AssertionPasses_DelegateAndConstraint()
        {
            Assert.That(new ActualValueDelegate<int>(ReturnsFour), Is.EqualTo(4));
        }

        private int ReturnsFour()
        {
            return 4;
        }

        [TestCase]
        public void FailureThrowsAssertionException_Boolean()
        {
            Assert.Throws<AssertionException>(() => Assert.That(2 + 2 == 5));
        }

        [TestCase]
        public void FailureThrowsAssertionException_BooleanWithMessage()
        {
            var ex = Assert.Throws<AssertionException>(() => Assert.That(2 + 2 == 5, "MESSAGE"));
            Assert.That(ex.Message, Is.EqualTo(
                "  MESSAGE" + NL +
                "  Expected: True"  + NL +
                "  But was:  False" + NL));
        }

        [TestCase]
        public void FailureThrowsAssertionException_ActualAndConstraint()
        {
            var ex = Assert.Throws<AssertionException>(() => Assert.That(2 + 2, Is.EqualTo(5)));
        }

        [TestCase]
        public void FailureThrowsAssertionException_ActualAndConstraintWithMessage()
        {
            var ex = Assert.Throws<AssertionException>(() => Assert.That(2 + 2, Is.EqualTo(5), "Error"));
            Assert.That(ex.Message, Is.EqualTo(
                "  Error" + NL +
                "  Expected: 5" + NL +
                "  But was:  4" + NL));
        }

        [TestCase]
        public void FailureThrowsAssertionException_ReferenceAndConstraint()
        {
            bool value = false;
            Assert.Throws<AssertionException>(() => Assert.That(ref value, Is.True));
        }

        [TestCase]
        public void FailureThrowsAssertionException_ReferenceAndConstraintWithMessage()
        {
            bool value = false;
            var ex = Assert.Throws<AssertionException>(() => Assert.That(ref value, Is.True, "message"));
            Assert.That(ex.Message, Is.EqualTo(
                "  message" + NL +
                "  Expected: True" + NL +
                "  But was:  False" + NL));
        }

        [TestCase]
        public void FailureThrowsAssertionException_DelegateAndConstraint()
        {
            var ex = Assert.Throws<AssertionException>(() => Assert.That(ReturnsFive, Is.EqualTo(4)));
        }

        [TestCase]
        public void FailureThrowsAssertionException_DelegateAndConstraintWithMessage()
        {
            var ex = Assert.Throws<AssertionException>(() => Assert.That(ReturnsFive, Is.EqualTo(4), "Error"));
            Assert.That(ex.Message, Is.EqualTo(
                "  Error" + NL +
                "  Expected: 4" + NL +
                "  But was:  5" + NL));
        }

#if NYI // TestBuilder
        [TestCase]
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

        [TestCase]
        public void AreEqualPassesWithSameStream()
        {
            Stream exampleStream = new MemoryStream(new byte[] { 1, 2, 3 });
            Assert.That(exampleStream, Is.EqualTo(exampleStream));
        }

        [TestCase]
        public void AreEqualPassesWithEqualStreams()
        {
            Stream stream1 = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            Stream stream2 = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            Assert.That(stream1, Is.EqualTo(stream2));
        }

        [TestCase]
        public void NonReadableStreamGivesException()
        {
            Stream exampleStream = new MemoryStream(new byte[] { 1, 2, 3 });
            Assert.Throws<InvalidOperationException>(() =>
                Assert.That(exampleStream, Is.EqualTo(new NonReadableStream())));
        }

        [TestCase]
        public void NonSeekableStreamGivesException()
        {
            Stream exampleStream = new MemoryStream(new byte[] { 1, 2, 3 });
            Assert.Throws<InvalidOperationException>(() =>
                Assert.That(exampleStream, Is.EqualTo(new NonSeekableStream())));
        }

        private class NonReadableStream : MemoryStream
        {
            public override bool CanRead
            {
                get { return false; }
            }
        }

        private class NonSeekableStream : MemoryStream
        {
            public override bool CanSeek
            {
                get { return false; }
            }
        }

        private int ReturnsFive()
        {
            return 5;
        }

#if NYI // async
        [TestCase]
        public void AssertThatSuccess()
        {
            Assert.That(async () => await One(), Is.EqualTo(1));
        }

        [TestCase]
        public void AssertThatFailure()
        {
            var exception = Assert.Throws<AssertionException>(() =>
                Assert.That(async () => await One(), Is.EqualTo(2)));
        }

        [TestCase]
        public void AssertThatErrorTask()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Assert.That(async () => await ThrowExceptionTask(), Is.EqualTo(1)));

            Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionTask"));
        }

        [TestCase]
        public void AssertThatErrorGenericTask()
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                Assert.That(async () => await ThrowExceptionGenericTask(), Is.EqualTo(1)));

            Assert.That(exception.StackTrace, Contains.Substring("ThrowExceptionGenericTask"));
        }

        [TestCase]
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
