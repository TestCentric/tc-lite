// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public ExactTypeConstraint TypeOf(Type expectedType)
        {
            return (ExactTypeConstraint)this.Append(new ExactTypeConstraint(expectedType));
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public ExactTypeConstraint TypeOf<T>()
        {
            return (ExactTypeConstraint)this.Append(new ExactTypeConstraint(typeof(T)));
        }
    }

    /// <summary>
    /// ExactTypeConstraint is used to test that an object
    /// is of the exact type provided in the constructor
    /// </summary>
    public class ExactTypeConstraint : TypeConstraint
    {
        /// <summary>
        /// Construct an ExactTypeConstraint for a given Type
        /// </summary>
        /// <param name="type">The expected Type.</param>
        public ExactTypeConstraint(Type type)
            : base(type)
        {
            DisplayName = "typeof";
        }

        public override string Description => $"<{ExpectedType.FullName}>";

        /// <summary>
        /// Test that an object is of the exact type specified
        /// </summary>
        /// <param name="actual">The actual value.</param>
        /// <returns>True if the tested object is of the exact type provided, otherwise false.</returns>
        protected override bool Matches<TActual>(TActual actual)
        {
            return actual != null && actual.GetType() == ExpectedType;
        }
    }
}