// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using TCLite.Constraints;

namespace TCLite
{
    /// <summary>
    /// Helper class with properties and methods that supply
    /// a number of constraints used in Asserts.
    /// </summary>
    // Abstract because we support syntax extension by inheriting and declaring new static members.
    public abstract class Does : Does_Syntax
    {
#if NYI
        #region Contain

        /// <summary>
        /// Returns a new <see cref="SomeItemsConstraint"/> checking for the
        /// presence of a particular object in the collection.
        /// </summary>
        public static SomeItemsConstraint Contain(object expected) =>
            new SomeItemsConstraint(new EqualConstraint(expected));


        #endregion

        #region DictionaryContain
        /// <summary>
        /// Returns a new DictionaryContainsKeyConstraint checking for the
        /// presence of a particular key in the Dictionary key collection.
        /// </summary>
        /// <param name="expected">The key to be matched in the Dictionary key collection</param>
        public static DictionaryContainsKeyConstraint ContainKey(object expected)
        {
            return Contains.Key(expected);
        }

        /// <summary>
        /// Returns a new DictionaryContainsValueConstraint checking for the
        /// presence of a particular value in the Dictionary value collection.
        /// </summary>
        /// <param name="expected">The value to be matched in the Dictionary value collection</param>
        public static DictionaryContainsValueConstraint ContainValue(object expected)
        {
            return Contains.Value(expected);
        }

        #endregion
#endif
    }
}
