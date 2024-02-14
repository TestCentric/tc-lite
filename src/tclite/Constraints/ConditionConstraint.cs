// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Constraints
{
    /// <summary>
    /// The ConditionConstraint class represents a constraint,
    /// which asserts a particular condition on an actual value
    /// of the Type specified in the TypeParameter.
    /// </summary>
    public abstract class ConditionConstraint<TActual> : Constraint
    {
        public ConditionConstraint(params object[] args) : base(args) { }

    }
}