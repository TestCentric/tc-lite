// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Assertions
{
    public class AssertionTestBase
    {
        protected static readonly string NL = Environment.NewLine;
    
		private static readonly string FAILURE_MESSAGE_FMT = "  Expected: {0}" + NL + "  But was:  {1}" + NL;

        protected string StandardErrorMessage(object expected, object actual)
        {
            return string.Format(FAILURE_MESSAGE_FMT, expected, actual);
        }
    }
}
