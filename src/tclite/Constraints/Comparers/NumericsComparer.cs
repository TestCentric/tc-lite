// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="Numerics"/>s.
    /// </summary>
    internal sealed class NumericsComparer : IEqualityComparer
    {
        public bool CanCompare<T1, T2>(T1 x, T2 y)
        {
            return Numerics.IsNumericType(x) && Numerics.IsNumericType(y);
        }

        public bool AreEqual<T1, T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x,y), "Invalid call");

            // if (!Numerics.IsNumericType(x) || !Numerics.IsNumericType(y))
            //     return null;

            return Numerics.AreEqual(x, y, ref tolerance);
        }
    }
}
