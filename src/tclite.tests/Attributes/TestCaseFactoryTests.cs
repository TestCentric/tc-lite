// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
    [TestFixture]
    public class TestCaseFactoryTests
    {
        [TestCaseFactory(typeof(DivideDataProvider))]
        public void CanUseTestCaseFactory(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        [TestCaseFactory(typeof(DivideDataProviderWithExpectedResult))]
        public int CanUseTestCaseFactoryWithExpectedResult(int n, int d)
        {
            return n / d;
        }

        [TestCase]
        public void InvalidFactoryReturnsOneNonRunnableTestCase()
        {
            var attr = new TestCaseFactoryAttribute(typeof(NotValidAsATestCaseFactory));
            var method = GetType().GetMethod(nameof(DummyTest));

            int count = 0;
            foreach(var testCase in attr.GetTestCasesFor(method))
            {
                Assert.That(count++ == 0, "More than one test case returned");

                Assert.That(testCase.RunState, Is.EqualTo(RunState.NotRunnable));
                Assert.That(testCase.Properties.Get(PropertyNames.SkipReason),
                    Contains.Substring("not a test case factory"));
            }
            
            Assert.That(count, Is.EqualTo(1), "No test cases were returned");
        }

        public void DummyTest(int x, int y) { }

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

#if NYI // TestBuilder
        [TestCase]
        public void TestMethodIsNotRunnableWhenSourceDoesNotExist()
        {
            TestSuite suiteToTest = TestBuilder.MakeParameterizedMethodSuite(typeof(TestCaseSourceAttributeFixture), nameof(TestCaseSourceAttributeFixture.MethodWithNonExistingSource));
            
            Assert.That(suiteToTest.Tests.Count == 1);
            Assert.AreEqual(RunState.NotRunnable, suiteToTest.Tests[0].RunState);
        }
#endif

        #region Test Case Factories

        private class DivideDataProvider : ITestCaseFactory
        {
            public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
            {
                yield return new TestCaseData( 100, 20, 5 );
                yield return new TestCaseData( 100, 4, 25 );
            }
        }

        public class DivideDataProviderWithExpectedResult : ITestCaseFactory
        {
            public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
            {
                yield return new TestCaseData(12, 3).Returns(4);
                yield return new TestCaseData(12, 2).Returns(6);
                yield return new TestCaseData(12, 4).Returns(3);
            }
        }

        public class NotValidAsATestCaseFactory
        {
        }

        #endregion
    }
}
