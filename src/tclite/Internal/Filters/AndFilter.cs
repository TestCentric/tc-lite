// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections.Generic;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
	/// <summary>
	/// Combines multiple filters so that a test must pass all 
	/// of them in order to pass this filter.
    /// </summary>
    public class AndFilter : CompositeFilter
	{
		/// <summary>
		/// Constructs an empty AndFilter
		/// </summary>
		public AndFilter() { }

		/// <summary>
		/// Constructs an AndFilter from an array of filters
		/// </summary>
		/// <param name="filters"></param>
		public AndFilter( params ITestFilter[] filters ) : base(filters) { }

		/// <summary>
		/// Checks whether the AndFilter is matched by a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if all the component filters pass, otherwise false</returns>
		public override bool Pass( ITest test )
		{
			foreach( ITestFilter filter in Filters )
				if ( !filter.Pass( test ) )
					return false;

			return true;
		}

		/// <summary>
		/// Checks whether the AndFilter is matched by a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if all the component filters match, otherwise false</returns>
		public override bool Match( ITest test )
		{
			foreach( TestFilter filter in Filters )
				if ( !filter.Match( test ) )
					return false;

			return true;
		}

        /// <summary>
        /// Checks whether the AndFilter is explicit matched by a test.
        /// </summary>
        /// <param name="test">The test to be matched</param>
        /// <returns>True if all the component filters explicit match, otherwise false</returns>
        public override bool IsExplicitMatch( ITest test )
        {
            foreach( TestFilter filter in Filters )
                if ( !filter.IsExplicitMatch( test ) )
                    return false;

            return true;
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "and"; }
        }
	}
}
