// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Constraints;

namespace TCLite.Framework
{
    /// <summary>
    /// Helper class with properties and methods that supply
    /// a number of constraints used in Asserts.
    /// </summary>
    public class Contains
    {
        #region Item

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if at least one of them succeeds.
        /// </summary>
        public static ConstraintExpression Item => new ConstraintExpression().Some;

        #endregion

        #region Substring

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public static SubstringConstraint Substring(string expected)
        {
            return new SubstringConstraint(expected);;
        }

        #endregion
    }
}
