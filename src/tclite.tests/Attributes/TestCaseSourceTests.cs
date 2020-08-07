// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Linq;
using TCLite.Internal;

namespace TCLite.Attributes
{
    [TestFixture]
    public class TestCaseSourceTests : TestSourceMayBeInherited
    {
        #region Tests With Static and Instance Members as Source

        [TestCaseSource(nameof(StaticProperty))]
        public void SourceCanBeStaticProperty(string source)
        {
            Assert.AreEqual("StaticProperty", source);
        }

        [TestCaseSource(nameof(InheritedStaticProperty))]
        public void TestSourceCanBeInheritedStaticProperty(bool source)
        {
            Assert.AreEqual(true, source);
        }

        static IEnumerable StaticProperty
        {
            get { return new object[] { new object[] { "StaticProperty" } }; }
        }

#if NYI // TestBuilder
        [TestCase]
        public void SourceUsingInstancePropertyIsNotRunnable()
        {
            var result = TestBuilder.RunParameterizedMethodSuite(typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithInstancePropertyAsSource));
            Assert.AreEqual(result.Children.ToArray()[0].ResultState, ResultState.NotRunnable);
        }
#endif        

        [TestCaseSource(nameof(StaticMethod))]
        public void SourceCanBeStaticMethod(string source)
        {
            Assert.AreEqual("StaticMethod", source);
        }

        static IEnumerable StaticMethod()
        {
            return new object[] { new object[] { "StaticMethod" } };
        }

#if NYI // TestBuilder
        [TestCase]
        public void SourceUsingInstanceMethodIsNotRunnable()
        {
            var result = TestBuilder.RunParameterizedMethodSuite(typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithInstanceMethodAsSource));
            Assert.AreEqual(result.Children.ToArray()[0].ResultState, ResultState.NotRunnable);
        }
#endif

        IEnumerable InstanceMethod()
        {
            return new object[] { new object[] { "InstanceMethod" } };
        }

        [TestCaseSource(nameof(StaticField))]
        public void SourceCanBeStaticField(string source)
        {
            Assert.AreEqual("StaticField", source);
        }

        static object[] StaticField =
            { new object[] { "StaticField" } };

#if NYI // TestBuilder
        [TestCase]
        public void SourceUsingInstanceFieldIsNotRunnable()
        {
            var result = TestBuilder.RunParameterizedMethodSuite(typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithInstanceFieldAsSource));
            Assert.AreEqual(result.Children.ToArray()[0].ResultState, ResultState.NotRunnable);
        }
