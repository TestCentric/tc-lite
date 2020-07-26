// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EmptyConstraint tests a whether a string or collection is empty,
    /// postponing the decision about which test is applied until the
    /// type of the actual argument is known.
    /// </summary>
    public class EmptyConstraint : Constraint
    {
        private Constraint _realConstraint;

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return _realConstraint == null ? "<empty>" : _realConstraint.Description; }
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override ConstraintResult ApplyConstraint<TActual>(TActual actual)
        {
            // NOTE: actual is string will fail for a null typed as string           
            Type actualType = typeof(TActual);
            
            if (actualType == typeof(string))
                _realConstraint = new EmptyStringConstraint();
            else if (actual == null)
                throw new System.ArgumentException($"The actual value must be a string, non-null IEnumerable or DirectoryInfo. The value passed was of type {actualType}.", nameof(actual));
            else
                _realConstraint = new EmptyCollectionConstraint();

            return _realConstraint.ApplyTo(actual);
        }
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a constraint that tests for empty
        /// </summary>
        public static EmptyConstraint Empty
        {
            get { return new EmptyConstraint(); }
        }
    }
}
