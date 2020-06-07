// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
    /// <summary>
    /// ClassName filter selects tests based on the class FullName
    /// </summary>
    public class ClassNameFilter : ValueMatchFilter
    {
        /// <summary>
        /// Construct a FullNameFilter for a single name
        /// </summary>
        /// <param name="expectedValue">The name the filter will recognize.</param>
        public ClassNameFilter(string expectedValue) : base(expectedValue) { }

        /// <summary>
        /// Match a test against a single value.
        /// </summary>
        public override bool Match(ITest test)
        {
            // tests below the fixture level may have non-null className
            // but we don't want to match them explicitly.
            if (!test.IsSuite || /*test is ParameterizedMethodSuite ||*/ test.ClassName == null)
                return false;

            return Match(test.ClassName);
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "class"; }
        }
    }
}
