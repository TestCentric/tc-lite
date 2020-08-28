// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
    public class IgnoreAttributeTests
    {
        protected ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        [TestCase]
        public void IgnoreAttributeIgnoresTest()
        {
            new IgnoreAttribute().ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Ignored));
        }

        [TestCase]
        public void IgnoreAttributeSetsIgnoreReason()
        {
            new IgnoreAttribute("BECAUSE").ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Ignored));
            Assert.That(_testDummy.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("BECAUSE"));
        }

        [TestCase]
        public void IgnoreAttributeDoesNotAffectNonRunnableTest()
        {
            _testDummy.RunState = RunState.NotRunnable;
            _testDummy.Properties.Set(PropertyNames.SkipReason, "UNCHANGED");
            new IgnoreAttribute("BECAUSE").ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(_testDummy.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("UNCHANGED"));
        }
    }
}