// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// StartsWithConstraint can test whether a string starts
    /// with an expected substring.
    /// </summary>
    public class StartsWithConstraint : StringConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartsWithConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected string</param>
        public StartsWithConstraint(string expected) : base(expected) { }

        public override string Description => "String starting with " + base.Description;

        /// <summary>
        /// Test whether the constraint is matched by the actual value.
        /// This is a template method, which calls the IsMatch method
        /// of the derived class.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override bool Matches(string actual)
        {
            if (_caseInsensitive)
                return actual != null && actual.ToLower().StartsWith(ExpectedValue.ToLower());
            else
                return actual != null && actual.StartsWith(ExpectedValue);
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public StartsWithConstraint StartsWith(string expected)
        {
            return (StartsWithConstraint)this.Append(new StartsWithConstraint(expected));
        }
    }

    public partial class Does_Syntax
    {
        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public static StartsWithConstraint StartWith(string expected)
        {
            return new StartsWithConstraint(expected);
        }
    }
}