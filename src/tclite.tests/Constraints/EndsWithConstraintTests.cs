// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class EndsWithConstraintTests : StringConstraintTestBase
    {
        protected override Constraint Constraint => new EndsWithConstraint("hello");
        protected override string ExpectedDescription => "String ending with \"hello\"";
        protected override string ExpectedRepresentation => "<endswith \"hello\">";

        protected static new string[] SuccessData => new string[] { "hello", "I said hello" };

        protected static new  TestCaseData[] FailureData => new TestCaseData[] {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "hello there", "\"hello there\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" ) };
    }

    [TestFixture]
    public class EndsWithConstraintTestsIgnoringCase : StringConstraintTestBase
    {
        protected override Constraint Constraint => new EndsWithConstraint("hello").IgnoreCase;
        protected override string ExpectedDescription => "String ending with \"hello\", ignoring case";
        protected override string ExpectedRepresentation => "<endswith \"hello\">";

        static string[] SuccessData => new string[] { "HELLO", "I said Hello" };

        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData( "goodbye", "\"goodbye\"" ), 
            new TestCaseData( "What the hell?", "\"What the hell?\"" ),
            new TestCaseData( "hello there", "\"hello there\"" ),
            new TestCaseData( "say hello to Fred", "\"say hello to Fred\"" ),
            new TestCaseData( string.Empty, "<string.Empty>" ),
            new TestCaseData( null , "null" )
        };
    }
}
