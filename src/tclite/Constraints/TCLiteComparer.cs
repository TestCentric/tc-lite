// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// NUnitComparer encapsulates NUnit's default behavior
    /// in comparing two objects.
    /// </summary>
    public class TCLiteComparer : IComparer
    {
        /// <summary>
        /// Returns the default NUnitComparer.
        /// </summary>
        public static TCLiteComparer Default
        {
            get { return new TCLiteComparer(); }
        }

        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (x == null)
                return y == null ? 0 : -1;
            else if (y == null)
                return +1;

            if (Numerics.IsNumericType(x) && Numerics.IsNumericType(y))
                return Numerics.Compare(x, y);

            if (x is IComparable)
                return ((IComparable)x).CompareTo(y);

            if (y is IComparable)
                return -((IComparable)y).CompareTo(x);

            Type xType = x.GetType();
            Type yType = y.GetType();

            MethodInfo method = xType.GetMethod("CompareTo", new Type[] { yType });
            if (method != null)
                return (int)method.Invoke(x, new object[] { y });

            method = yType.GetMethod("CompareTo", new Type[] { xType });
            if (method != null)
                return -(int)method.Invoke(y, new object[] { x });

            throw new ArgumentException("Neither value implements IComparable or IComparable<T>");
        }
    }
}