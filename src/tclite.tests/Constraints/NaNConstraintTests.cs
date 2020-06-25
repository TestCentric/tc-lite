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
    public class NaNConstraintTests
    {
        [Test]
        public void ProvidesProperDescription()
        {
            Assert.That(new NaNConstraint().Description, Is.EqualTo("NaN"));
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.That(new NaNConstraint().ToString(), Is.EqualTo("<nan>"));
        }

        [TestCase(double.NaN)]
        [TestCase(float.NaN)]
        public void SucceedsWithGoodValues(double value)
        {
            Assert.IsNaN(value);
            Assert.That(value, Is.NaN);
        }

        //[TestCase( null, "null" )]
        //[TestCase( "hello", "\"hello\"" )]
        [TestCase( 42, "42.0d" )]
        [TestCase( 42.0, "42.0d")]
        [TestCase( double.PositiveInfinity, "Infinity" )]
        [TestCase( double.NegativeInfinity, "-Infinity" )]
        [TestCase( float.PositiveInfinity, "Infinity" )]
        [TestCase( float.NegativeInfinity, "-Infinity" )]
        public void FailsWithBadValues(double badValue, string message)
        {
            var ex = Assert.Throws<AssertionException>(() =>
                Assert.IsNaN(badValue));

            Assert.That(ex.Message, Is.EqualTo(
                $"  Expected: NaN\n  But was:  {message}\n"));
        }

        [TestCase("InvalidData")]
        public void InvalidDataThrowsArgumentException(object value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new NaNConstraint().ApplyTo(value);
            });
        }
    }
}
