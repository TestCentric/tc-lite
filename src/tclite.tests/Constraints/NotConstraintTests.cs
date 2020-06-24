// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class NotConstraintTests
    {
        private static readonly string NL = Environment.NewLine;
        private const string DESCRIPTION = "not null"; // TODO: Should be "not equal to null"
        private const string STRING_REPRESENTATION = "<not <equal null>>";

        private NotConstraint _constraint = new NotConstraint( new EqualConstraint<object>(null) );

        private static object[] SuccessData = { 42, "Hello" };

        [Test]
        public void DescriptionTest()
        {
            Assert.AreEqual(DESCRIPTION, _constraint.Description);
        }

        [Test]
        public void ToStringTest()
        {
            Assert.AreEqual(STRING_REPRESENTATION, _constraint.ToString());
        }

        [TestCase(42)]
        [TestCase("Hello")]
        public void ApplyConstraintSucceeds<T>(T actual)
        {
            Assert.That(_constraint.ApplyTo(actual).IsSuccess);
        }

        [TestCase(null, "null")]
        public void ApplyConstraintFails(object actual, string message)
        {
            var result = _constraint.ApplyTo(actual);
            Assert.IsFalse(result.IsSuccess);
        
            TextMessageWriter writer = new TextMessageWriter();
            result.WriteMessageTo(writer);
            Assert.That( writer.ToString(), Is.EqualTo(
                TextMessageWriter.Pfx_Expected + DESCRIPTION + NL +
                TextMessageWriter.Pfx_Actual + message + NL ));
        }

        [Test]
        public void NotHonorsIgnoreCaseUsingConstructors()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That("abc", new NotConstraint(new EqualConstraint<string>("ABC").IgnoreCase));
            });

            Assert.That(ex.Message.Contains("ignoring case"), $"Message was {ex.Message}");
        }

        [Test]
        public void NotHonorsIgnoreCaseUsingPrefixNotation()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That( "abc", Is.Not.EqualTo( "ABC" ).IgnoreCase );
            });

            Assert.That(ex.Message.Contains("ignoring case"));
        }

        [Test]
        public void NotHonorsTolerance()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That( 4.99d, Is.Not.EqualTo( 5.0d ).Within( .05d ) );
            });

            Assert.That(ex.Message.Contains("+/-"));
        }

        // TODO: Move operator overrides to a separate test class
        [Test]
        public void CanUseNotOperator()
        {
            Assert.That(42, !new EqualConstraint<int>(99));
        }
    }
}