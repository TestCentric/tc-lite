// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace TCLite.Constraints
{
    using Comparers;

    /// <summary>
    /// NUnitEqualityComparer encapsulates NUnit's handling of
    /// equality tests between objects.
    /// </summary>
    public class TCLiteEqualityComparer
    {
        private const int BUFFER_SIZE = 4096;

        private ITCLiteEqualityComparer[] _comparers;

        /// <summary>
        /// RecursionDetector used to check for recursion when
        /// evaluating self-referencing enumerables.
        /// </summary>
        private RecursionDetector _recursionDetector;

        public TCLiteEqualityComparer()
        {
            var enumerablesComparer = new EnumerablesComparer(this);

            _comparers = new ITCLiteEqualityComparer[]
            {
                new ArraysComparer(this, enumerablesComparer),
                new DictionariesComparer(this),
                new DictionaryEntriesComparer(this),
                new KeyValuePairsComparer(this),
                new StringsComparer(this),
                new StreamsComparer(this),
                new CharsComparer(this),
                new NumericsComparer(),
                new DatesAndTimesComparer(this),
                new TupleComparer(this),
                new ValueTupleComparer(this),
                new EquatablesComparer(this),
                enumerablesComparer
            };
        }

        #region Properties

        /// <summary>
        /// Returns the default NUnitEqualityComparer
        /// </summary>
        public static TCLiteEqualityComparer Default
        {
            get { return new TCLiteEqualityComparer(); }
        }

        /// <summary>
        /// If true, all string comparisons will ignore case
        /// </summary>
        public bool IgnoreCase { get; set; }

        /// <summary>
        /// Gets and sets a flag indicating that arrays should be
        /// compared as collections, without regard to their shape.
        /// </summary>
        public bool CompareAsCollection { get; set; }

        /// <summary>
        /// Gets the list of external comparers to be used to
        /// test for equality. They are applied to members of
        /// collections, in place of NUnit's own logic.
        /// </summary>
        internal IList<EqualityAdapter> ExternalComparers { get; } = new EqualityAdapterList();

        /// <summary>
        /// Gets the list of failure points for the last Match performed.
        /// The list consists of objects to be interpreted by the caller.
        /// This generally means that the caller may only make use of
        /// objects it has placed on the list at a particular depthy.
        /// </summary>
        public IList<FailurePoint> FailurePoints { get;  private set; }

        #endregion

        #region Public Methods

        public bool AreEqual<TExpected,TActual>(TExpected expected, TActual actual, ref Tolerance tolerance)
        {
            _recursionDetector = new RecursionDetector();

            return ObjectsEqual(expected, actual, ref tolerance);
        }

        #endregion

        #region Helper Methods

        internal bool ObjectsEqual<TExpected,TActual>(TExpected expected, TActual actual, ref Tolerance tolerance)
        {
            FailurePoints = new FailurePointList();

            if (expected == null && actual == null)
                return true;

            if (expected == null || actual == null)
                return false;

            if (object.ReferenceEquals(expected, actual))
                return true;

            EqualityAdapter externalComparer = GetExternalComparer(expected, actual);
            if (externalComparer != null)
                return externalComparer.AreEqual(expected, actual);

            ITCLiteEqualityComparer comparer = GetEqualityComparer(expected, actual);
            if (comparer != null)
                return comparer.AreEqual(expected, actual, ref tolerance);

            return expected.Equals(actual);
        }

        private EqualityAdapter GetExternalComparer(object x, object y)
        {
            foreach (EqualityAdapter adapter in ExternalComparers)
                if (adapter.CanCompare(x, y))
                    return adapter;

            return null;
        }

        private ITCLiteEqualityComparer GetEqualityComparer<TExpected,TActual>(TExpected expected, TActual actual)
        {
            foreach (var comparer in _comparers)
                if (comparer.CanCompare(expected, actual))
                    return comparer;

            return null;
        }

        public bool CheckRecursion(IEnumerable expected, IEnumerable actual)
        {
            return _recursionDetector.CheckRecursion(expected, actual);
        }

        private bool EnumerablesEqual(IEnumerable expected, IEnumerable actual, ref Tolerance tolerance)
        {
            if (_recursionDetector.CheckRecursion(expected, actual))
                return false;

            IEnumerator expectedEnum = expected.GetEnumerator();
            IEnumerator actualEnum = actual.GetEnumerator();

            int count;
            for (count = 0; ; count++)
            {
                bool expectedHasData = expectedEnum.MoveNext();
                bool actualHasData = actualEnum.MoveNext();

                if (!expectedHasData && !actualHasData)
                    return true;

                if (expectedHasData != actualHasData ||
                    !ObjectsEqual(expectedEnum.Current, actualEnum.Current, ref tolerance))
                {
                    FailurePoint fp = new FailurePoint();
                    fp.Position = count;
                    fp.ExpectedHasData = expectedHasData;
                    if (expectedHasData)
                        fp.ExpectedValue = expectedEnum.Current;
                    fp.ActualHasData = actualHasData;
                    if (actualHasData)
                        fp.ActualValue = actualEnum.Current;
                    FailurePoints.Insert(0, fp);
                    return false;
                }
            }
        }

        #endregion

        #region Nested RecursionDetector class

        /// <summary>
        /// RecursionDetector detects when a comparison
        /// between two enumerables has reached a point
        /// where the same objects that were previously
        /// compared are again being compared. This allows
        /// the caller to stop the comparison if desired.
        /// </summary>
        class RecursionDetector
        {
            readonly Dictionary<UnorderedReferencePair, object> table = new Dictionary<UnorderedReferencePair, object>();

            /// <summary>
            /// Check whether two objects have previously
            /// been compared, returning true if they have.
            /// The two objects are remembered, so that a
            /// second call will always return true.
            /// </summary>
            public bool CheckRecursion(IEnumerable expected, IEnumerable actual)
            {
                UnorderedReferencePair pair = new UnorderedReferencePair(expected, actual);

                if (ContainsPair(pair))
                    return true;

                table.Add(pair, null);
                return false;
            }

            private bool ContainsPair(UnorderedReferencePair pair)
            {
                return table.ContainsKey(pair);
            }

            class UnorderedReferencePair : IEquatable<UnorderedReferencePair>
            {
                private readonly object _first;
                private readonly object _second;

                public UnorderedReferencePair(object first, object second)
                {
                    _first = first;
                    _second = second;
                }

                public bool Equals(UnorderedReferencePair other)
                {
                    return (Equals(_first, other._first) && Equals(_second, other._second)) ||
                           (Equals(_first, other._second) && Equals(_second, other._first));
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    return obj is UnorderedReferencePair && Equals((UnorderedReferencePair)obj);
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        return ((_first != null ? _first.GetHashCode() : 0) * 397) ^ ((_second != null ? _second.GetHashCode() : 0) * 397);
                    }
                }
            }
        }

        #endregion
    }
}