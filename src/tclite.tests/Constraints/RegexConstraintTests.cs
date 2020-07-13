// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class RegexConstraintTests : StringConstraintTestBase
    {
        protected override Constraint Constraint => new RegexConstraint("h.ll");
        protected override string ExpectedDescription => "String matching \"h.ll\"";
        protected override string ExpectedRepresentation => "<regex \"h.ll\">";

        static string[] SuccessData => new string[] { "hello", "top of the hill", "hall closet", "say hello to fred", "What the hell?" };

        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData( "goodbye", "\"goodbye\"" ),
            new TestCaseData( "HELLO", "\"HELLO\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null, "null" )
        };
    }
}
