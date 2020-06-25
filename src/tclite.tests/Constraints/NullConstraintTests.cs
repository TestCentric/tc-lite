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
    public class NullConstraintTests
    {
        [Test]
        public void ProvidesProperDescription()
        {
            Assert.That(new NullConstraint().Description, Is.EqualTo("null"));
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.That(new NullConstraint().ToString(), Is.EqualTo("<null>"));
        }

        [TestCase(null)]
        [TestCase((string)null)]
        public void SucceedsWithGoodValues(object value)
        {
            Assert.IsNull(value);
            Assert.That(value, Is.Null);
        }

        [TestCase("hello", "\"hello\"")]
        public void FailsWithBadValues(object badValue, string message)
        {
            var ex = Assert.Throws<AssertionException>(() =>
                Assert.IsNull(badValue));

            Assert.That(ex.Message, Is.EqualTo(
                $"  Expected: null\n  But was:  {message}\n"));
        }
    }
}
