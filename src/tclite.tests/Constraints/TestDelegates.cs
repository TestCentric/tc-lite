// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Constraints
{
    public class TestDelegates
    {
        public static void ThrowsArgumentException()
        {
            throw new ArgumentException("myMessage", "myParam");
        }

        public static void ThrowsSystemException()
        {
            throw new Exception();
        }

        public static void ThrowsNothing()
        {
        }

        public static void ThrowsDerivedException()
        {
            throw new DerivedException();
        }

        public class DerivedException : Exception
        {
        }
    }
}
