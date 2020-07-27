// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.TestUtilities.Comparers;

namespace TCLite.Framework.Syntax
{
    public class UniqueTest : SyntaxTest
    {
        public UniqueTest()
        {
            ParseTree = "<uniqueitems>";
            StaticSyntax = Is.Unique;
            BuilderSyntax = Builder().Unique;
        }
    }

#if NYI // Ordered
    public class CollectionOrderedTest : SyntaxTest
    {
        public CollectionOrderedTest()
        {
            ParseTree = "<ordered>";
            StaticSyntax = Is.Ordered;
            BuilderSyntax = Builder().Ordered;
        }
    }

    public class CollectionOrderedTest_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            ParseTree = "<ordered descending>";
            StaticSyntax = Is.Ordered.Descending;
            BuilderSyntax = Builder().Ordered.Descending;
        }
    }

    public class CollectionOrderedTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            IComparer comparer = ObjectComparer.Default;
            ParseTree = "<ordered NUnit.TestUtilities.Comparers.ObjectComparer>";
            StaticSyntax = Is.Ordered.Using(comparer);
            BuilderSyntax = Builder().Ordered.Using(comparer);
        }
    }

    public class CollectionOrderedTest_Comparer_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            IComparer comparer = ObjectComparer.Default;
            ParseTree = "<ordered descending NUnit.TestUtilities.Comparers.ObjectComparer>";
            StaticSyntax = Is.Ordered.Using(comparer).Descending;
            BuilderSyntax = Builder().Ordered.Using(comparer).Descending;
        }
    }

    public class CollectionOrderedByTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            ParseTree = "<orderedby SomePropertyName>";
            StaticSyntax = Is.Ordered.By("SomePropertyName");
            BuilderSyntax = Builder().Ordered.By("SomePropertyName");
        }
    }

    public class CollectionOrderedByTest_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            ParseTree = "<orderedby SomePropertyName descending>";
            StaticSyntax = Is.Ordered.By("SomePropertyName").Descending;
            BuilderSyntax = Builder().Ordered.By("SomePropertyName").Descending;
        }
    }

    public class CollectionOrderedByTest_Comparer : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            ParseTree = "<orderedby SomePropertyName NUnit.TestUtilities.Comparers.ObjectComparer>";
            StaticSyntax = Is.Ordered.By("SomePropertyName").Using(ObjectComparer.Default);
            BuilderSyntax = Builder().Ordered.By("SomePropertyName").Using(ObjectComparer.Default);
        }
    }

    public class CollectionOrderedByTest_Comparer_Descending : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            ParseTree = "<orderedby SomePropertyName descending NUnit.TestUtilities.Comparers.ObjectComparer>";
            StaticSyntax = Is.Ordered.By("SomePropertyName").Using(ObjectComparer.Default).Descending;
            BuilderSyntax = Builder().Ordered.By("SomePropertyName").Using(ObjectComparer.Default).Descending;
        }
    }
#endif

    public class CollectionContainsTest : SyntaxTest
    {
        public CollectionContainsTest()
        {
            ParseTree = "<some <equal 42>>";
            StaticSyntax = Has.Member.EqualTo(42);
            BuilderSyntax = Builder().Contains(42);
        }
    }

#if NYI // Subset
    public class CollectionSubsetTest : SyntaxTest
    {
        [SetUp]
        public void SetUp()
        {
            int[] ints = new int[] { 1, 2, 3 };
            ParseTree = "<subsetof System.Int32[]>";
            StaticSyntax = Is.SubsetOf(ints);
            BuilderSyntax = Builder().SubsetOf(ints);
        }
    }
#endif

    public class CollectionEquivalentTest : SyntaxTest
    {
        public CollectionEquivalentTest()
        {
            int[] ints = new int[] { 1, 2, 3 };
            ParseTree = "<equivalent System.Int32[]>";
            StaticSyntax = Is.EquivalentTo(ints);
            BuilderSyntax = Builder().EquivalentTo(ints);
        }
    }
}
