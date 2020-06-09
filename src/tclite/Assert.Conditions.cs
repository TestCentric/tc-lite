// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
    public abstract partial class Assert
    {
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void True(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.True ,message, args);
        }

        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionException"/>
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void False(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.False ,message, args);
        }

        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void NotNull(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Not.Null ,message, args);
        }

        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void Null(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Null ,message, args);
        }
    }
}
