// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
    [TestFixture]
    public class ApplyToTestTests
    {
        private ITest _testDummy;

        public ApplyToTestTests()
        {
            _testDummy = new TestDummy();
            _testDummy.RunState = RunState.Runnable;
        }

        #region CategoryAttribute

        [TestCase('!')]
        [TestCase('+')]
        [TestCase(',')]
        [TestCase('-')]
        public void CategoryAttributePassesOnSpecialCharacters(char specialCharacter)
        {
            var categoryName = new string(specialCharacter, 5);
            new CategoryAttribute(categoryName).ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Category), Is.EqualTo(categoryName));
        }

        [TestCase]
        public void CategoryAttributeSetsCategory()
        {
            new CategoryAttribute("database").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Category), Is.EqualTo("database"));
        }

        [TestCase]
        public void CategoryAttributeSetsCategoryOnNonRunnableTest()
        {
            _testDummy.RunState = RunState.NotRunnable;
            new CategoryAttribute("database").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Category), Is.EqualTo("database"));
        }

        [TestCase]
        public void CategoryAttributeSetsMultipleCategories()
        {
            new CategoryAttribute("group1").ApplyToTest(_testDummy);
            new CategoryAttribute("group2").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties[PropertyNames.Category],
                //Is.EquivalentTo(new string[] { "group1", "group2" }));
                Is.EqualTo(new string[] { "group1", "group2" }));
        }

        #endregion

        #region IgnoreAttribute

        [TestCase]
        public void IgnoreAttributeIgnoresTest()
        {
            new IgnoreAttribute("BECAUSE").ApplyToTest(_testDummy);
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

        #endregion

        #region CombinatorialAttribute

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

        #endregion

#if NYI // Culture
        #region CultureAttribute

        [Test]
        public void CultureAttributeIncludingCurrentCultureRunsTest()
        {
            string name = System.Globalization.CultureInfo.CurrentCulture.Name;
            new CultureAttribute(name).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
        }

        [Test]
        public void CultureAttributeDoesNotAffectNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            string name = System.Globalization.CultureInfo.CurrentCulture.Name;
            new CultureAttribute(name).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.NotRunnable));
        }

        [Test]
        public void CultureAttributeExcludingCurrentCultureSkipsTest()
        {
            string name = System.Globalization.CultureInfo.CurrentCulture.Name;
            CultureAttribute attr = new CultureAttribute(name);
            attr.Exclude = name;
            attr.ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Skipped));
            Assert.That(test.Properties.Get(PropertyNames.SkipReason),
                Is.EqualTo("Not supported under culture " + name));
        }

        [Test]
        public void CultureAttributeIncludingOtherCultureSkipsTest()
        {
            string name = "fr-FR";
            if (System.Globalization.CultureInfo.CurrentCulture.Name == name)
                name = "en-US";

            new CultureAttribute(name).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Skipped));
            Assert.That(test.Properties.Get(PropertyNames.SkipReason),
                Is.EqualTo("Only supported under culture " + name));
        }

        [Test]
        public void CultureAttributeExcludingOtherCultureRunsTest()
        {
            string other = "fr-FR";
            if (System.Globalization.CultureInfo.CurrentCulture.Name == other)
                other = "en-US";

            CultureAttribute attr = new CultureAttribute();
            attr.Exclude = other;
            attr.ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
        }

        [Test]
        public void CultureAttributeWithMultipleCulturesIncluded()
        {
            string current = System.Globalization.CultureInfo.CurrentCulture.Name;
            string other = current == "fr-FR" ? "en-US" : "fr-FR";
            string cultures = current + "," + other;

            new CultureAttribute(cultures).ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
        }

        #endregion
#endif

#if NYI // MaxTime
        #region MaxTimeAttribute

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

        #endregion
#endif

#if NYI // Pairwise
        #region PairwiseAttribute

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

        #endregion
#endif

#if NYI // Platform
        #region PlatformAttribute

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

        #endregion
#endif

#if NYI // Repeat
        #region RepeatAttribute

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

        #endregion
#endif

#if NYI // Sequential
        #region SequentialAttribute

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

        #endregion
#endif

#if NYI // SetCulture, SetUICulture
        #region SetCultureAttribute

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

        #endregion

        #region SetUICultureAttribute

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

        #endregion
#endif
    }
}
