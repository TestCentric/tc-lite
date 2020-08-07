// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    [TestFixture]
    public class ExactTypeConstraintTests : ConstraintTestBase<object>
    {
        protected override Constraint Constraint => new ExactTypeConstraint(typeof(D1));
        protected override string ExpectedDescription => string.Format("<{0}>", typeof(D1));
        protected override string ExpectedRepresentation => string.Format("<typeof {0}>", typeof(D1));

        static object[] SuccessData => new object[] { new D1() };

        static TestCaseData[] FailureData => new TestCaseData[] { 
            new TestCaseData( new B(), "<" + typeof(B).FullName + ">" ),
            new TestCaseData( new D2(), "<" + typeof(D2).FullName + ">" )
        };

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}
