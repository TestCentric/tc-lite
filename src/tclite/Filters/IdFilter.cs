// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Interfaces;

namespace TCLite.Filters
{
    /// <summary>
    /// IdFilter selects tests based on their id
    /// </summary>
    public class IdFilter : ValueMatchFilter
    {
        /// <summary>
        /// Construct an IdFilter for a single value
        /// </summary>
        /// <param name="id">The id the filter will recognize.</param>
        public IdFilter(string id) : base (id) { }

        /// <summary>
        /// Match a test against a single value.
        /// </summary>
        public override bool Match(ITest test)
        {
            // We make a direct test here rather than calling ValueMatchFilter.Match
            // because regular expressions are not supported for ID.
            return test.Id == ExpectedValue;
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "id"; }
        }
    }
}
