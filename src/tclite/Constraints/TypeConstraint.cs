// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// TypeConstraint is the abstract base for constraints
    /// that take a Type as their expected value.
    /// </summary>
    public abstract class TypeConstraint : Constraint
    {
        /// <summary>
        /// The expected Type used by the constraint
        /// </summary>
        protected Type ExpectedType { get; }
        protected Type ActualType { get; private set; }

        /// <summary>
        /// Construct a TypeConstraint for a given Type
        /// </summary>
        /// <param name="type"></param>
        protected TypeConstraint(Type type) : base(type)
        {
            ExpectedType = type;
        }

        // /// <summary>
        // /// Applies the constraint to an actual value, returning a ConstraintResult.
        // /// </summary>
        // /// <param name="actual">The value to be tested</param>
        // /// <returns>A ConstraintResult</returns>
        // public override ConstraintResult ApplyTo<TActual>(TActual actual)
        // {
        //     ActualType = actual == null ? null : actual.GetType();

        //     if (actual is Exception)
        //         return new ConstraintResult(this, actual, this.Matches(actual));

        //     return new ConstraintResult(this, ActualType, this.Matches(actual));
        // }

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public override ConstraintResult ApplyTo(object actual)
        {
            ActualType = actual == null ? null : actual.GetType();

            if (actual is Exception)
                return new ConstraintResult(this, actual, this.Matches(actual));

            return new ConstraintResult(this, ActualType, this.Matches(actual));
        }

        protected abstract bool Matches<TActual>(TActual actual);
    }
}