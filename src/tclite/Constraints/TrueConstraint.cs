// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// TrueConstraint tests that the actual value is true
    /// </summary>
    public class TrueConstraint : EqualConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TrueConstraint"/> class.
        /// </summary>
        public TrueConstraint() : base(true) { }
    }
}