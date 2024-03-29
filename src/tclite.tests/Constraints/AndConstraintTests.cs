// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    [TestFixture]
    public class AndConstraintTests : ConstraintTestBase<object>
    {        
        protected override Constraint Constraint =>
            new AndConstraint(new GreaterThanConstraint<int>(40), new LessThanConstraint<int>(50));
        protected override string ExpectedDescription => "greater than 40 and less than 50";
        protected override string ExpectedRepresentation => "<and <greaterthan 40> <lessthan 50>>";

        static object[] SuccessData => new object[] { 42 };
        static TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(37, "37"),
            new TestCaseData(53, "53")
        };
    }
}