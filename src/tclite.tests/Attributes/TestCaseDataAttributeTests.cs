// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TCLite.Interfaces;

namespace TCLite.Attributes
{
    public class TestCaseDataAttributeTests : TestSourceMayBeInherited
    {
        #region Tests With Static Members as Source

        [TestCaseData(nameof(StaticProperty))]
        public void SourceCanBeStaticProperty(string source)
        {
            Assert.AreEqual("StaticProperty", source);
        }

        [TestCaseData(nameof(InheritedStaticProperty))]
        public void TestSourceCanBeInheritedStaticProperty(bool source)
        {
            Assert.AreEqual(true, source);
        }

        static IEnumerable StaticProperty => new object[] { new object[] { "StaticProperty" } };

        [TestCaseData(nameof(StaticMethod))]
        public void SourceCanBeStaticMethod(string source)
        {
            Assert.AreEqual("StaticMethod", source);
        }

        static IEnumerable StaticMethod() => new object[] { new object[] { "StaticMethod" } };

        [TestCaseData(nameof(StaticField))]
        public void SourceCanBeStaticField(string source)
        {
            Assert.AreEqual("StaticField", source);
        }

        static object[] StaticField =
            { new object[] { "StaticField" } };

        #endregion

        #region Tests With Instance Members as Source

        [TestCase]
        public void SourceUsingInstancePropertyIsNotRunnable()
        {
            CheckCasesAreNotRunnable(nameof(InstanceProperty));
        }

        IEnumerable InstanceProperty => new object[] { new object[] { "InstanceProperty" } };

        [TestCase]
        public void SourceUsingInstanceMethodIsNotRunnable()
        {
            CheckCasesAreNotRunnable(nameof(InstanceMethod));
        }

        IEnumerable InstanceMethod() => new object[] { new object[] { "InstanceMethod" } };

        [TestCase]
        public void SourceUsingInstanceFieldIsNotRunnable()
        {
            CheckCasesAreNotRunnable(nameof(InstanceField));
        }

        IEnumerable  InstanceField = new object[] { new object[] { "InstanceField" } };
        private static void CheckCasesAreNotRunnable(string sourceName)
        {
            var attr = new TestCaseDataAttribute(sourceName);
            var cases = attr.GetTestCasesFor(DummyMethodInfo);
            foreach (ITestCaseData data in cases)
                Assert.That(data.RunState == RunState.NotRunnable);
        }

        private static void DummyTestMethod() { }

        private static MethodInfo DummyMethodInfo =
            typeof(TestCaseDataAttributeTests).GetMethod("DummyTestMethod", BindingFlags.NonPublic | BindingFlags.Static);

        #endregion

        #region Tests For Various Forms Of Data Supplied
        
