// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Threading;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Attributes
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

        #region AuthorAttribute

        [TestCase]
        public void AuthorAttributeSetsAuthor()
        {
            new AuthorAttribute("Charlie").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Author), Is.EqualTo("Charlie"));
        }

        [TestCase]
        public void AuthorAttributeSetsAuthorAndEmail()
        {
            new AuthorAttribute("Charlie", "charlie@someplace.com").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Author), Is.EqualTo("Charlie <charlie@someplace.com>"));
        }

        [TestCase]
        public void AuthorAttributeSetsAuthorOnNonRunnableTest()
        {
            _testDummy.RunState = RunState.NotRunnable;
            new AuthorAttribute("Charlie").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Author), Is.EqualTo("Charlie"));
        }

        #endregion

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

        #region DescriptionAttribute

        [TestCase]
        public void DescriptionAttributeSetsDescription()
        {
            new DescriptionAttribute("Cool test!").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Description), Is.EqualTo("Cool test!"));
        }

        [TestCase]
        public void DescriptionAttributeSetsDescriptionOnNonRunnableTest()
        {
            _testDummy.RunState = RunState.NotRunnable;
            new DescriptionAttribute("Cool test!").ApplyToTest(_testDummy);
            Assert.That(_testDummy.Properties.Get(PropertyNames.Description), Is.EqualTo("Cool test!"));
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

#if NYI // Until
        [TestCase]
        public void IgnoreAttributeIgnoresTestUntilDateSpecified()
        {
            var ignoreAttribute = new IgnoreAttribute("BECAUSE");
            ignoreAttribute.Until = "4242-01-01";
            ignoreAttribute.ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Ignored));
        }

        [TestCase]
        public void IgnoreAttributeIgnoresTestUntilDateTimeSpecified()
        {
            var ignoreAttribute = new IgnoreAttribute("BECAUSE");
            ignoreAttribute.Until = "4242-01-01 12:00:00Z";
            ignoreAttribute.ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Ignored));
        }

        [TestCase]
        public void IgnoreAttributeMarksTestAsRunnableAfterUntilDatePasses()
        {
            var ignoreAttribute = new IgnoreAttribute("BECAUSE");
            ignoreAttribute.Until = "1492-01-01";
            ignoreAttribute.ApplyToTest(test);
            Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
        }

        [TestCase("4242-01-01")]
        [TestCase("4242-01-01 00:00:00Z")]
        [TestCase("4242-01-01 00:00:00")]
        public void IgnoreAttributeUntilSetsTheReason(string date)
        {
            var ignoreAttribute = new IgnoreAttribute("BECAUSE");
            ignoreAttribute.Until = date;
            ignoreAttribute.ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("Ignoring until 4242-01-01 00:00:00Z. BECAUSE"));
        }

        [TestCase]
        public void IgnoreAttributeWithInvalidDateThrowsException()
        {
            var ignoreAttribute = new IgnoreAttribute("BECAUSE");
            Assert.Throws<FormatException>(() => ignoreAttribute.Until = "Thursday the twenty fifth of December");
        }

        [TestCase]
        public void IgnoreAttributeWithUntilAddsIgnoreUntilDateProperty()
        {
            var ignoreAttribute = new IgnoreAttribute("BECAUSE");
            ignoreAttribute.Until = "4242-01-01";
            ignoreAttribute.ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.IgnoreUntilDate), Is.EqualTo("4242-01-01 00:00:00Z"));
        }

        [TestCase]
        public void IgnoreAttributeWithUntilAddsIgnoreUntilDatePropertyPastUntilDate()
        {
            var ignoreAttribute = new IgnoreAttribute("BECAUSE");
            ignoreAttribute.Until = "1242-01-01";
            ignoreAttribute.ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.IgnoreUntilDate), Is.EqualTo("1242-01-01 00:00:00Z"));
        }
#endif

        [TestCase]
        public void IgnoreAttributeWithExplicitIgnoresTest()
        {
            new IgnoreAttribute("BECAUSE").ApplyToTest(_testDummy);
            new ExplicitAttribute().ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Ignored));
        }

        #endregion

        #region ExplicitAttribute

        [TestCase]
        public void ExplicitAttributeMakesTestExplicit()
        {
            new ExplicitAttribute().ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Explicit));
        }

        [TestCase]
        public void ExplicitAttributeSetsIgnoreReason()
        {
            new ExplicitAttribute("BECAUSE").ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Explicit));
            Assert.That(_testDummy.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("BECAUSE"));
        }

        [TestCase]
        public void ExplicitAttributeDoesNotAffectNonRunnableTest()
        {
            _testDummy.RunState = RunState.NotRunnable;
            _testDummy.Properties.Set(PropertyNames.SkipReason, "UNCHANGED");
            new ExplicitAttribute("BECAUSE").ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.NotRunnable));
            Assert.That(_testDummy.Properties.Get(PropertyNames.SkipReason), Is.EqualTo("UNCHANGED"));
        }

        [TestCase]
        public void ExplicitAttributeWithIgnoreIgnoresTest()
        {
            new ExplicitAttribute().ApplyToTest(_testDummy);
            new IgnoreAttribute("BECAUSE").ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Ignored));
        }

        #endregion

#if NYI
        #region CombinatorialAttribute

        [Test]
        public void CombinatorialAttributeSetsJoinType()
        {
            new CombinatorialAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Combinatorial"));
        }

        [Test]
        public void CombinatorialAttributeSetsJoinTypeOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new CombinatorialAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.JoinType), Is.EqualTo("Combinatorial"));
        }

        #endregion

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

        #region RequiresMTAAttribute

        [Test]
        public void RequiresMTAAttributeSetsApartmentState()
        {
            new ApartmentAttribute(ApartmentState.MTA).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.ApartmentState),
                Is.EqualTo(ApartmentState.MTA));
        }

        [Test]
        public void RequiresMTAAttributeSetsApartmentStateOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new ApartmentAttribute(ApartmentState.MTA).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.ApartmentState),
                Is.EqualTo(ApartmentState.MTA));
        }

        #endregion

        #region RequiresSTAAttribute

        [Test]
        public void RequiresSTAAttributeSetsApartmentState()
        {
            new ApartmentAttribute(ApartmentState.STA).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.ApartmentState),
                Is.EqualTo(ApartmentState.STA));
        }

        [Test]
        public void RequiresSTAAttributeSetsApartmentStateOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new ApartmentAttribute(ApartmentState.STA).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.ApartmentState),
                Is.EqualTo(ApartmentState.STA));
        }

        #endregion

        #region RequiresThreadAttribute

        [Test]
        public void RequiresThreadAttributeSetsRequiresThread()
        {
            new RequiresThreadAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.RequiresThread), Is.EqualTo(true));
        }

        [Test]
        public void RequiresThreadAttributeSetsRequiresThreadOnNonRunnableTest()
        {
            test.RunState = RunState.NotRunnable;
            new RequiresThreadAttribute().ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.RequiresThread), Is.EqualTo(true));
        }

        [Test]
        public void RequiresThreadAttributeMaySetApartmentState()
        {
            new RequiresThreadAttribute(ApartmentState.STA).ApplyToTest(test);
            Assert.That(test.Properties.Get(PropertyNames.RequiresThread), Is.EqualTo(true));
            Assert.That(test.Properties.Get(PropertyNames.ApartmentState),
                Is.EqualTo(ApartmentState.STA));
        }

        #endregion

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
