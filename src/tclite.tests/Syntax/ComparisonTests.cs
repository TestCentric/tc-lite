// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Syntax
{
    public class GreaterThanTest : SyntaxTest
    {
        public GreaterThanTest()
        {
            ParseTree = "<greaterthan 7>";
            StaticSyntax = Is.GreaterThan(7);
            BuilderSyntax = Builder().GreaterThan(7);
        }
    }

    public class GreaterThanOrEqualTest : SyntaxTest
    {
        public GreaterThanOrEqualTest()
        {
            ParseTree = "<greaterthanorequal 7>";
            StaticSyntax = Is.GreaterThanOrEqualTo(7);
            BuilderSyntax = Builder().GreaterThanOrEqualTo(7);
        }
    }

    public class AtLeastTest : SyntaxTest
    {
        public AtLeastTest()
        {
            ParseTree = "<greaterthanorequal 7>";
            StaticSyntax = Is.AtLeast(7);
            BuilderSyntax = Builder().AtLeast(7);
        }
    }

    public class LessThanTest : SyntaxTest
    {
        public LessThanTest()
        {
            ParseTree = "<lessthan 7>";
            StaticSyntax = Is.LessThan(7);
            BuilderSyntax = Builder().LessThan(7);
        }
    }

    public class LessThanOrEqualTest : SyntaxTest
    {
        public LessThanOrEqualTest()
        {
            ParseTree = "<lessthanorequal 7>";
            StaticSyntax = Is.LessThanOrEqualTo(7);
            BuilderSyntax = Builder().LessThanOrEqualTo(7);
        }
    }

    public class AtMostTest : SyntaxTest
    {
        public AtMostTest()
        {
            ParseTree = "<lessthanorequal 7>";
            StaticSyntax = Is.AtMost(7);
            BuilderSyntax = Builder().AtMost(7);
        }
    }
}
