// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
    [Description("Fixture Description")]
    public class DescriptionTests
    {
        private const string SHORT_DESCRIPTION="Short Description";
        private const string LONG_DESCRIPTION="This is a really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really long description";
        private IPropertyBag _properties = TestExecutionContext.CurrentContext.CurrentTest.Properties;

        [TestCase, Description(SHORT_DESCRIPTION)]
        public void DescriptionOnTestMethod()
        {
            Assert.AreEqual(SHORT_DESCRIPTION, _properties.Get(PropertyNames.Description));
        }

        [TestCase, Description(LONG_DESCRIPTION)]
        public void LongDescription()
        {
            Assert.AreEqual(LONG_DESCRIPTION, _properties.Get(PropertyNames.Description));
        }

        [TestCase]
        public void FixtureDescription()
        {
            Assert.AreEqual("Fixture Description", _properties.Get(PropertyNames.Description));
        }
    }
}
