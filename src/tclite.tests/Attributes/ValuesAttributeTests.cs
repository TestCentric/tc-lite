// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Attributes
{
    [TestFixture]
    public class ValuesAttributeTests
    {
        #region ValuesAttribute

        [TestCase]
        public void CanGetDataFromAttribute()
        {
            var method = GetType().GetMethod(nameof(MethodWithValues), BindingFlags.Instance | BindingFlags.NonPublic);
            var parameter = method.GetParameters()[0];
            var attr = parameter.GetCustomAttribute(typeof(ValuesAttribute), false) as ValuesAttribute;

            Assert.That(attr.GetData(parameter), Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [TestCase]
        public void CanGetDataFromParameterDataSource()
        {
            var method = GetType().GetMethod(nameof(MethodWithValues), BindingFlags.Instance | BindingFlags.NonPublic);
            var parameter = method.GetParameters()[0];
            var data = new ParameterDataSource(parameter).GetData(parameter);
            var expected = new[] { 1, 2, 3 };

            Assert.That(data, Is.EqualTo(expected));
        }

        [TestCase]
        public void CanGetDataFromParameterDataProvider()
        {
            var method = GetType().GetMethod(nameof(MethodWithValues), BindingFlags.Instance | BindingFlags.NonPublic);
            var parameter = method.GetParameters()[0];
            var data = new ParameterDataProvider(method).GetDataFor(parameter);
            var expected = new[] { 1, 2, 3 };

            Assert.That(data , Is.EqualTo(expected));
        }

        private void MethodWithValues([Values(1, 2, 3)] int x) { }

        [Combinatorial]
        public void ValuesAttributeProvidesSpecifiedValues([Values(42)] int x)
        {
            Assert.That(x, Is.EqualTo(42));
        }

        #endregion

        #region Conversion Tests

        [Combinatorial]
        public void CanConvertIntsToLong([Values(5, int.MaxValue)]long x)
        {
            Assert.That(x, Is.Not.EqualTo(default(long)));
        }

        [Combinatorial]
        public void CanConvertIntsToNullableLong([Values(5, int.MaxValue)]long? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        [Combinatorial]
        public void CanConvertSmallIntsToShort([Values(5)]short x)
        {
        }

        [Combinatorial]
        public void CanConvertSmallIntsToNullableShort([Values(5)]short? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        [Combinatorial]
        public void CanConvertSmallIntsToByte([Values(5)]byte x)
        {
        }

        [Combinatorial]
        public void CanConvertSmallIntsToNullableByte([Values(5)]byte? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        [Combinatorial]
        public void CanConvertSmallIntsToSByte([Values(5)]sbyte x)
        {
        }

        [Combinatorial]
        public void CanConvertSmallIntsToNullableSByte([Values(5)]sbyte? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        [Combinatorial]
        public void CanConvertValuesToDecimal([Values(12, 12.5, "12.5")]decimal x)
        {
            Assert.That(x, Is.Not.EqualTo(default(decimal)));
        }

        [Combinatorial]
        public void CanConvertValuesToNullableDecimal([Values(12, 12.5, "12.5")]decimal? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        [Combinatorial]
        public void CanConvertStringToDateTimeOffset([Values("2018-10-09 15:15:00+02:30")]DateTimeOffset x)
        {
            Assert.That(x, Is.Not.EqualTo(default(DateTimeOffset)));
        }

        [Combinatorial]
        public void CanConvertStringToNullableDateTimeOffset([Values("2018-10-09 15:15:00+02:30")]DateTimeOffset? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        [Combinatorial]
        public void CanConvertStringToTimeSpan([Values("4:44:15")]TimeSpan x)
        {
            Assert.That(x, Is.Not.EqualTo(default(TimeSpan)));
        }

        [Combinatorial]
        public void CanConvertStringToNullableTimeSpan([Values("4:44:15")]TimeSpan? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        [Combinatorial]
        public void CanConvertStringToDateTime([Values("2018-10-10")]DateTime x)
        {
            Assert.That(x, Is.Not.EqualTo(default(DateTime)));
        }

        [Combinatorial]
        public void CanConvertStringToNullableDateTime([Values("2018-10-10")]DateTime? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        #endregion

        [Combinatorial]
        public void SupportsNullableDecimal([Values(null)] decimal? x)
        {
            Assert.That(x.HasValue, Is.False);
        }

        [Combinatorial]
        public void SupportsNullableDateTime([Values(null)] DateTime? dt)
        {
            Assert.That(dt.HasValue, Is.False);
        }

        [Combinatorial]
        public void SupportsNullableTimeSpan([Values(null)] TimeSpan? dt)
        {
            Assert.That(dt.HasValue, Is.False);
        }

        [Combinatorial]
        public void NullableSimpleFormalParametersWithArgument([Values(1)] int? a)
        {
            Assert.AreEqual(1, a);
        }

        [Combinatorial]
        public void NullableSimpleFormalParametersWithNullArgument([Values(null)] int? a)
        {
            Assert.IsNull(a);
        }

        [Combinatorial]
        public void MethodWithArrayArguments([Values(
            (object)new object[] { 1, "text", null },
            (object)new object[0],
            (object)new object[] { 1, new int[] { 2, 3 }, 4 },
            (object)new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })] object o)
        {
            string testName = TestExecutionContext.CurrentContext.CurrentTest.Name;
            // TODO: We really want the contents to be reflected in the name
            string expectedName = @"MethodWithArrayArguments(System.Object[])";
                
            Assert.That(testName, Is.EqualTo(expectedName));
        }

        // [TestCase]
        // public void TestNameIntrospectsArrayValues()
        // {
        //     TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
        //         GetType(), nameof(MethodWithArrayArguments));

        //     Assert.That(suite.TestCaseCount, Is.EqualTo(4));

        //     Assert.Multiple(() =>
        //     {
        //         Assert.That(suite.Tests[0].Name, Is.EqualTo(@"MethodWithArrayArguments([1, ""text"", null])"));
        //         Assert.That(suite.Tests[1].Name, Is.EqualTo(@"MethodWithArrayArguments([])"));
        //         Assert.That(suite.Tests[2].Name, Is.EqualTo(@"MethodWithArrayArguments([1, Int32[], 4])"));
        //         Assert.That(suite.Tests[3].Name, Is.EqualTo(@"MethodWithArrayArguments([1, 2, 3, 4, 5, ...])"));
        //     });
        // }
    }
}
