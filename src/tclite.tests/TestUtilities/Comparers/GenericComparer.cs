// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TCLite.TestUtilities.Comparers
{
    /// <summary>
    /// GenericComparer is used in testing to ensure that only
    /// the <see cref="IComparer{T}"/> interface is used.
    /// </summary>
    public class GenericComparer<T> : IComparer<T>
    {
        public bool WasCalled = false;

        int IComparer<T>.Compare(T x, T y)
        {
            WasCalled = true;
            return Comparer<T>.Default.Compare(x, y);
        }
    }
}
