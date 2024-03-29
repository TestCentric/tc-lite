// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Internal 
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Thrown when an assertion failed. Here to preserve the inner
	/// exception and hence its stack trace.
	/// </summary>
	[Serializable]
	internal class TCLiteException : Exception 
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TCLiteException"/> class.
        /// </summary>
		public TCLiteException () : base() 
		{} 

		/// <summary>
        /// Initializes a new instance of the <see cref="TCLiteException"/> class.
        /// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		public TCLiteException(string message) : base (message)
		{}

		/// <summary>
        /// Initializes a new instance of the <see cref="TCLiteException"/> class.
        /// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		/// <param name="inner">The exception that caused the 
		/// current exception</param>
		public TCLiteException(string message, Exception inner) :
			base(message, inner) 
		{}

		/// <summary>
		/// Serialization Constructor
		/// </summary>
		protected TCLiteException(SerializationInfo info, 
			StreamingContext context) : base(info,context){}
	}
}
