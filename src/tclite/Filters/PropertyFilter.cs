﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;
using System.Xml;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Filters
{
    /// <summary>
    /// PropertyFilter is able to select or exclude tests
    /// based on their properties.
    /// </summary>
    public class PropertyFilter : ValueMatchFilter
    {
        private readonly string _propertyName;

        /// <summary>
        /// Construct a PropertyFilter using a property name and expected value
        /// </summary>
        /// <param name="propertyName">A property name</param>
        /// <param name="expectedValue">The expected value of the property</param>
        public PropertyFilter(string propertyName, string expectedValue) : base(expectedValue) 
        {
            _propertyName = propertyName;
        }

        /// <summary>
        /// Check whether the filter matches a test
        /// </summary>
        /// <param name="test">The test to be matched</param>
        /// <returns></returns>
        public override bool Match(ITest test)
        {
            IList values = test.Properties[_propertyName];

            if (values != null)
                foreach (string val in values)
                    if (Match(val))
                        return true;

            return false;
        }

        /// <summary>
        /// Adds an XML node
        /// </summary>
        /// <param name="parentNode">Parent node</param>
        /// <param name="recursive">True if recursive</param>
        /// <returns>The added XML node</returns>
        public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            XmlNode result = base.AddToXml(parentNode, recursive);
            result.AddAttribute("name", _propertyName);
            return result;
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected override string ElementName
        {
            get { return "prop"; }
        }
    }
}
