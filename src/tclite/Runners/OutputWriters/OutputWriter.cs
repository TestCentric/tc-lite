// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.IO;
using System.Text;
using TCLite.Interfaces;

namespace TCLite.Runners
{
    /// <summary>
    /// OutputWriter is an abstract class used to write test
    /// results to a file in various formats. Specific 
    /// OutputWriters are derived from this class.
    /// </summary>
    public abstract class OutputWriter
    {
        /// <summary>
        /// Writes a test result to a file
        /// </summary>
        /// <param name="result">The result to be written</param>
        /// <param name="outputPath">Path to the file to which the result is written</param>
        public void WriteResultFile(ITestResult result, string outputPath)
        {
            using (StreamWriter writer = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                WriteResultFile(result, writer);
            }
        }

        /// <summary>
        /// Abstract method that writes a test result to a TextWriter
        /// </summary>
        /// <param name="result">The result to be written</param>
        /// <param name="writer">A TextWriter to which the result is written</param>
        public abstract void WriteResultFile(ITestResult result, TextWriter writer);
    }
}
