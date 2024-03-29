// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite.Interfaces
{
	/// <summary>
	/// The RunState enum indicates whether a test
    /// can be executed. 
    /// </summary>
	public enum RunState
	{
        /// <summary>
        /// The test is not runnable.
        /// </summary>
		NotRunnable, 

        /// <summary>
        /// The test is runnable.
        /// </summary>
		Runnable,

        /// <summary>
        /// The test has been skipped. This value may
        /// appear on a Test when certain attributes
        /// are used to skip the test.
        /// </summary>
		Skipped,

        /// <summary>
        /// The test has been ignored. May appear on
        /// a Test, when the IgnoreAttribute is used.
        /// </summary>
		Ignored
	}
}
