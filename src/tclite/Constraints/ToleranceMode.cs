// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Constraints
{
    /// <summary>
    /// Modes in which the tolerance value for a comparison can be interpreted.
    /// </summary>
    public enum ToleranceMode
    {
        /// <summary>
        /// The tolerance was created with a value, without specifying 
        /// how the value would be used. This is used to prevent setting
        /// the mode more than once and is generally changed to Linear
        /// upon execution of the test.
        /// </summary>
        Default,
        /// <summary>
        /// The tolerance is used as a numeric range within which
        /// two compared values are considered to be equal.
        /// </summary>
        Linear,
        /// <summary>
        /// Interprets the tolerance as the percentage by which
        /// the two compared values my deviate from each other.
        /// </summary>
        Percent,
        /// <summary>
        /// Compares two values based in their distance in
        /// representable numbers.
        /// </summary>
        Ulps
    }
}