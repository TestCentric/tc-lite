// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;

namespace TCLite.Constraints
{
    /// <summary><see cref="CollectionTally"/> counts (tallies) the number of occurrences 
    /// of each object in one or more enumerations.</summary>
    public sealed class CollectionTally
    {
        /// <summary>The result of a <see cref="CollectionTally"/>.</summary>
        public sealed class CollectionTallyResult
        {
            /// <summary>Items that were not in the expected collection.</summary>
            public List<object> ExtraItems { get; }

            /// <summary>Items that were not accounted for in the expected collection.</summary>
            public List<object> MissingItems { get; }

            /// <summary>Initializes a new instance of the <see cref="CollectionTallyResult"/> class with the given fields.</summary>
            public CollectionTallyResult(List<object> missingItems, List<object> extraItems)
            {
                MissingItems = missingItems;
                ExtraItems = extraItems;
            }
        }

        private readonly TCLiteEqualityComparer _comparer;

        /// <summary>The result of the comparison between the two collections.</summary>
        public CollectionTallyResult Result
        {
            get
            {
                return new CollectionTallyResult(
                    new List<object>(_missingItems),
                    new List<object>(_extraItems));
            }
        }

        private readonly List<object> _missingItems = new List<object>();

        private readonly List<object> _extraItems = new List<object>();

        /// <summary>Construct a CollectionTally object from a comparer and a collection.</summary>
        /// <param name="comparer">The comparer to use for equality.</param>
        /// <param name="c">The expected collection to compare against.</param>
        public CollectionTally(TCLiteEqualityComparer comparer, IEnumerable c)
        {
            _comparer = comparer;

            foreach (object o in c)
                _missingItems.Add(o);
        }

        private bool ItemsEqual(object expected, object actual)
        {
            Tolerance tolerance = Tolerance.Default;
            return _comparer.AreEqual(expected, actual, ref tolerance);
        }

        /// <summary>Try to remove an object from the tally.</summary>
        /// <param name="o">The object to remove.</param>
        public void TryRemove(object o)
        {
            for (int index = _missingItems.Count - 1; index >= 0; index--)
            {
                if (ItemsEqual(_missingItems[index], o))
                {
                    _missingItems.RemoveAt(index);
                    return;
                }
            }

            _extraItems.Add(o);
        }

        /// <summary>Try to remove a set of objects from the tally.</summary>
        /// <param name="c">The objects to remove.</param>
        public void TryRemove(IEnumerable c)
        {
            foreach (object o in c)
                TryRemove(o);
        }
    }
}
