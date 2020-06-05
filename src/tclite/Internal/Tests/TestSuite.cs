// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Xml;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// TestSuite represents a composite test, which contains other tests.
    /// </summary>
	public class TestSuite : Test
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="name">The name of the suite.</param>
		public TestSuite( string name ) 
			: base( name ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="parentSuiteName">Name of the parent suite.</param>
        /// <param name="name">The name of the suite.</param>
		public TestSuite( string parentSuiteName, string name ) 
			: base( parentSuiteName, name ) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// </summary>
        /// <param name="fixtureType">Type of the fixture.</param>
        /// <param name="arguments">The arguments.</param>
        public TestSuite(Type fixtureType, object[] arguments = null)
            : base(fixtureType.Name)
        {
            FixtureType = fixtureType;
            FullName = fixtureType.FullName;
#if NYI
            string name = TypeHelper.GetDisplayName(fixtureType, arguments);
            this.Name = name;
            
            this.FullName = name;
            string nspace = fixtureType.Namespace;
            if (nspace != null && nspace != "")
                this.FullName = nspace + "." + name;
#endif
            Arguments = arguments;
        }

        /// <summary>
        /// Adds a test to the suite.
        /// </summary>
        /// <param name="test">The test.</param>
		public void Add( Test test ) 
		{
			test.Parent = this;
			Tests.Add(test);
		}

        /// <summary>
        /// Gets a count of test cases represented by
        /// or contained under this test.
        /// </summary>
        /// <value></value>
		public override int TestCaseCount
		{
			get
			{
				int count = 0;

				foreach(Test test in Tests)
				{
					count += test.TestCaseCount;
				}
				return count;
			}
		}

        /// <summary>
        /// The arguments to use in creating the fixture, or empty array if none are provided.
        /// </summary>
        public override object[] Arguments { get; }

        /// <summary>
        /// Gets the name used for the top-level element in the
        /// XML representation of this test
        /// </summary>
        public override string XmlElementName
        {
            get { return "test-suite"; }
        }

        /// <summary>
        /// Returns an XmlNode representing the current result after
        /// adding it as a child of the supplied parent node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="recursive">If true, descendant results are included</param>
        /// <returns></returns>
        public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            XmlNode thisNode = base.AddToXml(parentNode, recursive);
            
            thisNode.AddAttribute("type", this.TestType);
            thisNode.AddAttribute("testcasecount", this.TestCaseCount.ToString());

            if (recursive)
                foreach (Test test in this.Tests)
                    test.AddToXml(thisNode, recursive);

            return thisNode;
        }
    }
}
