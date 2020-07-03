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
    /// PropertyConstraint extracts a named property and uses
    /// its value as the actual value for a chained constraint.
    /// </summary>
    public class PropertyConstraint : PrefixConstraint
    {
        private readonly string name;
        private object propValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyConstraint"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="baseConstraint">The constraint to apply to the property.</param>
        public PropertyConstraint(string name, Constraint baseConstraint)
            : base(baseConstraint)
        {
            this.name = name;
            this.DescriptionPrefix = "property " + name;
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        public override ConstraintResult ApplyTo<T>(T actual)
        {
            // TODO: Use an error result for null
            Guard.ArgumentNotNull(actual, nameof(actual));

            Type actualType = actual as Type;
            if (actualType == null)
                actualType = actual.GetType();

            PropertyInfo property = actualType.GetTypeInfo().GetProperty(name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // TODO: Use an error result here
            if (property == null)
                throw new ArgumentException($"Property {name} was not found on {actualType}.", "name");

            propValue = property.GetValue(actual, null);
            var baseResult = BaseConstraint.ApplyTo(propValue);
            return new PropertyConstraintResult(this, baseResult);              
        }

        /// <summary>
        /// Returns the string representation of the constraint.
        /// </summary>
        protected override string GetStringRepresentation()
        {
            return string.Format("<property {0} {1}>", name, BaseConstraint);
        }
    }
}
