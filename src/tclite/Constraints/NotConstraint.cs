// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// NotConstraint negates the effect of some other constraint
    /// </summary>
    public class NotConstraint : PrefixConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotConstraint"/> class.
        /// </summary>
        /// <param name="baseConstraint">The base constraint to be negated.</param>
        public NotConstraint(Constraint baseConstraint)
            : base(baseConstraint) { }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for if the base constraint fails, false if it succeeds</returns>
        public override bool Matches(object actual)
        {
            this.actual = actual;
            return !baseConstraint.Matches(actual);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("not");
            baseConstraint.WriteDescriptionTo(writer);
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a MessageWriter.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            baseConstraint.WriteActualValueTo(writer);
        }
    }
}