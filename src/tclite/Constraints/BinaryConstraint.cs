// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// BinaryConstraint is the abstract base of all constraints
    /// that combine two other constraints in some fashion.
    /// </summary>
    public abstract class BinaryConstraint : Constraint
    {
        /// <summary>
        /// The first constraint being combined
        /// </summary>
        protected Constraint left;
        /// <summary>
        /// The second constraint being combined
        /// </summary>
        protected Constraint right;

        /// <summary>
        /// Construct a BinaryConstraint from two other constraints
        /// </summary>
        /// <param name="left">The first constraint</param>
        /// <param name="right">The second constraint</param>
        protected BinaryConstraint(Constraint left, Constraint right)
            : base(left, right)
        {
            this.left = left;
            this.right = right;
        }
    }
}