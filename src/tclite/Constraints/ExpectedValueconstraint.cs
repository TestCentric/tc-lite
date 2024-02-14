// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Constraints
{
    /// <summary>
    /// ExpectedValueConstraint is a constraint taking an
    /// expected value as a constructor argument, which any
    /// actual value is supposed to match in some way.
    /// In some cases, the Type of the expected value is used
    /// in determining the Types allowed as actual values.
    /// </summary>
    /// <typeparam name="TExpected">Type of the ExpectedValue</typeparam>
    public abstract class ExpectedValueConstraint<TExpected> : Constraint
    {
        public TExpected ExpectedValue;

        public ExpectedValueConstraint(TExpected expected)
            : base(expected)
        {
            ExpectedValue = expected;
        }
    }
}