// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// The Constraint class is the base of all built-in constraints
    /// within TCLite. The class models a constraint, which puts no
    /// limitations on the type of actual value it is able to handle.
    /// </summary>
    /// <remarks>
    /// This file contains Modifers that apply to all constraints as
    /// well as the two Binary Operators.
    /// </remarks>
    public abstract partial class Constraint : IConstraint, IResolveConstraint
    {
        #region Binary Operators

        /// <summary>
        /// Returns a ConstraintExpression by appending And
        /// to the current constraint.
        /// </summary>
        public ConstraintExpression And
        {
            get
            {
                if (Builder == null)
                {
                    Builder = new ConstraintBuilder();
                    Builder.Append(this);
                }

                Builder.Append(new AndOperator());

                return new ConstraintExpression(Builder);
            }
        }

        /// <summary>
        /// Returns a ConstraintExpression by appending And
        /// to the current constraint.
        /// </summary>
        public ConstraintExpression With
        {
            get { return And; }
        }

        /// <summary>
        /// Returns a ConstraintExpression by appending Or
        /// to the current constraint.
        /// </summary>
        public ConstraintExpression Or
        {
            get
            {
                if (Builder == null)
                {
                    Builder = new ConstraintBuilder();
                    Builder.Append(this);
                }

                Builder.Append(new OrOperator());

                return new ConstraintExpression(Builder);
            }
        }

        #endregion

        #region After Modifier


#if NYI // DelayedConstraint
        /// <summary>
        /// Returns a DelayedConstraint with the specified delay time.
        /// </summary>
        /// <param name="delayInMilliseconds">The delay in milliseconds.</param>
        /// <returns></returns>
        public DelayedConstraint After(int delayInMilliseconds)
        {
            return new DelayedConstraint(
                Builder == null ? this : Builder.Resolve(),
                delayInMilliseconds);
        }

        /// <summary>
        /// Returns a DelayedConstraint with the specified delay time
        /// and polling interval.
        /// </summary>
        /// <param name="delayInMilliseconds">The delay in milliseconds.</param>
        /// <param name="pollingInterval">The interval at which to test the constraint.</param>
        /// <returns></returns>
        public DelayedConstraint After(int delayInMilliseconds, int pollingInterval)
        {
            return new DelayedConstraint(
                Builder == null ? this : Builder.Resolve(),
                delayInMilliseconds,
                pollingInterval);
        }
#endif

        #endregion
    }
}