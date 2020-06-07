// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Xml;
using TCLite.Framework.Api;
using TCLite.Framework.Internal.Filters;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// Interface to be implemented by filters applied to tests.
	/// The filter applies when running the test, after it has been
	/// loaded, since this is the only time an ITest exists.
	/// </summary>
	public abstract class TestFilter : ITestFilter
	{
		/// <summary>
		/// Unique Empty filter.
		/// </summary>
        public static TestFilter Empty = new EmptyFilter();

        /// <summary>
        /// Indicates whether this is the EmptyFilter
        /// </summary>
        public bool IsEmpty
        {
            get { return this is TestFilter.EmptyFilter; }
        }

		        /// <summary>
        /// Indicates whether this is a top-level filter,
        /// not contained in any other filter.
        /// </summary>
        public bool TopLevel { get; set; }

		/// <summary>
		/// Determine if a particular test passes the filter criteria. The default 
		/// implementation checks the test itself, its parents and any descendants.
		/// 
		/// Derived classes may override this method or any of the Match methods
		/// to change the behavior of the filter.
		/// </summary>
		/// <param name="test">The test to which the filter is applied</param>
		/// <returns>True if the test passes the filter, otherwise false</returns>
		public virtual bool Pass( ITest test )
		{
			return Match(test) || MatchParent(test) || MatchDescendant(test);
		}

		        /// <summary>
        /// Determine if a test matches the filter explicitly. That is, it must
        /// be a direct match of the test itself or one of its children.
        /// </summary>
        /// <param name="test">The test to which the filter is applied</param>
        /// <returns>True if the test matches the filter explicitly, otherwise false</returns>
        public virtual bool IsExplicitMatch(ITest test)
        {
            return Match(test) || MatchDescendant(test);
        }

        /// <summary>
        /// Determine whether the test itself matches the filter criteria, without
        /// examining either parents or descendants. This is overridden by each
        /// different type of filter to perform the necessary tests.
        /// </summary>
        /// <param name="test">The test to which the filter is applied</param>
        /// <returns>True if the filter matches the any parent of the test</returns>
        public abstract bool Match(ITest test);

        /// <summary>
        /// Determine whether any ancestor of the test matches the filter criteria
        /// </summary>
        /// <param name="test">The test to which the filter is applied</param>
        /// <returns>True if the filter matches the an ancestor of the test</returns>
        public bool MatchParent(ITest test)
        {
            return test.Parent != null && (Match(test.Parent) || MatchParent(test.Parent));
        }

		/// <summary>
		/// Determine whether any descendant of the test matches the filter criteria.
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if at least one descendant matches the filter criteria</returns>
		protected virtual bool MatchDescendant(ITest test)
		{
            if (test.Tests == null)
                return false;

            foreach (ITest child in test.Tests)
            {
                if (Match(child) || MatchDescendant(child))
                    return true;
            }

            return false;
		}

        /// <summary>
        /// Create a TestFilter instance from an XML representation.
        /// </summary>
        public static TestFilter FromXml(string xmlText)
        {
            if (string.IsNullOrEmpty(xmlText))
                xmlText = "<filter />";

            XmlNode topNode = XmlHelper.CreateXmlNode(xmlText);

            if (topNode.Name != "filter")
                throw new Exception("Expected filter element at top level");

            int count = topNode.ChildNodes.Count;

            TestFilter filter = count == 0
                ? TestFilter.Empty
                : count == 1
                    ? FromXml(topNode.FirstChild)
                    : FromXml(topNode);

            filter.TopLevel = true;

            return filter;
        }

        /// <summary>
        /// Create a TestFilter from its Xml representation
        /// </summary>
        public static TestFilter FromXml(XmlNode node)
        {
            bool isRegex = node.GetAttribute("re") == "1";

            switch (node.Name)
            {
                case "filter":
                case "and":
                    var andFilter = new AndFilter();
                    foreach (XmlNode childNode in node.ChildNodes)
                        andFilter.Add(FromXml(childNode));
                    return andFilter;

                case "or":
                    var orFilter = new OrFilter();
                    foreach (XmlNode childNode in node.ChildNodes)
                        orFilter.Add(FromXml(childNode));
                    return orFilter;

                case "not":
                    return new NotFilter(FromXml(node.FirstChild));

                case "id":
                    return new IdFilter(node.InnerText);

                case "test":
                    return new TestNameFilter(node.InnerText) { IsRegex = isRegex };

                case "method":
                    return new MethodNameFilter(node.InnerText) { IsRegex = isRegex };

                case "class":
                    return new ClassNameFilter(node.InnerText) { IsRegex = isRegex };

                case "namespace":
                    return new NamespaceFilter(node.InnerText) { IsRegex = isRegex };

                case "cat":
                    return new CategoryFilter(node.InnerText) { IsRegex = isRegex };

                case "prop":
                    string name = node.GetAttribute("name");
                    if (name != null)
                        return new PropertyFilter(name, node.InnerText) { IsRegex = isRegex };
                    break;
            }

            throw new ArgumentException("Invalid filter element: " + node.Name, "xmlNode");
        }

		#region IXmlNodeBuilder Implementation

        /// <summary>
        /// Adds an XML node
        /// </summary>
        /// <param name="recursive">True if recursive</param>
        /// <returns>The added XML node</returns>
        public XmlNode ToXml(bool recursive)
        {
            return AddToXml(XmlHelper.CreateTopLevelElement("dummy"), recursive);
        }

        /// <summary>
        /// Adds an XML node
        /// </summary>
        /// <param name="parentNode">Parent node</param>
        /// <param name="recursive">True if recursive</param>
        /// <returns>The added XML node</returns>
        public abstract XmlNode AddToXml(XmlNode parentNode, bool recursive);

		#endregion

        /// <summary>
        /// Nested class provides an empty filter - one that always
        /// returns true when called. It never matches explicitly.
        /// </summary>
        private class EmptyFilter : TestFilter
        {
            public override bool Match( ITest test )
            {
                return true;
            }

            public override bool Pass( ITest test )
            {
                return true;
            }

            public override bool IsExplicitMatch( ITest test )
            {
                return false;
            }

            public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
            {
                return parentNode.AddElement("filter");
            }
        }
	}
}
