// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite
{
    public abstract partial class Assert
    {
        /// <summary>
        /// Asserts that two instances of a type are equal. If not,
        /// then a <see cref="TCLite.AssertionException"/> is thrown, ending
        /// the test and reporting it as a Failure.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual<T1,T2>(T1 expected, T2 actual, string message = null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Asserts that two instances of a type are equal. If not,
        /// then a <see cref="TCLite.AssertionException"/> is thrown, ending
        /// the test and reporting it as a Failure.
        /// </summary>
        /// <remarks>
        /// This overload is used when two different types are compared
        /// or if the compiler can't determine the proper types for
        /// the generic overload.
        /// </remarks>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreEqual(object expected, object actual, string message = null, params object[] args)
        {
            Assert.That(actual, Is.EqualTo(expected), message, args);
        }

        /// <summary>
        /// Asserts that two doubles are equal within the specified
        /// tolerance. If not, a <see cref="TCLite.AssertionException"/> is
        /// thrown, ending the test and reporting it as a Failure.
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
        /// Asserts that two doubles are equal within the specified
        /// tolerance. If not, a <see cref="TCLite.AssertionException"/> is
        /// thrown, ending the test and reporting it as a Failure.
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
        /// Asserts that two instances of a type are not equal. If equal,
        /// a <see cref="TCLite.AssertionException"/> is thrown, ending
        /// the test and reporting it as a Failure.
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
        /// Asserts that two references refer to the same instance of
        /// a Type. If not, a <see cref="TCLite.AssertionException"/> is thrown,
        /// ending the test and reporting it as a Failure.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreSame<T>(
            T expected, T actual, string message=null, params object[] args) where T : class
        {
            Assert.That(actual, Is.SameAs(expected), message, args);
        }

        /// <summary>
        /// Asserts that two references do not refer to the same instance.
        /// If they do, a <see cref="TCLite.AssertionException"/> is thrown,
        /// ending the test and reporting it as a Failure.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to display in case of failure</param>
        /// <param name="args">Array of objects to be used in formatting the message</param>
        public static void AreNotSame<T>(
            T expected, T actual, string message=null, params object[] args) where T : class
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
        private static void AssertDoublesAreEqual(double expected, double actual, double delta, string message, object[] args)
        {
            if (double.IsNaN(expected) || double.IsInfinity(expected))
                Assert.That(actual, Is.EqualTo(expected), message, args);
            else
                Assert.That(actual, Is.EqualTo(expected).Within(delta), message, args);
        }
    }
}
