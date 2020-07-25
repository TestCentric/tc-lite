// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="IDictionary"/>s.
    /// </summary>
    internal sealed class DictionariesComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal DictionariesComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            return x is IDictionary && y is IDictionary;
        }

        public bool AreEqual<T1,T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            IDictionary xDictionary = (IDictionary)x;
            IDictionary yDictionary = (IDictionary)y;

            if (xDictionary.Count != yDictionary.Count)
                return false;

            CollectionTally tally = new CollectionTally(_equalityComparer, xDictionary.Keys);
            tally.TryRemove(yDictionary.Keys);
            if ((tally.Result.MissingItems.Count > 0) || (tally.Result.ExtraItems.Count > 0))
                return false;

            foreach (object key in xDictionary.Keys)
                if (!_equalityComparer.AreEqual(xDictionary[key], yDictionary[key], ref tolerance))
                    return false;

            return true;
        }
    }
}
