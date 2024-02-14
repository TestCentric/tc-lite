// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Internal
{
    /// <summary>
    /// The PropertyNames class provides constants for the
    /// standard property names that NUnit uses on tests.
    /// </summary>
    internal static class PropertyNames
    {
        public const string Author = "Author";
        /// <summary>
        /// The Description of a test
        /// </summary>
        public const string Description = "Description";
        
        /// <summary>
        /// The reason a test was not run
        /// </summary>
        public const string SkipReason = "_SKIPREASON";

        /// <summary>
        /// The stack trace from any data provider that threw
        /// an exception.
        /// </summary>
        public const string ProviderStackTrace = "_PROVIDERSTACKTRACE";

        /// <summary>
        /// The culture to be set for a test
        /// </summary>
        public const string SetCulture = "SetCulture";

        /// <summary>
        /// The UI culture to be set for a test
        /// </summary>
        public const string SetUICulture = "SetUICulture";

        /// <summary>
        /// The categories applying to a test
        /// </summary>
        public const string Category = "Category";

        /// <summary>
        /// The timeout value for the test
        /// </summary>
        public const string Timeout = "Timeout";

        /// <summary>
        /// The number of times the test should be repeated
        /// </summary>
        public const string RepeatCount = "Repeat";

        /// <summary>
        /// The maximum time in ms, above which the test is considered to have failed
        /// </summary>
        public const string MaxTime = "MaxTime";

        /// <summary>
        /// The selected strategy for joining parameter data into test cases
        /// </summary>
        public const string JoinType = "_JOINTYPE";

        /// <summary>
        /// The process ID of the executing assembly
        /// </summary>
        public const string ProcessID = "_PID";
        
        /// <summary>
        /// The FriendlyName of the AppDomain in which the assembly is running
        /// </summary>
        public const string AppDomain = "_APPDOMAIN";
    }
}
