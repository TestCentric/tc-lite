// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Xml;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal.Filters
{
	/// <summary>
	/// NotFilter negates the operation of another filter
	/// </summary>
	public class NotFilter : TestFilter
	{
		/// <summary>
		/// Construct a not filter on another filter
		/// </summary>
		/// <param name="baseFilter">The filter to be negated</param>
		public NotFilter(TestFilter baseFilter)
		{
			BaseFilter = baseFilter;
		}

		/// <summary>
		/// Gets the base filter
		/// </summary>
		public TestFilter BaseFilter { get; }

        /// <summary>
        /// Determine if a particular test passes the filter criteria. The default 
        /// implementation checks the test itself, its parents and any descendants.
        /// 
        /// Derived classes may override this method or any of the Match methods
        /// to change the behavior of the filter.
        /// </summary>
        /// <param name="test">The test to which the filter is applied</param>
        /// <returns>True if the test passes the filter, otherwise false</returns>
        public override bool Pass(ITest test)
        {
            return !BaseFilter.Match (test) && !BaseFilter.MatchParent (test);
        }

		/// <summary>
		/// Check whether the filter matches a test
		/// </summary>
		/// <param name="test">The test to be matched</param>
		/// <returns>True if it matches, otherwise false</returns>
		public override bool Match( ITest test )
		{
			return !BaseFilter.Match(test);
		}

        /// <summary>
        /// Determine if a test matches the filter explicitly. That is, it must
        /// be a direct match of the test itself or one of its children.
        /// </summary>
        /// <param name="test">The test to which the filter is applied</param>
        /// <returns>True if the test matches the filter explicitly, otherwise false</returns>
        public override bool IsExplicitMatch(ITest test)
        {
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
            XmlNode result = parentNode.AddElement("not");
            if (recursive)
                BaseFilter.AddToXml(result, true);
            return result;
        }
	}
}
