// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using TCLite.Internal;

#if TASK_PARALLEL_LIBRARY_API
using System.Threading.Tasks;
#endif

#if NET40
using Task = System.Threading.Tasks.TaskEx;
#endif

namespace TCLite.Attributes
{
    [TestFixture]
    public class TestCaseAttributeTests
    {
        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        public void IntegerDivisionWithResultPassedToTest(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCase(12, 3, ExpectedResult = 4)]
        [TestCase(12, 2, ExpectedResult = 6)]
        [TestCase(12, 4, ExpectedResult = 3)]
        public int IntegerDivisionWithResultCheckedByNUnit(int n, int d)
        {
            return n / d;
        }

        [TestCase(2, 2, ExpectedResult=4)]
        public double CanConvertIntToDouble(double x, double y)
        {
            return x + y;
        }

        [TestCase("2.2", "3.3", ExpectedResult = 5.5)]
        public decimal CanConvertStringToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase(2.2, 3.3, ExpectedResult = 5.5)]
        public decimal CanConvertDoubleToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public decimal CanConvertIntToDecimal(decimal x, decimal y)
        {
            return x + y;
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public short CanConvertSmallIntsToShort(short x, short y)
        {
            return (short)(x + y);
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public byte CanConvertSmallIntsToByte(byte x, byte y)
        {
            return (byte)(x + y);
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public sbyte CanConvertSmallIntsToSByte(sbyte x, sbyte y)
        {
            return (sbyte)(x + y);
        }

#if NYI // TestBuilder
        [TestCase(nameof(TestCaseAttributeFixture.MethodCausesConversionOverflow), RunState.NotRunnable)]
        [TestCase(nameof(TestCaseAttributeFixture.VoidTestCaseWithExpectedResult), RunState.NotRunnable)]
        [TestCase(nameof(TestCaseAttributeFixture.TestCaseWithNullableReturnValueAndNullExpectedResult), RunState.Runnable)]
        public void TestCaseRunnableState(string methodName, RunState expectedState)
        {
            var test = (Test)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), methodName).Tests[0];
            Assert.AreEqual(expectedState, test.RunState);
        }
#endif

        [TestCase("12-October-1942")]
        public void CanConvertStringToDateTime(DateTime dt)
        {
            Assert.AreEqual(1942, dt.Year);
        }

        [TestCase("1942-10-12")]
        public void CanConvertIso8601DateStringToDateTime(DateTime dt)
        {
            Assert.AreEqual(new DateTime(1942,10,12), dt);
        }

        [TestCase("1942-10-12", ExpectedResult = "1942-10-12")]
        public DateTime CanConvertExpectedResultStringToDateTime(DateTime dt)
        {
            return dt;
        }

        [TestCase("4:44:15")]
        public void CanConvertStringToTimeSpan(TimeSpan ts)
        {
            Assert.AreEqual(4, ts.Hours);
            Assert.AreEqual(44, ts.Minutes);
            Assert.AreEqual(15, ts.Seconds);
        }

        [TestCase("4:44:15", ExpectedResult = "4:44:15")]
        public TimeSpan CanConvertExpectedResultStringToTimeSpan(TimeSpan ts)
        {
            return ts;
        }

        [TestCase("2018-10-09 15:15:00+02:30")]
        public void CanConvertStringToDateTimeOffset(DateTimeOffset offset)
        {
            Assert.AreEqual(2018, offset.Year);
            Assert.AreEqual(10, offset.Month);
            Assert.AreEqual(9, offset.Day);

            Assert.AreEqual(15, offset.Hour);
            Assert.AreEqual(15, offset.Minute);
            Assert.AreEqual(0, offset.Second);

            Assert.AreEqual(2, offset.Offset.Hours);
            Assert.AreEqual(30, offset.Offset.Minutes);
        }

        [TestCase("2018-10-09 15:15:00+02:30", ExpectedResult = "2018-10-09 15:15:00+02:30")]
        public DateTimeOffset CanConvertExpectedResultStringToDateTimeOffset(DateTimeOffset offset)
        {
            return offset;
        }

        [TestCase(null)]
        public void CanPassNullAsFirstArgument(object a)
        {
            Assert.IsNull(a);
        }

        [TestCase(new object[] { 1, "two", 3.0 })]
        [TestCase(new object[] { "zip" })]
        public void CanPassObjectArrayAsFirstArgument(object[] a)
        {
        }

        [TestCase(new object[] { "a", "b" })]
        public void CanPassArrayAsArgument(object[] array)
        {
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
        }

#if NYI // Invalid?
        [TestCase("a", "b")]
        public void ArgumentsAreCoalescedInObjectArray(object[] array)
        {
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
        }
#endif        

        [TestCase(1, "b")]
        public void ArgumentsOfDifferentTypeAreCoalescedInObjectArray(object[] array)
        {
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual("b", array[1]);
        }

        [TestCase(new object[] { null })]
        public void NullArgumentsAreCoalescedInObjectArray(object[] array)
        {
            Assert.That(array, Is.EqualTo(new object[] { null }));
        }

        [TestCase(ExpectedResult = null)]
        public object ResultCanBeNull()
        {
            return null;
        }

        [TestCase("a", "b")]
        public void HandlesParamsArrayAsSoleArgument(params string[] array)
        {
            Assert.AreEqual("a", array[0]);
            Assert.AreEqual("b", array[1]);
        }

        [TestCase("a")]
        public void HandlesParamsArrayWithOneItemAsSoleArgument(params string[] array)
        {
            Assert.AreEqual("a", array[0]);
        }

        [TestCase("a", "b", "c", "d")]
        public void HandlesParamsArrayAsLastArgument(string s1, string s2, params object[] array)
        {
            Assert.AreEqual("a", s1);
            Assert.AreEqual("b", s2);
            Assert.AreEqual("c", array[0]);
            Assert.AreEqual("d", array[1]);
        }

        [TestCase("a", "b")]
        public void HandlesParamsArrayWithNoItemsAsLastArgument(string s1, string s2, params object[] array)
        {
            Assert.AreEqual("a", s1);
            Assert.AreEqual("b", s2);
            Assert.AreEqual(0, array.Length);
        }

        [TestCase("a", "b", "c")]
        public void HandlesParamsArrayWithOneItemAsLastArgument(string s1, string s2, params object[] array)
        {
            Assert.AreEqual("a", s1);
            Assert.AreEqual("b", s2);
            Assert.AreEqual("c", array[0]);
        }

#if NYI // TestCase optional arguments
        [TestCase("x", ExpectedResult = new []{"x", "b", "c"})]
        [TestCase("x", "y", ExpectedResult = new[] { "x", "y", "c" })]
        [TestCase("x", "y", "z", ExpectedResult = new[] { "x", "y", "z" })]
        public string[] HandlesOptionalArguments(string s1, string s2 = "b", string s3 = "c")
        {
            return new[] {s1, s2, s3};
        }

        [TestCase(ExpectedResult = new []{"a", "b"})]
        [TestCase("x", ExpectedResult = new[] { "x", "b" })]
        [TestCase("x", "y", ExpectedResult = new[] { "x", "y" })]
        public string[] HandlesAllOptionalArguments(string s1 = "a", string s2 = "b")
        {
            return new[] {s1, s2};
        }
#endif

        private const string SHORT_DESCRIPTION = "Short Description";
        [TestCase(Description = SHORT_DESCRIPTION)]
        public void CanSpecifyDescription()
        {
            Assert.AreEqual(SHORT_DESCRIPTION, TestContext.CurrentTest.Description);
        }

        private const string LONG_DESCRIPTION = "This is a really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really long description";
        [TestCase(Description = LONG_DESCRIPTION)]
        public void CanSpecifyLongDescription()
        {
            Assert.AreEqual(LONG_DESCRIPTION, TestContext.CurrentTest.Description);
        }

        [TestCase(Category = "XYZ")]
        public void CanSpecifyCategory()
        {
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("XYZ"));
        }

