// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Internal;

namespace TCLite.Constraints
{
    /// <summary>
    /// StringConstraint is the abstract base for constraints
    /// that operate on strings. It supports the IgnoreCase
    /// modifier for string operations.
    /// </summary>
    public abstract class StringConstraint : ExpectedValueConstraint<string>
    {
        /// <summary>
        /// Indicates whether tests should be case-insensitive
        /// </summary>
        // ReSharper disable once InconsistentNaming
        // Disregarding naming convention for back-compat
        protected bool _caseInsensitive;

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get
            {
                var desc = $"\"{ExpectedValue}\"";
                if (_caseInsensitive)
                    desc += ", ignoring case";
                return desc;
            }
        }

        /// <summary>
        /// Constructs a StringConstraint given an expected value
        /// </summary>
        /// <param name="expected">The expected value</param>
        protected StringConstraint(string expected) : base(expected) { }

        /// <summary>
        /// Modify the constraint to ignore case in matching.
        /// </summary>
        public virtual StringConstraint IgnoreCase
        {
            get { _caseInsensitive = true; return this; }
        }

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentOfType<string>(actual, nameof(actual));
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new ConstraintResult(this, actual, Matches(actual as string));
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given string
        /// </summary>
        /// <param name="actual">The string to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected abstract bool Matches(string actual);
    }
}
