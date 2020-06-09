// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Abstract base class used for prefixes
    /// </summary>
    public abstract class PrefixConstraint : Constraint
    {
        /// <summary>
        /// The base constraint
        /// </summary>
        protected Constraint baseConstraint;

        /// <summary>
        /// Construct given a base constraint
        /// </summary>
        /// <param name="resolvable"></param>
        protected PrefixConstraint(IResolveConstraint resolvable) : base(resolvable)
        {
            if (resolvable != null)
                this.baseConstraint = resolvable.Resolve();
        }
    }
}