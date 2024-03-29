// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
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
        protected IConstraint Left;
        /// <summary>
        /// The second constraint being combined
        /// </summary>
        protected IConstraint Right;

        /// <summary>
        /// Construct a BinaryConstraint from two other constraints
        /// </summary>
        /// <param name="left">The first constraint</param>
        /// <param name="right">The second constraint</param>
        protected BinaryConstraint(IConstraint left, IConstraint right)
            : base(left, right)
        {
            Guard.ArgumentNotNull(left, nameof(left));
            Left = left;

            Guard.ArgumentNotNull(right, nameof(right));
            Right = right;
        }
    }
}