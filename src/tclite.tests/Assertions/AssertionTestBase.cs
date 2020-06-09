// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Assertions
{
    public class AssertionTestBase
    {
        protected static readonly string NL = Environment.NewLine;

        protected static void ThrowsSuccessException(TestDelegate del, string message=null)
        {
            var ex = Assert.Throws<SuccessException>(() => del());
            if (message != null)
                Assert.AreEqual(message, ex.Message);
        }

        protected static void ThrowsAssertionException(TestDelegate del, string message=null)
        {
            var ex = Assert.Throws<AssertionException>(() => del());
            if (message != null)
                Assert.AreEqual(message, ex.Message);
        }

        protected static void ThrowsIgnoreException(TestDelegate del, string message=null)
        {
            var ex = Assert.Throws<IgnoreException>(() => del());
            if (message != null)
                Assert.AreEqual(message, ex.Message);
        }

        protected static void ThrowsInconclusiveException(TestDelegate del, string message=null)
        {
            var ex = Assert.Throws<InconclusiveException>(() => del());
            if (message != null)
                Assert.AreEqual(message, ex.Message);
        }
    }
}
