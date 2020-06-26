// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class NotConstraintTests : ConstraintTestBase<object>
    {
        protected override Constraint Constraint => new NotConstraint(new EqualConstraint(null));
        protected override string ExpectedDescription => "not null";
        protected override string ExpectedRepresentation => "<not <equal null>>";
        private static readonly string NL = Environment.NewLine;

        protected override object[] SuccessData => new object[] { 42, "Hello" };

        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(null, "null")
        };

        [Test]
        public void NotHonorsIgnoreCaseUsingConstructors()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That("abc", new NotConstraint(new EqualConstraint("ABC").IgnoreCase));
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
            Assert.That(42, !new EqualConstraint(99));
        }
    }
}