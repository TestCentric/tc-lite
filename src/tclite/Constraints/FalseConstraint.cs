// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// FalseConstraint tests that the actual value is false
    /// </summary>
    public class FalseConstraint : EqualConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:FalseConstraint"/> class.
        /// </summary>
        public FalseConstraint() : base(false) { }
    }
}