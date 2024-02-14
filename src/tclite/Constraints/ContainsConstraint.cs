// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    // TODO Needs tests
    /// <summary>
    /// ContainsConstraint tests a whether a string contains a substring
    /// or a collection contains an object. It postpones the decision of
    /// which test to use until the type of the actual argument is known.
    /// This allows testing whether a string is contained in a collection
    /// or as a substring of another string using the same syntax.
    /// </summary>
    public class ContainsConstraint<TExpected> : ExpectedValueConstraint<TExpected>
    {
        private Constraint _realConstraint;
        private bool _ignoreCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainsConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value contained within the string/collection.</param>
        public ContainsConstraint(TExpected expected) : base(expected) { }

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get
            {
                if (_realConstraint != null)
                {
                    return _realConstraint.Description;
                }

                var description = "containing " + MsgUtils.FormatValue(ExpectedValue);

                if (_ignoreCase)
                    description += ", ignoring case";

                return description;
            }
        }

        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public ContainsConstraint<TExpected> IgnoreCase
        {
            get { _ignoreCase = true; return this; }
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override ConstraintResult ApplyConstraint<TActual>(TActual actual)
        {
            if (actual is string && ExpectedValue is string)
            {
                StringConstraint constraint = new SubstringConstraint(ExpectedValue as string);
                if (_ignoreCase)
                    constraint = constraint.IgnoreCase;
                _realConstraint = constraint;
            }
            else
            {
                var itemConstraint = new EqualConstraint<TExpected>(ExpectedValue);
                if (_ignoreCase)
                    itemConstraint = itemConstraint.IgnoreCase;
                _realConstraint = new SomeItemsConstraint(itemConstraint);
            }

            return _realConstraint.ApplyTo(actual);
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a new ContainsConstraint. This constraint
        /// will, in turn, make use of the appropriate second-level
        /// constraint, depending on the type of the actual argument. 
        /// This overload is only used if the item sought is a string,
        /// since any other type implies that we are looking for a 
        /// collection member.
        /// </summary>
        public ContainsConstraint<string> Contains(string expected)
        {
            return (ContainsConstraint<string>)Append(new ContainsConstraint<string>(expected));
        }
    }
}
