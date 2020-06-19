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

        public override string Description => $"Instance of Type {ExpectedType}";

        /// <summary>
        /// Test whether an object is of the specified type or a derived type
        /// </summary>
        /// <param name="actual">The object to be tested</param>
        /// <returns>True if the object is of the provided type or derives from it, otherwise false.</returns>
        protected override bool Matches<TActual>(TActual actual)
        {
            return actual != null && ExpectedType.IsInstanceOfType(actual);
        }
    }
}