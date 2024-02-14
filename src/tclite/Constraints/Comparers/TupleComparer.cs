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
    /// Comparator for two <c>Tuple</c>s.
    /// </summary>
    internal sealed class TupleComparer : TupleComparerBase
    {
        internal TupleComparer(TCLiteEqualityComparer equalityComparer)
            : base(equalityComparer)
        { }

        protected override bool IsCorrectType(Type type)
        {
            return TypeHelper.IsTuple(type);
        }

        protected override object GetValue(Type type, string propertyName, object obj)
        {
            return type.GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
