// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// Negates the test of the constraint it wraps.
    /// </summary>
    public class NotOperator : PrefixOperator
    {
        /// <summary>
        /// Constructs a new NotOperator
        /// </summary>
        public NotOperator()
        {
            // Not stacks on anything and only allows other
            // prefix ops to stack on top of it.
            this.left_precedence = this.right_precedence = 1;
        }

        /// <summary>
        /// Returns a NotConstraint applied to its argument.
        /// </summary>
        public override IConstraint ApplyPrefix(IConstraint constraint)
        {
            return new NotConstraint(constraint);
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public ConstraintExpression Not
        {
            get { return this.Append(new NotOperator()); }
        }

        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public ConstraintExpression No
        {
            get { return this.Append(new NotOperator()); }
        }
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public static ConstraintExpression Not
        {
            get { return new ConstraintExpression().Not; }
        }
    }

    public partial class Has_Syntax
    {
        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public static ConstraintExpression No
        {
            get { return new ConstraintExpression().Not; }
        }
    }

    public partial class Does_Syntax
    {
        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public static ConstraintExpression Not
        {
            get { return new ConstraintExpression().Not; }
        }
    }
}
