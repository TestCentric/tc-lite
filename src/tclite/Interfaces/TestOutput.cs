// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Interfaces
{
	using System;

    /// <summary>
    /// The TestOutput class holds a unit of output from 
    /// a test to either stdOut or stdErr
    /// </summary>
	public class TestOutput
	{
        /// <summary>
        /// Construct with text and an output destination type
        /// </summary>
        /// <param name="text">Text to be output</param>
        /// <param name="type">Destination of output</param>
		public TestOutput(string text, TestOutputType type)
		{
			Text = text;
			Type = type;
		}

        /// <summary>
        /// Return string representation of the object for debugging
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return $"{Type}: {Text}";
		}

        /// <summary>
        /// Get the text 
        /// </summary>
		public string Text { get; }

        /// <summary>
        /// Get the output type
        /// </summary>
		public TestOutputType Type { get; }
	}

    /// <summary>
    /// Enum representing the output destination
    /// It uses combinable flags so that a given
    /// output control can accept multiple types
    /// of output. Normally, each individual
    /// output uses a single flag value.
    /// </summary>
	public enum TestOutputType
	{
        /// <summary>
        /// Send output to stdOut
        /// </summary>
		Out, 
        
        /// <summary>
        /// Send output to stdErr
        /// </summary>
        Error,

		/// <summary>
		/// Send output to Trace
		/// </summary>
		Trace,

		/// <summary>
		/// Send output to Log
		/// </summary>
		Log
	}
}
