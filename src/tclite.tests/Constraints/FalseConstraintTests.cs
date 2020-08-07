// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Constraints
{
    [TestFixture]
    public class FalseConstraintTests : ConstraintTestBase<bool>
    {
        protected override Constraint Constraint => new FalseConstraint();
        protected override string ExpectedDescription => "False";
        protected override string ExpectedRepresentation => "<false>";

        static bool[] SuccessData => new bool[] { false, 2 + 2 == 5 };
        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(true, "True"),
            new TestCaseData(2 + 2 == 4, "True")
        };

        static TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData("hello", typeof(ArgumentException)),
            new TestCaseData(null, typeof(ArgumentNullException))
        };
    }
}