        [TestCase(Category = "X,Y,Z")]
        public void CanSpecifyMultipleCategories()
        {
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("X"));
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("Y"));
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("Z"));
        }

        [ExpectIgnored()]
        [TestCase(Ignore = true)]
        public void CanIgnoreIndividualTestCase()
        {
            Assert.Fail("Test should not be run!");
        }

        [ExpectIgnored("My Reason")]
        [TestCase(Ignore = true, Reason = "My Reason")]
        public void CanIgnoreIndividualTestCaseWithReasonSpecified()
        {
            Assert.Fail("Test should not be run!");
        }

#if NYI // TestBuilder
        [TestCase]
        public void CanSpecifyTestName_FixedText()
        {
            Test test = (Test)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), nameof(TestCaseAttributeFixture.MethodHasTestNameSpecified_FixedText)).Tests[0];
            Assert.AreEqual("XYZ", test.Name);
            Assert.AreEqual("NUnit.TestData.TestCaseAttributeFixture.TestCaseAttributeFixture.XYZ", test.FullName);
        }

        [TestCase]
        public void CanSpecifyTestName_WithMethodName()
        {
            Test test = (Test)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), nameof(TestCaseAttributeFixture.MethodHasTestNameSpecified_WithMethodName)).Tests[0];
            var expectedName = "MethodHasTestNameSpecified_WithMethodName+XYZ";
            Assert.AreEqual(expectedName, test.Name);
            Assert.AreEqual("NUnit.TestData.TestCaseAttributeFixture.TestCaseAttributeFixture." + expectedName, test.FullName);
        }

        [TestCase]
        public void CanIncludePlatform()
        {
            bool isLinux = OSPlatform.CurrentPlatform.IsUnix;
            bool isMacOSX = OSPlatform.CurrentPlatform.IsMacOSX;

            const string methodName = nameof(TestCaseAttributeFixture.MethodWithIncludePlatform);
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), methodName);

            Test testCase1 = TestFinder.Find($"{methodName}(1)", suite, false);
            Test testCase2 = TestFinder.Find($"{methodName}(2)", suite, false);
            Test testCase3 = TestFinder.Find($"{methodName}(3)", suite, false);
            Test testCase4 = TestFinder.Find($"{methodName}(4)", suite, false);
            if (isLinux)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase4.RunState, Is.EqualTo(RunState.Skipped));
            }
            else if (isMacOSX)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase4.RunState, Is.EqualTo(RunState.Skipped));
            }
            else
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase4.RunState, Is.EqualTo(RunState.Skipped));
            }
        }

        [TestCase]
        public void CanExcludePlatform()
        {
            bool isLinux = OSPlatform.CurrentPlatform.IsUnix;
            bool isMacOSX = OSPlatform.CurrentPlatform.IsMacOSX;

            const string methodName = nameof(TestCaseAttributeFixture.MethodWithExcludePlatform);
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), methodName);

            Test testCase1 = TestFinder.Find($"{methodName}(1)", suite, false);
            Test testCase2 = TestFinder.Find($"{methodName}(2)", suite, false);
            Test testCase3 = TestFinder.Find($"{methodName}(3)", suite, false);
            Test testCase4 = TestFinder.Find($"{methodName}(4)", suite, false);
            if (isLinux)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase4.RunState, Is.EqualTo(RunState.Runnable));
            }
            else if (isMacOSX)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase4.RunState, Is.EqualTo(RunState.Runnable));
            }
            else
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase4.RunState, Is.EqualTo(RunState.Runnable));
            }
        }

        [TestCase]
        public void CanIncludeRuntime()
        {
            bool isNetCore;
            Type monoRuntimeType = Type.GetType("Mono.Runtime", false);
            bool isMono = monoRuntimeType != null;
#if NETCOREAPP
            isNetCore = true;
#else
            isNetCore = false;
#endif

            const string methodName = nameof(TestCaseAttributeFixture.MethodWithIncludeRuntime);
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), methodName);

            Test testCase1 = TestFinder.Find($"{methodName}(1)", suite, false);
            Test testCase2 = TestFinder.Find($"{methodName}(2)", suite, false);
            Test testCase3 = TestFinder.Find($"{methodName}(3)", suite, false);
            if (isNetCore)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Skipped));
            }
            else if (isMono)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Runnable));
            }
            else
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Skipped));
            }
        }

        [TestCase]
        public void CanExcludeRuntime()
        {
            bool isNetCore;
            Type monoRuntimeType = Type.GetType("Mono.Runtime", false);
            bool isMono = monoRuntimeType != null;
#if NETCOREAPP
            isNetCore = true;
#else
            isNetCore = false;
#endif

            const string methodName = nameof(TestCaseAttributeFixture.MethodWithExcludeRuntime);
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), methodName);

            Test testCase1 = TestFinder.Find($"{methodName}(1)", suite, false);
            Test testCase2 = TestFinder.Find($"{methodName}(2)", suite, false);
            Test testCase3 = TestFinder.Find($"{methodName}(3)", suite, false);
            if (isNetCore)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Runnable));
            }
            else if (isMono)
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Skipped));
            }
            else
            {
                Assert.That(testCase1.RunState, Is.EqualTo(RunState.Skipped));
                Assert.That(testCase2.RunState, Is.EqualTo(RunState.Runnable));
                Assert.That(testCase3.RunState, Is.EqualTo(RunState.Runnable));
            }
        }

        [TestCase]
        public void TestNameIntrospectsArrayValues()
        {
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseAttributeFixture), nameof(TestCaseAttributeFixture.MethodWithArrayArguments));

            Assert.That(suite.TestCaseCount, Is.EqualTo(4));
            var expectedNames = new[]
            {
                @"MethodWithArrayArguments([])",
                @"MethodWithArrayArguments([1, ""text"", null])",
                @"MethodWithArrayArguments([1, Int32[], 4])",
                @"MethodWithArrayArguments([1, 2, 3, 4, 5, ...])"
            };
            Assert.That(suite.Tests.Select(t => t.Name), Is.EquivalentTo(expectedNames));
        }
