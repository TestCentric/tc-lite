// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using TCLite.Internal;
using TCLite.TestUtilities;

namespace TCLite.Constraints
{
    [TestFixture]
    public class EqualConstraintTests
    {
        private static readonly string NL = Environment.NewLine;
        private const string DESCRIPTION = "4";
        private const string STRING_REPRESENTATION = "<equal 4>";

        private EqualConstraint<int> _constraint = Is.EqualTo(4);

        internal static object[] SuccessData => new object[] { 4, 4.0f, 4.0d, 4.0000m };

        internal static object[] FailureData = new object[] { 
            new TestCaseData( 5, "5" ), 
            //new TestCaseData( null, "null" ),
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( double.NaN, "NaN" ),
            new TestCaseData( double.PositiveInfinity, "Infinity" ) };

        [TestCaseData(nameof(SuccessData))]
        public void ApplyConstraintSucceeds<T>(T actual)
        {
            Assert.That(_constraint.ApplyTo(actual).IsSuccess);
        }

        [TestCaseData(nameof(FailureData))]
        public void ApplyConstraintFails<T>(T actual, string message)
        {
            Assert.IsFalse(_constraint.ApplyTo(actual).IsSuccess);
        }

        [TestCase(float.NaN)]
        [TestCase(float.PositiveInfinity)]
        [TestCase(float.NegativeInfinity)]
        public void CanMatchSpecialFloatValues(float value)
        {
            Assert.That(value, Is.EqualTo(value));
        }

        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void CanMatchSpecialDoubleValues(double value)
        {
            Assert.That(value, Is.EqualTo(value));
        }

