// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Attributes
{
    public class TestFixtureAttributeTests
    {
        static readonly object[] FIXTURE_ARGS = { 10, 20, "Charlie" };
        static readonly Type[] TYPE_ARGS = { typeof(int), typeof(string) };

        [TestCase]
        public void ConstructWithoutArguments()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute();
            Assert.That(attr.Arguments.Length == 0);
            Assert.That(attr.TypeArgs.Length == 0);
        }

        [TestCase]
        public void ConstructWithFixtureArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute(FIXTURE_ARGS);
            Assert.That(attr.Arguments, Is.EqualTo( FIXTURE_ARGS ) );
            Assert.That(attr.TypeArgs.Length == 0 );
        }

        [TestCase]
        public void ConstructWithJustTypeArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute() { TypeArgs = TYPE_ARGS };
            Assert.That(attr.Arguments.Length == 0);
            Assert.That(attr.TypeArgs, Is.EqualTo(TYPE_ARGS));
        }

        [TestCase]
        public void ConstructWithFixtureArgsAndSetTypeArgs()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute(FIXTURE_ARGS) { TypeArgs = TYPE_ARGS };
            Assert.That(attr.Arguments, Is.EqualTo(FIXTURE_ARGS));
            Assert.That(attr.TypeArgs, Is.EqualTo(TYPE_ARGS));
        }

        [TestCase]
        public void ConstructWithWeakTypedNullArgument()
        {
            TestFixtureAttribute attr = new TestFixtureAttribute(null);
            Assert.That(attr.Arguments, Is.EqualTo(new object[] { null }));
            Assert.That(attr.TypeArgs.Length == 0);
        }
    }
}
