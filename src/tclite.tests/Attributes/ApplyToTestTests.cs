// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Globalization;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
#if NYI // MaxTime
    public class MaxTimeAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        [Test]
        public void MaxTimeAttributeSetsMaxTime()
        {
            new MaxTimeAttribute(2000).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.MaxTime), Is.EqualTo(2000));
        }

        [Test]
        public void MaxTimeAttributeSetsMaxTimeOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new MaxTimeAttribute(2000).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.MaxTime), Is.EqualTo(2000));
        }
    }
#endif

#if NYI // Pairwise
    public class PairwiseAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        [Test]
        public void PairwiseAttributeSetsJoinType()
        {
            new PairwiseAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Pairwise"));
        }

        [Test]
        public void PairwiseAttributeSetsJoinTypeOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new PairwiseAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Pairwise"));
        }
    }
#endif

#if NYI // Platform
    public class PlatformAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        [Test]
        public void PlatformAttributeRunsTest()
        {
            string myPlatform = GetMyPlatform();
            new PlatformAttribute(myPlatform).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
        }

        [Test]
        public void PlatformAttributeSkipsTest()
        {
            string notMyPlatform = System.IO.Path.DirectorySeparatorChar == '/'
                ? "Win" : "Linux";
            new PlatformAttribute(notMyPlatform).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Skipped));
        }

        [Test]
        public void PlatformAttributeDoesNotAffectNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            string myPlatform = GetMyPlatform();
            new PlatformAttribute(myPlatform).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.NotRunnable));
        }

        [Test]
        public void InvalidPlatformAttributeIsNotRunnable()
        {
            var invalidPlatform = "FakePlatform";
            new PlatformAttribute(invalidPlatform).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(test.Properties.Get(PropertyNames.SkipReason),
                Does.StartWith("Invalid platform name"));
            Assert.That(test.Properties.Get(PropertyNames.SkipReason),
                Does.Contain(invalidPlatform));
        }

        string GetMyPlatform()
        {
            if (System.IO.Path.DirectorySeparatorChar == '/')
            {
                return OSPlatform.CurrentPlatform.IsMacOSX ? "MacOSX" : "Linux";
            }
            return "Win";
        }
    }
#endif

#if NYI // Repeat
    public class RepeatAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        [Test]
        public void RepeatAttributeSetsRepeatCount()
        {
            new RepeatAttribute(5).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.RepeatCount), Is.EqualTo(5));
        }

        [Test]
        public void RepeatAttributeSetsRepeatCountOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new RepeatAttribute(5).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.RepeatCount), Is.EqualTo(5));
        }
    }
#endif

#if NYI // Sequential
    public class SequentialAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        [Test]
        public void SequentialAttributeSetsJoinType()
        {
            new SequentialAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Sequential"));
        }

        [Test]
        public void SequentialAttributeSetsJoinTypeOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new SequentialAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Sequential"));
        }
    }
#endif

#if NYI // SetCulture, SetUICulture
    public class SetCultureAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        public void SetCultureAttributeSetsSetCultureProperty()
        {
            new SetCultureAttribute("fr-FR").ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.SetCulture), Is.EqualTo("fr-FR"));
        }

        public void SetCultureAttributeSetsSetCulturePropertyOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new SetCultureAttribute("fr-FR").ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.SetCulture), Is.EqualTo("fr-FR"));
        }
    }

    public class SetUICultureAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        public void SetUICultureAttributeSetsSetUICultureProperty()
        {
            new SetUICultureAttribute("fr-FR").ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.SetUICulture), Is.EqualTo("fr-FR"));
        }

        public void SetUICultureAttributeSetsSetUICulturePropertyOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new SetUICultureAttribute("fr-FR").ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.SetUICulture), Is.EqualTo("fr-FR"));
        }
    }
#endif
}
