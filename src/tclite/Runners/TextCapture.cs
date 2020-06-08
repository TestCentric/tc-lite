// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text;
using TCLite.Framework.Api;

namespace TCLite.Runners
{
    /// <summary>
    /// The TextCapture class intercepts console output and sends it
    /// through the ITestListener.TestOutput method.
    /// </summary>
    public class TextCapture : TextWriter
    {
        private readonly ITestListener _listener;
        private readonly TestOutputType _outputType;

        public TextCapture(ITestListener listener, TestOutputType outputType)
        {
            _listener = listener;
            _outputType = outputType;
        }

        /// <summary>
        /// Gets the Encoding in use by this TextWriter
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        /// <summary>
        /// Writes a single character
        /// </summary>
        /// <param name="value">The char to write</param>
        public override void Write(char value)
        {
            _listener.TestOutput(new TestOutput(value.ToString(), _outputType));
        }

        /// <summary>
        /// Writes a string
        /// </summary>
        /// <param name="value">The string to write</param>
        public override void Write(string value)
        {
            _listener.TestOutput(new TestOutput(value, _outputType));
        }

        /// <summary>
        /// Writes a string followed by a line terminator
        /// </summary>
        /// <param name="value">The string to write</param>
        public override void WriteLine(string value)
        {
            _listener.TestOutput(new TestOutput(value + Environment.NewLine, _outputType));
        }
    }
}
