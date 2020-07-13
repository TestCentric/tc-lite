// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints
{
    public static class ComparisonAdapterTests
    {
        public static IEnumerable<ComparisonAdapter> ComparisonAdapters()
        {
            return new[]
            {
                ComparisonAdapter.Default,
                ComparisonAdapter.For((IComparer)StringComparer.Ordinal),
                ComparisonAdapter.For((IComparer<string>)StringComparer.Ordinal),
                ComparisonAdapter.For<string>(StringComparer.Ordinal.Compare)
            };
        }

#if NYI
        [TestCaseSource(nameof(ComparisonAdapters))]
        public static void CanCompareWithNull(ComparisonAdapter adapter)
        {
            Assert.That(adapter.Compare(null, "a"), Is.LessThan(0));
            Assert.That(adapter.Compare("a", null), Is.GreaterThan(0));
            Assert.That(adapter.Compare(null, null), Is.Zero);
        }
#endif
    }
}
