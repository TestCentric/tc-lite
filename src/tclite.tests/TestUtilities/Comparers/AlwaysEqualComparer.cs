// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.TestUtilities
{
    internal class AlwaysEqualComparer : IComparer
    {
        public bool Called = false;

        int IComparer.Compare(object x, object y)
        {
            Called = true;

            // This comparer ALWAYS returns zero (equal)!
            return 0;
        }
    }
}
