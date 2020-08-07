// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.IO;
using TCLite.Assertions;
using TCLite.TestUtilities;

namespace TCLite.Constraints
{
    [TestFixture]
    public class EmptyConstraintTest : ConstraintTestBase<object>
    {
        protected override Constraint Constraint => new EmptyConstraint();
        protected override string ExpectedDescription => "<empty>";
        protected override string ExpectedRepresentation => "<empty>";

        static object[] SuccessData = new object[]
        {
            string.Empty,
            new object[0],
            new ArrayList(),
            new System.Collections.Generic.List<int>()
        };

        static object[] FailureData = new object[]
        {
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( new object[] { 1, 2, 3 }, "< 1, 2, 3 >" )
        };

        [TestCase(null)]
        [TestCase(5)]
        public void InvalidDataThrowsArgumentException(object data)
        {
            Assert.Throws<ArgumentException>(() => Constraint.ApplyTo(data));
        }

        [TestCase]
        public void NullStringGivesFailureResult()
        {
            string actual = null;
            var result = Constraint.ApplyTo(actual);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
        }

        [TestCase]
        public void NullArgumentExceptionMessageContainsTypeName()
        {
            int? testInput = null;
            Assert.That(() => Constraint.ApplyTo(testInput),
               Throws.ArgumentException);
#if NYI // Message
               .With.Message.Contains("System.Int32"));
#endif
        }
    }

    [TestFixture]
    public class EmptyStringConstraintTest : StringConstraintTestBase
    {
        protected override Constraint Constraint => new EmptyStringConstraint();
        protected override string ExpectedDescription => "<empty>";
        protected override string ExpectedRepresentation => "<emptystring>";

        static string[] SuccessData = new string[]
        {
            string.Empty
        };

        static object[] FailureData = new object[]
        {
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( null, "null")
        };
    }
}
