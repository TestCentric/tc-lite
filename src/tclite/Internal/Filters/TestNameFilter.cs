// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
	/// <summary>
	/// SimpleName filter selects tests based on their name
	/// </summary>
    public class TestNameFilter : ValueMatchFilter
    {
        /// <summary>
        /// Construct a FullNameFilter for a single name
        /// </summary>
        /// <param name="expectedValue">The name the filter will recognize.</param>
        public TestNameFilter(string expectedValue) : base(expectedValue) { }

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if it matches, otherwise false</returns>
		public override bool Match( ITest test )
		{
            if(Match(test.FullName))
                return true;

            if (IsRegex)
                return false; // Base class will have handled this

            // Extra checks because the tree of tests does not contain
            // any nodes for namespaces, only fixtures.
            if (!test.FullName.StartsWith(ExpectedValue))
                return false;

            var ch = test.FullName[ExpectedValue.Length];

            return ch == '.' || ch == '(';
		}

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "test"; }
        }
	}
}
