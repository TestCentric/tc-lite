// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Runners;

namespace TCLite.Tests
{
    class Program
    {
        static int Main(string[] args)
        {
            return new TestRunner().Execute(args);
        }
    }
}
