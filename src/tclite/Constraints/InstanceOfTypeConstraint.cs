// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// InstanceOfTypeConstraint is used to test that an object
    /// is of the same type provided or derived from it.
    /// </summary>
    public class InstanceOfTypeConstraint : TypeConstraint
    {
        /// <summary>
        /// Construct an InstanceOfTypeConstraint for the type provided
        /// </summary>
        /// <param name="type">The expected Type</param>
        public InstanceOfTypeConstraint(Type type)
            : base(type)
        {
            DisplayName = "instanceof";
        }

        /// <summary>
        /// Test whether an object is of the specified type or a derived type
        /// </summary>
        /// <param name="actual">The object to be tested</param>
        /// <returns>True if the object is of the provided type or derives from it, otherwise false.</returns>
        public override bool Matches<T>(T actual)
        {
            ActualValue = actual;
            return actual != null && ExpectedType.IsInstanceOfType(actual);
        }

        /// <summary>
        /// Write a description of this constraint to a MessageWriter
        /// </summary>
        /// <param name="writer">The MessageWriter to use</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WritePredicate("instance of");
            writer.WriteExpectedValue(ExpectedType);
        }
    }
}