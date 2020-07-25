// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Base class for comparators for tuples (both regular Tuples and ValueTuples).
    /// </summary>
    internal abstract class TupleComparerBase : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal TupleComparerBase(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        protected abstract bool IsCorrectType(Type type);

        protected abstract object GetValue(Type type, string propertyName, object obj);

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            return (IsCorrectType(typeof(T1)) && IsCorrectType(typeof(T2)));
        }

        public bool AreEqual<T1,T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Type xType = x.GetType();
            Type yType = y.GetType();

            if (!IsCorrectType(xType) || !IsCorrectType(yType))
                return false;

            int numberOfGenericArgs = xType.GetGenericArguments().Length;

            if (numberOfGenericArgs != yType.GetGenericArguments().Length)
                return false;

            for (int i = 0; i < numberOfGenericArgs; i++)
            {
                string propertyName = i < 7 ? "Item" + (i + 1) : "Rest";
                object xItem = GetValue(xType, propertyName, x);
                object yItem = GetValue(yType, propertyName, y);

                bool comparison = _equalityComparer.AreEqual(xItem, yItem, ref tolerance);
                if (!comparison)
                    return false;
            }

            return true;
        }
    }
}
