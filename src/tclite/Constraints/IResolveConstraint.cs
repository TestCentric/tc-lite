// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// The IConstraintExpression interface is implemented by all
    /// complete and resolvable constraints and expressions.
    /// </summary>
    public interface IResolveConstraint
    {
        /// <summary>
        /// Return the top-level constraint for this expression
        /// </summary>
        /// <returns></returns>
        Constraint Resolve();
    }
}
