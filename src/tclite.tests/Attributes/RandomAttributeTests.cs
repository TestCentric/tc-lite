// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TCLite.Attributes
{
    [TestFixture]
    public class RandomAttributeTests
    {
        public const int COUNT = 3;

        #region Int

        public void RandomInt([Random(COUNT)] int x)
        {
            Assert.That(x, Is.InRange(0, int.MaxValue - 1));
        }

        public void RandomInt_IntRange([Random(7, 47, COUNT)] int x)
        {
            Assert.That(x, Is.InRange(7, 46));
        }

        #endregion

        #region Unsigned Int

        public void RandomUInt([Random(COUNT)] uint x)
        {
            Assert.That(x, Is.InRange(0u, uint.MaxValue - 1));
        }

        public void RandomUInt_UIntRange([Random(7u, 47u, COUNT)] uint x)
        {
            Assert.That(x, Is.InRange(7u, 46u));
        }

        #endregion

        #region Short

        public void RandomShort([Random(COUNT)] short x)
        {
            Assert.That(x, Is.InRange(0, short.MaxValue - 1));
        }

        public void RandomShort_ShortRange([Random((short)7, (short)47, COUNT)] short x)
        {
            Assert.That(x, Is.InRange((short)7, (short)46));
        }

        public void RandomShort_IntRange([Random(7, 47, COUNT)] short x)
        {
            Assert.That(x, Is.InRange((short)7, (short)46));
        }

        #endregion

        #region Unsigned Short

        public void RandomUShort([Random(COUNT)] ushort x)
        {
            Assert.That(x, Is.InRange(0, ushort.MaxValue - 1));
        }

        public void RandomUShort_UShortRange([Random((ushort)7, (ushort)47, COUNT)] ushort x)
        {
            Assert.That(x, Is.InRange((ushort)7, (ushort)46));
        }

        public void RandomUShort_IntRange([Random(7, 47, COUNT)] ushort x)
        {
            Assert.That(x, Is.InRange((ushort)7, (ushort)46));
        }

        #endregion

        #region Long

        public void RandomLong([Random(COUNT)] long x)
        {
            Assert.That(x, Is.InRange(0L, long.MaxValue - 1));
        }

        public void RandomLong_LongRange([Random(7L, 47L, COUNT)] long x)
        {
            Assert.That(x, Is.InRange(7L, 46L));
        }

        #endregion

        #region Unsigned Long

        public void RandomULong([Random(COUNT)] ulong x)
        {
            Assert.That(x, Is.InRange(0UL, ulong.MaxValue - 1));
        }

        public void RandomULong_ULongRange([Random(7ul, 47ul, COUNT)] ulong x)
        {
            Assert.That(x, Is.InRange(7ul, 46ul));
        }

        #endregion

        #region Double

        public void RandomDouble([Random(COUNT)] double x)
        {
            Assert.That(x, Is.InRange(0.0d, 1.0d));
        }

        public void RandomDouble_DoubleRange([Random(7.0d, 47.0d, COUNT)] double x)
        {
            Assert.That(x, Is.InRange(7.0d, 47.0d));
        }

        #endregion

        #region Float

        public void RandomFloat([Random(COUNT)] float x)
        {
            Assert.That(x, Is.InRange(0.0f, 1.0f));
        }

        public void RandomFloat_FloatRange([Random(7.0f, 47.0f, COUNT)] float x)
        {
            Assert.That(x, Is.InRange(7.0f, 47.0f));
        }

        #endregion

        #region Enum

        public void RandomEnum([Random(COUNT)] TestEnum x)
        {
            Assert.That(x, Is.EqualTo(TestEnum.A).Or.EqualTo(TestEnum.B).Or.EqualTo(TestEnum.C));
        }

        public enum TestEnum
        {
            A, B, C
        }

        #endregion

        #region Decimal

        public void RandomDecimal([Random(COUNT)] decimal x)
        {
            Assert.That(x, Is.InRange(0M, decimal.MaxValue));
        }

        public void RandomDecimal_IntRange([Random(7, 47, COUNT)] decimal x)
        {
            Assert.That(x, Is.InRange(7M, 46M));
        }

        public void RandomDecimal_DoubleRange([Random(7.0, 47.0, COUNT)] decimal x)
        {
            Assert.That(x, Is.InRange(7M, 47M));
        }

        #endregion

        #region Byte

        public void RandomByte([Random(COUNT)] byte x)
        {
            Assert.That(x, Is.InRange(0, byte.MaxValue - 1));
        }

        public void RandomByte_ByteRange([Random((byte)7, (byte)47, COUNT)] byte x)
        {
            Assert.That(x, Is.InRange((byte)7, (byte)46));
        }

        public void RandomByte_IntRange([Random(7, 47, COUNT)] byte x)
        {
            Assert.That(x, Is.InRange((byte)7, (byte)46));
        }

        #endregion

        #region SByte

        public void RandomSByte([Random(COUNT)] sbyte x)
        {
            Assert.That(x, Is.InRange(0, sbyte.MaxValue - 1));
        }

        public void RandomSByte_SByteRange([Random((sbyte)7, (sbyte)47, COUNT)] sbyte x)
        {
            Assert.That(x, Is.InRange((sbyte)7, (sbyte)46));
        }

        public void RandomSByte_IntRange([Random(7, 47, COUNT)] sbyte x)
        {
            Assert.That(x, Is.InRange((sbyte)7, (sbyte)46));
        }

        #endregion
    }
}
