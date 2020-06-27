// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class StartsWithConstraintTests : StringConstraintTestBase
    {
        protected override Constraint Constraint => new StartsWithConstraint("hello");
        protected override string ExpectedDescription => "String starting with \"hello\"";
        protected override string ExpectedRepresentation => "<startswith \"hello\">";

        protected override string[] SuccessData => new string[] { "hello", "hello there" };
        protected override TestCaseData[] FailureData => new TestCaseData[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "HELLO THERE", "\"HELLO THERE\"" ),
            new TestCaseData( "I said hello", "\"I said hello\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" ) };
    }

    [TestFixture]
    public class StartsWithConstraintTestsIgnoringCase : ConstraintTestBase<string>
    {
        protected override Constraint Constraint => new StartsWithConstraint("hello").IgnoreCase;
        protected override string ExpectedDescription => "String starting with \"hello\", ignoring case";
        protected override string ExpectedRepresentation => "<startswith \"hello\">";

        protected override string[] SuccessData => new string[] { "Hello", "HELLO there" };

        protected override TestCaseData[] FailureData => new TestCaseData[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "What the hell?", "\"What the hell?\"" ),
            new TestCaseData( "I said hello", "\"I said hello\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" ) };
    }
}
