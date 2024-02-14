// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="Numerics"/>s.
    /// </summary>
    internal sealed class NumericsComparer : ITCLiteEqualityComparer
    {
        public bool CanCompare<T1, T2>(T1 x, T2 y)
        {
            return Numerics.IsNumericType(x) && Numerics.IsNumericType(y);
        }

        public bool AreEqual<T1, T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x,y), "Invalid call");

            return Numerics.AreEqual(x, y, ref tolerance);
        }
    }
}