#endif        

        #endregion

        #region Test With IEnumerable Class as Source

        [TestCaseSource(typeof(DataSourceClass))]
        public void SourceCanBeInstanceOfIEnumerable(string source)
        {
            Assert.AreEqual("DataSourceClass", source);
        }

        class DataSourceClass : IEnumerable
        {
            public DataSourceClass()
            {
            }

            public IEnumerator GetEnumerator()
            {
                yield return "DataSourceClass";
            }
        }

        #endregion

        [TestCaseSource(nameof(MyData))]
        public void SourceMayReturnArgumentsAsObjectArray(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCaseSource(nameof(MyData))]
        public void TestAttributeIsOptional(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCaseSource(nameof(MyIntData))]
        public void SourceMayReturnArgumentsAsIntArray(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

#if NYI // Source is int array
        [TestCaseSource(nameof(MyArrayData))]
        public void SourceMayReturnArrayForArray(int[] array)
        {
            Assert.That(true);
        }
#endif

        [TestCaseSource(nameof(EvenNumbers))]
        public void SourceMayReturnSinglePrimitiveArgumentAlone(int n)
        {
            Assert.AreEqual(0, n % 2);
        }

        [TestCaseSource(nameof(Params))]
        public int SourceMayReturnArgumentsAsParamSet(int n, int d)
        {
            return n / d;
        }

        [TestCaseSource(nameof(MyData))]
        [TestCaseSource(nameof(MoreData), Category = "Extra")]
        [TestCase(12, 2, 6)]
        public void TestMayUseMultipleSourceAttributes(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCaseSource(nameof(FourArgs))]
        public void TestWithFourArguments(int n, int d, int q, int r)
        {
            Assert.AreEqual(q, n / d);
            Assert.AreEqual(r, n % d);
        }

        [/*Category("Top"),*/ TestCaseSource(typeof(DivideDataProvider), nameof(DivideDataProvider.HereIsTheData))]
        public void SourceMayBeInAnotherClass(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

#if NYI // TestCase source with arguments
        [/*Category("Top"),*/ TestCaseSource(typeof(DivideDataProvider), "HereIsTheDataWithParameters", new object[] { 100, 4, 25 })]
        public void SourceInAnotherClassPassingSomeDataToConstructor(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [/*Category("Top"),*/ TestCaseSource(nameof(StaticMethodDataWithParameters), new object[] { 8000, 8, 1000 })]
        public void SourceCanBeStaticMethodPassingSomeDataToConstructor(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }
#endif        

#if NYI // TestBuilder
        [TestCase]
        public void SourceInAnotherClassPassingParamsToField()
        {
            var testMethod = (TestMethod)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.SourceInAnotherClassPassingParamsToField)).Tests[0];
            Assert.AreEqual(RunState.NotRunnable, testMethod.RunState);
            ITestResult result = TestBuilder.RunTest(testMethod, null);
            Assert.AreEqual(ResultState.NotRunnable, result.ResultState);
            Assert.AreEqual("You have specified a data source field but also given a set of parameters. Fields cannot take parameters, " +
                            "please revise the 3rd parameter passed to the TestCaseSourceAttribute and either remove " +
                            "it or specify a method.", result.Message);
        }

        [TestCase]
        public void SourceInAnotherClassPassingParamsToProperty()
        {
            var testMethod = (TestMethod)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.SourceInAnotherClassPassingParamsToProperty)).Tests[0];
            Assert.AreEqual(RunState.NotRunnable, testMethod.RunState);
            ITestResult result = TestBuilder.RunTest(testMethod, null);
            Assert.AreEqual(ResultState.NotRunnable, result.ResultState);
            Assert.AreEqual("You have specified a data source property but also given a set of parameters. " +
                            "Properties cannot take parameters, please revise the 3rd parameter passed to the " +
                            "TestCaseSource attribute and either remove it or specify a method.", result.Message);
        }

        [TestCase]
        public void SourceInAnotherClassPassingSomeDataToConstructorWrongNumberParam()
        {
            var testMethod = (TestMethod)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.SourceInAnotherClassPassingSomeDataToConstructorWrongNumberParam)).Tests[0];
            Assert.AreEqual(RunState.NotRunnable, testMethod.RunState);
            ITestResult result = TestBuilder.RunTest(testMethod, null);
            Assert.AreEqual(ResultState.NotRunnable, result.ResultState);
            Assert.AreEqual("You have given the wrong number of arguments to the method in the TestCaseSourceAttribute" +
                            ", please check the number of parameters passed in the object is correct in the 3rd parameter for the " +
                            "TestCaseSourceAttribute and this matches the number of parameters in the target method and try again.", result.Message);
        }
#endif

        [TestCaseSource(typeof(DivideDataProviderWithReturnValue), nameof(DivideDataProviderWithReturnValue.TestCases))]
        public int SourceMayBeInAnotherClassWithReturn(int n, int d)
        {
            return n / d;
        }

