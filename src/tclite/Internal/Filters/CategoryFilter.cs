// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
	/// <summary>
	/// CategoryFilter is able to select or exclude tests
	/// based on their categories.
	/// </summary>
	/// 
	public class CategoryFilter : ValueMatchFilter
	{
        List<string> categories = new List<string>();

		/// <summary>
		/// Construct a CategoryFilter using a single category name
		/// </summary>
		/// <param name="name">A category name</param>
		public CategoryFilter( string name ) : base(name) { }

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns></returns>
        public override bool Match(ITest test)
        {
            IList testCategories = test.Properties[PropertyNames.Category] as IList;

            if ( testCategories != null)
                foreach (string cat in testCategories)
                    if ( Match(cat))
                        return true;

			return false;
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "cat"; }
        }
	}
}
