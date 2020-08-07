// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Syntax
{
    public class PropertyExistsTest : SyntaxTest
    {
        public PropertyExistsTest()
        {
            ParseTree = "<propertyexists X>";
            StaticSyntax = Has.Property("X");
            BuilderSyntax = Builder().Property("X");
        }
    }

    public class PropertyExistsTest_AndFollows : SyntaxTest
    {
        public PropertyExistsTest_AndFollows()
        {
            ParseTree = "<and <propertyexists X> <equal 7>>";
            StaticSyntax = Has.Property("X").And.EqualTo(7);
            BuilderSyntax = Builder().Property("X").And.EqualTo(7);
        }
    }

    public class PropertyTest_ConstraintFollows : SyntaxTest
    {
        public PropertyTest_ConstraintFollows()
        {
            ParseTree = "<property X <greaterthan 5>>";
            StaticSyntax = Has.Property("X").GreaterThan(5);
            BuilderSyntax = Builder().Property("X").GreaterThan(5);
        }
    }

    public class PropertyTest_NotFollows : SyntaxTest
    {
        public PropertyTest_NotFollows()
        {
            ParseTree = "<property X <not <greaterthan 5>>>";
            StaticSyntax = Has.Property("X").Not.GreaterThan(5);
            BuilderSyntax = Builder().Property("X").Not.GreaterThan(5);
        }
    }

    public class LengthTest : SyntaxTest
    {
        public LengthTest()
        {
            ParseTree = "<property Length <greaterthan 5>>";
            StaticSyntax = Has.Length.GreaterThan(5);
            BuilderSyntax = Builder().Length.GreaterThan(5);
        }
    }

    public class CountTest : SyntaxTest
    {
        public CountTest()
        {
            ParseTree = "<property Count <equal 5>>";
            StaticSyntax = Has.Count.EqualTo(5);
            BuilderSyntax = Builder().Count.EqualTo(5);
        }
    }

    public class MessageTest : SyntaxTest
    {
        public MessageTest()
        {
            ParseTree = @"<property Message <startswith ""Expected"">>";
            StaticSyntax = Has.Message.StartsWith("Expected");
            BuilderSyntax = Builder().Message.StartsWith("Expected");
        }
    }

    public class PropertySyntaxVariations
    {
        private readonly int[] ints = new int[] { 1, 2, 3 };

        [TestCase]
        public void ExistenceTest()
        {
            Assert.That(ints, Has.Property("Length"));
            Assert.That(ints, Has.Length);
        }

        [TestCase]
        public void SeparateConstraintTest()
        {
            Assert.That(ints, Has.Property("Length").EqualTo(3));
            Assert.That(ints, Has.Length.EqualTo(3));
        }
    }
}
