// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Interfaces
{
	/// <summary>
	/// The ITestListener interface is used internally to receive 
	/// notifications of significant events while a test is being 
    /// run. The events are propagated to clients by means of an
    /// AsyncCallback.
	/// </summary>
	public interface ITestListener
	{
		/// <summary>
		/// Called when a test has just started
		/// </summary>
		/// <param name="test">The test that is starting</param>
		void TestStarted(ITest test);
			
		/// <summary>
		/// Called when a test has finished
		/// </summary>
		/// <param name="result">The result of the test</param>
		void TestFinished(ITestResult result);

		/// <summary>
		/// Called when the test creates text output.
		/// </summary>
		/// <param name="testOutput">A console message</param>
		void TestOutput(TestOutput testOutput);
	}
}
