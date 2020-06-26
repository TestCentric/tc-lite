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
    public class NaNConstraintTests : ConstraintTestBase<double>
    {
        protected override Constraint Constraint => new NaNConstraint();
        protected override string ExpectedDescription => "NaN";
        protected override string ExpectedRepresentation => "<nan>";

        protected override double[] SuccessData => new double[] { double.NaN, float.NaN };
        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData(42, "42.0d"),
            new TestCaseData(42.0, "42.0d"),
            new TestCaseData(double.PositiveInfinity, "Infinity"),
            new TestCaseData(double.NegativeInfinity, "-Infinity"),
            new TestCaseData(float.PositiveInfinity, "Infinity"),
            new TestCaseData(float.NegativeInfinity, "-Infinity")
        };
        protected override TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData("hello", typeof(ArgumentException)),
            new TestCaseData(null, typeof(ArgumentNullException))
        };
    }
}
