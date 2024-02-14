// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TCLite.Interfaces;
using TCLite.Internal;
using TCLite.TestUtilities;

namespace TCLite.Attributes
{
    public class RangeAttributeTests
    {
        private static readonly MethodInfo _method = typeof(RangeAttributeTests).GetMethod(nameof(MethodWithIntArgsAndValues));
        private static readonly ParameterInfo _parameter = _method.GetParameters()[0];
        private static readonly int[] Range1to5 = new[] { 1, 2, 3, 4, 5 };

        [TestCase]
        public void CanGetDataFromAttribute()
        {
            var attr = _parameter.GetCustomAttribute(typeof(RangeAttribute), false) as RangeAttribute;

            Assert.That(attr.GetData(_parameter), Is.EqualTo(Range1to5));
        }

        [TestCase]
        public void CanGetDataFromParameterDataSource()
        {
            var data = new ParameterDataSource(_parameter).GetData(_parameter);

            Assert.That(data, Is.EqualTo(Range1to5));
        }

        [TestCase]
        public void CanGetDataFromParameterDataProvider()
        {
            var data = new ParameterDataProvider(_method).GetDataFor(_parameter);

            Assert.That(data, Is.EqualTo(Range1to5));
        }

        public void MethodWithIntArgsAndValues([Range(1, 5)] int x)
        {
            Assert.That(x >= 1 && x <= 5);
        }

        public void MethodWithUIntArgsAndValues([Range(1U, 5U)] uint x)
        {
            Assert.That(x >= 1U && x <= 5U);
        }

        public void MethodWithLongArgsAndValues([Range(1L, 5L)] long x)
        {
            Assert.That(x >= 1L && x <= 5L);
        }

        public void MethodWithULongArgsAndValues([Range(1UL, 5UL)] ulong x)
        {
            Assert.That(x >= 1UL && x <= 5UL);
        }

        public void MethodWithDoubleArgsAndValues([Range(1.0D, 5.0D, 1.0D)] double x)
        {
            Assert.That(x >= 1.0D && x <= 5.0D);
        }

        public void MethodWithFloatArgsAndValues([Range(1.0F, 5.0F, 1.0F)] float x)
        {
            Assert.That(x >= 1.0F && x <= 5.0F);
        }

        public void MethodWithLongArgsAndIntValues([Range(1, 5)] long x)
        {
            Assert.That(x >= 1L && x <= 5L);
        }
        public void MethodWithDoubleArgsAndIntValues([Range(1, 5)] double x)
        {
            Assert.That(x >= 1.0D && x <= 5.0D);
        }

        public void MethodWithTwoRanges([Range(1, 3)][Range(10, 30, 10)] int x)
        {
            Assert.That(x == 1 || x == 2 || x == 3 || x == 10 || x == 20 || x == 30);
        }
    }
}
