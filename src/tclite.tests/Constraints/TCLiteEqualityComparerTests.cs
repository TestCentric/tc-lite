// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class EqualityComparerTests
    {
        private Tolerance tolerance = Tolerance.Default;
        private TCLiteEqualityComparer comparer = new TCLiteEqualityComparer();

        [TestCase(4, 4)]
        [TestCase(4.0d, 4.0d)]
        [TestCase(4.0f, 4.0f)]
        [TestCase(4, 4.0d)]
        [TestCase(4, 4.0f)]
        [TestCase(4.0d, 4)]
        [TestCase(4.0d, 4.0f)]
        [TestCase(4.0f, 4)]
        [TestCase(4.0f, 4.0d)]
        //[TestCase(null, null)]
        [TestCase((char)4, (char)4)]
        // [TestCase((char)4, 4)]
        // [TestCase(4, (char)4)]
        public void EqualItems<T,U>(T x, U y)
        {
            Assert.That(comparer.AreEqual(x, y, ref tolerance));
        }

        [TestCase(4, 2)]
        [TestCase(4.0d, 2.0d)]
        [TestCase(4.0f, 2.0f)]
        [TestCase(4, 2.0d)]
        [TestCase(4, 2.0f)]
        [TestCase(4.0d, 2)]
        [TestCase(4.0d, 2.0f)]
        [TestCase(4.0f, 2)]
        [TestCase(4.0f, 2.0d)]
        [TestCase(4, null)]
        [TestCase((char)4, (char)2)]
        [TestCase((char)4, 2)]
        [TestCase(4, (char)2)]
        public void UnequalItems(object greater, object lesser)
        {
            Assert.IsFalse(comparer.AreEqual(greater, lesser, ref tolerance));
            Assert.IsFalse(comparer.AreEqual(lesser, greater, ref tolerance));
        }

        [TestCase(double.PositiveInfinity, double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity, double.NegativeInfinity)]
        [TestCase(double.NaN, double.NaN)]
        [TestCase(float.PositiveInfinity, float.PositiveInfinity)]
        [TestCase(float.NegativeInfinity, float.NegativeInfinity)]
        [TestCase(float.NaN, float.NaN)]
        public void SpecialFloatingPointValuesCompareAsEqual(object x, object y)
        {
            Assert.That(comparer.AreEqual(x, y, ref tolerance));
        }

#if NYI // Directories
        [Test]
        public void SameDirectoriesAreEqual()
        {
            using (var testDir = new TestDirectory())
            {
                var one = new DirectoryInfo(testDir.Directory.FullName);
                var two = new DirectoryInfo(testDir.Directory.FullName);
                Assert.That(comparer.AreEqual(one, two, ref tolerance));
            }
        }

        [Test]
        public void DifferentDirectoriesAreNotEqual()
        {
            using (var one = new TestDirectory())
            using (var two = new TestDirectory())
            {
                Assert.That(comparer.AreEqual(one, two, ref tolerance), Is.False);
            }
        }
#endif

        [Test]
        public void CanCompareArrayContainingSelfToSelf()
        {
            object[] array = new object[1];
            array[0] = array;

            Assert.IsTrue(comparer.AreEqual(array, array, ref tolerance));
        }

        [Test]
        public void IEquatableSuccess()
        {
            IEquatableWithoutEqualsOverridden x = new IEquatableWithoutEqualsOverridden(1);
            IEquatableWithoutEqualsOverridden y = new IEquatableWithoutEqualsOverridden(1);

            Assert.IsTrue(comparer.AreEqual(x, y, ref tolerance));
        }

        [Test]
        public void IEquatableDifferentTypesSuccess_WhenActualImplementsIEquatable()
        {
            int x = 1;
            Int32IEquatable y = new Int32IEquatable(1);

            // y.Equals(x) is what gets actually called
            // TODO: This should work both ways
            Assert.IsTrue(comparer.AreEqual(x, y, ref tolerance));
        }

        [Test]
        public void IEquatableDifferentTypesSuccess_WhenExpectedImplementsIEquatable()
        {
            int x = 1;
            Int32IEquatable y = new Int32IEquatable(1);

            // y.Equals(x) is what gets actually called
            // TODO: This should work both ways
            Assert.IsTrue(comparer.AreEqual(y, x, ref tolerance));
        }

        [Test]
        public void IEquatableSuccess_WhenExplicitInterfaceImplementation()
        {
            IEquatableWithExplicitImplementation x = new IEquatableWithExplicitImplementation(1);
            IEquatableWithExplicitImplementation y = new IEquatableWithExplicitImplementation(1);

            Assert.IsTrue(comparer.AreEqual(x, y, ref tolerance));
        }

        [Test]
        public void IEquatableFailure_WhenExplicitInterfaceImplementationWithDifferentValues()
        {
            IEquatableWithExplicitImplementation x = new IEquatableWithExplicitImplementation(1);
            IEquatableWithExplicitImplementation y = new IEquatableWithExplicitImplementation(2);

            Assert.IsFalse(comparer.AreEqual(x, y, ref tolerance));
        }

        [Test]
        public void ReferenceEqualityHasPrecedenceOverIEquatable()
        {
            NeverEqualIEquatable z = new NeverEqualIEquatable();

            Assert.IsTrue(comparer.AreEqual(z, z, ref tolerance));
        }

        [Test]
        public void IEquatableHasPrecedenceOverDefaultEquals()
        {
            NeverEqualIEquatableWithOverriddenAlwaysTrueEquals x = new NeverEqualIEquatableWithOverriddenAlwaysTrueEquals();
            NeverEqualIEquatableWithOverriddenAlwaysTrueEquals y = new NeverEqualIEquatableWithOverriddenAlwaysTrueEquals();

            Assert.IsFalse(comparer.AreEqual(x, y, ref tolerance));
        }

        [Test]
        public void IEquatableHasPrecedenceOverEnumerableEquals()
        {
            var x = new EquatableWithEnumerableObject<int>(new[] { 1, 2, 3, 4, 5 }, 42);
            var y = new EnumerableObject<int>(new[] { 5, 4, 3, 2, 1 }, 42);
            var z = new EnumerableObject<int>(new[] { 1, 2, 3, 4, 5 }, 15);

            Assert.That(comparer.AreEqual(x, y, ref tolerance), Is.True);
            Assert.That(comparer.AreEqual(y, x, ref tolerance), Is.True);
            Assert.That(comparer.AreEqual(x, z, ref tolerance), Is.False);
            Assert.That(comparer.AreEqual(z, x, ref tolerance), Is.False);

            // TODO: Reinstate commented cases
            //Assert.That(y, Is.EqualTo(x));
            //Assert.That(x, Is.EqualTo(y));
            //Assert.That(z, Is.Not.EqualTo(x));
            //Assert.That(x, Is.Not.EqualTo(z));
        }

        [Test]
        public void IEquatableIsIgnoredAndEnumerableEqualsUsedWithAsCollection()
        {
            comparer.CompareAsCollection = true;

            var x = new EquatableWithEnumerableObject<int>(new[] { 1, 2, 3, 4, 5 }, 42);
            var y = new EnumerableObject<int>(new[] { 5, 4, 3, 2, 1 }, 42);
            var z = new EnumerableObject<int>(new[] { 1, 2, 3, 4, 5 }, 15);

            Assert.That(comparer.AreEqual(x, y, ref tolerance), Is.False);
            Assert.That(comparer.AreEqual(y, x, ref tolerance), Is.False);
            Assert.That(comparer.AreEqual(x, z, ref tolerance), Is.True);
            Assert.That(comparer.AreEqual(z, x, ref tolerance), Is.True);

            //Assert.That(y, Is.Not.EqualTo(x).AsCollection);
            //Assert.That(x, Is.Not.EqualTo(y).AsCollection);
            Assert.That(z, Is.EqualTo(x).AsCollection);
            Assert.That(x, Is.EqualTo(z).AsCollection);
        }

        [Test]
        public void InheritingAndOverridingIEquatable()
        {
            var obj1 = new InheritingEquatableObject { SomeProperty = 1, OtherProperty = 2 };
            var obj2 = new InheritingEquatableObject { SomeProperty = 1, OtherProperty = 2 };
            var obj3 = new InheritingEquatableObject { SomeProperty = 1, OtherProperty = 3 };
            var obj4 = new InheritingEquatableObject { SomeProperty = 4, OtherProperty = 2 };

            // TODO: Reinstate
            //Assert.That(obj1, Is.EqualTo(obj2));
            Assert.That(obj1, Is.Not.EqualTo(obj3));
            Assert.That(obj1, Is.Not.EqualTo(obj4));

            var n = new TCLiteEqualityComparer();
            var tolerance = Tolerance.Exact;
            Assert.That(n.AreEqual(obj1, obj2, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj2, obj1, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj1, obj3, ref tolerance), Is.False);
            Assert.That(n.AreEqual(obj3, obj1, ref tolerance), Is.False);
            Assert.That(n.AreEqual(obj1, obj4, ref tolerance), Is.False);
            Assert.That(n.AreEqual(obj4, obj1, ref tolerance), Is.False);
        }

        [Test]
        public void ImplementingIEquatableDirectlyOnTheClass()
        {
            var obj1 = new EquatableObject { SomeProperty = 1 };
            var obj2 = new EquatableObject { SomeProperty = 1 };

            var n = new TCLiteEqualityComparer();
            var tolerance = Tolerance.Exact;
            Assert.That(n.AreEqual(obj1, obj2, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj2, obj1, ref tolerance), Is.True);
        }

#if NYI // IEquatable on base class
        [Test]
        public void ImplementingIEquatableOnABaseClassOrInterface()
        {
            var obj1 = new InheritedEquatableObject { SomeProperty = 1 };
            var obj2 = new InheritedEquatableObject { SomeProperty = 1 };

            var n = new TCLiteEqualityComparer();
            var tolerance = Tolerance.Exact;
            Assert.That(n.AreEqual(obj1, obj2, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj2, obj1, ref tolerance), Is.True);
        }
#endif

        [Test]
        public void ImplementingIEquatableOnABaseClassOrInterfaceThroughInterface()
        {
            IEquatableObject obj1 = new InheritedEquatableObject { SomeProperty = 1 };
            IEquatableObject obj2 = new InheritedEquatableObject { SomeProperty = 1 };

            var n = new TCLiteEqualityComparer();
            var tolerance = Tolerance.Exact;
            Assert.That(n.AreEqual(obj1, obj2, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj2, obj1, ref tolerance), Is.True);
        }

        [Test]
        public void CanHandleMultipleImplementationsOfIEquatable()
        {
            IEquatableObject obj1 = new InheritedEquatableObject { SomeProperty = 1 };
            IEquatableObject obj2 = new MultipleIEquatables { SomeProperty = 1 };
            var obj3 = new EquatableObject { SomeProperty = 1 };

            var n = new TCLiteEqualityComparer();
            var tolerance = Tolerance.Exact;
            Assert.That(n.AreEqual(obj1, obj2, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj2, obj1, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj1, obj3, ref tolerance), Is.False);
            Assert.That(n.AreEqual(obj3, obj1, ref tolerance), Is.False);
            Assert.That(n.AreEqual(obj2, obj3, ref tolerance), Is.True);
            Assert.That(n.AreEqual(obj3, obj2, ref tolerance), Is.True);
        }

        [Test]
        public void IEnumeratorIsDisposed()
        {
            var enumeration = new EnumerableWithDisposeChecks<int>(new[] { 0, 1, 2, 3 });
            Assert.IsTrue(comparer.AreEqual(enumeration, enumeration, ref tolerance));
            Assert.That(enumeration.EnumeratorsDisposed);
        }

        [TestCaseSource(nameof(GetRecursiveContainsTestCases))]
        public void SelfContainedItemFoundInCollection<T>(T x, ICollection y)
        {
            var equalityComparer = new TCLiteEqualityComparer();
            var tolerance = Tolerance.Default;
            var equality = equalityComparer.AreEqual(x, y, ref tolerance);

            Assert.IsFalse(equality);
            // NYI: Assert.Contains(x, y);
            // NYI: Assert.That(y, Contains.Item(x));
            // NYI: Assert.That(y, Does.Contain(x));
        }

        [TestCaseSource(nameof(GetRecursiveComparerTestCases))]
        public void SelfContainedItemDoesntRecurseForever<T>(T x, ICollection y)
        {
            var equalityComparer = new TCLiteEqualityComparer();
            var tolerance = Tolerance.Default;
            equalityComparer.ExternalComparers.Add(new DetectRecursionComparer(30));

            Assert.DoesNotThrow(() => equalityComparer.AreEqual(x, y, ref tolerance));
        }

#if NYI // Duplicate items
        [Test]
        public void SelfContainedDuplicateItemsAreCompared()
        {
            var equalityComparer = new TCLiteEqualityComparer();
            var equalInstance1 = new[] { 1 };
            var equalInstance2 = new[] { 1 };

            var x = new[] { equalInstance1, equalInstance1 };
            var y = new[] { equalInstance2, equalInstance2 };

            Assert.IsTrue(equalityComparer.AreEqual(x, y, ref tolerance));
        }
#endif

        public static IEnumerable<TestCaseData> GetRecursiveComparerTestCases()
        {
            // Separate from 'GetRecursiveContainsTestCases' until a stackoverflow issue in 
            // 'MsgUtils.FormatValue()' can be fixed for the below cases
            foreach (var testCase in GetRecursiveContainsTestCases())
                yield return testCase;

            var dict = new Dictionary<object, object>();
            var dictItem = "nunit";

            dict[1] = dictItem;
            dict[2] = dict;

            yield return new TestCaseData(dictItem, dict);
        }

        public static IEnumerable<TestCaseData> GetRecursiveContainsTestCases()
        {
            var enumerable = new SelfContainer();
            var enumerableContainer = new SelfContainer[] { new SelfContainer(), enumerable };

            yield return new TestCaseData(enumerable, enumerableContainer);

            object itemB = 1;
            object[] itemBSet = new object[2];
            itemBSet[0] = itemB;
            itemBSet[1] = itemBSet;

            yield return new TestCaseData(itemB, itemBSet);
        }
    }

    internal class DetectRecursionComparer : EqualityAdapter
    {
        private readonly int maxRecursion;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public DetectRecursionComparer(int maxRecursion)
        {
            var callerDepth = new StackTrace().FrameCount - 1;
            this.maxRecursion = callerDepth + maxRecursion;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public override bool CanCompare(object x, object y)
        {
            var currentDepth = new StackTrace().FrameCount - 1;
            return currentDepth >= maxRecursion;
        }

        public override bool AreEqual(object x, object y)
        {
            throw new InvalidOperationException("Recurses");
        }
    }

    internal class SelfContainer : IEnumerable
    {
        public IEnumerator GetEnumerator() { yield return this; }
    }

    internal class EnumerableWithDisposeChecks<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> data;
        private readonly List<DisposableEnumerator<T>> enumerators = new List<DisposableEnumerator<T>>();

        public EnumerableWithDisposeChecks(T[] data)
        {
            this.data = data;
        }

        public bool EnumeratorsDisposed
        {
            get
            {
                foreach (var disposableEnumerator in enumerators)
                {
                    if (!disposableEnumerator.Disposed)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = new DisposableEnumerator<T>(data.GetEnumerator());
            enumerators.Add(enumerator);
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }

    internal class DisposableEnumerator<T> : IEnumerator<T>
    {
        private bool disposedValue = false;
        private readonly IEnumerator<T> enumerator;

        public DisposableEnumerator(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;
        }

        public bool Disposed { get { return disposedValue; } }

        public T Current
        {
            get
            {
                return enumerator.Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return enumerator.Current;
            }
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

        public void Dispose()
        {
            disposedValue = true;
        }

    }

    public class NeverEqualIEquatableWithOverriddenAlwaysTrueEquals : IEquatable<NeverEqualIEquatableWithOverriddenAlwaysTrueEquals>
    {
        public bool Equals(NeverEqualIEquatableWithOverriddenAlwaysTrueEquals other)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Int32IEquatable : IEquatable<int>
    {
        private readonly int value;

        public Int32IEquatable(int value)
        {
            this.value = value;
        }

        public bool Equals(int other)
        {
            return value.Equals(other);
        }
    }

    public class NeverEqualIEquatable : IEquatable<NeverEqualIEquatable>
    {
        public bool Equals(NeverEqualIEquatable other)
        {
            return false;
        }
    }

    public class IEquatableWithoutEqualsOverridden : IEquatable<IEquatableWithoutEqualsOverridden>
    {
        private readonly int value;

        public IEquatableWithoutEqualsOverridden(int value)
        {
            this.value = value;
        }

        public bool Equals(IEquatableWithoutEqualsOverridden other)
        {
            return value.Equals(other.value);
        }
    }

    public class IEquatableWithExplicitImplementation : IEquatable<IEquatableWithExplicitImplementation>
    {
        private readonly int value;

        public IEquatableWithExplicitImplementation(int value)
        {
            this.value = value;
        }

        bool IEquatable<IEquatableWithExplicitImplementation>.Equals(IEquatableWithExplicitImplementation other)
        {
            return value.Equals(other.value);
        }
    }

    public class EquatableObject : IEquatable<EquatableObject>
    {
        public int SomeProperty { get; set; }
        public bool Equals(EquatableObject other)
        {
            if (other == null)
                return false;

            return SomeProperty == other.SomeProperty;
        }
    }

    public class InheritingEquatableObject : EquatableObject, IEquatable<InheritingEquatableObject>
    {
        public int OtherProperty { get; set; }
        public bool Equals(InheritingEquatableObject other)
        {
            if (other == null)
                return false;

            return OtherProperty == other.OtherProperty && Equals((EquatableObject) other);
        }
    }

    public interface IEquatableObject : IEquatable<IEquatableObject>
    {
        int SomeProperty { get; set; }
    }

    public class InheritedEquatableObject : IEquatableObject
    {
        public int SomeProperty { get; set; }

        public bool Equals(IEquatableObject other)
        {
            if (other == null)
                return false;

            return SomeProperty == other.SomeProperty;
        }
    }

    public class MultipleIEquatables : InheritedEquatableObject, IEquatable<EquatableObject>
    {
        public bool Equals(EquatableObject other)
        {
            if (other == null)
                return false;

            return SomeProperty == other.SomeProperty;
        }
    }

    public class EnumerableObject<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> enumerable;

        public EnumerableObject(IEnumerable<T> enumerable, int someProperty)
        {
            this.enumerable = enumerable;
            SomeProperty = someProperty;
        }

        public int SomeProperty { get; }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class EquatableWithEnumerableObject<T> : IEnumerable<T>, IEquatable<EnumerableObject<T>>
    {
        private readonly IEnumerable<T> enumerable;

        public EquatableWithEnumerableObject(IEnumerable<T> enumerable, int otherProperty)
        {
            this.enumerable = enumerable;
            OtherProperty = otherProperty;
        }

        public int OtherProperty { get; }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return enumerable.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(EnumerableObject<T> other)
        {
            if (other == null)
                return false;

            return OtherProperty == other.SomeProperty;
        }
    }
}
