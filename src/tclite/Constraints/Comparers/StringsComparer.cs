// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="String"/>s.
    /// </summary>
    internal sealed class StringsComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal StringsComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1, T2>(T1 x, T2 y)
        {
            return x is string && y is string;
        }

        public bool AreEqual<T1, T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x, y), "Invalid call");

            // if (!(x is string) || !(y is string))
            //     return null;

            string xString = x as string;
            string yString = y as string;

            bool caseInsensitive = _equalityComparer.IgnoreCase;

            string s1 = caseInsensitive ? xString.ToLower() : xString;
            string s2 = caseInsensitive ? yString.ToLower() : yString;

            return s1.Equals(s2);
        }
    }
}
