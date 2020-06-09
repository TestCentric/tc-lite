// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework
{
    public abstract partial class Assert
    {
        /// <summary>
        /// Verifies that two instances of a type are equal. If they are not, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual<T>(T expected, T actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }


        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(double expected, double actual, double delta, string message=null, params object[] args)
        {
            AssertDoublesAreEqual(expected, actual, delta, message, args);
        }

         /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If 
        /// they are not equal then an <see cref="AssertionException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(double expected, double? actual, double delta, string message=null, params object[] args)
        {
            AssertDoublesAreEqual(expected, (double)actual, delta, message, args);
        }

       /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. TCLite
        /// has special semantics for some object types.
        /// If they are not equal an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two instances of a type are not equal. If they are equal, then an 
        /// <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual<T>(T expected, T actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Verifies that two objects are not equal.  Two objects are considered
        /// equal if both are null, or if both have the same value. TCLite
        /// has special semantics for some object types.
        /// If they are equal an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotEqual(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreSame(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.SameAs(expected), message, args);
        }

        /// <summary>
        /// Asserts that two objects do not refer to the same object. If they
        /// are the same an <see cref="AssertionException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotSame(object expected, object actual, string message=null, params object[] args)
        {
            Assert.That(actual, Is.Not.SameAs(expected), message, args);
        }

        /// <summary>
        /// Helper for Assert.AreEqual(double expected, double actual, ...)
        /// allowing code generation to work consistently.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        protected static void AssertDoublesAreEqual(double expected, double actual, double delta, string message, object[] args)
        {
            if (double.IsNaN(expected) || double.IsInfinity(expected))
                Assert.That(actual, Is.EqualTo(expected), message, args);
            else
                Assert.That(actual, Is.EqualTo(expected).Within(delta), message, args);
        }
    }
}