        [TestCase]
        public void CanMatchDates()
        {
            DateTime expected = new DateTime(2007, 4, 1);
            DateTime actual = new DateTime(2007, 4, 1);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase]
        public void CanMatchDatesWithinTimeSpan()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 1, 13, 1, 0);
            TimeSpan tolerance = TimeSpan.FromMinutes(5.0);
            Assert.That(actual, Is.EqualTo(expected).Within(tolerance));
        }

        [TestCase]
        public void CanMatchDatesWithinDays()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 4, 13, 0, 0);
            Assert.That(actual, Is.EqualTo(expected).Within(5).Days);
        }

        [TestCase]
        public void CanMatchDatesWithinHours()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 1, 16, 0, 0);
            Assert.That(actual, Is.EqualTo(expected).Within(5).Hours);
        }

        [TestCase]
        public void CanMatchDatesWithinMinutes()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 1, 13, 1, 0);
            Assert.That(actual, Is.EqualTo(expected).Within(5).Minutes);
        }

        [TestCase]
        public void CanMatchTimeSpanWithinMinutes()
        {
            TimeSpan expected = new TimeSpan(10, 0, 0);
            TimeSpan actual = new TimeSpan(10, 2, 30);
            Assert.That(actual, Is.EqualTo(expected).Within(5).Minutes);
        }

        [TestCase]
        public void CanMatchDatesWithinSeconds()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 1, 13, 1, 0);
            Assert.That(actual, Is.EqualTo(expected).Within(300).Seconds);
        }

        [TestCase]
        public void CanMatchDatesWithinMilliseconds()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 1, 13, 1, 0);
            Assert.That(actual, Is.EqualTo(expected).Within(300000).Milliseconds);
        }

        [TestCase]
        public void CanMatchDatesWithinTicks()
        {
            DateTime expected = new DateTime(2007, 4, 1, 13, 0, 0);
            DateTime actual = new DateTime(2007, 4, 1, 13, 1, 0);
            Assert.That(actual, Is.EqualTo(expected).Within(TimeSpan.TicksPerMinute * 5).Ticks);
        }

        [TestCase]
        public void CanMatchStrings()
        {
            Assert.That("hello", Is.EqualTo("hello"));
        }

        [TestCase]
        public void CanMatchStringsIgnoringCase()
        {
            Assert.That("Hello", Is.EqualTo("HELLO").IgnoreCase);
        }

        #region Dictionary Tests

        // TODO: Move these to a separate fixture
        [TestCase]
        public void CanMatchHashtables_SameOrder()
        {
            Assert.AreEqual(new Hashtable { { 0, 0 }, { 1, 1 }, { 2, 2 } },
                            new Hashtable { { 0, 0 }, { 1, 1 }, { 2, 2 } });
        }

        [TestCase]
        public void CanMatchHashtables_Failure()
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.AreEqual(
                    new Hashtable { { 0, 0 }, { 1, 1 }, { 2, 2 } },
                    new Hashtable { { 0, 0 }, { 1, 5 }, { 2, 2 } });
            });
        }

        [TestCase]
        public void CanMatchHashtables_DifferentOrder()
        {
            Assert.AreEqual(new Hashtable { { 0, 0 }, { 1, 1 }, { 2, 2 } },
                            new Hashtable { { 0, 0 }, { 2, 2 }, { 1, 1 } });
        }

        [TestCase]
        public void CanMatchDictionaries_SameOrder()
        {
            Assert.AreEqual(new Dictionary<int, int> { { 0, 0 }, { 1, 1 }, { 2, 2 } },
                            new Dictionary<int, int> { { 0, 0 }, { 1, 1 }, { 2, 2 } });
        }

        [TestCase]
        public void CanMatchDictionaries_Failure()
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.AreEqual(
                    new Dictionary<int, int> { { 0, 0 }, { 1, 1 }, { 2, 2 } },
                    new Dictionary<int, int> { { 0, 0 }, { 1, 5 }, { 2, 2 } });
            });
        }

        [TestCase]
        public void CanMatchDictionaries_DifferentOrder()
        {
            Assert.AreEqual(new Dictionary<int, int> { { 0, 0 }, { 1, 1 }, { 2, 2 } },
                            new Dictionary<int, int> { { 0, 0 }, { 2, 2 }, { 1, 1 } });
        }

        [TestCase]
        public void CanMatchHashtableWithDictionary()
        {
            // With classic assert, we need to tell the compiler what type to use
            Assert.AreEqual(new Hashtable { { 0, 0 }, { 1, 1 }, { 2, 2 } },
                            new Dictionary<int, int> { { 0, 0 }, { 2, 2 }, { 1, 1 } });

            // With Assert.That, we don't 
            Assert.That(new Dictionary<int, int> { { 0, 0 }, { 2, 2 }, { 1, 1 } },
                Is.EqualTo(new Hashtable { { 0, 0 }, { 1, 1 }, { 2, 2 } }));
        }

        #endregion

        [TestCase(20000000000000004.0)]
        [TestCase(19999999999999996.0)]
        public void CanMatchDoublesWithUlpTolerance(double value)
        {
            Assert.That(value, Is.EqualTo(20000000000000000.0).Within(1).Ulps);
        }

        [TestCase(20000000000000008.0)]
        [TestCase(19999999999999992.0)]
        public void FailsOnDoublesOutsideOfUlpTolerance(double value)
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.That(value, Is.EqualTo(20000000000000000.0).Within(1).Ulps);
            });
        }

        [TestCase(19999998.0f)]
        [TestCase(20000002.0f)]
        public void CanMatchSinglesWithUlpTolerance(float value)
        {
            Assert.That(value, Is.EqualTo(20000000.0f).Within(1).Ulps);
        }

        [TestCase(19999996.0f)]
        [TestCase(20000004.0f)]
        public void FailsOnSinglesOutsideOfUlpTolerance(float value)
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.That(value, Is.EqualTo(20000000.0f).Within(1).Ulps);
            });
        }

        [TestCase(9500.0)]
        [TestCase(10000.0)]
        [TestCase(10500.0)]
        public void CanMatchDoublesWithRelativeTolerance(double value)
        {
            Assert.That(value, Is.EqualTo(10000.0).Within(10.0).Percent);
        }

        [TestCase(8500.0)]
        [TestCase(11500.0)]
        public void FailsOnDoublesOutsideOfRelativeTolerance(double value)
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.That(value, Is.EqualTo(10000.0).Within(10.0).Percent);
            });
        }

        [TestCase(9500.0f)]
        [TestCase(10000.0f)]
        [TestCase(10500.0f)]
        public void CanMatchSinglesWithRelativeTolerance(float value)
        {
            Assert.That(value, Is.EqualTo(10000.0f).Within(10.0f).Percent);
        }

        [TestCase(8500.0f)]
        [TestCase(11500.0f)]
        public void FailsOnSinglesOutsideOfRelativeTolerance(float value)
        {
            Assert.Throws<AssertionException>(() =>
            {
                Assert.That(value, Is.EqualTo(10000.0f).Within(10.0f).Percent);
            });
        }

        /// <summary>Applies both the Percent and Ulps modifiers to cause an exception</summary>
        [TestCase]
        public void ErrorWithPercentAndUlpsToleranceModes()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var shouldFail = Is.EqualTo(100.0f).Within(10.0f).Percent.Ulps;
            });
        }

        /// <summary>Applies both the Ulps and Percent modifiers to cause an exception</summary>
        [TestCase]
        public void ErrorWithUlpsAndPercentToleranceModes()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var shouldFail = Is.EqualTo(100.0f).Within(10.0f).Ulps.Percent;
            });
        }

        [TestCase]
        public void ErrorIfPercentPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Assert.That(1010, Is.EqualTo(1000).Percent.Within(5));
            });
        }

        [TestCase]
        public void ErrorIfUlpsPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Assert.That(1010.0, Is.EqualTo(1000.0).Ulps.Within(5));
            });
        }

        [TestCase]
        public void ErrorIfDaysPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Assert.That(DateTime.Now, Is.EqualTo(DateTime.Now).Days.Within(5));
            });
        }

        [TestCase]
        public void ErrorIfHoursPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(
                () => Assert.That(DateTime.Now, Is.EqualTo(DateTime.Now).Hours.Within(5)));
        }

        [TestCase]
        public void ErrorIfMinutesPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(
                () => Assert.That(DateTime.Now, Is.EqualTo(DateTime.Now).Minutes.Within(5)));
        }

        [TestCase]
        public void ErrorIfSecondsPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                Assert.That(DateTime.Now, Is.EqualTo(DateTime.Now).Seconds.Within(5));
            });
        }

        [TestCase]
        public void ErrorIfMillisecondsPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(
                () => Assert.That(DateTime.Now, Is.EqualTo(DateTime.Now).Milliseconds.Within(5)));
        }

        [TestCase]
        public void ErrorIfTicksPrecedesWithin()
        {
            Assert.Throws<InvalidOperationException>(
                () => Assert.That(DateTime.Now, Is.EqualTo(DateTime.Now).Ticks.Within(5)));
        }

        [TestCase(1000, 1010)]
        [TestCase(1000U, 1010U)]
        [TestCase(1000L, 1010L)]
        [TestCase(1000UL, 1010UL)]
        public void ErrorIfUlpsIsUsedOnIntegralType(object x, object y)
        {
            Assert.Throws<InvalidOperationException>(
                () => Assert.That(y, Is.EqualTo(x).Within(2).Ulps));
        }

        [TestCase]
        public void ErrorIfUlpsIsUsedOnDecimal()
        {
            Assert.Throws<InvalidOperationException>(
                () => Assert.That(100m, Is.EqualTo(100m).Within(2).Ulps));
        }

        [TestCase]
        public void UsesProvidedIComparer()
        {
            SimpleObjectComparer comparer = new SimpleObjectComparer();
            Assert.That(2 + 2, Is.EqualTo(4).Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [TestCase]
        public void UsesProvidedEqualityComparer()
        {
            SimpleEqualityComparer comparer = new SimpleEqualityComparer();
            Assert.That(2 + 2, Is.EqualTo(4).Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [TestCase]
        public void UsesProvidedEqualityComparerOfT()
        {
            SimpleEqualityComparer<int> comparer = new SimpleEqualityComparer<int>();
            Assert.That(2 + 2, Is.EqualTo(4).Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [TestCase]
        public void UsesProvidedComparerOfT()
        {
            SimpleEqualityComparer<int> comparer = new SimpleEqualityComparer<int>();
            Assert.That(2 + 2, Is.EqualTo(4).Using(comparer));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        [TestCase]
        public void UsesProvidedComparisonOfT()
        {
            MyComparison<int> comparer = new MyComparison<int>();
            Assert.That(2 + 2, Is.EqualTo(4).Using(new Comparison<int>(comparer.Compare)));
            Assert.That(comparer.Called, "Comparer was not called");
        }

        class MyComparison<T>
        {
            public bool Called;

            public int Compare(T x, T y)
            {
                Called = true;
                return Comparer<T>.Default.Compare(x, y);
            }
        }

        [TestCase]
        public void UsesProvidedLambda_IntArgs()
        {
            Assert.That(2 + 2, Is.EqualTo(4).Using<int>((x, y) => x.CompareTo(y)));
        }

        [TestCase]
        public void UsesProvidedLambda_StringArgs()
        {
            Assert.That("hello", Is.EqualTo("HELLO").Using<string>((x, y) => x.ToLower().CompareTo(y.ToLower())));
        }

        [TestCase]
        public void UsesProvidedListComparer()
        {
            var list1 = new List<int>() { 2, 3 };
            var list2 = new List<int>() { 3, 4 };

            var list11 = new List<List<int>>() { list1 };
            var list22 = new List<List<int>>() { list2 };
            var comparer = new IntListEqualComparer();

            Assert.That(list11, Is.EqualTo(list22).Using(comparer));
        }

        public class IntListEqualComparer : IEqualityComparer<List<int>>
        {
            public bool Equals(List<int> x, List<int> y)
            {
                return x.Count == y.Count;
            }
 
            public int GetHashCode(List<int> obj)
            {
                return obj.Count.GetHashCode();
            }
        }

        [TestCase]
        public void UsesProvidedArrayComparer()
        {
            var array1 = new int[] { 2, 3 };
            var array2 = new int[] { 3, 4 };

            var list11 = new List<int[]>() { array1 };
            var list22 = new List<int[]>() { array2 };
            var comparer = new IntArrayEqualComparer();

            Assert.That(list11, Is.EqualTo(list22).Using(comparer));
            //Assert.That(list11, new CollectionEquivalentConstraint(list22).Using(comparer));
        }

        public class IntArrayEqualComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[] x, int[] y)
            {
                return x.Length == y.Length;
            }

            public int GetHashCode(int[] obj)
            {
                return obj.Length.GetHashCode();
            }
        }
    }
}
