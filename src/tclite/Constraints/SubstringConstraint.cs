// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    public partial class Contains_Syntax
    {
        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public static SubstringConstraint Substring(string expected)
        {
            return new SubstringConstraint(expected); ;
        }
    }
}

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// SubstringConstraint can test whether a string contains
    /// the expected substring.
    /// </summary>
    public class SubstringConstraint : StringConstraint
    {
        private StringComparison? _comparisonType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstringConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected.</param>
        public SubstringConstraint(string expected) : base(expected) { }

        public override string Description => "String containing " + base.Description;

        /// <summary>
        /// Modify the constraint to ignore case in matching.
        /// This will call Using(StringComparison.CurrentCultureIgnoreCase).
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when a comparison type different
        /// than <see cref="StringComparison.CurrentCultureIgnoreCase"/> was already set.</exception>
        public override StringConstraint IgnoreCase
        {
            get { Using(StringComparison.CurrentCultureIgnoreCase); return base.IgnoreCase; }
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches(string actual)
        {
            if (actual == null) return false;

            var actualComparison = _comparisonType ?? StringComparison.CurrentCulture;
            return actual.IndexOf(ExpectedValue, actualComparison) >= 0;
        }

        /// <summary>
        /// Modify the constraint to the specified comparison.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when a comparison type different
        /// than <paramref name="comparisonType"/> was already set.</exception>
        public SubstringConstraint Using(StringComparison comparisonType)
        {
            if (this._comparisonType == null)
                this._comparisonType = comparisonType;
            else if (this._comparisonType != comparisonType)
                throw new InvalidOperationException("A different comparison type was already set.");

            return this;
        }
    }
}