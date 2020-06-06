// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
    using System;

    /// <summary>
    /// Thrown when a test executes inconclusively.
    /// </summary>
    [Serializable]
    public class InconclusiveException : System.Exception
    {
        /// <param name="message">The error message that explains 
        /// the reason for the exception</param>
        public InconclusiveException(string message)
            : base(message)
        { }

        /// <param name="message">The error message that explains 
        /// the reason for the exception</param>
        /// <param name="inner">The exception that caused the 
        /// current exception</param>
        public InconclusiveException(string message, Exception inner)
            :
            base(message, inner)
        { }

#if !NETCF && !SILVERLIGHT
        /// <summary>
        /// Serialization Constructor
        /// </summary>
        protected InconclusiveException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
#endif
    }
}
