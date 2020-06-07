#if NYI
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Builders;
using TCLite.TestData;
using TCLite.TestUtilities;

namespace TCLite.Framework.Internal
{
	[TestFixture]
	public class NUnitTestCaseBuilderTests
	{
        private static readonly System.Type fixtureType = typeof(AsyncDummyFixture);

        [TestCase("AsyncVoid", RunState.Runnable)]
        [TestCase("AsyncTask", RunState.Runnable)]
        [TestCase("AsyncGenericTask", RunState.NotRunnable)]
        [TestCase("NonAsyncTask", RunState.NotRunnable)]
        [TestCase("NonAsyncGenericTask", RunState.NotRunnable)]
        public void AsyncTests(string methodName, RunState expectedState)
        {
            var test = TestBuilder.MakeTestCase(fixtureType, methodName);
            Assert.That(test.RunState, Is.EqualTo(expectedState));
        }

        [TestCase("AsyncVoidTestCase", RunState.Runnable)]
        [TestCase("AsyncVoidTestCaseWithExpectedResult", RunState.NotRunnable)]
        [TestCase("AsyncTaskTestCase", RunState.Runnable)]
        [TestCase("AsyncTaskTestCaseWithExpectedResult", RunState.NotRunnable)]
        [TestCase("AsyncGenericTaskTestCase", RunState.NotRunnable)]
        [TestCase("AsyncGenericTaskTestCaseWithExpectedResult", RunState.Runnable)]
        [TestCase("AsyncGenericTaskTestCaseWithExpectedException", RunState.Runnable)]
        public void AsyncTestCases(string methodName, RunState expectedState)
        {
            var suite = TestBuilder.MakeTestCase(fixtureType, methodName);
            var testCase = (Test)suite.Tests[0];
            Assert.That(testCase.RunState, Is.EqualTo(expectedState));
        }
    }
}
#endif
