// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TCLite.TestUtilities.Comparers
{
    internal class GenericComparison<T>
    {
        public bool WasCalled = false;

        public Comparison<T> Delegate
        {
            get { return new Comparison<T>(Compare); }
        }

        public int Compare(T x, T y)
        {
            WasCalled = true;
            return Comparer<T>.Default.Compare(x, y);
        }
    }
}
