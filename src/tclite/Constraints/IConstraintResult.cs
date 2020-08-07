// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// ConstraintStatus represents the status of a ConstraintResult
    /// returned by a Constraint being applied to an actual value.
    /// </summary>
    public enum ConstraintStatus
    {
        /// <summary>
        /// The status has not yet been set
        /// </summary>
        Unknown,

        /// <summary>
        /// The constraint succeeded
        /// </summary>
        Success,

        /// <summary>
        /// The constraint failed
        /// </summary>
        Failure,

        /// <summary>
        /// An error occurred in applying the constraint (reserved for future use)
        /// </summary>
        Error
    }

    /// <summary>
    /// Interface for all constraints
    /// </summary>
    public interface IConstraintResult
    {
        /// <summary>
        /// Display friendly name of the constraint.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets and sets the ResultStatus for this result.
        /// </summary>
        ConstraintStatus Status { get; set; }

        /// <summary>
        /// True if actual value meets the Constraint criteria otherwise false.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// The description of this constraint used in messages
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The value used in calling ApplyTo to produce this result
        /// </summary>
        object ActualValue { get; }

    }
}
