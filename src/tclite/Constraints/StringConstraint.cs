// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// StringConstraint is the abstract base for constraints
    /// that operate on strings. It supports the IgnoreCase
    /// modifier for string operations.
    /// </summary>
    public abstract class StringConstraint : Constraint<string>
    {
        /// <summary>
        /// The expected value
        /// </summary>
#pragma warning disable IDE1006
        // ReSharper disable once InconsistentNaming
        // Disregarding naming convention for back-compat
        protected string _expected;
#pragma warning restore IDE1006

        /// <summary>
        /// Indicates whether tests should be case-insensitive
        /// </summary>
#pragma warning disable IDE1006
        // ReSharper disable once InconsistentNaming
        // Disregarding naming convention for back-compat
        protected bool _caseInsensitive;
#pragma warning restore IDE1006

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get
            {
                var desc = $"\"{_expected}\"";
                if (_caseInsensitive)
                    desc += ", ignoring case";
                return desc;
            }
        }

        /// <summary>
        /// Constructs a StringConstraint without an expected value
        /// </summary>
        protected StringConstraint() { }

        /// <summary>
        /// Constructs a StringConstraint given an expected value
        /// </summary>
        /// <param name="expected">The expected value</param>
        protected StringConstraint(string expected)
            : base(expected)
        {
            _expected = expected;
        }

        /// <summary>
        /// Modify the constraint to ignore case in matching.
        /// </summary>
        public virtual StringConstraint IgnoreCase
        {
            get { _caseInsensitive = true; return this; }
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override ConstraintResult ApplyTo<T>(T actual)
        {
            Guard.ArgumentOfType<string>(actual, nameof(actual));
            //var stringValue = ConstraintUtils.RequireActual<string>(actual, nameof(actual), allowNull: true);

            return new ConstraintResult(this, actual, Matches(actual as string));
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override ConstraintResult ApplyTo(string actual)
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
