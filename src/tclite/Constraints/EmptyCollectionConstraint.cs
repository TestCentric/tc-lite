// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EmptyCollectionConstraint tests whether a collection is empty. 
    /// </summary>
    public class EmptyCollectionConstraint : CollectionConstraint
    {
        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return "<empty>"; }
        }

        /// <summary>
        /// Check that the collection is empty
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        protected override bool Match(IEnumerable collection)
        {
            return IsEmpty(collection);
        }
    }
}