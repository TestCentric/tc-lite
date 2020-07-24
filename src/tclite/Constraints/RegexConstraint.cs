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
    public class RegexConstraint : Constraint
    {
        private Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegexConstraint"/> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        public RegexConstraint(string pattern, RegexOptions options = RegexOptions.None) : base(pattern)
        { 
            _regex = new Regex(pattern, options);
        }

        public RegexConstraint(Regex regex) : base(regex.ToString())
        {
            _regex = regex;
        }

        public override string Description => $"String matching pattern \"{_regex.ToString()}\"";

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<string>(actual, nameof(actual));
        }

        protected override ConstraintResult ApplyConstraint<TActual>(TActual actual)
        {
            return new ConstraintResult(this, actual, 
                actual != null && actual is string && _regex.Match(actual.ToString()).Success);
        }
    }
}