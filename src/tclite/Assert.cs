// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.ComponentModel;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Delegate used by tests that execute code and
    /// capture any thrown exception.
    /// </summary>
    public delegate void TestDelegate();

    /// <summary>
    /// The Assert class contains a collection of static methods that
    /// implement the most common assertions used in TCLite.
    /// </summary>
    /// <remarks>
    /// We don't actually want any instances of this object, but some people
    /// like to inherit from it to add other static methods. Hence, the
    /// use of abstract in the declaration. 
    /// </remarks>
    public abstract partial class Assert
    {
#if !NETCF
        /// <summary>
        /// The Equals method throws an AssertionException. This is done 
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b)
        {
            throw new InvalidOperationException("Assert.Equals should not be used for Assertions");
        }

        /// <summary>
        /// override the default ReferenceEquals to throw an AssertionException. This 
        /// implementation makes sure there is no mistake in calling this function 
        /// as part of Assert. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static new void ReferenceEquals(object a, object b)
        {
            throw new InvalidOperationException("Assert.ReferenceEquals should not be used for Assertions");
        }
#endif

        /// <summary>
        /// Throws a <see cref="SuccessException"/> with the message and arguments 
        /// that are passed in. This allows a test to be cut short, with a result
        /// of success returned to TCLite.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Pass(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new SuccessException(message);
        }

        /// <summary>
        /// Throws an <see cref="AssertionException"/> with the message and arguments 
        /// that are passed in. This is used by the other Assert functions. 
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Fail(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new AssertionException(message);
        }

        /// <summary>
        /// Throws an <see cref="IgnoreException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as ignored.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Ignore(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new IgnoreException(message);
        }

        /// <summary>
        /// Throws an <see cref="InconclusiveException"/> with the message and arguments 
        /// that are passed in.  This causes the test to be reported as inconclusive.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="InconclusiveException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Inconclusive(string message=null, params object[] args)
        {
            if (message == null) message = string.Empty;
            else if (args != null && args.Length > 0)
                message = string.Format(message, args);

            throw new InconclusiveException(message);
        }

        private static void IncrementAssertCount()
        {
            TestExecutionContext.CurrentContext.IncrementAssertCount();
        }
    }
}
