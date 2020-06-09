// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class AndConstraintTests : ConstraintTestBase
    {
        public AndConstraintTests()
        {
            _constraint = new AndConstraint(new GreaterThanConstraint(40), new LessThanConstraint(50));
            _expectedDescription = "greater than 40 and less than 50";
            _expectedRepresentation = "<and <greaterthan 40> <lessthan 50>>";
        }

		internal object[] SuccessData = new object[] { 42 };

        internal object[] FailureData = new object[] { new object[] { 37, "37" }, new object[] { 53, "53" } };

		[Test]
        public void CanCombineTestsWithAndOperator()
        {
            Assert.That(42, new GreaterThanConstraint(40) & new LessThanConstraint(50));
        }
    }
}