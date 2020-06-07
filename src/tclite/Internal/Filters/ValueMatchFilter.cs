// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
    /// <summary>
    /// ValueMatchFilter selects tests based on some value, which
    /// is expected to be contained in the test.
    /// </summary>
    public abstract class ValueMatchFilter : TestFilter
    {
        /// <summary>
        /// Returns the value matched by the filter - used for testing
        /// </summary>
        public string ExpectedValue { get; }

        /// <summary>
        /// Indicates whether the value is a regular expression
        /// </summary>
        public bool IsRegex { get; set; }

        /// <summary>
        /// Construct a ValueMatchFilter for a single value.
        /// </summary>
        /// <param name="expectedValue">The value to be included.</param>
        public ValueMatchFilter(string expectedValue)
        {
            ExpectedValue = expectedValue;
        }

        /// <summary>
        /// Match the input provided by the derived class
        /// </summary>
        /// <param name="input">The value to be matchedT</param>
        /// <returns>True for a match, false otherwise.</returns>
        protected bool Match(string input)
        {
            if (IsRegex)
                return input != null && new Regex(ExpectedValue).IsMatch(input);
            else
                return ExpectedValue == input;
        }

        /// <summary>
        /// Adds an XML node
        /// </summary>
        /// <param name="parentNode">Parent node</param>
        /// <param name="recursive">True if recursive</param>
        /// <returns>The added XML node</returns>
        public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            XmlNode result = parentNode.AddElement(ElementName);
            result.InnerText = ExpectedValue;
            if (IsRegex)
                result.AddAttribute("re", "1");
            return result;
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected abstract string ElementName { get; }
    }
}
