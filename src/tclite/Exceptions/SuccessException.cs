// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
    using System;

    /// <summary>
    /// Thrown when an assertion failed.
    /// </summary>
    [Serializable]
    public class SuccessException : System.Exception
    {
        /// <param name="message"></param>
        public SuccessException(string message)
            : base(message)
        { }

        /// <param name="message">The error message that explains 
        /// the reason for the exception</param>
        /// <param name="inner">The exception that caused the 
        /// current exception</param>
        public SuccessException(string message, Exception inner)
            :
            base(message, inner)
        { }

#if !NETCF && !SILVERLIGHT
        /// <summary>
        /// Serialization Constructor
        /// </summary>
        protected SuccessException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context)
        { }
#endif
    }
}
