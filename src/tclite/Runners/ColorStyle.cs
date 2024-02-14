// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Runners
{
    /// <summary>
    /// ColorStyle enumerates the various styles used in the console display
    /// </summary>
    public enum ColorStyle
    {
        /// <summary>
        /// Color for headers
        /// </summary>
        Header,
        /// <summary>
        /// Color for sub-headers
        /// </summary>
        SubHeader,
        /// <summary>
        /// Color for each of the section headers
        /// </summary>
        SectionHeader,
        /// <summary>
        /// The default color for items that don't fit into the other categories
        /// </summary>
        Default,
        /// <summary>
        /// Test output
        /// </summary>
        Output,
        /// <summary>
        /// Color for help text
        /// </summary>
        Help,
        /// <summary>
        /// Color for labels
        /// </summary>
        Label,
        /// <summary>
        /// Color for values, usually go beside labels
        /// </summary>
        Value,
        /// <summary>
        /// Color for passed tests
        /// </summary>
        Pass,
        /// <summary>
        /// Color for failed tests
        /// </summary>
        Failure,
        /// <summary>
        /// Color for warnings, ignored or skipped tests
        /// </summary>
        Warning,
        /// <summary>
        /// Color for errors and exceptions
        /// </summary>
        Error
    }
}