#if NYI // TestBuilder
        [TestCase]
        public void IgnoreTakesPrecedenceOverExpectedException()
        {
            var result = TestBuilder.RunParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodCallsIgnore)).Children.ToArray()[0];
            Assert.AreEqual(ResultState.Ignored, result.ResultState);
            Assert.AreEqual("Ignore this", result.Message);
        }

        [TestCase]
        public void CanIgnoreIndividualTestCases()
        {
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithIgnoredTestCases));
            Assert.Multiple(() =>
            {
                Test testCase = TestFinder.Find("MethodWithIgnoredTestCases(1)", suite, false);
                Assert.That(testCase.RunState, Is.EqualTo(RunState.Runnable));

                testCase = TestFinder.Find("MethodWithIgnoredTestCases(2)", suite, false);
                Assert.That(testCase.RunState, Is.EqualTo(RunState.Ignored));
                Assert.That(testCase.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("Don't Run Me!"));
            });
        }

        [TestCase]
        public void CanIgnoreIndividualTestCasesWithUntilDate()
        {
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithIgnoredTestCases));

            DateTimeOffset untilDate = DateTimeOffset.Parse("4242-01-01 00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

            Test testCase = TestFinder.Find("MethodWithIgnoredTestCases(3)", suite, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Ignored));
            Assert.That(testCase.Properties.Get(PropertyNames.SkipReason), Is.EqualTo(string.Format("Ignoring until {0}. Ignore Me Until The Future", untilDate.ToString("u"))));
            Assert.That(testCase.Properties.Get(PropertyNames.IgnoreUntilDate), Is.EqualTo(untilDate.ToString("u")));

            untilDate = DateTimeOffset.Parse("1492-01-01", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

            testCase = TestFinder.Find("MethodWithIgnoredTestCases(4)", suite, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Runnable));
            Assert.That(testCase.Properties.Get(PropertyNames.IgnoreUntilDate), Is.EqualTo(untilDate.ToString("u")));

            untilDate = DateTimeOffset.Parse("4242-01-01 12:42:33Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);

            testCase = TestFinder.Find("MethodWithIgnoredTestCases(5)", suite, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Ignored));
            Assert.That(testCase.Properties.Get(PropertyNames.SkipReason), Is.EqualTo(string.Format("Ignoring until {0}. Ignore Me Until The Future", untilDate.ToString("u"))));
            Assert.That(testCase.Properties.Get(PropertyNames.IgnoreUntilDate), Is.EqualTo(untilDate.ToString("u")));
        }

        [TestCase]
        public void CanMarkIndividualTestCasesExplicit()
        {
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithExplicitTestCases));

            Test testCase = TestFinder.Find("MethodWithExplicitTestCases(1)", suite, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Runnable));

            testCase = TestFinder.Find("MethodWithExplicitTestCases(2)", suite, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Explicit));

            testCase = TestFinder.Find("MethodWithExplicitTestCases(3)", suite, false);
            Assert.That(testCase.RunState, Is.EqualTo(RunState.Explicit));
            Assert.That(testCase.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("Connection failing"));
        }

        [TestCase]
        public void HandlesExceptionInTestCaseSource()
        {
            var testMethod = (TestMethod)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithSourceThrowingException)).Tests[0];
            Assert.AreEqual(RunState.NotRunnable, testMethod.RunState);
            ITestResult result = TestBuilder.RunTest(testMethod, null);
            Assert.AreEqual(ResultState.NotRunnable, result.ResultState);
            Assert.AreEqual("System.Exception : my message", result.Message);
        }

        [TestCaseSource(nameof(exception_source)), Explicit("Used for GUI tests")]
        public void HandlesExceptionInTestCaseSource_GuiDisplay(string lhs, string rhs)
        {
            Assert.AreEqual(lhs, rhs);
        }
#endif

        private static IEnumerable<TestCaseData> ZeroTestCasesSource() => Enumerable.Empty<TestCaseData>();

        [TestCaseSource(nameof(ZeroTestCasesSource))]
        public void TestWithZeroTestSourceCasesShouldPassWithoutRequiringArguments(int requiredParameter)
        {
        }

#if NYI // TestBuilder
        [TestCase]
        public void TestMethodIsNotRunnableWhenSourceDoesNotExist()
        {
            TestSuite suiteToTest = TestBuilder.MakeParameterizedMethodSuite(typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithNonExistingSource));
            
            Assert.That(suiteToTest.Tests.Count == 1);
            Assert.AreEqual(RunState.NotRunnable, suiteToTest.Tests[0].RunState);
        }
#endif

        static object[] testCases =
        {
            new TestCaseData(
                new string[] { "A" },
                new string[] { "B" })
        };

        [TestCaseSource(nameof(testCases))]
        public void MethodTakingTwoStringArrays(string[] a, string[] b)
        {
            Assert.That(a, Is.TypeOf(typeof(string[])));
            Assert.That(b, Is.TypeOf(typeof(string[])));
        }

#if NYI // Source return string array
        [TestCaseSource(nameof(SingleMemberArrayAsArgument))]
        public void Issue1337SingleMemberArrayAsArgument(string[] args)
        {
            Assert.That(args.Length == 1 && args[0] == "1");
        }
#endif

        static string[][] SingleMemberArrayAsArgument = { new[] { "1" }  };

        #region Test name tests

