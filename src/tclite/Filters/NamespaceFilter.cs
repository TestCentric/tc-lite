// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Filters
{
    /// <summary>
    /// ClassName filter selects tests based on the class FullName
    /// </summary>
    public class NamespaceFilter : ValueMatchFilter
    {
        /// <summary>
        /// Construct a NamespaceFilter for a single namespace
        /// </summary>
        /// <param name="expectedValue">The namespace the filter will recognize.</param>
        public NamespaceFilter(string expectedValue) : base(expectedValue) { }

        /// <summary>
        /// Match a test against a single value.
        /// </summary> 
        public override bool Match(ITest test)
        {
            string containingNamespace = ((Test)test).FixtureType?.Namespace;

            return Match(containingNamespace);
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "namespace"; }
        }
    }
}
