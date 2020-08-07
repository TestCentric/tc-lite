// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Internal;

namespace TCLite.Constraints
{
    [TestFixture]
    public class NoItemConstraintTests
    {
        private readonly string NL = Environment.NewLine;

        [TestCase]
        public void NoItemsAreNotNull()
        {
            object[] c = new object[] { 1, "hello", 3, Environment.NewLine };
            Assert.That(c, new NoItemConstraint(Is.Null));
        }

        [TestCase]
        public void NoItemsAreNotNullFails()
        {
            object[] c = new object[] { 1, "hello", null, 3 };
            var expectedMessage =
                TextMessageWriter.Pfx_Expected + "no item null" + NL +
                TextMessageWriter.Pfx_Actual + "< 1, \"hello\", null, 3 >" + NL;
                //+ "  First non-matching item at index [2]:  null" + NL;
            var ex = Assert.Throws<AssertionException>(() =>
                Assert.That(c, new NoItemConstraint(Is.Null)));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

#if NYI // Message
        [TestCase]
        public void FailsWhenNotUsedAgainstAnEnumerable()
        {
            var notEnumerable = 42;
            TestDelegate act = () => Assert.That(notEnumerable, new NoItemConstraint(Is.Null));
            Assert.That(act, Throws.ArgumentException.With.Message.Contains("IEnumerable"));
        }
#endif
    }
}
