// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite.Attributes
{
    public class IncludeAttributeTests
    {
        private ITest _testDummy = new TestDummy() { RunState = RunState.Runnable };

        private static readonly string _currentCulture = CultureInfo.CurrentCulture.Name;
        private static readonly string _otherCulture = _currentCulture == "fr-FR"
            ? "en-US"
            : "fr-FR";

        [TestCase]
        public void IncludeCurrentCultureRunsTest()
        {
            (new IncludeAttribute() { Cultures = _currentCulture }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Runnable));
        }

        [TestCase]
        public void IncludeDoesNotAffectNonRunnableTest()
        {
            _testDummy.RunState = RunState.NotRunnable;
            (new IncludeAttribute() { Cultures = _currentCulture }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.NotRunnable));
        }

        // [Test]
        // public void CultureAttributeExcludingCurrentCultureSkipsTest()
        // {
        //     string name = System.Globalization.CultureInfo.CurrentCulture.Name;
        //     CultureAttribute attr = new CultureAttribute(name);
        //     attr.Exclude = name;
        //     attr.ApplyToTest(test);
        //     Assert.That(test.RunState, Is.EqualTo(RunState.Skipped));
        //     Assert.That(test.Properties.Get(PropertyNames.SkipReason),
        //         Is.EqualTo("Not supported under culture " + name));
        // }

        [TestCase]
        public void IncludeDifferentCultureSkipsTest()
        {
            (new IncludeAttribute() { Cultures = _otherCulture }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Skipped));
            Assert.That(_testDummy.Properties.Get(PropertyNames.SkipReason),
                Is.EqualTo($"Only supported under culture {_otherCulture}"));
        }

        // [Test]
        // public void CultureAttributeExcludingOtherCultureRunsTest()
        // {
        //     string other = "fr-FR";
        //     if (System.Globalization.CultureInfo.CurrentCulture.Name == other)
        //         other = "en-US";

        //     CultureAttribute attr = new CultureAttribute();
        //     attr.Exclude = other;
        //     attr.ApplyToTest(test);
        //     Assert.That(test.RunState, Is.EqualTo(RunState.Runnable));
        // }

        [TestCase]
        public void IncludeMultipleCultures()
        {
            string twoCultures = _currentCulture + "," + _otherCulture;
            (new IncludeAttribute() { Cultures = twoCultures }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Runnable));
        }

        [TestCase]
        public void IncludeLinux()
        {
            (new IncludeAttribute() { OSPlatforms = "LINUX" }).ApplyToTest(_testDummy);
            var expected = OSPlatform.CurrentPlatform.IsUnix ? RunState.Runnable : RunState.Skipped;
            Assert.That(_testDummy.RunState, Is.EqualTo(expected));
        }

        [TestCase]
        public void IncludeWindows()
        {
            (new IncludeAttribute() { OSPlatforms = "WINDOWS" }).ApplyToTest(_testDummy);
            var expected = OSPlatform.CurrentPlatform.IsWindows ? RunState.Runnable : RunState.Skipped;
            Assert.That(_testDummy.RunState, Is.EqualTo(expected));
        }

        [TestCase]
        public void IncludeMacOSX()
        {
            (new IncludeAttribute() { OSPlatforms = "MACOSX" }).ApplyToTest(_testDummy);
            var expected = OSPlatform.CurrentPlatform.IsMacOSX ? RunState.Runnable : RunState.Skipped;
            Assert.That(_testDummy.RunState, Is.EqualTo(expected));
        }

        [TestCase]
        public void Include64BitOS()
        {
            (new IncludeAttribute() { OSPlatforms = "64-BIT-OS" }).ApplyToTest(_testDummy);
            var expected = Environment.Is64BitOperatingSystem ? RunState.Runnable : RunState.Skipped;
            Assert.That(_testDummy.RunState, Is.EqualTo(expected));
        }

        [TestCase]
        public void Include32BitOS()
        {
            (new IncludeAttribute() { OSPlatforms = "32-BIT-OS" }).ApplyToTest(_testDummy);
            var expected = Environment.Is64BitOperatingSystem ? RunState.Skipped : RunState.Runnable;
            Assert.That(_testDummy.RunState, Is.EqualTo(expected));
        }

        [TestCase]
        public void IncludeMultipleOSPlatforms()
        {
            (new IncludeAttribute() { OSPlatforms = "WINDOWS,LINUX,MACOSX" }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Runnable));
        }

        [TestCase]
        public void IncludeCurrentCultureAndOSPlatform()
        {
            var os = OSPlatform.CurrentPlatform;
            string name = os.IsWindows ? "WINDOWS" : os.IsMacOSX ? "MACOSX" : "LINUX";
            (new IncludeAttribute() { OSPlatforms = name, Cultures = _currentCulture }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Runnable));
        }

        [TestCase]
        public void IncludeCurrentCultureAndDifferentOSPlatform()
        {
            var os = OSPlatform.CurrentPlatform;
            string name = os.IsWindows ? "LINUX" : "WINDOWS";
            (new IncludeAttribute() { OSPlatforms = name, Cultures = _currentCulture }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Skipped));
        }

        [TestCase]
        public void IncludeDifferentCultureAndCurrentOSPlatform()
        {
            var os = OSPlatform.CurrentPlatform;
            string name = os.IsWindows ? "WINDOWS" : os.IsMacOSX ? "MACOSX" : "LINUX";
            (new IncludeAttribute() { OSPlatforms = name, Cultures = _otherCulture }).ApplyToTest(_testDummy);
            Assert.That(_testDummy.RunState, Is.EqualTo(RunState.Skipped));
        }
    }
}