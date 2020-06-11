// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Tests
{
    [TestFixture]
    public class OrConstraintTests : ConstraintTestBase
    {
        public OrConstraintTests()
        {
            _constraint = new OrConstraint(new EqualConstraint<int>(42), new EqualConstraint<int>(99));
            _expectedDescription = "42 or 99";
            _expectedRepresentation = "<or <equal 42> <equal 99>>";
        }

        internal object[] SuccessData = new object[] { 99, 42 };

        internal object[] FailureData = new object[] { new object[] { 37, "37" } };

		[Test]
        public void CanCombineTestsWithOrOperator()
        {
            Assert.That(99, new EqualConstraint<int>(42) | new EqualConstraint<int>(99) );
        }
    }
}