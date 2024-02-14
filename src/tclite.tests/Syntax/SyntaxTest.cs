// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using TCLite.Constraints;

namespace TCLite.Syntax
{
    public abstract class SyntaxTest
    {
        protected string ParseTree;
        protected IResolveConstraint StaticSyntax;
        protected IResolveConstraint BuilderSyntax;

        protected ConstraintExpression Builder()
        {
            return new ConstraintExpression();
        }

        [TestCase]
        public void SupportedByStaticSyntax()
        {
            Assert.That(
                StaticSyntax.Resolve().ToString(),
                Is.EqualTo(ParseTree).NoClip);
        }

        [TestCase]
        public void SupportedByConstraintBuilder()
        {
            Assert.That(
                BuilderSyntax.Resolve().ToString(),
                Is.EqualTo(ParseTree).NoClip);
        }
    }
}
