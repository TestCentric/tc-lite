// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// TypeConstraint is the abstract base for constraints
    /// that take a Type as their expected value.
    /// </summary>
    public abstract class TypeConstraint : Constraint<Type>
    {
        /// <summary>
        /// The expected Type used by the constraint
        /// </summary>
        protected readonly Type expectedType;

        /// <summary>
        /// Construct a TypeConstraint for a given Type
        /// </summary>
        /// <param name="type"></param>
        protected TypeConstraint(Type type) : base(type)
        {
            this.expectedType = type;
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. TypeConstraints override this method to write
        /// the name of the type.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteActualValue(actual == null ? null : actual.GetType());
        }
    }
}