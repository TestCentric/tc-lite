// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Internal;

namespace TCLite.Attributes
{
    [TestFixture]
    public class ValuesAttributeTests
    {
        #region ValuesAttribute

        [TestCase]
        public void CanGetDataFromAttribute()
        {
            var method = GetType().GetMethod(nameof(MethodWithValues), BindingFlags.Instance | BindingFlags.Public);
            var parameter = method.GetParameters()[0];
            var attr = parameter.GetCustomAttribute(typeof(ValuesAttribute), false) as ValuesAttribute;

            Assert.That(attr.GetData(parameter), Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [TestCase]
        public void CanGetDataFromParameterDataSource()
        {
            var method = GetType().GetMethod(nameof(MethodWithValues), BindingFlags.Instance | BindingFlags.Public);
            var parameter = method.GetParameters()[0];
            var data = new ParameterDataSource(parameter).GetData(parameter);
            var expected = new[] { 1, 2, 3 };

            Assert.That(data, Is.EqualTo(expected));
        }

        [TestCase]
        public void CanGetDataFromParameterDataProvider()
        {
            var method = GetType().GetMethod(nameof(MethodWithValues), BindingFlags.Instance | BindingFlags.Public);
            var parameter = method.GetParameters()[0];
            var data = new ParameterDataProvider(method).GetDataFor(parameter);
            var expected = new[] { 1, 2, 3 };

            Assert.That(data, Is.EqualTo(expected));
        }

        public void MethodWithValues([Values(1, 2, 3)] int x)
        {
            Assert.That(x == 1 || x == 2 || x == 3);
        }

        #endregion

        #region Conversion Tests

        public void CanConvertIntsToLong([Values(5, int.MaxValue)]long x)
        {
            Assert.That(x, Is.Not.EqualTo(default(long)));
        }

        public void CanConvertIntsToNullableLong([Values(5, int.MaxValue)]long? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        public void CanConvertSmallIntsToShort([Values(5)]short x)
        {
        }

        public void CanConvertSmallIntsToNullableShort([Values(5)]short? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        public void CanConvertSmallIntsToByte([Values(5)]byte x)
        {
        }

        public void CanConvertSmallIntsToNullableByte([Values(5)]byte? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        public void CanConvertSmallIntsToSByte([Values(5)]sbyte x)
        {
        }

        public void CanConvertSmallIntsToNullableSByte([Values(5)]sbyte? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        public void CanConvertValuesToDecimal([Values(12, 12.5, "12.5")]decimal x)
        {
            Assert.That(x, Is.Not.EqualTo(default(decimal)));
        }

        public void CanConvertValuesToNullableDecimal([Values(12, 12.5, "12.5")]decimal? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        public void CanConvertStringToDateTimeOffset([Values("2018-10-09 15:15:00+02:30")]DateTimeOffset x)
        {
            Assert.That(x, Is.Not.EqualTo(default(DateTimeOffset)));
        }

        public void CanConvertStringToNullableDateTimeOffset([Values("2018-10-09 15:15:00+02:30")]DateTimeOffset? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        public void CanConvertStringToTimeSpan([Values("4:44:15")]TimeSpan x)
        {
            Assert.That(x, Is.Not.EqualTo(default(TimeSpan)));
        }

        public void CanConvertStringToNullableTimeSpan([Values("4:44:15")]TimeSpan? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        public void CanConvertStringToDateTime([Values("2018-10-10")]DateTime x)
        {
            Assert.That(x, Is.Not.EqualTo(default(DateTime)));
        }

        public void CanConvertStringToNullableDateTime([Values("2018-10-10")]DateTime? x)
        {
            Assert.That(x.HasValue, Is.True);
        }

        #endregion

        public void SupportsNullableDecimal([Values(null)] decimal? x)
        {
            Assert.That(x.HasValue, Is.False);
        }

        public void SupportsNullableDateTime([Values(null)] DateTime? dt)
        {
            Assert.That(dt.HasValue, Is.False);
        }

        public void SupportsNullableTimeSpan([Values(null)] TimeSpan? dt)
        {
            Assert.That(dt.HasValue, Is.False);
        }

        public void NullableSimpleFormalParametersWithArgument([Values(1)] int? a)
        {
            Assert.AreEqual(1, a);
        }

        public void NullableSimpleFormalParametersWithNullArgument([Values(null)] int? a)
        {
            Assert.IsNull(a);
        }

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
