// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="IEnumerable"/>s.
    /// </summary>
    internal sealed class EnumerablesComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal EnumerablesComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1, T2>(T1 x, T2 y)
        {
            return x is IEnumerable && y is IEnumerable;
        }

        public bool AreEqual<T1, T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x, y), "Unexpected call");

            var xEnumerable = x as IEnumerable;
            var yEnumerable = y as IEnumerable;

            if (_equalityComparer.CheckRecursion(xEnumerable, yEnumerable))
                return false;

            var expectedEnum = xEnumerable.GetEnumerator();
            using (expectedEnum as IDisposable)
            {
                var actualEnum = yEnumerable.GetEnumerator();
                using (actualEnum as IDisposable)
                {
                    for (int count = 0; ; count++)
                    {
                        bool expectedHasData = expectedEnum.MoveNext();
                        bool actualHasData = actualEnum.MoveNext();

                        if (!expectedHasData && !actualHasData)
                            return true;

                        if (expectedHasData != actualHasData ||
                            !_equalityComparer.ObjectsEqual(expectedEnum.Current, actualEnum.Current, ref tolerance))
                        {
                            FailurePoint fp = new FailurePoint();
                            fp.Position = count;
                            fp.ExpectedHasData = expectedHasData;
                            if (expectedHasData)
                                fp.ExpectedValue = expectedEnum.Current;
                            fp.ActualHasData = actualHasData;
                            if (actualHasData)
                                fp.ActualValue = actualEnum.Current;
                            _equalityComparer.FailurePoints.Insert(0, fp);
                            return false;
                        }
                    }
                }
            }
        }
    }
}
