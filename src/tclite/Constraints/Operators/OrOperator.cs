// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// Operator that requires at least one of it's arguments to succeed
    /// </summary>
    public class OrOperator : BinaryOperator
    {
        /// <summary>
        /// Construct an OrOperator
        /// </summary>
        public OrOperator()
        {
            this.left_precedence = this.right_precedence = 3;
        }

        /// <summary>
        /// Apply the operator to produce an OrConstraint
        /// </summary>
        public override IConstraint ApplyOperator(IConstraint left, IConstraint right)
        {
            return new OrConstraint(left, right);
        }
    }
}