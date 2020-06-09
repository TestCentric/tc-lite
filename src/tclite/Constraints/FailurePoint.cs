// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// FailurePoint class represents one point of failure
    /// in an equality test.
    /// </summary>
    public class FailurePoint
    {
        /// <summary>
        /// The location of the failure
        /// </summary>
        public int Position;

        /// <summary>
        /// The expected value
        /// </summary>
        public object ExpectedValue;

        /// <summary>
        /// The actual value
        /// </summary>
        public object ActualValue;

        /// <summary>
        /// Indicates whether the expected value is valid
        /// </summary>
        public bool ExpectedHasData;

        /// <summary>
        /// Indicates whether the actual value is valid
        /// </summary>
        public bool ActualHasData;
    }

    /// <summary>
    /// FailurePointList represents a set of FailurePoints
    /// in a cross-platform way.
    /// </summary>
    class FailurePointList : System.Collections.Generic.List<FailurePoint> { }

}
