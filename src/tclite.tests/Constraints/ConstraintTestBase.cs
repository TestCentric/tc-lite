// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Base class for constraint tests
    /// </summary>
    /// <typeparam name="TExpected">Type required for actual parameters.</typeparam>
    public abstract class ConstraintTestBase<TExpected>
    {
        protected abstract Constraint Constraint { get; }
        protected abstract string ExpectedDescription { get; }
        protected abstract string ExpectedRepresentation { get; }

        protected abstract TExpected[] SuccessData { get; }
        protected abstract TestCaseData[] FailureData { get; }
        // TODO: Make this abstract after all classes define it
        protected virtual TestCaseData[] InvalidData => new TestCaseData[0];

        [Test]
        public void ProvidesProperDescription()
        {
            Assert.That(Constraint.Description, Is.EqualTo(ExpectedDescription));
        }

        [Test]
        public void ProvidesProperStringRepresentation()
        {
            Assert.That(Constraint.ToString(), Is.EqualTo(ExpectedRepresentation));
        }

        [TestCaseSource(nameof(SuccessData))]
        public void SucceedsWithGoodValues(TExpected value)
        {
            var result = Constraint.ApplyTo(value);
            if (!result.IsSuccess)
            {
                MessageWriter writer = new TextMessageWriter();
                result.WriteMessageTo(writer);
                Assert.Fail(writer.ToString());
            }
        }

        [TestCaseSource(nameof(FailureData))]
        public void FailsWithBadValues(TExpected badValue, string message)
        {
            string NL = Environment.NewLine;

            var result = Constraint.ApplyTo(badValue);
            Assert.IsFalse(result.IsSuccess);

            TextMessageWriter writer = new TextMessageWriter();
            result.WriteMessageTo(writer);
            Assert.That(writer.ToString(), Is.EqualTo(
                TextMessageWriter.Pfx_Expected + ExpectedDescription + NL +
                TextMessageWriter.Pfx_Actual + message + NL));
        }

        [TestCaseSource(nameof(InvalidData))]
        public void InvalidDataThrowsException(object value, Type exType)
        {
            Assert.Throws(exType, () =>
            {
                Constraint.ValidateActualValue(value);
            });
        }
    }
}