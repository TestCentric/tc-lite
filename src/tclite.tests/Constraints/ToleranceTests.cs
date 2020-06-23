﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class ToleranceTests
    {
        [Test, DefaultFloatingPointTolerance(0.1)]
        public void DefaultTolerance_Success()
        {
            Assert.That(2.05d, Is.EqualTo(2.0d));
        }

        [Test, DefaultFloatingPointTolerance(0.01)]
        public void DefaultTolerance_Failure()
        {
            Assert.That(2.05d, Is.Not.EqualTo(2.0d));
        }

        [Test, DefaultFloatingPointTolerance(0.5)]
        public void TestToleranceDefault()
        {
            var defaultTolerance = Tolerance.Default;
            Assert.IsTrue(defaultTolerance.IsDefault);

            var comparer = new TCLiteEqualityComparer();
            Assert.IsTrue(comparer.AreEqual(2.0d, 2.1d, ref defaultTolerance ));
        }

        [Test, DefaultFloatingPointTolerance(0.5)]
        public void TestToleranceExact()
        {
            var noneTolerance = Tolerance.Exact;
            Assert.IsFalse(noneTolerance.IsDefault);

            var comparer = new TCLiteEqualityComparer();
            Assert.IsFalse(comparer.AreEqual(2.0d, 2.1d, ref noneTolerance));
        }

        [Test]
        public void TestWithinCanOnlyBeUsedOnce()
        {
            Assert.That(() => Is.EqualTo(1.1d).Within(0.5d).Within(0.2d),
                Throws.TypeOf<InvalidOperationException>()); // NYI: .With.Message.Contains("modifier may appear only once"));
        }

        [Test]
        public void TestModesCanOnlyBeUsedOnce()
        {
            var tolerance = new Tolerance(5);
            Assert.That(() => tolerance.Percent.Ulps,
                Throws.TypeOf<InvalidOperationException>()); // NYI: .With.Message.Contains("multiple tolerance modes"));
        }

        [Test]
        public void TestNumericToleranceRequired()
        {
            var tolerance = new Tolerance("Five");
            Assert.That(() => tolerance.Percent,
                Throws.TypeOf<InvalidOperationException>()); // NYI: .With.Message.Contains("numeric tolerance is required"));
        }

        [Test]
        public void TestModeMustFollowTolerance()
        {
            var tolerance = Tolerance.Default; // which is new Tolerance(0, ToleranceMode.Unset)
            Assert.That(() => tolerance.Percent, 
                Throws.TypeOf<InvalidOperationException>()); // NYI: .With.Message.Contains("Tolerance amount must be specified"));
        }
    }
}