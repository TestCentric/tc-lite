// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="Array"/>s.
    /// </summary>
    internal sealed class ArraysComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;
        private readonly EnumerablesComparer _enumerablesComparer;

        internal ArraysComparer(TCLiteEqualityComparer equalityComparer, EnumerablesComparer enumerablesComparer)
        {
            _equalityComparer = equalityComparer;
            _enumerablesComparer = enumerablesComparer;
        }

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            return x is Array && y is Array && !_equalityComparer.CompareAsCollection;
        }

        public bool AreEqual<T1, T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x, y), "Unexpected call");

            // if (!x.GetType().IsArray || !y.GetType().IsArray || _equalityComparer.CompareAsCollection)
            //     return null;

            Array xArray = x as Array;
            Array yArray = y as Array;

            int rank = xArray.Rank;

            if (rank != yArray.Rank)
                return false;

            for (int r = 1; r < rank; r++)
                if (xArray.GetLength(r) != yArray.GetLength(r))
                    return false;

            return _enumerablesComparer.AreEqual(xArray, yArray, ref tolerance);
        }
    }
}
