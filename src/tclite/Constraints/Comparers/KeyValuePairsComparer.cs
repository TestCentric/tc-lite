// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;

namespace TCLite.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="KeyValuePair{TKey, TValue}"/>s.
    /// </summary>
    internal sealed class KeyValuePairsComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal KeyValuePairsComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            var xType = x.GetType();
            var yType = y.GetType();
            if (!xType.IsGenericType || !yType.IsGenericType)
                return false;

            Type xGenericTypeDefinition = xType.GetGenericTypeDefinition();
            Type yGenericTypeDefinition = yType.GetGenericTypeDefinition();

            return xGenericTypeDefinition == typeof(KeyValuePair<,>) &&
                yGenericTypeDefinition == typeof(KeyValuePair<,>);
        }

        public bool AreEqual<T1,T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            var xType = x.GetType();
            var yType = y.GetType();

            // IDictionary<,> will eventually try to compare its key value pairs when using CollectionTally
            var keyTolerance = Tolerance.Exact;
                object xKey = xType.GetProperty("Key").GetValue(x, null);
                object yKey = yType.GetProperty("Key").GetValue(y, null);
                object xValue = xType.GetProperty("Value").GetValue(x, null);
                object yValue = yType.GetProperty("Value").GetValue(y, null);

            return _equalityComparer.AreEqual(xKey, yKey, ref keyTolerance) 
                && _equalityComparer.AreEqual(xValue, yValue, ref tolerance);
        }
    }
}
