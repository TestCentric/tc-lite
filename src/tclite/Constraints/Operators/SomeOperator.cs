﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Represents a constraint that succeeds if any of the 
    /// members of a collection match a base constraint.
    /// </summary>
    public class SomeOperator : CollectionOperator
    {
        /// <summary>
        /// Returns a constraint that will apply the argument
        /// to the members of a collection, succeeding if
        /// any of them succeed.
        /// </summary>
        public override IConstraint ApplyPrefix(IConstraint constraint)
        {
            return new SomeItemsConstraint(constraint);
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if at least one of them succeeds.
        /// </summary>
        public ConstraintExpression Some
        {
            get { return this.Append(new SomeOperator()); }
        }
    }

    public partial class Contains_Syntax
    {
        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if at least one of them succeeds.
        /// </summary>
        public static ConstraintExpression Item => new ConstraintExpression().Some;
    }

    public partial class Has_Syntax
    {
        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if at least one of them succeeds.
        /// </summary>
        public static ConstraintExpression Some => new ConstraintExpression().Some;

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if at least one of them succeeds.
        /// </summary>
        public static ConstraintExpression Member => new ConstraintExpression().Some;
    }
}
