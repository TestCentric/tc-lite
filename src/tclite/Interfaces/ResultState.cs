// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Interfaces
{
    /// <summary>
	/// The ResultState class represents the outcome of running a test.
    /// It contains two pieces of information. The Status of the test
    /// is an enum indicating whether the test passed, failed, was
    /// skipped or was inconclusive. The Label provides a more
    /// detailed breakdown for use by client runners.
	/// </summary>
	public class ResultState
	{

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultState"/> class.
        /// </summary>
        /// <param name="status">The TestStatus.</param>
        public ResultState(TestStatus status) : this (status, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultState"/> class.
        /// </summary>
        /// <param name="status">The TestStatus.</param>
        /// <param name="label">The label.</param>
        public ResultState(TestStatus status, string label=null)
        {
            Status = status;
            Label = label ?? string.Empty;
        }

        #endregion

        #region Predefined ResultStates

        /// <summary>
        /// The result is inconclusive
        /// </summary>
        public readonly static ResultState Inconclusive = new ResultState(TestStatus.Inconclusive);
        
        /// <summary>
        /// The test was not runnable.
        /// </summary>
        public readonly static ResultState NotRunnable = new ResultState(TestStatus.Failed, "Invalid");

        /// <summary>
        /// The test has been skipped. 
        /// </summary>
        public readonly static ResultState Skipped = new ResultState(TestStatus.Skipped);

        /// <summary>
        /// The test has been ignored.
        /// </summary>
        public readonly static ResultState Ignored = new ResultState(TestStatus.Skipped, "Ignored");

        /// <summary>
        /// The test was skipped because it is explicit
        /// </summary>
        public readonly static ResultState Explicit = new ResultState(TestStatus.Skipped, "Explicit");

        /// <summary>
        /// The test succeeded
        /// </summary>
        public readonly static ResultState Success = new ResultState(TestStatus.Passed);

        /// <summary>
        /// The test failed
        /// </summary>
        public readonly static ResultState Failure = new ResultState(TestStatus.Failed);

        /// <summary>
        /// The test issued a warning
        /// </summary>
        public readonly static ResultState Warning = new ResultState(TestStatus.Warning);

        /// <summary>
        /// The test encountered an unexpected exception
        /// </summary>
        public readonly static ResultState Error = new ResultState(TestStatus.Failed, "Error");

        /// <summary>
        /// The test was cancelled by the user
        /// </summary>
        public readonly static ResultState Cancelled = new ResultState(TestStatus.Failed, "Cancelled");
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the TestStatus for the test.
        /// </summary>
        /// <value>The status.</value>
        public TestStatus Status { get; }

        /// <summary>
        /// Gets the label under which this test resullt is
        /// categorized, if any.
        /// </summary>
        public string Label { get; }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as ResultState;
            if (object.ReferenceEquals(other, null)) return false;

            return Status.Equals(other.Status) && Label.Equals(other.Label);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (int)Status << 8 + Label.GetHashCode(); ;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.IsNullOrEmpty(Label)
                ? Status.ToString()
                : $"{Status}:{Label}";
        }
    }
}
