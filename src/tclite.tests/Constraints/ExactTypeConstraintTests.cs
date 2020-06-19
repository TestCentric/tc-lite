// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class ExactTypeConstraintTests : ConstraintTestBase
    {
        public ExactTypeConstraintTests()
        {
            _constraint = new ExactTypeConstraint(typeof(D1));
            _expectedDescription = string.Format("<{0}>", typeof(D1));
            _expectedRepresentation = string.Format("<typeof {0}>", typeof(D1));
        }

        static object[] SuccessData = new object[] { new D1() };
        
        static object[] FailureData = new object[] { 
            new TestCaseData( new B(), "<" + typeof(B).FullName + ">" ),
            new TestCaseData( new D2(), "<" + typeof(D2).FullName + ">" )
        };

        class B { }

        class D1 : B { }

        class D2 : D1 { }
    }
}
