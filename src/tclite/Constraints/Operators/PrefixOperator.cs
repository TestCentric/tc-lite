// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// PrefixOperator takes a single constraint and modifies
    /// it's action in some way.
    /// </summary>
    public abstract class PrefixOperator : ConstraintOperator
    {
        /// <summary>
        /// Reduce produces a constraint from the operator and 
        /// any arguments. It takes the arguments from the constraint 
        /// stack and pushes the resulting constraint on it.
        /// </summary>
        /// <param name="stack"></param>
        public override void Reduce(ConstraintBuilder.ConstraintStack stack)
        {
            stack.Push(ApplyPrefix(stack.Pop()));
        }

        /// <summary>
        /// Returns the constraint created by applying this
        /// prefix to another constraint.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public abstract Constraint ApplyPrefix(Constraint constraint);
    }
}