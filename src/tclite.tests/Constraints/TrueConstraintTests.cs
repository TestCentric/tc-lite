// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class TrueConstraintTests
    {
        [Test]
        public void ProvidesProperDescription()
        {
            Assert.That(new TrueConstraint().Description, Is.EqualTo("True"));
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.That(new TrueConstraint().ToString(), Is.EqualTo("<true>"));
        }

        [TestCase(true)]
        [TestCase(2 + 2 == 4)]
        public void SucceedsWithGoodValues(bool goodValue)
        {
            Assert.IsTrue(goodValue);
            Assert.That(goodValue, Is.True);
        }

        [TestCase(false)]
        [TestCase(2 + 2 == 5)]
        public void FailsWithBadValues(bool badValue)
        {
            var ex = Assert.Throws<AssertionException>(() =>
                Assert.IsTrue(badValue));

            Assert.That(ex.Message, Is.EqualTo(
                $"  Expected: True\n  But was:  False\n"));
        }

        static object[] FailureData = new object[] { 
            //new object[] { null, "null" }, new object[] { "hello", "\"hello\"" },
            new object[] { false, "False"}, new object[] { 2+2==5, "False" } };
    }
}
