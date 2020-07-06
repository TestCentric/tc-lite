// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    class SomeItemsConstraintTests : ConstraintTestBase<IEnumerable<string>>
    {
        protected override Constraint Constraint => new SomeItemsConstraint(new EqualConstraint<string>("hello"));
        protected override string ExpectedDescription => "some item hello";
        protected override string ExpectedRepresentation => "<some <equal \"hello\">>";

        protected override IEnumerable<string>[] SuccessData => new IEnumerable<string>[]
        {
            new string[] { "bonjour", "hello", "goodbye" }
        };

        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData( new string[] { "goodbye", "so long" }, "< \"goodbye\", \"so long\" >"),
            new TestCaseData( new string[] { "Bonjour", "Hello", "Goodbye" }, "< \"Bonjour\", \"Hello\", \"Goodbye\" >")
        };

        [Test]
        public void EqualConstraintWithIgnoreCaseDoesNotThrow()
        {
            Assert.That(new string[] { "Hello", "Goodbye" }, Has.Some.EqualTo("HELLO").IgnoreCase);
            Assert.That(new string[] { "Hello", "Goodbye" }, Has.Member.EqualTo("HELLO").IgnoreCase);
        }
    }
}