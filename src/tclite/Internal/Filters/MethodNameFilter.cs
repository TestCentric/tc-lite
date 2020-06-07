// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
    /// <summary>
    /// FullName filter selects tests based on their FullName
    /// </summary>
    public class MethodNameFilter : ValueMatchFilter
    {
        /// <summary>
        /// Construct a MethodNameFilter for a single name
        /// </summary>
        /// <param name="expectedValue">The name the filter will recognize.</param>
        public MethodNameFilter(string expectedValue) : base(expectedValue) { }

        /// <summary>
        /// Match a test against a single value.
        /// </summary>
        public override bool Match(ITest test)
        {
            return Match(test.MethodName);
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "method"; }
        }
    }
}
