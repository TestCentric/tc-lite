// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class FalseConstraintTests
    {
        [Test]
        public void ProvidesProperDescription()
        {
            Assert.That(new FalseConstraint().Description, Is.EqualTo("False"));
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.That(new FalseConstraint().ToString(), Is.EqualTo("<false>"));
        }

        [TestCase(false)]
        [TestCase(2 + 2 == 5)]
        public void SucceedsWithGoodValues(bool goodValue)
        {
            Assert.IsFalse(goodValue);
            Assert.That(goodValue, Is.False);
        }

        [TestCase(true)]
        [TestCase(2 + 2 == 4)]
        public void FailsWithBadValues(bool badValue)
        {
            var ex = Assert.Throws<AssertionException>(() =>
                Assert.IsFalse(badValue));

            Assert.That(ex.Message, Is.EqualTo(
                $"  Expected: False\n  But was:  True\n"));
        }

        static object[] FailureData = new object[] { 
            //new TestCaseData( null, "null" ),
            //new TestCaseData( "hello", "\"hello\"" ),
            new TestCaseData( true, "True" ),
            new TestCaseData( 2+2==4, "True" )};
    }
}
