// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TCLite.Framework.Internal;
using TCLite.TestUtilities.Collections;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    class CollectionEqualsTests
    {
        [TestCase]
        public void CanMatchTwoCollections()
        {
            ICollection expected = new SimpleObjectCollection(1, 2, 3);
            ICollection actual = new SimpleObjectCollection(1, 2, 3);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase]
        public void CanMatchTwoLists()
        {
            IList expected = new List<int> { 1, 2, 3 };
            IList actual = new List<int> { 1, 2, 3 };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase]
        public void CanMatchAnArrayWithACollection()
        {
            ICollection collection = new SimpleObjectCollection(1, 2, 3);
            int[] array = new int[] { 1, 2, 3 };

            Assert.That(collection, Is.EqualTo(array));
            Assert.That(array, Is.EqualTo(collection));
        }

        [TestCase]
        public void FailureForEnumerablesWithDifferentSizes()
        {
            IEnumerable<int> expected = new int[] { 1, 2, 3 }.Select(i => i);
            IEnumerable<int> actual = expected.Take(2);

            var ex = Assert.Throws<AssertionException>(() => Assert.That(actual, Is.EqualTo(expected)));
            Assert.That(ex.Message, Is.EqualTo(
                $"  Expected is {MsgUtils.GetTypeRepresentation(expected)}, actual is {MsgUtils.GetTypeRepresentation(actual)}" + Environment.NewLine));
#if NYI // Extra lines
                "  Values differ at index [2]" + Environment.NewLine +
                "  Missing:  < 3, ... >"));
#endif
        }

        [TestCase]
        public void FailureMatchingArrayAndCollection()
        {
            int[] expected = new int[] { 1, 2, 3 };
            ICollection actual = new SimpleObjectCollection(1, 5, 3);

            var ex = Assert.Throws<AssertionException>(() => Assert.That(actual, Is.EqualTo(expected)));
            Assert.That(ex.Message, Is.EqualTo(
                "  Expected is <System.Int32[3]>, actual is <TCLite.TestUtilities.Collections.SimpleObjectCollection> with 3 elements" + Environment.NewLine));
#if NYI // Extra lines
                "  Values differ at index [1]" + Environment.NewLine +
                TextMessageWriter.Pfx_Expected + "2" + Environment.NewLine +
                TextMessageWriter.Pfx_Actual + "5" + Environment.NewLine));
#endif
        }

        [TestCaseSource(nameof(IgnoreCaseData))]
        public void HonorsIgnoreCase(IEnumerable expected, IEnumerable actual)
        {
            Assert.That(expected, Is.EqualTo(actual).IgnoreCase);
        }

        private static readonly object[] IgnoreCaseData =
        {
            new object[] {new SimpleObjectCollection("x", "y", "z"),new SimpleObjectCollection("x", "Y", "Z")},
            new object[] {new[] {'A', 'B', 'C'}, new object[] {'a', 'b', 'c'}},
            new object[] {new[] {"a", "b", "c"}, new object[] {"A", "B", "C"}},
            new object[] {new Dictionary<int, string> {{ 1, "a" }}, new Dictionary<int, string> {{ 1, "A" }}},
            new object[] {new Dictionary<int, char> {{ 1, 'A' }}, new Dictionary<int, char> {{ 1, 'a' }}},
            new object[] {new List<char> {'A', 'B', 'C'}, new List<char> {'a', 'b', 'c'}},
            new object[] {new List<string> {"a", "b", "c"}, new List<string> {"A", "B", "C"}},
        };
    }
}
