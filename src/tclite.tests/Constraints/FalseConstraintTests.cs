// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class FalseConstraintTests : ConstraintTestBase<bool>
    {
        protected override Constraint Constraint => new FalseConstraint();
        protected override string ExpectedDescription => "False";
        protected override string ExpectedRepresentation => "<false>";

        protected override bool[] SuccessData => new bool[] { false, 2 + 2 == 5 };
        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(true, "True"),
            new TestCaseData(2 + 2 == 4, "True")
        };

        protected override TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData("hello", typeof(ArgumentException)),
            new TestCaseData(null, typeof(ArgumentNullException))
        };
    }
}
