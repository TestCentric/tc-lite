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
    
		private const string FAILURE_MESSAGE_FMT = "  Expected: {0}\n  But was:  {1}\n";

        protected string StandardErrorMessage(object expected, object actual)
        {
            return string.Format(FAILURE_MESSAGE_FMT, expected, actual);
        }
    }
}
