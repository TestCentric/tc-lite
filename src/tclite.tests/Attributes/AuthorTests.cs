// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Attributes
{
    //[TestFixture(Author = "Rob Prouse <rob@prouse.org>"), Author("Charlie Poole", "Charlie@poole.org")]
    [Author("Fred")]
    //[TestOf(typeof(AuthorAttribute))]
    public class AuthorTests
    {
        private IPropertyBag _properties = TestExecutionContext.CurrentContext.CurrentTest.Properties;

        [TestCase]
        public void AuthorOnFixtureOnly()
        {
            Assert.That(_properties[PropertyNames.Author], Is.EqualTo(new[] { "Fred" }));
        }

        [TestCase, Author("Charlie")]
        public void AuthorOnFixtureAndTestMethod()
        {
            Assert.That(_properties[PropertyNames.Author], Is.EqualTo(new[] { "Fred", "Charlie" }));
        }

        [TestCase, Author("Charlie"), Author("John")]
        public void AuthorOnFixtureAndTwoOnTestMethod()
        {
            Assert.That(_properties[PropertyNames.Author], Is.EqualTo(new[] { "Fred", "Charlie", "John" }));
        }
    }
}
