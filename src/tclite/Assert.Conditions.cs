// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite
{
    using Constraints;

    public abstract partial class Assert
    {
        /// <summary>
        /// Asserts that a condition is true. If the condition is false
        /// the method throws a <see cref="TCLite.AssertionException"/>,
        /// ending the running test and reporting it as a Failure.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsTrue(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.True ,message, args);
        }

        /// <summary>
        /// Asserts that a condition is false. If the condition is true
        /// the method throws a <see cref="TCLite.AssertionException"/>,
        /// ending the running test and reporting it as a Failure.
        /// </summary> 
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsFalse(bool condition, string message=null, params object[] args)
        {
            Assert.That(condition, Is.False ,message, args);
        }

        /// <summary>
        /// Asserts that an object is not <c>null</c>. If the object is <c>null</c>
        /// a <see cref="TCLite.AssertionException"/> is thrown, ending the running
        /// test and  reporting it as a Failure.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotNull(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Not.Null ,message, args);
        }

        /// <summary>
        /// Asserts that an object is <c>null</c>. If the object is not <c>null</c>
        /// a <see cref="TCLite.AssertionException"/> is thrown, ending the running
        /// test and  reporting it as a Failure.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNull(object anObject, string message=null, params object[] args)
        {
            Assert.That(anObject, Is.Null ,message, args);
        }

        /// <summary>
        /// Asserts that a string is empty. If the string is not empty
        /// a <see cref="TCLite.AssertionException"/> is thrown, ending
        /// the running test and  reporting it as a Failure.
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsEmpty(string aString, string message=null, params object[] args)
        {
            Assert.That(aString, new EmptyStringConstraint(), message, args);
        }

        /// <summary>
        /// Asserts that an array, list, collection or enumeration
        /// is empty. If it is not empty a <see cref="TCLite.AssertionException"/>
        /// is thrown, ending the running test and  reporting it as a Failure.
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsEmpty(IEnumerable collection, string message=null, params object[] args)
        {
            Assert.That(collection, new EmptyCollectionConstraint(), message, args);
        }

        /// <summary>
        /// Asserts that a string is not empty. If the string is empty
        /// a <see cref="TCLite.AssertionException"/> is thrown, ending
        /// the running test and  reporting it as a Failure.
        /// </summary>
        /// <param name="aString">The string to be tested</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotEmpty(string aString, string message=null, params object[] args)
        {
            Assert.That(aString, new NotConstraint(new EmptyStringConstraint()), message, args);
        }

        /// <summary>
        /// Asserts that an array, list, collection or enumeration is 
        /// not empty. If it is empty a <see cref="TCLite.AssertionException"/>
        /// is thrown, ending the running test and  reporting it as a Failure.
        /// </summary>
        /// <param name="collection">An array, list or other collection implementing ICollection</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void IsNotEmpty(IEnumerable collection, string message=null, params object[] args)
        {
            Assert.That(collection, new NotConstraint(new EmptyCollectionConstraint()), message, args);
        }
    }
}
