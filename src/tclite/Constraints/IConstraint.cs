// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Interface for all constraints
    /// </summary>
    public interface IConstraint : IResolveConstraint
    {
        /// <summary>
        /// The display name of this Constraint for use by ToString().
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Arguments provided to this Constraint, for use in
        /// formatting the description.
        /// </summary>
        object[] Arguments { get; }

        /// <summary>
        /// The ConstraintBuilder holding this constraint
        /// </summary>
        ConstraintBuilder Builder { get; set; }
    }
}
