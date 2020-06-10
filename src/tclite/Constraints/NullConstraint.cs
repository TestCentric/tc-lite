// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// NullConstraint tests that the actual value is null
    /// </summary>
    public class NullConstraint : EqualConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NullConstraint"/> class.
        /// </summary>
        public NullConstraint() : base(null) { }
    }
}