// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Attributes
{
    [Description("Fixture Description")]
    public class DescriptionAttributeTests
    {
        private const string SHORT_DESCRIPTION="Short Description";
        private const string LONG_DESCRIPTION="This is a really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really, really long description";
        private string _testDescription = TestContext.CurrentTest.Description;

        [TestCase, Description(SHORT_DESCRIPTION)]
        public void DescriptionOnTestMethod()
        {
            Assert.AreEqual(SHORT_DESCRIPTION, _testDescription);
        }

        [TestCase, Description(LONG_DESCRIPTION)]
        public void LongDescription()
        {
            Assert.AreEqual(LONG_DESCRIPTION, _testDescription);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Description("Common Description")]
        public void DescriptionAttributeAppliesToEveryTestCase(int x)
        {
            Assert.AreEqual("Common Description", TestContext.CurrentTest.Description);
        }

        [TestCase(Description = "From Property"), Description("OVERRIDDEN")]
        public void DescriptionAttributeOverridesDescriptionProperty()
        {
            Assert.AreEqual("OVERRIDDEN", TestContext.CurrentTest.Description);
        }

        [TestCase]
        public void FixtureDescriptionAppliesToEachTestWithoutItsOwnDescription()
        {
            Assert.AreEqual("Fixture Description", _testDescription);
        }
    }
}
