// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Xml;
using TCLite.Interfaces;
using TCLite.Internal;
using TCLite.WorkItems;

namespace TCLite.Attributes
{
    public class TestDummy : Test
    {
        public TestDummy() : base("TestDummy") { }

        #region Overrides

        public string TestKind
        {
            get { return "dummy-test"; }
        }

        public override XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            throw new NotImplementedException();
        }

        public Commands.TestCommand MakeTestCommand()
        {
            throw new NotImplementedException();
        }

        public override TestResult MakeTestResult()
        {
            throw new NotImplementedException();
        }

        public override WorkItem CreateWorkItem(ITestFilter childFilter)
        {
            throw new NotImplementedException();
        }

        public override string XmlElementName
        {
            get { throw new NotImplementedException(); }
        }

        public override object[] Arguments
        {
            get
            {
                return new object[0];
            }
        }

        #endregion
    }
}
