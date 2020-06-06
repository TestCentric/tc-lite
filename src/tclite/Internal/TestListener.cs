// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// TestListener provides an implementation of ITestListener that
    /// does nothing. It is used only throught its NULL property.
	/// </summary>
	public class TestListener : ITestListener
	{
        /// <summary>
        /// Called when a test has just started
        /// </summary>
        /// <param name="test">The test that is starting</param>
		public void TestStarted(ITest test){}

        /// <summary>
        /// Called when a test case has finished
        /// </summary>
        /// <param name="result">The result of the test</param>
		public void TestFinished(ITestResult result){}

        /// <summary>
        /// Called when the test creates text output.
        /// </summary>
        /// <param name="testOutput">A console message</param>
		public void TestOutput(TestOutput testOutput) {}

        /// <summary>
        /// Construct a new TestListener - private so it may not be used.
        /// </summary>
        private TestListener() { }

        /// <summary>
        /// Get a listener that does nothing
        /// </summary>
		public static ITestListener NULL
		{
			get { return new TestListener();}
		}
	}
}
