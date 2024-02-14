// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="DictionaryEntry"/>s.
    /// </summary>
    internal sealed class DictionaryEntriesComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal DictionaryEntriesComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            return x is DictionaryEntry && y is DictionaryEntry;
        }

        public bool AreEqual<T1,T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x,y), "Should not be called");
            // Issue #70 - EquivalentTo isn't compatible with IgnoreCase for dictionaries

            DictionaryEntry xDictionaryEntry = (DictionaryEntry)Convert.ChangeType(x, typeof(DictionaryEntry));
            DictionaryEntry yDictionaryEntry = (DictionaryEntry)Convert.ChangeType(y, typeof(DictionaryEntry));

            var keyTolerance = Tolerance.Exact;
            return _equalityComparer.AreEqual(xDictionaryEntry.Key, yDictionaryEntry.Key, ref keyTolerance) 
                && _equalityComparer.AreEqual(xDictionaryEntry.Value, yDictionaryEntry.Value, ref tolerance);
        }
    }
}
