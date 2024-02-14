// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TCLite.Internal;
using TCLite.Builders;
using TCLite.TestUtilities;

namespace TCLite.Attributes
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
    [TestFixture(null, null, "null test")]
    [TestFixture((string)null, (string)null, "typed null test")]
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

    [TestFixture(1,2,3)]
    [TestFixture(4,5,6)]
    public class ParameterizedTestFixtureNamingTests
    {
        const string TYPE_NAME = "ParameterizedTestFixtureNamingTests";
        const string TYPE_FULLNAME = "TCLite.Attributes." + TYPE_NAME;

        int X, Y, Z;

        public ParameterizedTestFixtureNamingTests(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }

        [TestCase]
        public void FixtureInstancesAreNamedCorrectly()
        {
            var fixture = TestContext.CurrentTest.Parent;
            Assert.That(fixture.Name, Is.EqualTo(GetType().Name));
            Assert.That(fixture.FullName, Is.EqualTo(GetType().FullName));
        }

        [TestCase]
        public void MethodWithoutParamsIsNamedCorrectly()
        {
            var myName = nameof(MethodWithoutParamsIsNamedCorrectly);
            var test = TestContext.CurrentTest;
            Assert.That(test.Name, Is.EqualTo(myName));
            Assert.That(test.FullName, Is.EqualTo(TYPE_FULLNAME + "." + myName));
        }

        [TestCase(1, 5.2, "Hello")]
        public void MethodWithParamsIsNamedCorrectly(int x, double y, string z)
        {
            var myName = nameof(MethodWithParamsIsNamedCorrectly) + "(1,5.2d,\"Hello\")";
            var typeName = GetType().FullName;
            var test = TestContext.CurrentTest;
            Assert.That(test.Name, Is.EqualTo(myName));
            Assert.That(test.FullName, Is.EqualTo(TYPE_FULLNAME + "." + myName));
        }
    }

#if NYI
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
