// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Runners.TestSelection
{
    /// <summary>
    /// TestSelectionParserException is thrown when an error
    /// is found while parsing the selection expression.
    /// </summary>
    public class TestSelectionParserException : Exception
    {
        /// <summary>
        /// Construct with a message
        /// </summary>
        public TestSelectionParserException(string message) : base(message) { }

        /// <summary>
        /// Construct with a message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TestSelectionParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
