// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class NumericsTests
    {
        private Tolerance _tenPercent = new Tolerance(10.0).Percent;
        private Tolerance _zeroTolerance = Tolerance.Exact;

        [TestCase(123456789)]
        [TestCase(123456789U)]
        [TestCase(123456789L)]
        [TestCase(123456789UL)]
        [TestCase(1234.5678f)]
        [TestCase(1234.5678)]
        public void CanMatchWithoutToleranceMode<T>(T value)
        {
            Assert.IsTrue(Numerics.AreEqual(value, value, ref _zeroTolerance));
        }

        // Separate test case because you can't use decimal in an attribute (24.1.3)
        [TestCase]
        public void CanMatchDecimalWithoutToleranceMode()
        {
            Assert.IsTrue(Numerics.AreEqual(123m, 123m, ref _zeroTolerance));
        }

        [TestCase((int)9500)]
        [TestCase((int)10000)]
        [TestCase((int)10500)]
        [TestCase((uint)9500)]
        [TestCase((uint)10000)]
        [TestCase((uint)10500)]
        [TestCase((long)9500)]
        [TestCase((long)10000)]
        [TestCase((long)10500)]
        [TestCase((ulong)9500)]
        [TestCase((ulong)10000)]
        [TestCase((ulong)10500)]
        public void CanMatchIntegralsWithPercentage(object value)
        {
            Assert.IsTrue(Numerics.AreEqual(10000, value, ref _tenPercent));
        }

        [TestCase]
        public void CanMatchDecimalWithPercentage()
        {
            Assert.IsTrue(Numerics.AreEqual(10000m, 9500m, ref _tenPercent));
            Assert.IsTrue(Numerics.AreEqual(10000m, 10000m, ref _tenPercent));
            Assert.IsTrue(Numerics.AreEqual(10000m, 10500m, ref _tenPercent));
        }

        [TestCase((int)8500)]
        [TestCase((int)11500)]
        [TestCase((uint)8500)]
        [TestCase((uint)11500)]
        [TestCase((long)8500)]
        [TestCase((long)11500)]
        [TestCase((ulong)8500)]
        [TestCase((ulong)11500)]
        public void FailsOnIntegralsOutsideOfPercentage(object value)
        {
            Assert.Throws<AssertionException>(() => Assert.IsTrue(Numerics.AreEqual(10000, value, ref _tenPercent)));
        }

        [TestCase]
        public void FailsOnDecimalBelowPercentage()
        {
            Assert.Throws<AssertionException>(() => Assert.IsTrue(Numerics.AreEqual(10000m, 8500m, ref _tenPercent)));
        }

        [TestCase]
        public void FailsOnDecimalAbovePercentage()
        {
            Assert.Throws<AssertionException>(() => Assert.IsTrue(Numerics.AreEqual(10000m, 11500m, ref _tenPercent)));
        }
    }
}