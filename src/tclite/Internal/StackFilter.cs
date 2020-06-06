// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// StackFilter class is used to remove internal NUnit
    /// entries from a stack trace so that the resulting
    /// trace provides better information about the test.
    /// </summary>
    public static class StackFilter
    {
        private const string TOP_OF_STACK_PATTERN = @" TCLite\.Framework\.(Assert|Assume|Warn|CollectionAssert|StringAssert|FileAssert|DirectoryAssert)\.";
        private static readonly Regex TOP_OF_STACK_REGEX = new Regex(TOP_OF_STACK_PATTERN, RegexOptions.Compiled);

        private const string BOTTOM_OF_STACK_PATTERN = @" System\.(Reflection|RuntimeMethodHandle|Threading\.ExecutionContext)\.";
        private static readonly Regex BOTTOM_OF_STACK_REGEX = new Regex(BOTTOM_OF_STACK_PATTERN, RegexOptions.Compiled);

        /// <summary>
        /// Filters a raw stack trace and returns the result.
        /// </summary>
        /// <param name="rawTrace">The original stack trace</param>
        /// <returns>A filtered stack trace</returns>
        public static string Filter(string rawTrace)
        {
            if (rawTrace == null) return null;

            StringReader sr = new StringReader(rawTrace);
            StringWriter sw = new StringWriter();

            try
            {
                string line = sr.ReadLine();

                while (line != null && TOP_OF_STACK_REGEX.IsMatch(line))
                    line = sr.ReadLine();

                while (line != null)
                {
                    if (BOTTOM_OF_STACK_REGEX.IsMatch(line))
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
