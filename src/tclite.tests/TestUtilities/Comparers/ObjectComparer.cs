// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;

namespace TCLite.TestUtilities.Comparers
{
    /// <summary>
    /// ObjectComparer is used in testing to ensure that only
    /// methods of the IComparer interface are used.
    /// </summary>
    public class ObjectComparer : IComparer
    {
        public bool WasCalled = false;
        public static readonly IComparer Default = new ObjectComparer();

        int IComparer.Compare(object x, object y)
        {
            WasCalled = true;
            return Comparer.Default.Compare(x, y);
        }
    }
}
