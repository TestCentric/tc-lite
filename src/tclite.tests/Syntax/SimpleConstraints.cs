// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Syntax
{
    public class NullTest : SyntaxTest
    {
        public NullTest()
        {
            ParseTree = "<null>";
            StaticSyntax = Is.Null;
            BuilderSyntax = Builder().Null;
        }
    }

    public class TrueTest : SyntaxTest
    {
        public TrueTest()
        {
            ParseTree = "<true>";
            StaticSyntax = Is.True;
            BuilderSyntax = Builder().True;
        }
    }

    public class FalseTest : SyntaxTest
    {
        public FalseTest()
        {
            ParseTree = "<false>";
            StaticSyntax = Is.False;
            BuilderSyntax = Builder().False;
        }
    }

    public class PositiveTest : SyntaxTest
    {
        public PositiveTest()
        {
            ParseTree = "<greaterthan 0>";
            StaticSyntax = Is.Positive;
            BuilderSyntax = Builder().Positive;
        }
    }

    public class NegativeTest : SyntaxTest
    {
        public NegativeTest()
        {
            ParseTree = "<lessthan 0>";
            StaticSyntax = Is.Negative;
            BuilderSyntax = Builder().Negative;
        }
    }

    public class ZeroTest : SyntaxTest
    {
        public ZeroTest()
        {
            ParseTree = "<equal 0>";
            StaticSyntax = Is.Zero;
            BuilderSyntax = Builder().Zero;
        }
    }

    public class NaNTest : SyntaxTest
    {
        public NaNTest()
        {
            ParseTree = "<nan>";
            StaticSyntax = Is.NaN;
            BuilderSyntax = Builder().NaN;
        }
    }
}
