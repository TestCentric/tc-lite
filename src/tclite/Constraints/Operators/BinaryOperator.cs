// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Abstract base class for all binary operators
    /// </summary>
    public abstract class BinaryOperator : ConstraintOperator
    {
        /// <summary>
        /// Reduce produces a constraint from the operator and 
        /// any arguments. It takes the arguments from the constraint 
        /// stack and pushes the resulting constraint on it.
        /// </summary>
        /// <param name="stack"></param>
        public override void Reduce(ConstraintBuilder.ConstraintStack stack)
        {
            Constraint right = stack.Pop();
            Constraint left = stack.Pop();
            stack.Push(ApplyOperator(left, right));
        }

        /// <summary>
        /// Gets the left precedence of the operator
        /// </summary>
        public override int LeftPrecedence
        {
            get
            {
                return RightContext is CollectionOperator
                    ? base.LeftPrecedence + 10
                    : base.LeftPrecedence;
            }
        }

        /// <summary>
        /// Gets the right precedence of the operator
        /// </summary>
        public override int RightPrecedence
        {
            get
            {
                return RightContext is CollectionOperator
                    ? base.RightPrecedence + 10
                    : base.RightPrecedence;
            }
        }

        /// <summary>
        /// Abstract method that produces a constraint by applying
        /// the operator to its left and right constraint arguments.
        /// </summary>
        public abstract Constraint ApplyOperator(Constraint left, Constraint right);
    } 
}