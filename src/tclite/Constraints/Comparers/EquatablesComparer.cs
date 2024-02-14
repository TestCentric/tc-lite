// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;

namespace TCLite.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two types related by <see cref="IEquatable{T}"/>.
    /// </summary>
    internal sealed class EquatablesComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal EquatablesComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            return !_equalityComparer.CompareAsCollection && (x is IEquatable<T2> || y is IEquatable<T1>);
        }

        public bool AreEqual<T1,T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Type xType = x.GetType();
            Type yType = y.GetType();

            MethodInfo equals = FirstImplementsIEquatableOfSecond(xType, yType);
            if (equals != null)
                return InvokeFirstIEquatableEqualsSecond(x, y, equals);

            equals = FirstImplementsIEquatableOfSecond(yType, xType);
            if (xType != yType && equals != null)
                return InvokeFirstIEquatableEqualsSecond(y, x, equals);

            throw new InvalidOperationException("Invalid call");
        }

        private static MethodInfo FirstImplementsIEquatableOfSecond(Type first, Type second)
        {
            var pair = new KeyValuePair<Type, MethodInfo>();

            foreach (var xEquatableArgument in GetEquatableGenericArguments(first))
                if (xEquatableArgument.Key.IsAssignableFrom(second))
                    if (pair.Key == null || pair.Key.IsAssignableFrom(xEquatableArgument.Key))
                        pair = xEquatableArgument;

            return pair.Value;
        }

        private static IList<KeyValuePair<Type, MethodInfo>> GetEquatableGenericArguments(Type type)
        {
            var genericArgs = new List<KeyValuePair<Type, MethodInfo>>();

            foreach (Type @interface in type.GetInterfaces())
            {
                if (@interface.GetTypeInfo().IsGenericType && @interface.GetGenericTypeDefinition().Equals(typeof(IEquatable<>)))
                {
                    genericArgs.Add(new KeyValuePair<Type, MethodInfo>(
                        @interface.GetGenericArguments()[0], @interface.GetMethod("Equals")));
                }
            }

            return genericArgs;
        }

        private static bool InvokeFirstIEquatableEqualsSecond(object first, object second, MethodInfo equals)
        {
            return equals != null ? (bool)equals.Invoke(first, new object[] { second }) : false;
        }
    }
}
