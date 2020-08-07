// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Internal;

namespace TCLite.Constraints
{
    [TestFixture]
    public class TrueConstraintTests : ConstraintTestBase<bool>
    {
        protected override Constraint Constraint => new TrueConstraint();

        protected override string ExpectedDescription => "True";
        protected override string ExpectedRepresentation => "<true>";

        static bool[] SuccessData => new bool[] { true, 2 + 2 == 4 };
        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(false, "False"),
            new TestCaseData(2 + 2 == 5, "False")
        };
        static TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData("hello", typeof(ArgumentException)),
            new TestCaseData(null, typeof(ArgumentNullException))
        };
    }
}
