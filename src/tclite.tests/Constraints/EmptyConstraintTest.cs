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
    public class EmptyConstraintTest : ConstraintTestBase
    {
        public EmptyConstraintTest()
        {
            _constraint = new EmptyConstraint();
            _expectedDescription = "<empty>";
            _expectedRepresentation = "<empty>";
        }

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
            Assert.Throws<ArgumentException>(() => _constraint.ApplyTo(data));
        }

#if NYI // Null string
        [Test]
        public void NullStringGivesFailureResult()
        {
            string actual = null;
            var result = _constraint.ApplyTo(actual);
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

    // [TestFixture]
    // public class EmptyStringConstraintTest : StringConstraintTests
    // {
    //     [SetUp]
    //     public void SetUp()
    //     {
    //         TheConstraint = new EmptyStringConstraint();
    //         ExpectedDescription = "<empty>";
    //         StringRepresentation = "<emptystring>";
    //     }

    //     static object[] SuccessData = new object[]
    //     {
    //         string.Empty
    //     };

    //     static object[] FailureData = new object[]
    //     {
    //         new TestCaseData( "Hello", "\"Hello\"" ),
    //         new TestCaseData( null, "null")
    //     };
    // }

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