        [TestCaseData(nameof(MyData))]
        public void ObjectArrayOfObjectArrays(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        static object[] MyData = new object[] {
            new object[] { 12, 3, 4 },
            new object[] { 12, 4, 3 },
            new object[] { 12, 6, 2 } };

        [TestCaseData(nameof(MyIntData))]
        public void ObjectArrayOfIntArrays(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        static object[] MyIntData = new object[] {
            new int[] { 12, 3, 4 },
            new int[] { 12, 4, 3 },
            new int[] { 12, 6, 2 } };

        [TestCaseData(nameof(Params))]
        public int ObjectArrayOfTestCaseData(int n, int d)
        {
            return n / d;
        }

        static object[] Params = new object[] {
            new TestCaseData(24, 3).Returns(8),
            new TestCaseData(24, 2).Returns(12) };

        [TestCaseData(nameof(MyData))]
        [TestCaseData(nameof(MoreData), Category = "Extra")]
        [TestCase(12, 2, 6)]
        public void TestMayUseMultipleSourceAttributes(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        static object[] MoreData = new object[] {
            new object[] { 12, 1, 12 },
            new object[] { 12, 2, 6 } };

        [TestCaseData(nameof(FourArgs))]
        public void TestCaseDataArray(int n, int d, int q, int r)
        {
            Assert.AreEqual(q, n / d);
            Assert.AreEqual(r, n % d);
        }

        static TestCaseData[] FourArgs = new TestCaseData[] {
            new TestCaseData( 12, 3, 4, 0 ),
            new TestCaseData( 12, 4, 3, 0 ),
            new TestCaseData( 12, 5, 2, 2 ) };

        #endregion

        #region Source in a separate class

        [TestCaseData(typeof(DivideDataProvider), nameof(DivideDataProvider.HereIsTheData))]
        public void SourceInAnotherClass(int n, int d, int q)
        {
            Assert.AreEqual(q, n / d);
        }

        private static class DivideDataProvider
        {
            public static IEnumerable HereIsTheData
            {
                get
                {
                    yield return new object[] { 100, 20, 5 };
                    yield return new object[] { 100, 4, 25 };
                }
            }
        }

        [TestCaseData(typeof(DivideDataProviderWithExpectedResults), nameof(DivideDataProviderWithExpectedResults.TestCases))]
        public int SourceInAnotherClassWithExpectedResults(int n, int d)
        {
            return n / d;
        }

        private static class DivideDataProviderWithExpectedResults
        {
            public static IEnumerable TestCases => new TestCaseData[]
            {
                new TestCaseData(12, 3).Returns(4),
                new TestCaseData(12, 2).Returns(6),
                new TestCaseData(12, 4).Returns(3)
            };
        }

        #endregion

        #region Special Cases

        [TestCaseData(nameof(MyArrayData))]
        public void MustPassTestCaseDataArrayWhenSingleArgIsAnArray(int[] array)
        {
            Assert.That(true);
        }

        static TestCaseData[] MyArrayData = new TestCaseData[]
        {
            new TestCaseData(new int[] { 12 }),
            new TestCaseData(new int[] { 12, 4 }),
            new TestCaseData(new int[] { 12, 6, 2 })
        };

        [TestCaseData(nameof(EvenNumbers))]
        public void CanPassIntArrayWhenSingleArgIsInt(int n)
        {
            Assert.AreEqual(0, n % 2);
        }

        static int[] EvenNumbers = new int[] { 2, 4, 6, 8 };

        [TestCaseData(nameof(ZeroTestCasesSource))]
        public void SourceGeneratingNoCasesShouldPass(int requiredParameter)
        {
        }

        private static IEnumerable<TestCaseData> ZeroTestCasesSource() => Enumerable.Empty<TestCaseData>();

        #endregion

        #region Ignoring Test Cases

        [ExpectIgnored("Ignoring this!")]
        [TestCaseData(nameof(IgnoreOneItem))]
        public void CanIgnoreIndividualTestCase()
        {
            Assert.Fail("Test should not be run!");
        }

        static TestCaseData[] IgnoreOneItem = new TestCaseData[]
        {
            new TestCaseData().Ignore("Ignoring this!")
        };

        #endregion

        #region Description

        [TestCaseData(nameof(DataWithDescriptions))]
        public void CanSpecifyDescriptionOnEachCase(string description)
        {
            Assert.AreEqual(description, TestContext.CurrentTest.Description);
        }

        [TestCaseData(nameof(DataWithDescriptions), Description="OVERRIDDEN")]
        public void DescriptionPropertyOverridesIndividualDescriptions(string description)
        {
            Assert.AreEqual("OVERRIDDEN", TestContext.CurrentTest.Description);
        }

        private const string SHORT_DESCRIPTION = "Short Description";
        private const string LONG_DESCRIPTION = "This is a really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really long description";
        private static TestCaseData[] DataWithDescriptions = new TestCaseData[]
        {
            new TestCaseData(SHORT_DESCRIPTION).SetDescription(SHORT_DESCRIPTION),
            new TestCaseData(LONG_DESCRIPTION).SetDescription(LONG_DESCRIPTION)
        };

        #endregion

        [TestCaseData(nameof(CategoryData), Category="XYZ")]
        public void CanSpecifyCategory(int x)
        {
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("XYZ"));
        }

        [TestCaseData(nameof(CategoryData), Category = "X,Y,Z")]
        public void CanSpecifyMultipleCategories(int x)
        {
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("X"));
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("Y"));
            Assert.That(TestContext.CurrentTest.Categories, Contains.Item.EqualTo("Z"));
        }

        private static TestCaseData[] CategoryData = new TestCaseData[]
        {
            new TestCaseData(1)
        };

        [TestCaseData(nameof(testCases))]
        public void MethodTakingTwoStringArrays(string[] a, string[] b)
        {
            Assert.That(a, Is.TypeOf(typeof(string[])));
            Assert.That(b, Is.TypeOf(typeof(string[])));
        }

        static object[] testCases =
        {
            new TestCaseData(
                new string[] { "A" },
                new string[] { "B" })
        };

#if NYI // Source return string array
        [TestCaseData(nameof(SingleMemberArrayAsArgument))]
        public void Issue1337SingleMemberArrayAsArgument(string[] args)
        {
            Assert.That(args.Length == 1 && args[0] == "1");
        }

        static string[][] SingleMemberArrayAsArgument = { new[] { "1" }  };
#endif

        #region Sources used by the tests

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
