// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TCLite.Internal
{
    /// <summary>
    /// StackFilter class is used to remove internal TCLite
    /// entries from a stack trace so that the resulting
    /// trace provides better information about the test.
    /// </summary>
    public class StackFilter
    {
        private const string DEFAULT_TOP_OF_STACK_PATTERN = @" TCLite\.(Assert|Assume|Warn)\.";
        private const string DEFAULT_BOTTOM_OF_STACK_PATTERN = @" System\.(Reflection|RuntimeMethodHandle|Threading\.ExecutionContext)\.";

        /// <summary>
        /// Single instance of our default filter
        /// </summary>
        public static StackFilter DefaultFilter = new StackFilter();

        private readonly Regex _topOfStackRegex;
        private readonly Regex _bottomOfStackRegex;

        private static readonly Regex TOP_OF_STACK_REGEX = new Regex(DEFAULT_TOP_OF_STACK_PATTERN, RegexOptions.Compiled);
        private static readonly Regex BOTTOM_OF_STACK_REGEX = new Regex(DEFAULT_BOTTOM_OF_STACK_PATTERN, RegexOptions.Compiled);

        /// <summary>
        /// Construct a stack filter instance
        /// </summary>
        /// <param name="topOfStackPattern">Regex pattern used to delete lines from the top of the stack</param>
        /// <param name="bottomOfStackPattern">Regex pattern used to delete lines from the bottom of the stack</param>
        public StackFilter(string topOfStackPattern, string bottomOfStackPattern)
        {
            if (topOfStackPattern != null)
                _topOfStackRegex = new Regex(topOfStackPattern, RegexOptions.Compiled);
            if (bottomOfStackPattern != null)
                _bottomOfStackRegex = new Regex(bottomOfStackPattern, RegexOptions.Compiled);
        }

        /// <summary>
        /// Construct a stack filter instance
        /// </summary>
        /// <param name="topOfStackPattern">Regex pattern used to delete lines from the top of the stack</param>
        public StackFilter(string topOfStackPattern)
            : this(topOfStackPattern, DEFAULT_BOTTOM_OF_STACK_PATTERN) { }

        /// <summary>
        /// Construct a stack filter instance
        /// </summary>
        public StackFilter()
            : this(DEFAULT_TOP_OF_STACK_PATTERN, DEFAULT_BOTTOM_OF_STACK_PATTERN) { }

        /// <summary>
        /// Filters a raw stack trace and returns the result.
        /// </summary>
        /// <param name="rawTrace">The original stack trace</param>
        /// <returns>A filtered stack trace</returns>
        public string Filter(string rawTrace)
        {
            if (rawTrace == null) return null;

            StringReader sr = new StringReader(rawTrace);
            StringWriter sw = new StringWriter();

            try
            {
                string line = sr.ReadLine();

                if (_topOfStackRegex != null)
                    // First, skip past any Assert, Assume or MultipleAssertBlock lines
                    while (line != null && _topOfStackRegex.IsMatch(line))
                        line = sr.ReadLine();

                // Copy lines down to the line that invoked the failing method.
                // This is actually only needed for the compact framework, but
                // we do it on all platforms for simplicity. Desktop platforms
                // won't have any System.Reflection lines.
                while (line != null)
                {
                    if (_bottomOfStackRegex != null && _bottomOfStackRegex.IsMatch(line))
                        break;
                        
                    sw.WriteLine(line);
                    line = sr.ReadLine();
                }
            }
            catch (Exception)
            {
                return rawTrace;
            }

            return sw.ToString();
        }
    }
}
