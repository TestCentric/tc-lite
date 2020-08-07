// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TCLite.Interfaces;
using TCLite.Internal;
using TCLite.TestUtilities;

namespace TCLite.Attributes
{
    [TestFixture]
    public class ValueSourceTests : ValueSourceMayBeInherited
    {
        public void ValueSourceCanBeStaticProperty(
            [ValueSource(nameof(StaticProperty))] string source)
        {
            Assert.AreEqual("StaticProperty", source);
        }

        static IEnumerable StaticProperty
        {
            get
            {
                yield return "StaticProperty";
            }
        }

        public void ValueSourceCanBeInheritedStaticProperty(
            [ValueSource(nameof(InheritedStaticProperty))] bool source)
        {
            Assert.AreEqual(true, source);
        }

#if WIP
        [TestCase]
        public void ValueSourceMayNotBeInstanceProperty()
        {
            var result = TestBuilder.RunParameterizedMethodSuite(GetType(), "MethodWithValueSourceInstanceProperty");
            Assert.That(result.Children.ToArray()[0].ResultState, Is.EqualTo(ResultState.NotRunnable));
        }

        private void MethodWithValueSourceInstanceProperty(
            [ValueSource(nameof(InstanceProperty))] string source)
        {
            Assert.AreEqual("InstanceProperty", source);
        }

        IEnumerable InstanceProperty
        {
            get { return new object[] { "InstanceProperty" }; }
        }
#endif

        public void ValueSourceCanBeStaticMethod(
            [ValueSource(nameof(StaticMethod))] string source)
        {
            Assert.AreEqual("StaticMethod", source);
        }

        static IEnumerable StaticMethod()
        {
            return new object[] { "StaticMethod" };
        }

#if WIP
        [TestCase]
        public void ValueSourceMayNotBeInstanceMethod()
        {
            var result = TestBuilder.RunParameterizedMethodSuite(GetType(), "MethodWithValueSourceInstanceMethod");
            Assert.That(result.Children.ToArray()[0].ResultState, Is.EqualTo(ResultState.NotRunnable));
        }

        public void MethodWithValueSourceInstanceMethod(
            [ValueSource(nameof(InstanceMethod))] string source)
        {
            Assert.AreEqual("InstanceMethod", source);
        }

        IEnumerable InstanceMethod()
        {
            return new object[] { "InstanceMethod" };
        }
#endif

        public void ValueSourceCanBeStaticField(
            [ValueSource(nameof(StaticField))] string source)
        {
            Assert.AreEqual("StaticField", source);
        }

        internal static object[] StaticField = { "StaticField" };

#if WIP
        [TestCase]
        public void ValueSourceMayNotBeInstanceField()
        {
            var result = TestBuilder.RunParameterizedMethodSuite(GetType(), "MethodWithValueSourceInstanceField");
            Assert.That(result.Children.ToArray ()[0].ResultState, Is.EqualTo(ResultState.NotRunnable));
        }

        public void MethodWithValueSourceInstanceField(
            [ValueSource(nameof(InstanceField))] string source)
        {
            Assert.AreEqual("InstanceField", source);
        }

        internal object[] InstanceField = { "InstanceField" };
#endif

#if WIP
        [Test, Sequential]
        public void MultipleArguments(
            [ValueSource(nameof(Numerators))] int n,
            [ValueSource(nameof(Denominators))] int d,
            [ValueSource(nameof(Quotients))] int q)
        {
            Assert.AreEqual(q, n / d);
        }

        internal static int[] Numerators = new int[] { 12, 12, 12 };
        internal static int[] Denominators = new int[] { 3, 4, 6 };
        internal static int[] Quotients = new int[] { 4, 3, 2 };

        [Test, Sequential]
        public void ValueSourceMayBeInAnotherClass(
            [ValueSource(typeof(DivideDataProvider), nameof(DivideDataProvider.Numerators))] int n,
            [ValueSource(typeof(DivideDataProvider), nameof(DivideDataProvider.Denominators))] int d,
            [ValueSource(typeof(DivideDataProvider), nameof(DivideDataProvider.Quotients))] int q)
        {
            Assert.AreEqual(q, n / d);
        }

        private class DivideDataProvider
        {
            internal static int[] Numerators = new int[] { 12, 12, 12 };
            internal static int[] Denominators = new int[] { 3, 4, 6 };
            internal static int[] Quotients = new int[] { 4, 3, 2 };
        }

        [TestCase]
        public void ValueSourceMayBeGeneric(
            [ValueSourceAttribute(typeof(ValueProvider), nameof(ValueProvider.IntegerProvider))] int val)
        {
            Assert.That(2 * val, Is.EqualTo(val + val));
        }

        public class ValueProvider
        {
            public static IEnumerable<int> IntegerProvider()
            {
                List<int> dataList = new List<int>();

                dataList.Add(1);
                dataList.Add(2);
                dataList.Add(4);
                dataList.Add(8);

                return dataList;
            }

            public static IEnumerable<int> ForeignNullResultProvider()
            {
                return null;
            }
        }

        public static string NullSource = null;

        public static IEnumerable<int> NullDataSourceProvider()
        {
            return null;
        }

        public static IEnumerable<int> NullDataSourceProperty
        {
            get { return null; }
        }

        [Test, Explicit("Null or nonexistent data sources definitions should not prevent other tests from run #1121")]
        public void ValueSourceMayNotBeNull(
            [ValueSource(nameof(NullSource))] string nullSource,
            [ValueSource(nameof(NullDataSourceProvider))] string nullDataSourceProvided,
            [ValueSource(typeof(ValueProvider), nameof(ValueProvider.ForeignNullResultProvider))] string nullDataSourceProvider,
            [ValueSource(typeof(object), sourceName: null)] string typeNotImplementingIEnumerableAndNullSourceName,
            [ValueSource(nameof(NullDataSourceProperty))] int nullDataSourceProperty,
            [ValueSource("SomeNonExistingMemberSource")] int nonExistingMember)
        {
            Assert.Fail();
        }

        [TestCase]
        public void ValueSourceAttributeShouldThrowInsteadOfReturningNull()
        {
            var method = new MethodWrapper(GetType(), "ValueSourceMayNotBeNull");
            var parameters = method.GetParameters();

            foreach (var parameter in parameters)
            {
                var dataSource = parameter.GetCustomAttributes<IParameterDataSource>(false)[0];
                Assert.Throws<InvalidDataSourceException>(() => dataSource.GetData(parameter));
            }
        }

        [TestCase]
        public void MethodWithArrayArguments([ValueSource(nameof(ComplexArrayBasedTestInput))] object o)
        {
        }

        static object[] ComplexArrayBasedTestInput = new[]
        {
            new object[] { 1, "text", new object() },
            new object[0],
            new object[] { 1, new int[] { 2, 3 }, 4 },
            new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
            new object[] { new byte[,] { { 1, 2 }, { 2, 3 } } }
        };

        [TestCase]
        public void TestNameIntrospectsArrayValues()
        {
            TestSuite suite = TestBuilder.MakeParameterizedMethodSuite(
                GetType(), nameof(MethodWithArrayArguments));

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
    }

    public class ValueSourceMayBeInherited
    {
        protected static IEnumerable<bool> InheritedStaticProperty
        {
            get { yield return true; }
        }
    }
}
