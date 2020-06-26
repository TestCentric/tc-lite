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
    public class TrueConstraintTests : ConstraintTestBase<bool>
    {
        protected override Constraint Constraint => new TrueConstraint();

        protected override string ExpectedDescription => "True";
        protected override string ExpectedRepresentation => "<true>";

        protected override bool[] SuccessData => new bool[] { true, 2 + 2 == 4 };
        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(false, "False"),
            new TestCaseData(2 + 2 == 5, "False")
        };
        protected override TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData("hello", typeof(ArgumentException)),
            new TestCaseData(null, typeof(ArgumentNullException))
        };
    }
}
