// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Text.RegularExpressions;
using TCLite.Internal;

namespace TCLite.Constraints
{
    [TestFixture(false)]
    [TestFixture(true)]
    public class RegexConstraintTests : ConstraintTestBase<string>
    {
        private bool _useRegexArgument;

        public RegexConstraintTests(bool useRegexArgument)
        {
            _useRegexArgument = useRegexArgument;

            Constraint = useRegexArgument
                ? new RegexConstraint(new Regex("h.ll"))
                : new RegexConstraint("h.ll");
        }

        protected override Constraint Constraint { get; }
        protected override string ExpectedDescription => "String matching pattern \"h.ll\"";
        protected override string ExpectedRepresentation => "<regex \"h.ll\">";

        static string[] SuccessData => new string[] { "hello", "top of the hill", "hall closet", "say hello to fred", "What the hell?" };

        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData( "goodbye", "\"goodbye\"" ),
            new TestCaseData( "HELLO", "\"HELLO\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" )
        };

        static TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData( null, typeof(ArgumentNullException)),
            new TestCaseData( 123, typeof(ArgumentException))
        };

        [TestCase]
        public void CanIgnoreCase()
        {
            if (_useRegexArgument)
                Assert.That("HELLO!", new RegexConstraint(new Regex("h.ll", RegexOptions.IgnoreCase)));
            else
                Assert.That("HELLO!", new RegexConstraint("h.ll", RegexOptions.IgnoreCase));
        }
    }
}
