// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// Abstract base class used for prefixes
    /// </summary>
    public abstract class PrefixConstraint : Constraint
    {
        /// <summary>
        /// The base constraint
        /// </summary>
        protected IConstraint BaseConstraint { get; }

        /// <summary>
        /// Prefix used in forming the constraint description
        /// </summary>
        protected string DescriptionPrefix { get; set; }

        /// <summary>
        /// Construct given a base constraint
        /// </summary>
        /// <param name="baseConstraint"></param>
        protected PrefixConstraint(IConstraint baseConstraint)
            : base(baseConstraint)
        {
            Guard.ArgumentNotNull(baseConstraint, nameof(baseConstraint));

            BaseConstraint = baseConstraint;
        }
        
        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return FormatDescription(DescriptionPrefix, BaseConstraint); }
        }

        /// <summary>
        /// Formats a prefix constraint's description.
        /// </summary>
        internal static string FormatDescription(string descriptionPrefix, IConstraint baseConstraint)
        {
            return string.Format(
                baseConstraint.GetType().Name.StartsWith("EqualConstraint")
                    ? "{0} equal to {1}" 
                    : "{0} {1}",
                descriptionPrefix,
                baseConstraint.Description);
        }
    }
}