// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite.TestUtilities
{
    public class SimpleObjectComparer : IComparer
    {
        public bool Called;

        public int Compare(object x, object y)
        {
            Called = true;
            return System.Collections.Generic.Comparer<object>.Default.Compare(x, y);
            //return System.Collections.Comparer.Default.Compare(x, y);
        }
    }
}
