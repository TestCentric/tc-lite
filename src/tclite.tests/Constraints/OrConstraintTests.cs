// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class OrConstraintTests : ConstraintTestBase<object>
    {
        protected override Constraint Constraint => new OrConstraint(new EqualConstraint<int>(42), new EqualConstraint<int>(99));
        protected override string ExpectedDescription => "42 or 99";
        protected override string ExpectedRepresentation => "<or <equal 42> <equal 99>>";

        static object[] SuccessData => new object[] { 99, 42 };

        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(37, "37")
        };
    }
}