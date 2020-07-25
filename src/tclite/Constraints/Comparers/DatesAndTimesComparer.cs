// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints.Comparers
{
    /// <summary>
    /// Comparator for two <see cref="DateTime"/>s, 
    /// <see cref="TimeSpans"/>s or <see cref="DateTimeOffset"/>s.
    /// </summary>
    internal sealed class DatesAndTimesComparer : ITCLiteEqualityComparer
    {
        private readonly TCLiteEqualityComparer _equalityComparer;

        internal DatesAndTimesComparer(TCLiteEqualityComparer equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool CanCompare<T1,T2>(T1 x, T2 y)
        {
            return x is DateTime && y is DateTime
                || x is TimeSpan && y is TimeSpan
                || x is DateTimeOffset && y is DateTimeOffset;
        }

        public bool AreEqual<T1,T2>(T1 x, T2 y, ref Tolerance tolerance)
        {
            Guard.OperationValid(CanCompare(x,y), "Invalid call");

            if (x is DateTimeOffset && y is DateTimeOffset)
                return AreEqual(ToDateTimeOffset(x), ToDateTimeOffset(y), tolerance);

            if (x is DateTime && y is DateTime)
                return AreEqual(ToDateTime(x), ToDateTime(y), tolerance);

            if (x is TimeSpan && y is TimeSpan)
                return AreEqual(ToTimeSpan(x), ToTimeSpan(y), tolerance);

            throw new InvalidOperationException("Invalid call");
        }

        private bool AreEqual(DateTimeOffset x, DateTimeOffset y, Tolerance tolerance)
        {
            return tolerance.Amount is TimeSpan
                ? (x - y).Duration() <= (TimeSpan)tolerance.Amount
                : x.Equals(y);
        }

        private bool AreEqual(DateTime x, DateTime y, Tolerance tolerance)
        {
            return tolerance.Amount is TimeSpan
                ? (x - y).Duration() <= (TimeSpan)tolerance.Amount
                : x.Equals(y);
        }

        private bool AreEqual(TimeSpan x, TimeSpan y, Tolerance tolerance)
        {
            return tolerance.Amount is TimeSpan
                ? (x - y).Duration() <= (TimeSpan)tolerance.Amount
                : x.Equals(y);
        }

        private DateTimeOffset ToDateTimeOffset<T>(T x)
        {
            return (DateTimeOffset)Convert.ChangeType(x, typeof(DateTimeOffset));
        }

        private DateTime ToDateTime<T>(T x)
        {
            return Convert.ToDateTime(x);
        }

        private TimeSpan ToTimeSpan<T>(T x)
        {
            return (TimeSpan)Convert.ChangeType(x, typeof(TimeSpan));
        }
#if NYI
            if (result && _equalityComparer.WithSameOffset)
            {
                result = xOffset.Offset == yOffset.Offset;
            }
#endif
    }
}
