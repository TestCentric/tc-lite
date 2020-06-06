// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// The PropertyNames class provides static constants for the
    /// standard property names that NUnit uses on tests.
    /// </summary>
    public class PropertyNames
    {
        /// <summary>
        /// The Description of a test
        /// </summary>
        public static readonly string Description = "Description";
        
        /// <summary>
        /// The reason a test was not run
        /// </summary>
        public static readonly string SkipReason = "_SKIPREASON";

        /// <summary>
        /// The stack trace from any data provider that threw
        /// an exception.
        /// </summary>
        public static readonly string ProviderStackTrace = "_PROVIDERSTACKTRACE";

        /// <summary>
        /// The culture to be set for a test
        /// </summary>
        public static readonly string SetCulture = "SetCulture";

        /// <summary>
        /// The UI culture to be set for a test
        /// </summary>
        public static readonly string SetUICulture = "SetUICulture";

        /// <summary>
        /// The categories applying to a test
        /// </summary>
        public static readonly string Category = "Category";

        /// <summary>
        /// The timeout value for the test
        /// </summary>
        public static readonly string Timeout = "Timeout";

        /// <summary>
        /// The number of times the test should be repeated
        /// </summary>
        public static readonly string RepeatCount = "Repeat";

        /// <summary>
        /// The maximum time in ms, above which the test is considered to have failed
        /// </summary>
        public static readonly string MaxTime = "MaxTime";

        /// <summary>
        /// The selected strategy for joining parameter data into test cases
        /// </summary>
        public static readonly string JoinType = "_JOINTYPE";

        /// <summary>
        /// The process ID of the executing assembly
        /// </summary>
        public static readonly string ProcessID = "_PID";
        
        /// <summary>
        /// The FriendlyName of the AppDomain in which the assembly is running
        /// </summary>
        public static readonly string AppDomain = "_APPDOMAIN";
    }
}
