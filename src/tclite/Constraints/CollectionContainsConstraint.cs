// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// CollectionContainsConstraint is used to test whether a collection
    /// contains an expected object as a member.
    /// </summary>
    public class CollectionContainsConstraint : CollectionItemsEqualConstraint
    {
        private readonly object ExpectedValue;

        /// <summary>
        /// Construct a CollectionContainsConstraint
        /// </summary>
        /// <param name="expected"></param>
        public CollectionContainsConstraint(object expected)
            : base(expected)
        {
            ExpectedValue = expected;
            DisplayName = "contains";
        }

        public override string Description => $"contains {ExpectedValue}";

        /// <summary>
        /// Test whether the expected item is contained in the collection
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool Match(IEnumerable actual)
        {
            foreach (object obj in actual)
                if (ItemsEqual(obj, ExpectedValue))
                    return true;

            return false;
        }
    }
}
