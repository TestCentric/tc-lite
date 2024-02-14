// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Internal;

namespace TCLite.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <c>ValueTuple</c>s.
    /// </summary>
    internal sealed class ValueTupleComparer : TupleComparerBase
    {
        internal ValueTupleComparer(TCLiteEqualityComparer equalityComparer)
            : base(equalityComparer)
        { }

        protected override bool IsCorrectType(Type type)
        {
            return TypeHelper.IsValueTuple(type);
        }

        protected override object GetValue(Type type, string propertyName, object obj)
        {
            return type.GetField(propertyName).GetValue(obj);
        }
    }
}
