// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// PropertyExistsConstraint tests that a named property
    /// exists on the object provided through Match.
    /// 
    /// Originally, PropertyConstraint provided this feature
    /// in addition to making optional tests on the value
    /// of the property. The two constraints are now separate.
    /// </summary>
    public class PropertyExistsConstraint : Constraint
    {
        private readonly string name;

        Type actualType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyExistsConstraint"/> class.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public PropertyExistsConstraint(string name)
            : base(name)
        {
            this.name = name;
        }

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return "property " + name; }
        }

        // /// <summary>
        // /// Test whether the property exists for a given object
        // /// </summary>
        // /// <param name="actual">The object to be tested</param>
        // /// <returns>True for success, false for failure</returns>
        // public override ConstraintResult ApplyTo<TActual>(TActual actual)
        // {
        //     return ApplyTo((object)actual);
        // }

        /// <summary>
        /// Test whether the property exists for a given object
        /// </summary>
        /// <param name="actual">The object to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override ConstraintResult ApplyTo(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));

            actualType = actual as Type;
            if (actualType == null)
                actualType = actual.GetType();

            PropertyInfo property = actualType.GetTypeInfo().GetProperty(name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return new ConstraintResult(this, actualType, property != null);
        }

        /// <summary>
        /// Returns the string representation of the constraint.
        /// </summary>
        /// <returns></returns>
        protected override string GetStringRepresentation()
        {
            return string.Format("<propertyexists {0}>", name);
        }
    }
}
