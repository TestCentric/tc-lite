// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework
{
    /// <summary>
    /// Class used to guard against unexpected argument values
    /// by throwing an appropriate exception.
    /// </summary>
    public class Guard
    {
        /// <summary>
        /// Throws an exception if an argument is null
        /// </summary>
        /// <param name="value">The value to be tested</param>
        /// <param name="name">The name of the argument</param>
        public static void ArgumentNotNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException("Argument " + name + " must not be null", name);
        }

        /// <summary>
        /// Throws an exception if a string argument is null or empty
        /// </summary>
        /// <param name="value">The value to be tested</param>
        /// <param name="name">The name of the argument</param>
        public static void ArgumentNotNullOrEmpty(string value, string name)
        {
            ArgumentNotNull(value, name);

            if (value == string.Empty)
                throw new ArgumentException("Argument " + name +" must not be the empty string", name);
        }

        /// <summary>
        /// Throws an ArgumentException if the specified condition is not met.
        /// </summary>
        /// <param name="condition">The condition that must be met</param>
        /// <param name="message">The exception message to be used</param>
        /// <param name="paramName">The name of the argument</param>
        public static void ArgumentValid(bool condition, string message, string paramName)
        {
            if (!condition)
                throw new ArgumentException(message, paramName);
        }

        /// <summary>
        /// Throws an ArgumentException if the argument is not of the required type.
        /// We use this to give a runtime error until the Constraint system can be
        /// made completely type-safe.
        /// </summary>
        /// <param name="condition">The condition that must be met</param>
        /// <param name="message">The exception message to be used</param>
        /// <param name="paramName">The name of the argument</param>
        public static void ArgumentOfType<T>(object value, string paramName)
        {
            // Specifically excludes null, which must be checked separately
            if (value != null && !(value is T))
                throw new ArgumentException($"Argument must be of Type {typeof(T).Name}", paramName);
        }
    }
}
