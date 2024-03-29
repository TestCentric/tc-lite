// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints.Comparers
{
    internal interface ITCLiteEqualityComparer
    {
        bool CanCompare<T1, T2>(T1 x, T2 y);
        bool AreEqual<T1, T2>(T1 x, T2 y, ref Tolerance tolerance);
    }
}
