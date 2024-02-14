// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// The IConstraintExpression interface is implemented by all
    /// complete and resolvable constraints and expressions.
    /// </summary>
    public interface IResolveConstraint
    {
        /// <summary>
        /// Returns True if the constraint or expression is complete,
        /// False if still under construction.
        /// </summary>
        /// <value></value>
        bool IsResolvable { get; }

        /// <summary>
        /// Return the top-level constraint for the complete expression.
        /// Throws InvalidOperationException if the expression is incomplete.
        /// </summary>
        IConstraint Resolve();
    }
}
