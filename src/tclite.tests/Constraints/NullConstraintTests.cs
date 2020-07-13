// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class NullConstraintTests : ConstraintTestBase<object>
    {
        protected override Constraint Constraint => new NullConstraint();

        protected override string ExpectedDescription => "null";
        protected override string ExpectedRepresentation => "<null>";

        static object[] SuccessData = new object[] { null, (string)null };
        static TestCaseData[] FailureData = new TestCaseData[]
        {
            new TestCaseData("hello", "\"hello\""),
            new TestCaseData(42, "42")
        };
    }
}
