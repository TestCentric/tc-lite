// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.TestUtilities;

namespace TCLite.Internal
{
    public class RandomizerTests
    {
        private Randomizer _randomizer = Randomizer.CreateRandomizer();

        [TestCase]
        public void RandomizersAreUnique()
        {
            int[] values = new int[10];
            for (int i = 0; i < 10; i++)
                values[i] = Randomizer.CreateRandomizer().Next();

            Assert.That(values, Is.Unique);
        }

        #region Ints

        [TestCase]
        public void RandomIntsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.Next(), 10, 10);
        }

        [TestCase]
        public void RandomIntsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.Next(-300, 300), 10, 12);
        }

        #endregion

        #region Unsigned Ints

        [TestCase]
        public void RandomUInt()
        {
            uint u = _randomizer.NextUInt();
            Assert.That(u, Is.InRange(0u, uint.MaxValue));
        }

        [TestCase]
        public void RandomUIntWithMaximum()
        {
            uint val = _randomizer.NextUInt(100u);
            Assert.That(val >= 0u && val < 100u, "Out of range");
        }

        [TestCase]
        public void RandomUIntInRange()
        {
            uint val = _randomizer.NextUInt(42u, 99u);
            Assert.That(val >= 42u && val < 99u, "Out of range");
        }

        [TestCase]
        public void RandomUintInRange_Reversed()
        {
            Assert.That(() => _randomizer.NextUInt(99u, 42u), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomUintInRange_MinEqualsMax()
        {
            Assert.That(_randomizer.NextUInt(42u, 42u), Is.EqualTo(42u));
        }

        [TestCase]
        public void RandomUIntsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextUInt(), 10, 100);
        }

        [TestCase]
        public void RandomUIntsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextUInt(27u, 777u), 10, 100);
        }

        #endregion

        #region Shorts

        [TestCase]
        public void RandomShort()
        {
            short s = _randomizer.NextShort();
            Assert.That(s, Is.InRange((short)0, short.MaxValue));
        }

        [TestCase]
        public void RandomShortWithMaximum()
        {
            short zero = 0;
            short max = 100;
            short s = _randomizer.NextShort(max);
            Assert.That(s >= zero && s < max, "Out of range");
        }

        [TestCase]
        public void RandomShortInRange()
        {
            short min = -10;
            short max = 100;
            short s = _randomizer.NextShort(min, max);
            Assert.That(s >= min && s < max, "Out of range");
        }

        [TestCase]
        public void RandomShortInRange_Reversed()
        {
            short min = 100;
            short max = -10;
            Assert.That(() => _randomizer.NextShort(min, max), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomShortInRange_MinEqualsMax()
        {
            short s = 42;
            Assert.That(_randomizer.NextShort(s, s), Is.EqualTo(s));
        }

        [TestCase]
        public void RandomShortsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextShort(), 10, 100);
        }

        [TestCase]
        public void RandomShortsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextShort(-300, 300), 10, 100);
        }

        #endregion

        #region Unsigned Shorts

        [TestCase]
        public void RandomUShort()
        {
            ushort us = _randomizer.NextUShort();
            Assert.That(us, Is.InRange((ushort)0, ushort.MaxValue));
        }

        [TestCase]
        public void RandomUShortWithMaximum()
        {
            ushort zero = 0;
            ushort max = 100;
            ushort us = _randomizer.NextUShort(max);
            Assert.That(us >= zero && us < max, "Out of range");
        }

        [TestCase]
        public void RandomUShortInRange()
        {
            ushort min = 42;
            ushort max = 99;
            ushort val = _randomizer.NextUShort(min, max);
            Assert.That(val >= min && val < max, "Out of range");
        }

        [TestCase]
        public void RandomUShortInRange_Reversed()
        {
            ushort min = 99;
            ushort max = 42;
            Assert.That(() => _randomizer.NextUShort(min, max), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomUShortInRange_MinEqualsMax()
        {
            ushort val = 42;
            Assert.That(_randomizer.NextUShort(val, val), Is.EqualTo(val));
        }

        [TestCase]
        public void RandomUShortsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextUShort(), 10, 100);
        }

        [TestCase]
        public void RandomUShortsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextUShort((ushort)27, (ushort)200), 10, 100);
        }

        #endregion

        #region Longs

        [TestCase]
        public void RandomLong()
        {
            long val = _randomizer.NextLong();
            Assert.That(val, Is.InRange(0L, long.MaxValue));
        }

        [TestCase]
        public void RandomLongWithMaximum()
        {
            long val = _randomizer.NextLong(1066L);
            Assert.That(val >= 0L && val < 1066L, "Out of range");
        }

        [TestCase]
        public void RandomLongInRange()
        {
            long val = _randomizer.NextLong(1066L, 1942L);
            Assert.That(val >= 1066L && val < 1942L, "Out of range");
        }

        [TestCase]
        public void RandomLongInRange_Reversed()
        {
            Assert.That(() => _randomizer.NextLong(1942L, 1066L), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomLongInRange_MinEqualsMax()
        {
            long val = 42L;
            Assert.That(_randomizer.NextLong(val, val), Is.EqualTo(val));
        }

        [TestCase]
        public void RandomLongsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextLong(), 10, 100);
        }

        [TestCase]
        public void RandomLongsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextLong(1066L, 2010L), 10, 100);
        }

        #endregion

        #region Unsigned Longs

        [TestCase]
        public void RandomULong()
        {
            ulong val = _randomizer.NextULong();
            Assert.That(val, Is.InRange(0UL, ulong.MaxValue));
        }

        [TestCase]
        public void RandomULongWithMaximum()
        {
            ulong val = _randomizer.NextULong(1066UL);
            Assert.That(val < 1066UL, "Out of range");
        }

        [TestCase]
        public void RandomULongInRange()
        {
            ulong val = _randomizer.NextULong(1066UL, 1942UL);
            Assert.That(val >= 1066UL && val < 1942UL, "Out of range");
        }

        [TestCase]
        public void RandomULongInRange_Reversed()
        {
            Assert.That(() => _randomizer.NextULong(1942UL, 1066UL), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomULongInRange_MinEqualsMax()
        {
            ulong val = 42ul;
            Assert.That(_randomizer.NextULong(val, val), Is.EqualTo(val));
        }

        [TestCase]
        public void RandomULongsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextULong(), 10, 100);
        }

        [TestCase]
        public void RandomULongsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextULong(1066UL, 2010UL), 10, 100);
        }

        #endregion

        #region Bytes

        [TestCase]
        public void RandomByte()
        {
            byte b = _randomizer.NextByte();
            Assert.That(b, Is.InRange(byte.MinValue, byte.MaxValue));
        }

        [TestCase]
        public void RandomByteWithMaximum()
        {
            byte max = (byte)96;
            byte b = _randomizer.NextByte(max);
            Assert.That(b >= byte.MinValue && b < max);
        }

        [TestCase]
        public void RandomByteInRange()
        {
            byte min = (byte)16;
            byte max = (byte)96;
            byte b = _randomizer.NextByte(min, max);
            Assert.That(b >= min && b < max);
        }

        [TestCase]
        public void RandomByteInRange_Reversed()
        {
            byte min = (byte)96;
            byte max = (byte)16;
            Assert.That(() => _randomizer.NextByte(min, max), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomByteInRange_MinEqualsMax()
        {
            byte val = 42;
            Assert.That(_randomizer.NextByte(val, val), Is.EqualTo(val));
        }

        [TestCase]
        public void RandomBytesAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextByte(), 10, 1000);
        }

        [TestCase]
        public void RandomBytesInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextByte(byte.MinValue, byte.MaxValue), 10, 1000);
        }

        #endregion

        #region SBytes

        [TestCase]
        public void RandomSByte()
        {
            sbyte b = _randomizer.NextSByte();
            Assert.That(b, Is.InRange(sbyte.MinValue, sbyte.MaxValue));
        }

        [TestCase]
        public void RandomSByteWithMaximum()
        {
            sbyte max = (sbyte)96;
            sbyte b = _randomizer.NextSByte(max);
            Assert.That(b >= 0 && b < max);
        }

        [TestCase]
        public void RandomSByteInRange()
        {
            sbyte min = (sbyte)16;
            sbyte max = (sbyte)96;
            sbyte b = _randomizer.NextSByte(min, max);
            Assert.That(b >= min && b < max);
        }

        [TestCase]
        public void RandomSByteInRange_Reversed()
        {
            sbyte min = (sbyte)96;
            sbyte max = (sbyte)16;
            Assert.That(() => _randomizer.NextSByte(min, max), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomSbyteInRange_MinEqualsMax()
        {
            sbyte val = 42;
            Assert.That(_randomizer.NextSByte(val, val), Is.EqualTo(val));
        }

        [TestCase]
        public void RandomSBytesAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextSByte(), 10, 1000);
        }

        [TestCase]
        public void RandomSBytesInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextSByte(sbyte.MinValue, sbyte.MaxValue), 10, 1000);
        }

        #endregion

        #region Bool

        [TestCase]
        public void RandomBool()
        {
            UniqueValues.Check(() => _randomizer.NextBool(), 2, 100);
        }

        [TestCase]
        public void RandomBoolWithProbability()
        {
            UniqueValues.Check(() => _randomizer.NextBool(.25), 2, 200);
        }

        [TestCase]
        public void RandomBoolWithProbabilityOutOfRange()
        {
            Assert.That(() => _randomizer.NextBool(2.0), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomBoolWithProbabilityZeroIsAlwaysFalse()
        {
            for (int i = 0; i < 10; i++)
                Assert.IsFalse(_randomizer.NextBool(0.0));
        }

        [TestCase]
        public void RandomBoolWithProbabilityOneIsAlwaysTrue()
        {
            for (int i = 0; i < 10; i++)
                Assert.IsTrue(_randomizer.NextBool(1.0));
        }

        #endregion

        #region Doubles

        [TestCase]
        public void RandomDouble()
        {
            double d = _randomizer.NextDouble();
            Assert.That(d, Is.InRange(0.0, 1.0));
        }

        [TestCase]
        public void RandomDoubleWithMaximum()
        {
            double d = _randomizer.NextDouble(100.0);
            Assert.That(d >= 0.0 && d < 100.0, "Out of range");
        }

        [TestCase]
        public void RandomDoubleInRange()
        {
            double d = _randomizer.NextDouble(0.2, 0.7);
            Assert.That(d >= 0.2 && d < 0.7, "Out of range");
        }

        [TestCase]
        public void RandomDoubleInRange_Reversed()
        {
            Assert.That(() => _randomizer.NextDouble(0.7, 0.2), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomDoubleInRange_MinEqualsMax()
        {
            double val = 42.0;
            Assert.That(_randomizer.NextDouble(val, val), Is.EqualTo(val));
        }

        [TestCase]
        public void RandomDoublesAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextDouble(), 10, 100);
        }

        [TestCase]
        public void RandomDoublesInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextDouble(0.1, 0.7), 10, 100);
        }

        #endregion

        #region Floats

        [TestCase]
        public void RandomFloat()
        {
            float f = _randomizer.NextFloat();
            Assert.That(f, Is.InRange(0.0f, 1.0f));
        }

        [TestCase]
        public void RandomFloatWithMaximum()
        {
            float f = _randomizer.NextFloat(100.0f);
            Assert.That(f >= 0.0f && f < 100.0f, "Out of range");
        }

        [TestCase]
        public void RandomFloatInRange()
        {
            float f = _randomizer.NextFloat(0.2f, 0.7f);
            Assert.That(f >= 0.2f && f < 0.7f, "Out of range");
        }

        [TestCase]
        public void RandomFloatInRange_Reversed()
        {
            Assert.That(() => _randomizer.NextFloat(0.7f, 0.2f), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomFloatInRange_MinEqualsMax()
        {
            float val = 42.0f;
            Assert.That(_randomizer.NextFloat(val, val), Is.EqualTo(val));
        }

        [TestCase]
        public void RandomFloatsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextFloat(), 10, 100);
        }

        [TestCase]
        public void RandomFloatsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextFloat(0.5f, 1.5f), 10, 100);
        }

        /// <summary>
        /// Return an array of random floats between 0.0 and 1.0.
        /// </summary>
        public float[] GetFloats(int count)
        {
            float[] floats = new float[count];

            for (int index = 0; index < count; index++)
                floats[index] = _randomizer.NextFloat();

            return floats;
        }

        /// <summary>
        /// Return an array of random floats with values in a specified range.
        /// </summary>
        public float[] GetFloats(float min, float max, int count)
        {
            float[] floats = new float[count];

            for (int index = 0; index < count; index++)
                floats[index] = _randomizer.NextFloat(min, max);

            return floats;
        }

        #endregion

        #region Decimals

        [TestCase]
        public void RandomDecimal()
        {
            decimal d = _randomizer.NextDecimal();
            Assert.That(d, Is.InRange(0M, decimal.MaxValue));
            CheckScaleIsZero(d);
        }

        private static void CheckScaleIsZero(decimal d)
        {
            int[] bits = decimal.GetBits(d);
            Assert.That(bits[3] & 0x00ff0000, Is.EqualTo(0));
        }

        [TestCase]
        public void RandomDecimalWithMaximum()
        {
            decimal d = _randomizer.NextDecimal(1000M);
            Assert.That(d, Is.InRange(0M, 999M));
            CheckScaleIsZero(d);
        }

        [TestCase]
        public void RandomDecimalInRange()
        {
            decimal d = _randomizer.NextDecimal(-500M, 500M);
            Assert.That(d, Is.InRange(-500M, 499M));
            CheckScaleIsZero(d);
        }

        [TestCase]
        public void RandomDecimalInRange_Reversed()
        {
            Assert.That(() => _randomizer.NextDecimal(1000M, 100M), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase]
        public void RandomDecimalInRange_MinEqualsMax()
        {
            Assert.That(_randomizer.NextDecimal(42M, 42M), Is.EqualTo(42M));
        }

        [TestCase]
        public void RandomDecimalRangeTooGreat()
        {
            Assert.That(() => _randomizer.NextDecimal(-1M, decimal.MaxValue), Throws.InstanceOf<ArgumentException>());
        }

        [TestCase]
        public void RandomDecimalsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextDecimal(), 10, 100);
        }

        [TestCase]
        public void RandomDecimalsInRangeAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextDecimal(1066M, 2010M), 10, 100);
        }

        #endregion

        #region Strings

        [TestCase]
        [Description("Test that all generated strings are unique")]
        public void RandomStringsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.GetString(), 10, 10);
        }

        [TestCase(30, "Tｈｅɋúｉｃｋƃｒòｗｎｆ߀хｊｕｍｐëԁoѵerｔհëｌａȥｙｄｏɢ")]
        [TestCase(200, "ａèí߀ù123456")]
        [TestCase(1000, Randomizer.DefaultStringChars)]
        [Description("Test that all generated strings are unique for varying output length")]
        public void RandomStringsAreUnique(int outputLength, string allowedChars)
        {
            UniqueValues.Check(() => _randomizer.GetString(outputLength, allowedChars), 10, 10);
        }

        #endregion

        #region Enums

        [TestCase]
        public void RandomEnum()
        {
            UniqueValues.Check(() => _randomizer.NextEnum(typeof(AttributeTargets)), 5, 50);
        }

        [TestCase]
        public void RandomEnum_Generic()
        {
            UniqueValues.Check(() => _randomizer.NextEnum<AttributeTargets>(), 5, 50);
        }

        #endregion

        #region Guids

        [TestCase]
        [Description("Test that all generated Guids are unique")]
        public void RandomGuidsAreUnique()
        {
            UniqueValues.Check(() => _randomizer.NextGuid(), 10, 10);
        }

        [TestCase]
        [Description("Test that generated Guids are version 4 variant 1 Guids")]
        public void RandomGuidsAreV4()
        {
            Guid guid = _randomizer.NextGuid();
            byte[] bytes = guid.ToByteArray();
            //check the version
            Assert.That(bytes[7] & 0xf0, Is.EqualTo(0x40));
            //check the variant
            Assert.That(bytes[8] & 0xc0, Is.EqualTo(0x80));
        }

        #endregion

        #region Repeatability
        
        [TestCase]
        public void RandomizersWithSameSeedsReturnSameValues()
        {
            Randomizer r1 = new Randomizer(1234);
            Randomizer r2 = new Randomizer(1234);

            for (int i = 0; i < 10; i++)
                Assert.That(r1.NextDouble(), Is.EqualTo(r2.NextDouble()));
        }

        [TestCase]
        public void RandomizersWithDifferentSeedsReturnDifferentValues()
        {
            Randomizer r1 = new Randomizer(1234);
            Randomizer r2 = new Randomizer(4321);

            for (int i = 0; i < 10; i++)
                Assert.That(r1.NextDouble(), Is.Not.EqualTo(r2.NextDouble()));
        }

        [TestCase]
        public void ReturnsSameRandomizerForSameParameter()
        {
            ParameterInfo p = testMethod1.GetParameters()[0];
            Randomizer r1 = Randomizer.GetRandomizer(p);
            Randomizer r2 = Randomizer.GetRandomizer(p);
            Assert.That(r1, Is.SameAs(r2));
        }

        [TestCase]
        public void ReturnsSameRandomizerForDifferentParametersOfSameMethod()
        {
            ParameterInfo p1 = testMethod1.GetParameters()[0];
            ParameterInfo p2 = testMethod1.GetParameters()[1];
            Randomizer r1 = Randomizer.GetRandomizer(p1);
            Randomizer r2 = Randomizer.GetRandomizer(p2);
            Assert.That(r1, Is.SameAs(r2));
        }

        [TestCase]
        public void ReturnsSameRandomizerForSameMethod()
        {
            Randomizer r1 = Randomizer.GetRandomizer(testMethod1);
            Randomizer r2 = Randomizer.GetRandomizer(testMethod1);
            Assert.That(r1, Is.SameAs(r2));
        }

        [TestCase]
        public void ReturnsDifferentRandomizersForDifferentMethods()
        {
            Randomizer r1 = Randomizer.GetRandomizer(testMethod1);
            Randomizer r2 = Randomizer.GetRandomizer(testMethod2);
            Assert.That(r1, Is.Not.SameAs(r2));
        }

        static readonly MethodInfo testMethod1 =
            typeof(RandomizerTests).GetMethod("TestMethod1", BindingFlags.NonPublic | BindingFlags.Instance);
        private void TestMethod1(int x, int y)
        {
        }

        static readonly MethodInfo testMethod2 =
            typeof(RandomizerTests).GetMethod("TestMethod2", BindingFlags.NonPublic | BindingFlags.Instance);
        private void TestMethod2(int x, int y)
        {
        }
    }

    #endregion
}
