// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="Char"/>s.
    /// </summary>
    internal sealed class CharsComparer : IEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal CharsComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            return x is char && y is char;
        }

        public bool AreEqual<T1,T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x,y), "Invalid call to AreEqual");


            char xChar = Convert.ToChar(x);
            char yChar = Convert.ToChar(y);

            bool caseInsensitive = _equalityComparer.IgnoreCase;

            char c1 = caseInsensitive ? Char.ToLower(xChar) : xChar;
            char c2 = caseInsensitive ? Char.ToLower(yChar) : yChar;

            return c1 == c2;
        }
    }
}
