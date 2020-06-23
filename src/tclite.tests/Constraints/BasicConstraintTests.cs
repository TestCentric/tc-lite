// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Globalization;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class NullConstraintTest : ConstraintTestBase
    {
        public NullConstraintTest()
        {
            _constraint = new NullConstraint();
            _expectedDescription = "null";
            _expectedRepresentation = "<null>";
        }
        
        static object[] SuccessData = new object[] { null };

        static object[] FailureData = new object[] { new object[] { "hello", "\"hello\"" } };
    }

    [TestFixture]
    public class TrueConstraintTest : ConstraintTestBase
    {
        public TrueConstraintTest()
        {
            _constraint = new TrueConstraint();
            _expectedDescription = "True";
            _expectedRepresentation = "<true>";
        }
        
        static object[] SuccessData = new object[] { true, 2+2==4 };
        
        static object[] FailureData = new object[] { 
            //new object[] { null, "null" }, new object[] { "hello", "\"hello\"" },
            new object[] { false, "False"}, new object[] { 2+2==5, "False" } };
    }

    [TestFixture]
    public class FalseConstraintTest : ConstraintTestBase
    {
        public FalseConstraintTest()
        {
            _constraint = new FalseConstraint();
            _expectedDescription = "False";
            _expectedRepresentation = "<false>";
        }

        static object[] SuccessData = new object[] { false, 2 + 2 == 5 };

        static object[] FailureData = new object[] { 
            //new TestCaseData( null, "null" ),
            //new TestCaseData( "hello", "\"hello\"" ),
            new TestCaseData( true, "True" ),
            new TestCaseData( 2+2==4, "True" )};
    }

#if NYI // NaN
    [TestFixture]
    public class NaNConstraintTest : ConstraintTestBase
    {
        public NaNConstraintTest()
        {
            _constraint = new NaNConstraint();
            _expectedDescription = "NaN";
            _expectedRepresentation = "<nan>";
        }
        
        static object[] SuccessData = new object[] { double.NaN, float.NaN };

        static object[] FailureData = new object[] { 
            new TestCaseData( null, "null" ),
            new TestCaseData( "hello", "\"hello\"" ),
            new TestCaseData( 42, "42" ), 
            new TestCaseData( double.PositiveInfinity, double.PositiveInfinity.ToString() ),
            new TestCaseData( double.NegativeInfinity, double.NegativeInfinity.ToString() ),
            new TestCaseData( float.PositiveInfinity, double.PositiveInfinity.ToString() ),
            new TestCaseData( float.NegativeInfinity, double.NegativeInfinity.ToString() ) };
    }
#endif
}
