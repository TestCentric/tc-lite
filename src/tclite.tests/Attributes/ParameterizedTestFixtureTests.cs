// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TCLite.Framework.Internal;
using TCLite.Framework.Builders;
using TCLite.TestUtilities;

namespace TCLite.Framework.Attributes
{
    // [TestFixture(45, 45, 90)]
    // [TestFixture(null, null, null)]
    // public class NullableParameterizedTestFixture
    // {
    //     int? _one;
    //     int? _two;
    //     int? _expected;

    //     public NullableParameterizedTestFixture(int? one, int? two, int? expected)
    //     {
    //         _one = one;
    //         _two = two;
    //         _expected = expected;
    //     }

    //     [TestCase]
    //     public void TestAddition()
    //     {
    //         if(_one.HasValue && _two.HasValue && _expected.HasValue)
    //         {
    //             Assert.That(_one.Value + _two.Value, Is.EqualTo(_expected.Value));
    //         }
    //         else
    //         {
    //             Assert.That(_one, Is.Null);
    //             Assert.That(_two, Is.Null);
    //         }
    //     }
    // }

    [TestFixture("hello", "hello", "goodbye")]
    [TestFixture("zip", "zip")]
    [TestFixture(42, 42, 99)]
#if NYI // Null Fixture Arguments not yet supported
    [TestFixture(null, null, "null test")]
    [TestFixture((string)null, (string)null, "typed null test")]
#endif
    public class ParameterizedTestFixture
    {
        private readonly string eq1;
        private readonly string eq2;
        private readonly string neq;
        
        public ParameterizedTestFixture(string eq1, string eq2, string neq)
        {
            this.eq1 = eq1;
            this.eq2 = eq2;
            this.neq = neq;
        }

        public ParameterizedTestFixture(string eq1, string eq2)
            : this(eq1, eq2, null) { }

        public ParameterizedTestFixture(int eq1, int eq2, int neq)
        {
            this.eq1 = eq1.ToString();
            this.eq2 = eq2.ToString();
            this.neq = neq.ToString();
        }

        [TestCase]
        public void TestEquality()
        {
            Assert.AreEqual(eq1, eq2);
            if (eq1 != null && eq2 != null)
                Assert.AreEqual(eq1.GetHashCode(), eq2.GetHashCode());
        }

        [TestCase]
        public void TestInequality()
        {
            Assert.AreNotEqual(eq1, neq);
            if (eq1 != null && neq != null)
                Assert.AreNotEqual(eq1.GetHashCode(), neq.GetHashCode());
        }
    }

#if NYI // TestBuilder
    public class ParameterizedTestFixtureNamingTests
    {
        TestSuite fixture;

        [SetUp]
        public void MakeFixture()
        {
            fixture = TestBuilder.MakeFixture(typeof(NUnit.TestData.ParameterizedTestFixture));
        }

        [TestCase]
        public void TopLevelSuiteIsNamedCorrectly()
        {
            Assert.That(fixture.Name, Is.EqualTo("ParameterizedTestFixture"));
            Assert.That(fixture.FullName, Is.EqualTo("NUnit.TestData.ParameterizedTestFixture"));
        }

        [TestCase]
        public void SuiteHasCorrectNumberOfInstances()
        {
            Assert.That(fixture.Tests.Count, Is.EqualTo(2));
        }

        [TestCase]
        public void FixtureInstancesAreNamedCorrectly()
        {
            var names = new List<string>();
            var fullnames = new List<string>();
            foreach (Test test in fixture.Tests)
            {
                names.Add(test.Name);
                fullnames.Add(test.FullName);
            }

            Assert.That(names, Is.EquivalentTo(new string[] {
                "ParameterizedTestFixture(1)", "ParameterizedTestFixture(2)" }));
            Assert.That(fullnames, Is.EquivalentTo(new string[] {
                "NUnit.TestData.ParameterizedTestFixture(1)", "NUnit.TestData.ParameterizedTestFixture(2)" }));
        }

        [TestCase]
        public void MethodWithoutParamsIsNamedCorrectly()
        {
            TestSuite instance = (TestSuite)fixture.Tests[0];
            Test method = TestFinder.Find("MethodWithoutParams", instance, false);
            Assert.That(method, Is.Not.Null );
            Assert.That(method.FullName, Is.EqualTo(instance.FullName + ".MethodWithoutParams"));
        }

        [TestCase]
        public void MethodWithParamsIsNamedCorrectly()
        {
            TestSuite instance = (TestSuite)fixture.Tests[0];
            TestSuite method = (TestSuite)TestFinder.Find("MethodWithParams", instance, false);
            Assert.That(method, Is.Not.Null);
            
            Test testcase = (Test)method.Tests[0];
            Assert.That(testcase.Name, Is.EqualTo("MethodWithParams(10,20)"));
            Assert.That(testcase.FullName, Is.EqualTo(instance.FullName + ".MethodWithParams(10,20)"));
        }
    }

    public class ParameterizedTestFixtureTests
    {
        [TestCase]
        public void CanSpecifyCategory()
        {
            Test fixture = TestBuilder.MakeFixture(typeof(NUnit.TestData.TestFixtureWithSingleCategory));
            Assert.AreEqual("XYZ", fixture.Properties.Get(PropertyNames.Category));
        }

        [TestCase]
        public void CanSpecifyMultipleCategories()
        {
            Test fixture = TestBuilder.MakeFixture(typeof(NUnit.TestData.TestFixtureWithMultipleCategories));
            Assert.AreEqual(new string[] { "X", "Y", "Z" }, fixture.Properties[PropertyNames.Category]);
        }

        [TestCase]
        public void NullArgumentForOrdinaryValueTypeParameterDoesNotThrowNullReferenceException()
        {
            Test fixture = TestBuilder.MakeFixture(typeof(NUnit.TestData.TestFixtureWithNullArgumentForOrdinaryValueTypeParameter));
            Assert.That(fixture.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(fixture.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("No suitable constructor was found"));
        }

        [TestCase]
        public void NullArgumentForGenericParameterDoesNotThrowNullReferenceException()
        {
            Test parameterizedFixture = TestBuilder.MakeFixture(typeof(NUnit.TestData.TestFixtureWithNullArgumentForGenericParameter<>));
            ITest fixture = parameterizedFixture.Tests.Single();

            Assert.That(fixture.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(fixture.Properties.Get(PropertyNames.SkipReason), Is.EqualTo(
                "Fixture type contains generic parameters. You must either provide Type arguments or specify constructor arguments that allow NUnit to deduce the Type arguments."));
        }
    }
#endif

    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class ParameterizedTestFixtureWithTypeAsArgument
    {
        private readonly Type _someType;

        public ParameterizedTestFixtureWithTypeAsArgument(Type someType)
        {
            _someType = someType;
        }

        [TestCase]
        public void MakeSureTypeIsInSystemNamespace()
        {
            Assert.AreEqual("System", _someType.Namespace);
        }
    }
}