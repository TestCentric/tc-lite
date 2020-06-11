// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    public abstract class ConstraintTestBaseNoData
    {
        protected Constraint _constraint;
        protected string _expectedDescription = "<NOT SET>";
        protected string _expectedRepresentation = "<NOT SET>";

        [Test]
        public void ProvidesProperDescription()
        {
            TextMessageWriter writer = new TextMessageWriter();
            _constraint.WriteDescriptionTo(writer);
            Assert.That(writer.ToString(), Is.EqualTo(_expectedDescription));
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.That(_constraint.ToString(), Is.EqualTo(_expectedRepresentation));
        }
    }

    public abstract class ConstraintTestBase : ConstraintTestBaseNoData
    {
        [Test, TestCaseSource("SuccessData")]
        public void SucceedsWithGoodValues(object value)
        {
            if (!_constraint.Matches(value))
            {
                MessageWriter writer = new TextMessageWriter();
                _constraint.WriteMessageTo(writer);
                Assert.Fail(writer.ToString());
            }
        }

        [Test, TestCaseSource("FailureData")]
        public void FailsWithBadValues(object badValue, string message)
        {
            string NL = Environment.NewLine;

            Assert.False(_constraint.Matches(badValue));

            TextMessageWriter writer = new TextMessageWriter();
            _constraint.WriteMessageTo(writer);
            Assert.That( writer.ToString(), Is.EqualTo(
                TextMessageWriter.Pfx_Expected + _expectedDescription + NL +
                TextMessageWriter.Pfx_Actual + message + NL ));
        }
    }

    /// <summary>
    /// Base class for testing constraints that can throw an ArgumentException
    /// </summary>
    public abstract class ConstraintTestBaseWithArgumentException : ConstraintTestBase
    {
        [Test, TestCaseSource("InvalidData")]
        public void InvalidDataThrowsArgumentException(object value)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _constraint.Matches(value);
            });
        }
    }

    /// <summary>
    /// Base class for tests that can throw multiple exceptions. Use
    /// TestCaseData class to specify the expected exception type.
    /// </summary>
    public abstract class ConstraintTestBaseWithExceptionTests : ConstraintTestBase
    {
        [Test, TestCaseSource("InvalidData")]
        public void InvalidDataThrowsException(object value)
        {
            _constraint.Matches(value);
        }
    }
}