#endif

        #region Nullable<> tests

#if NYI // Nullable<> test case arguments
        [TestCase(12, 3, 4)]
        [TestCase(12, 2, 6)]
        [TestCase(12, 4, 3)]
        public void NullableIntegerDivisionWithResultPassedToTest(int? n, int? d, int? q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCase(12, 3, ExpectedResult = 4)]
        [TestCase(12, 2, ExpectedResult = 6)]
        [TestCase(12, 4, ExpectedResult = 3)]
        public int? NullableIntegerDivisionWithResultCheckedByNUnit(int? n, int? d)
        {
            return n / d;
        }

        [TestCase(2, 2, ExpectedResult = 4)]
        public double? CanConvertIntToNullableDouble(double? x, double? y)
        {
            return x + y;
        }

        [TestCase(1)]
        public void CanConvertIntToNullableShort(short? x)
        {
            Assert.That(x.HasValue);
            Assert.That(x.Value, Is.EqualTo(1));
        }

        [TestCase(1)]
        public void CanConvertIntToNullableByte(byte? x)
        {
            Assert.That(x.HasValue);
            Assert.That(x.Value, Is.EqualTo(1));
        }

        [TestCase(1)]
        public void CanConvertIntToNullableSByte(sbyte? x)
        {
            Assert.That(x.HasValue);
            Assert.That(x.Value, Is.EqualTo(1));
        }

        [TestCase(1)]
        public void CanConvertIntToNullableLong(long? x)
        {
            Assert.That(x.HasValue);
            Assert.That(x.Value, Is.EqualTo(1));
        }

        [TestCase(1)]
        public void CanConvertIntToLong(long x)
        {
            Assert.That(x, Is.EqualTo(1));
        }

        [TestCase("2.2", "3.3", ExpectedResult = 5.5)]
        public decimal? CanConvertStringToNullableDecimal(decimal? x, decimal? y)
        {
            Assert.That(x.HasValue);
            Assert.That(y.HasValue);
            return x.Value + y.Value;
        }

        [TestCase(null)]
        public void SupportsNullableDecimal(decimal? x)
        {
            Assert.That(x.HasValue, Is.False);
        }

        [TestCase(2.2, 3.3, ExpectedResult = 5.5)]
        public decimal? CanConvertDoubleToNullableDecimal(decimal? x, decimal? y)
        {
            return x + y;
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public decimal? CanConvertIntToNullableDecimal(decimal? x, decimal? y)
        {
            return x + y;
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public short? CanConvertSmallIntsToNullableShort(short? x, short? y)
        {
            return (short)(x + y);
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public byte? CanConvertSmallIntsToNullableByte(byte? x, byte? y)
        {
            return (byte)(x + y);
        }

        [TestCase(5, 2, ExpectedResult = 7)]
        public sbyte? CanConvertSmallIntsToNullableSByte(sbyte? x, sbyte? y)
        {
            return (sbyte)(x + y);
        }

        [TestCase("12-October-1942")]
        public void CanConvertStringToNullableDateTime(DateTime? dt)
        {
            Assert.That(dt.HasValue);
            Assert.AreEqual(1942, dt.Value.Year);
        }

        [TestCase(null)]
        public void SupportsNullableDateTime(DateTime? dt)
        {
            Assert.That(dt.HasValue, Is.False);
        }

        [TestCase("4:44:15")]
        public void CanConvertStringToNullableTimeSpan(TimeSpan? ts)
        {
            Assert.That(ts.HasValue);
            Assert.AreEqual(4, ts.Value.Hours);
            Assert.AreEqual(44, ts.Value.Minutes);
            Assert.AreEqual(15, ts.Value.Seconds);
        }

        [TestCase(null)]
        public void SupportsNullableTimeSpan(TimeSpan? dt)
        {
            Assert.That(dt.HasValue, Is.False);
        }

        [TestCase(1)]
        public void NullableSimpleFormalParametersWithArgument(int? a)
        {
            Assert.AreEqual(1, a);
        }

        [TestCase(null)]
        public void NullableSimpleFormalParametersWithNullArgument(int? a)
        {
            Assert.IsNull(a);
        }

        [TestCase(null, ExpectedResult = null)]
        [TestCase(1, ExpectedResult = 1)]
        public int? TestCaseWithNullableReturnValue(int? a)
        {
            return a;
        }

        [TestCase(1, ExpectedResult = 1)]
        public T TestWithGenericReturnType<T>(T arg1)
        {
            return arg1;
        }

#if TASK_PARALLEL_LIBRARY_API
        [TestCase(1, ExpectedResult = 1)]
        public async Task<T> TestWithAsyncGenericReturnType<T>(T arg1)
        {
            return await Task.Run(() => arg1);
        }
#endif
#endif

        #endregion
    }
}
