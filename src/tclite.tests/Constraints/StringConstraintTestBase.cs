// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{

    public abstract class StringConstraintTestBase : ConstraintTestBase<string>
    {
        [TestCase]
        public void NonStringDataThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => Constraint.ValidateActualValue(123));
        }
    }
}
