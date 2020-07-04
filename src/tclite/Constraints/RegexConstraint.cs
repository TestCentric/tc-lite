// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Text.RegularExpressions;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// RegexConstraint can test whether a string matches
    /// the pattern provided.
    /// </summary>
    public class RegexConstraint : StringConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegexConstraint"/> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public RegexConstraint(string pattern) : base(pattern) { }

        public override string Description => "String matching " + base.Description;

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override bool Matches(string actual)
        {
            return actual != null && Regex.IsMatch(
                    actual,
                    ExpectedValue,
                    _caseInsensitive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
    }
}