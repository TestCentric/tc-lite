// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;
using TCLite.TestUtilities.Comparers;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class AllItemsConstraintTests
    {
        private readonly string NL = Environment.NewLine;

        [TestCase]
        public void AllItemsAreNotNull()
        {
            object[] c = new object[] { 1, "hello", 3, Environment.NewLine };
            Assert.That(c, new AllItemsConstraint(new NotConstraint(new NullConstraint())));
        }

        [TestCase]
        public void AllItemsAreNotNullFails()
        {
            object[] c = new object[] { 1, "hello", null, 3 };
            var expectedMessage =
                TextMessageWriter.Pfx_Expected + "all items not null" + NL +
                TextMessageWriter.Pfx_Actual + "< 1, \"hello\", null, 3 >" + NL;
                // + "  First non-matching item at index [2]:  null" + NL;
            var ex = Assert.Throws<AssertionException>(() => 
                Assert.That(c, new AllItemsConstraint(new NotConstraint(new NullConstraint()))));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [TestCase]
        public void AllItemsAreInRange()
        {
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint<int>(10, 100)));
        }

        [TestCase]
        public void AllItemsAreInRange_UsingIComparer()
        {
            var comparer = new ObjectComparer();
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, Is.All.InRange(10, 100).Using(comparer));
            Assert.That(comparer.WasCalled);
        }

        [TestCase]
        public void AllItemsAreInRange_UsingGenericComparer()
        {
            var comparer = new GenericComparer<int>();
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, Is.All.InRange(10, 100).Using(comparer));
            Assert.That(comparer.WasCalled);
        }

        [TestCase]
        public void AllItemsAreInRange_UsingGenericComparison()
        {
            var comparer = new GenericComparison<int>();
            int[] c = new int[] { 12, 27, 19, 32, 45, 99, 26 };
            Assert.That(c, new AllItemsConstraint(new RangeConstraint<int>(10, 100).Using(comparer.Delegate)));
            Assert.That(comparer.WasCalled);
        }

#if NYI // Extra Line
        [TestCase]
        public void AllItemsAreInRangeFailureMessage()
        {
            int[] c = new int[] { 12, 27, 19, 32, 107, 99, 26 };
            var expectedMessage =
                TextMessageWriter.Pfx_Expected + "all items in range (10,100)" + NL +
                TextMessageWriter.Pfx_Actual + "< 12, 27, 19, 32, 107, 99, 26 >" + NL +
                "  First non-matching item at index [4]:  107" + NL;
            var ex = Assert.Throws<AssertionException>(() => Assert.That(c, new AllItemsConstraint(new RangeConstraint<int>(10, 100))));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }
#endif

        [TestCase]
        public void AllItemsAreInstancesOfType()
        {
            object[] c = new object[] { 'a', 'b', 'c' };
            Assert.That(c, Is.All.InstanceOf<char>());
        }

        [TestCase]
        public void AllItemsAreInstancesOfTypeFailureMessage()
        {
            object[] c = new object[] { 'a', "b", 'c' };
            var expectedMessage =
                TextMessageWriter.Pfx_Expected + "all items instance of <System.Char>" + NL +
                TextMessageWriter.Pfx_Actual + "< 'a', \"b\", 'c' >" + NL;
                //+ "  First non-matching item at index [1]:  \"b\"" + NL;
            var ex = Assert.Throws<AssertionException>(() =>
                Assert.That(c, Is.All.InstanceOf<char>()));
            Assert.That(ex.Message, Is.EqualTo(expectedMessage));
        }

        [TestCase]
        public void WorksOnICollection()
        {
            var c = new TCLite.TestUtilities.Collections.SimpleObjectCollection(1, 2, 3);
            Assert.That(c, Is.All.Not.Null);
        }

        [TestCase]
        public void FailsWhenNotUsedAgainstAnEnumerable()
        {
            var notEnumerable = 42;
            TestDelegate act = () => Assert.That(notEnumerable, Is.All.InRange(10, 100));
            Assert.That(act, Throws.ArgumentException);
#if NYI // Message
                .With.Message.Contains("IEnumerable"));
#endif
        }
    }
}
