// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    public delegate bool IncludeDelegate();

    /// <summary>
    /// Marks an assembly, test fixture or test method as applying to a specific Culture.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = false, Inherited=false)]
    public class IncludeAttribute : TCLiteAttribute, IApplyToTest
    {
        /// <summary>
        /// Constructor with no cultures specified, for use
        /// with named property syntax.
        /// </summary>
        public IncludeAttribute() { }

        public string Cultures { get; set; }

        public string OSPlatforms { get; set; }

        #region IApplyToTest members

        /// <summary>
        /// Causes a test to be skipped if this CultureAttribute is not satisfied.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(ITest test)
        {
            if (test.RunState != RunState.NotRunnable)
            {
                bool checkCulture = !string.IsNullOrEmpty(Cultures);
                bool checkPlatform = !string.IsNullOrEmpty(OSPlatforms);

                if (!checkCulture && !checkPlatform)
                {
                    test.RunState = RunState.NotRunnable;
                    test.Properties.Set(PropertyNames.SkipReason, "No criteria for inclusion were specified");
                    return;
                }

                if (checkCulture && !IsCurrentCultureIncluded())
                {
                    test.RunState = RunState.Skipped;
                    test.Properties.Set(PropertyNames.SkipReason,
                        $"Only supported under culture {Cultures}");
                    return;
                }

                if (checkPlatform && !IsCurrentPlatformIncluded())
                {
                    test.RunState = RunState.Skipped;
                    test.Properties.Set(PropertyNames.SkipReason,
                        $"Only supported under platform {OSPlatforms}");
                    return;
                }
            }
        }

        #endregion

        /// <summary>
        /// Test to determine if the a particular culture or comma-
        /// delimited set of cultures is in use.
        /// </summary>
        /// <param name="culture">Name of the culture or comma-separated list of culture ids</param>
        /// <returns>True if the culture is in use on the system</returns>
        private bool IsCurrentCultureIncluded()
        {
            foreach(string culture in Cultures.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                if (culture == CultureInfo.CurrentCulture.Name)
                    return true;

            return false;
        }

        private bool IsCurrentPlatformIncluded()
        {
            foreach(string platformName in OSPlatforms.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                if (OSPlatform.CurrentPlatform.IsIncludedBy(platformName))
                    return true;

            return false;
        }
    }
}
