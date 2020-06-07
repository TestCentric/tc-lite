// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Xml;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
    /// <summary>
    /// A base class for multi-part filters
    /// </summary>
    public abstract class CompositeFilter : TestFilter
    {
        /// <summary>
        /// Constructs an empty CompositeFilter
        /// </summary>
        public CompositeFilter()
        {
            Filters = new List<ITestFilter>();
        }

        /// <summary>
        /// Constructs a CompositeFilter from an array of filters
        /// </summary>
        /// <param name="filters"></param>
        public CompositeFilter( params ITestFilter[] filters )
        {
            Filters = new List<ITestFilter>(filters);
        }

        /// <summary>
        /// Adds a filter to the list of filters
        /// </summary>
        /// <param name="filter">The filter to be added</param>
        public void Add(ITestFilter filter)
        {
            Filters.Add(filter);
        }

        /// <summary>
        /// Return a list of the composing filters.
        /// </summary>
        public IList<ITestFilter> Filters { get; }

        /// <summary>
        /// Checks whether the CompositeFilter is matched by a test.
        /// </summary>
        /// <param name="test">The test to be matched</param>
        public abstract override bool Pass(ITest test);

        /// <summary>
        /// Checks whether the CompositeFilter is matched by a test.
        /// </summary>
        /// <param name="test">The test to be matched</param>
        public abstract override bool Match(ITest test);

        /// <summary>
        /// Checks whether the CompositeFilter is explicit matched by a test.
        /// </summary>
        /// <param name="test">The test to be matched</param>
        public abstract override bool IsExplicitMatch(ITest test);

        /// <summary>
        /// Adds an XML node
        /// </summary>
        /// <param name="parentNode">Parent node</param>
        /// <param name="recursive">True if recursive</param>
        /// <returns>The added XML node</returns>
        public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            XmlNode result = parentNode.AddElement(ElementName);

            if (recursive)
                foreach (ITestFilter filter in Filters)
                    filter.AddToXml(result, true);

            return result;
        }

        /// <summary>
        /// Gets the element name
        /// </summary>
        /// <value>Element name</value>
        protected abstract string ElementName { get; }
    }
}
