// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using TCLite.Framework.Constraints;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class NotConstraintTests : ConstraintTestBase
    {
        public NotConstraintTests()
        {
            _constraint = new NotConstraint( new EqualConstraint<object>(null) );
            _expectedDescription = "not null";
            _expectedRepresentation = "<not <equal null>>";
        }

        internal object[] SuccessData = new object[] { 42, "Hello" };

        internal object[] FailureData = new object[] { new object[] { null, "null" } };

        [Test]
        public void NotHonorsIgnoreCaseUsingConstructors()
        {
            var ex = Assert.Throws<AssertionException>(() =>
            {
                Assert.That("abc", new NotConstraint(new EqualConstraint<string>("ABC").IgnoreCase));
            });

            Assert.That(ex.Message.Contains("ignoring case"));
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

        [Test]
        public void CanUseNotOperator()
        {
            Assert.That(42, !new EqualConstraint<int>(99));
        }
    }
}