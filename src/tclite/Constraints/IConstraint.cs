// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// Interface implemented by all constraints
    /// </summary>
    public interface IConstraint
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

        /// <summary>
        /// Validate the actual value argument based on what the
        /// particular constraint allows.virtual The default 
        /// implementation does nothing, implying that the constraint
        /// can handle any Type as well as null values.
        /// </summary>
        /// <param name="actual"></param>
        void ValidateActualValue(object actual);

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        ConstraintResult ApplyTo<TActual>(TActual actual);

        /// <summary>
        /// Applies the constraint to an ActualValueDelegate that returns 
        /// the value to be tested. The default implementation simply evaluates 
        /// the delegate but derived classes may override it to provide for 
        /// delayed processing.
        /// </summary>
        /// <param name="del">An ActualValueDelegate</param>
        /// <returns>A ConstraintResult</returns>
        ConstraintResult ApplyTo<TActual>(ActualValueDelegate<TActual> del);

        /// <summary>
        /// Test whether the constraint is satisfied by a given reference.
        /// The default implementation simply dereferences the value but
        /// derived classes may override it to provide for delayed processing.
        /// </summary>
        /// <param name="actual">A reference to the value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        ConstraintResult ApplyTo<TActual>(ref TActual actual);
    }
}