#if NYI // TestBuilder
        public static IEnumerable<TestCaseData> IndividualInstanceNameTestDataSource()
        {
            var suite = (ParameterizedMethodSuite)TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture),
                nameof(TestCaseSourceAttributeFixture.TestCaseNameTestDataMethod));

            foreach (var test in suite.Tests)
            {
                var expectedName = (string)test.Properties.Get("ExpectedTestName");

                yield return new TestCaseData(test, expectedName)
                    .SetArgDisplayNames(expectedName); // SetArgDisplayNames (here) is purely cosmetic for the purposes of these tests
            }
        }

        [TestCaseSource(nameof(IndividualInstanceNameTestDataSource))]
        public static void IndividualInstanceName(ITest test, string expectedName)
        {
            Assert.That(test.Name, Is.EqualTo(expectedName));
        }

        [TestCaseSource(nameof(IndividualInstanceNameTestDataSource))]
        public static void IndividualInstanceFullName(ITest test, string expectedName)
        {
            var expectedFullName = typeof(TestCaseSourceAttributeFixture).FullName + "." + expectedName;
            Assert.That(test.FullName, Is.EqualTo(expectedFullName));
        }
#endif

        #endregion

#if NYI // TestBuilder
        [TestCase]
        public void TestNameIntrospectsArrayValues()
        {
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithArrayArguments));

            Assert.That(suite.TestCaseCount, Is.EqualTo(5));

            Assert.Multiple(() =>
            {
                Assert.That(suite.Tests[0].Name, Is.EqualTo(@"MethodWithArrayArguments([1, ""text"", System.Object])"));
                Assert.That(suite.Tests[1].Name, Is.EqualTo(@"MethodWithArrayArguments([])"));
                Assert.That(suite.Tests[2].Name, Is.EqualTo(@"MethodWithArrayArguments([1, Int32[], 4])"));
                Assert.That(suite.Tests[3].Name, Is.EqualTo(@"MethodWithArrayArguments([1, 2, 3, 4, 5, ...])"));
                Assert.That(suite.Tests[4].Name, Is.EqualTo(@"MethodWithArrayArguments([System.Byte[,]])"));
            });
        }
#endif        

        #region Sources used by the tests
        static object[] MyData = new object[] {
            new object[] { 12, 3, 4 },
            new object[] { 12, 4, 3 },
            new object[] { 12, 6, 2 } };

        static object[] MyIntData = new object[] {
            new int[] { 12, 3, 4 },
            new int[] { 12, 4, 3 },
            new int[] { 12, 6, 2 } };

        static object[] MyArrayData = new object[]
        {
            new int[] { 12 },
            new int[] { 12, 4 },
            new int[] { 12, 6, 2 }
        };

        public static IEnumerable StaticMethodDataWithParameters(int inject1, int inject2, int inject3)
        {
            yield return new object[] { inject1, inject2, inject3 };
        }

        static object[] FourArgs = new object[] {
            new TestCaseData( 12, 3, 4, 0 ),
            new TestCaseData( 12, 4, 3, 0 ),
            new TestCaseData( 12, 5, 2, 2 ) };

        static int[] EvenNumbers = new int[] { 2, 4, 6, 8 };

        static object[] MoreData = new object[] {
            new object[] { 12, 1, 12 },
            new object[] { 12, 2, 6 } };

        static object[] Params = new object[] {
            new TestCaseData(24, 3).Returns(8),
            new TestCaseData(24, 2).Returns(12) };

        private class DivideDataProvider
        {
#pragma warning disable 0169    // x is never assigned
            private static object[] myObject;
#pragma warning restore 0169

            public static IEnumerable HereIsTheDataWithParameters(int inject1, int inject2, int inject3)
            {
                yield return new object[] { inject1, inject2, inject3 };
            }
            public static IEnumerable HereIsTheData
            {
                get
                {
                    yield return new object[] { 100, 20, 5 };
                    yield return new object[] { 100, 4, 25 };
                }
            }
        }

        public class DivideDataProviderWithReturnValue
        {
            public static IEnumerable TestCases
            {
                get
                {
                    return new object[] {
                        new TestCaseData(12, 3).Returns(4).SetName("TC1"),
                        new TestCaseData(12, 2).Returns(6).SetName("TC2"),
                        new TestCaseData(12, 4).Returns(3).SetName("TC3")
                    };
                }
            }
        }

        private static IEnumerable exception_source
        {
            get
            {
                yield return new TestCaseData("a", "a");
                yield return new TestCaseData("b", "b");

                throw new System.Exception("my message");
            }
        }
        #endregion
    }

    public class TestSourceMayBeInherited
    {
        protected static IEnumerable<bool> InheritedStaticProperty
        {
            get { yield return true; }
        }
    }
}
