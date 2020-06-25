// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EmptyConstraint tests a whether a string or collection is empty,
    /// postponing the decision about which test is applied until the
    /// type of the actual argument is known.
    /// </summary>
    public class EmptyConstraint : Constraint
    {
        private Constraint realConstraint;

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return realConstraint == null ? "<empty>" : realConstraint.Description; }
        }

//         /// <summary>
//         /// Test whether the constraint is satisfied by a given value
//         /// </summary>
//         /// <param name="actual">The value to be tested</param>
//         /// <returns>True for success, false for failure</returns>
//         public override ConstraintResult ApplyTo<TActual>(TActual actual)
//         {
//             if (typeof(TActual) == typeof(string))
//                 realConstraint = new EmptyStringConstraint();
//             else if (actual == null)
//                 throw new System.ArgumentException($"The actual value must be a string, non-null IEnumerable or DirectoryInfo. The value passed was of type {typeof(TActual)}.", nameof(actual));
// #if NYI // Directory
//             else if (actual is System.IO.DirectoryInfo)
//                 realConstraint = new EmptyDirectoryConstraint();
// #endif
//             else
//                 realConstraint = new EmptyCollectionConstraint();

//             return realConstraint.ApplyTo(actual);
//         }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override ConstraintResult ApplyTo(object actual)
        {
            if (actual is string)
                realConstraint = new EmptyStringConstraint();
            else if (actual == null)
                throw new System.ArgumentException($"The actual value must be a string, non-null IEnumerable or DirectoryInfo.", nameof(actual));
#if NYI // Directory
            else if (actual is System.IO.DirectoryInfo)
                realConstraint = new EmptyDirectoryConstraint();
#endif
            else
                realConstraint = new EmptyCollectionConstraint();

            return realConstraint.ApplyTo(actual);
        }
    }
}
