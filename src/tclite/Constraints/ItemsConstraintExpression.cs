// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// An extension of ResolvableConstraintExpression that adds a no-op Items property for readability.
    /// </summary>
    public sealed class ItemsConstraintExpression : ConstraintExpression
    {
        /// <summary>
        /// Create a new instance of ItemsConstraintExpression
        /// </summary>
        public ItemsConstraintExpression() { }

        /// <summary>
        /// Create a new instance of ResolvableConstraintExpression,
        /// passing in a pre-populated ConstraintBuilder.
        /// </summary>
        /// <param name="builder"></param>
        public ItemsConstraintExpression(ConstraintBuilder builder)
            : base(builder) { }

        /// <summary>
        /// No-op property for readability.
        /// </summary>
        public ResolvableConstraintExpression Items
        {
            get { return new ResolvableConstraintExpression(Builder); }
        }
    }
}