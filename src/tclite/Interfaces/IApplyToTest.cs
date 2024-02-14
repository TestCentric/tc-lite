// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Interfaces
{
    /// <summary>
    /// The IApplyToTest interface is implemented by self-applying
    /// attributes that modify the state of a test in some way.
    /// </summary>
    public interface IApplyToTest
    {
        /// <summary>
        /// Modifies a test as defined for the specific attribute.
        /// </summary>
        /// <param name="test">The test to modify</param>
        void ApplyToTest(ITest test);
    }
}
