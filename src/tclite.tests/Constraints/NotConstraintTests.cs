// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using TCLite.Internal;

namespace TCLite.Constraints
{
    [TestFixture]
    public class NotConstraintTests : ConstraintTestBase<object>
    {
        protected override Constraint Constraint => new NotConstraint(new EqualConstraint<int>(42));
        protected override string ExpectedDescription => "not equal to 42";
        protected override string ExpectedRepresentation => "<not <equal 42>>";
        private static readonly string NL = Environment.NewLine;

        static object[] SuccessData = new object[] { 99, null, "Hello" };

        static TestCaseData[] FailureData = new TestCaseData[]
        {
            new TestCaseData(42, "42")
        };

        [TestCase]
        public void NotHonorsIgnoreCaseUsingConstructors()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That("abc", Is.Not.EqualTo("ABC").IgnoreCase);
            });

            Assert.That(ex.Message.Contains("ignoring case"), $"Message was {ex.Message}");
        }

        [TestCase]
        public void NotHonorsIgnoreCaseUsingPrefixNotation()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That( "abc", Is.Not.EqualTo( "ABC" ).IgnoreCase );
            });

            Assert.That(ex.Message.Contains("ignoring case"));
        }

        [TestCase]
        public void NotHonorsTolerance()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That( 4.99d, Is.Not.EqualTo( 5.0d ).Within( .05d ) );
            });

            Assert.That(ex.Message.Contains("+/-"));
        }
    }
}