// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.IO;
using TCLite.Framework.Assertions;
using TCLite.TestUtilities;

namespace TCLite.Framework.Constraints
{
    [TestFixture]
    public class EmptyConstraintTest : ConstraintTestBase<object>
    {
        protected override Constraint Constraint => new EmptyConstraint();
        protected override string ExpectedDescription => "<empty>";
        protected override string ExpectedRepresentation => "<empty>";

        protected override object[] SuccessData => new object[]
        {
            string.Empty,
            new object[0],
            new ArrayList(),
            new System.Collections.Generic.List<int>()
        };

        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( new object[] { 1, 2, 3 }, "< 1, 2, 3 >" )
        };

        protected override TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData(null, typeof(ArgumentException)),
            new TestCaseData(5, typeof(ArgumentException))
        };

#if NYI // Null string
        [Test]
        public void NullStringGivesFailureResult()
        {
            string actual = null;
            var result = Constraint.ApplyTo(actual);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
        }
#endif

        // [Test]
        // public void NullArgumentExceptionMessageContainsTypeName()
        // {
        //     int? testInput = null;
        //     Assert.That(() => _constraint.ApplyTo(testInput),
        //        Throws.ArgumentException.With.Message.Contains("System.Int32"));
        // }
    }

    [TestFixture]
    public class EmptyStringConstraintTest : ConstraintTestBase<string>
    {
        protected override Constraint Constraint => new EmptyStringConstraint();
        protected override string ExpectedDescription => "<empty>";
        protected override string ExpectedRepresentation => "<emptystring>";

        protected override string[] SuccessData => new string[]
        {
            string.Empty
        };

        protected override TestCaseData[] FailureData => new TestCaseData[]
        {
            new TestCaseData( "Hello", "\"Hello\"" ),
            new TestCaseData( null, "null")
        };

        protected override TestCaseData[] InvalidData => new TestCaseData[]
        {
            new TestCaseData(5, typeof(ArgumentException))
        };
    }

    // [TestFixture]
    // public class EmptyDirectoryConstraintTest
    // {
    //     [Test]
    //     public void EmptyDirectory()
    //     {
    //         using (var testDir = new TestDirectory())
    //         {
    //             Assert.That(testDir.Directory, Is.Empty);
    //         }
    //     }

    //     [Test]
    //     public void NotEmptyDirectory()
    //     {
    //         using (var testDir = new TestDirectory())
    //         {
    //             File.Create(Path.Combine(testDir.Directory.FullName, "DUMMY.FILE")).Dispose();

    //             Assert.That(testDir.Directory, Is.Not.Empty);
    //         }
    //     }
    // }
}
