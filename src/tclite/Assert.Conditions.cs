// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite.Framework
{
    using Constraints;

    public abstract partial class Assert
    {
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsTrue(bool condition, string message=null, params object[] args)
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
        public static void IsFalse(bool condition, string message=null, params object[] args)
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
        public static void IsNotNull(object anObject, string message=null, params object[] args)
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
        public static void IsNull(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Null ,message, args);
        }

        /// <summary>
        /// Verifies that the double that is passed in is an <c>NaN</c> value. Returns without throwing an
        /// exception when inside a multiple assert block.
        /// </summary>
        /// <param name="aDouble">The value that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNaN(double aDouble, string message=null, params object[] args)
        {
            Assert.That(aDouble, Is.NaN, message, args);
        }

        /// <summary>
        /// Assert that a string is empty. Returns without throwing an exception when inside a multiple assert block.
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsEmpty(string aString, string message=null, params object[] args)
        {
            Assert.That(aString, new EmptyStringConstraint(), message, args);
        }

        /// <summary>
        /// Assert that an array, list or other collection is empty. Returns without throwing an exception when inside a
        /// multiple assert block.
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsEmpty(IEnumerable collection, string message=null, params object[] args)
        {
            Assert.That(collection, new EmptyCollectionConstraint(), message, args);
        }

        /// <summary>
        /// Assert that a string is not empty. Returns without throwing an exception when inside a multiple assert
        /// block.
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotEmpty(string aString, string message=null, params object[] args)
        {
            Assert.That(aString, Is.Not.Empty, message, args);
        }

        /// <summary>
        /// Assert that an array, list or other collection is not empty. Returns without throwing an exception when
        /// inside a multiple assert block.
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotEmpty(IEnumerable collection, string message=null, params object[] args)
        {
            Assert.That(collection, Is.Not.Empty, message, args);
        }
    }
}
