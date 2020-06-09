// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// NullConstraint tests that the actual value is null
    /// </summary>
    public class NullConstraint : BasicConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NullConstraint"/> class.
        /// </summary>
        public NullConstraint() : base(null, "null") { }
    }
}