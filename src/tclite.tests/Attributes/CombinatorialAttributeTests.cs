// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
    public class CombinatorialAttributeTests
    {
        protected ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        [TestCase]
        public void CombinatorialAttributeSetsJoinType()
        {
            new CombinatorialAttribute().ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Combinatorial"));
        }

        [TestCase]
        public void CombinatorialAttributeSetsJoinTypeOnNonRunnableTest()
        {
            _testDummy.RunState = RunState.NotRunnable;
            new CombinatorialAttribute().ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Combinatorial"));
        }
    }
}