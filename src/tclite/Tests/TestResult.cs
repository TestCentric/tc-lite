// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Xml;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Tests
{
    /// <summary>
    /// The TestResult class represents the result of a test.
    /// </summary>
    public abstract class TestResult : ITestResult
    {
        /// <summary>
        /// Construct a test result given a Test
        /// </summary>
        /// <param name="test">The test to be used</param>
        public TestResult(ITest test)
        {
            Test = test;
            ResultState = ResultState.Inconclusive;
        }

        #region ITestResult Members

        /// <summary>
        /// Gets the test with which this result is associated.
        /// </summary>
        public ITest Test { get; }

        /// <summary>
        /// Gets the ResultState of the test result, which
        /// indicates the success or failure of the test.
        /// </summary>
        public ResultState ResultState { get; private set; }

        /// <summary>
        /// Gets the name of the test result
        /// </summary>
        public virtual string Name => Test.Name;

        /// <summary>
        /// Gets the full name of the test result
        /// </summary>
        public virtual string FullName => Test.FullName;

        /// <summary>
        /// Gets or sets the elapsed time for running the test
        /// </summary>{
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// failure or with not running the test
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets any stacktrace associated with an
        /// error or failure. Not available in
        /// the Compact Framework 1.0.
        /// </summary>
        public virtual string StackTrace { get; private set; }

        /// <summary>
        /// Gets or sets the count of asserts executed
        /// when running the test.
        /// </summary>
        public int AssertCount { get; set; }

        /// <summary>
        /// Gets the number of test cases that failed
        /// when running the test and all its children.
        /// </summary>
        public abstract int FailCount { get; }

        /// <summary>
        /// Gets the number of test cases that issued a warning
        /// when running the test and all its children.
        /// </summary>
        public abstract int WarningCount { get; }

        /// <summary>
        /// Gets the number of test cases that passed
        /// when running the test and all its children.
        /// </summary>
        public abstract int PassCount { get; }

        /// <summary>
        /// Gets the number of test cases that were skipped
        /// when running the test and all its children.
        /// </summary>
        public abstract int SkipCount { get; }

        /// <summary>
        /// Gets the number of test cases that were inconclusive
        /// when running the test and all its children.
        /// </summary>
        public abstract int InconclusiveCount { get; }

        /// <summary>
        /// Indicates whether this result has any child results.
        /// Test HasChildren before accessing Children to avoid
        /// the creation of an empty collection.
        /// </summary>
        public bool HasChildren => Children.Count > 0;

        /// <summary>
        /// Gets the collection of child results.
        /// </summary>
        public IList<ITestResult> Children { get; } = new List<ITestResult>();

        #endregion

        #region IXmlNodeBuilder Members

        /// <summary>
        /// Returns the Xml representation of the result.
        /// </summary>
        /// <param name="recursive">If true, descendant results are included</param>
        /// <returns>An XmlNode representing the result</returns>
        public XmlNode ToXml(bool recursive)
        {
            XmlNode topNode = XmlHelper.CreateTopLevelElement("dummy");

            AddToXml(topNode, recursive);

            return topNode.FirstChild;
        }

        /// <summary>
        /// Adds the XML representation of the result as a child of the
        /// supplied parent node..
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="recursive">If true, descendant results are included</param>
        /// <returns></returns>
        public virtual XmlNode AddToXml(XmlNode parentNode, bool recursive)
        {
            // A result node looks like a test node with extra info added
            XmlNode thisNode = Test.AddToXml(parentNode, false);

            thisNode.AddAttribute("result", ResultState.Status.ToString());
            if (ResultState.Label != string.Empty) // && ResultState.Label != ResultState.Status.ToString())
                thisNode.AddAttribute("label", ResultState.Label);

            thisNode.AddAttribute("time", Duration.ToString());

            if (Test is TestSuite)
            {
                thisNode.AddAttribute("total", (PassCount + FailCount + SkipCount + InconclusiveCount).ToString());
                thisNode.AddAttribute("passed", PassCount.ToString());
                thisNode.AddAttribute("failed", FailCount.ToString());
                thisNode.AddAttribute("inconclusive", InconclusiveCount.ToString());
                thisNode.AddAttribute("skipped", SkipCount.ToString());                
            }            

            thisNode.AddAttribute("asserts", AssertCount.ToString());

            switch (ResultState.Status)
            {
                case TestStatus.Failed:
                    AddFailureElement(thisNode);
                    break;
                case TestStatus.Skipped:
                    AddReasonElement(thisNode);
                    break;
                case TestStatus.Passed:
                case TestStatus.Inconclusive:
                    if (Message != null)
                        AddReasonElement(thisNode);
                    break;
            }

            if (recursive && HasChildren)
                foreach (TestResult child in Children)
                    child.AddToXml(thisNode, recursive);

            return thisNode;
        }

        #endregion

        #region Other Public Methods

        /// <summary>
        /// Add a child result
        /// </summary>
        /// <param name="result">The child result to be added</param>
        public virtual void AddResult(TestResult result)
        {
            Children.Add(result);

            AssertCount += result.AssertCount;

            switch (result.ResultState.Status)
            {
                case TestStatus.Passed:

                    if (ResultState.Status == TestStatus.Inconclusive)
                        SetResult(ResultState.Success);

                    break;

                case TestStatus.Failed:

                    if (ResultState.Status != TestStatus.Failed)
                        SetResult(ResultState.Failure, "One or more child tests had errors");

                    break;

                case TestStatus.Skipped:

                    switch (result.ResultState.Label)
                    {
                        case "Invalid":

                            if (ResultState != ResultState.NotRunnable && ResultState.Status != TestStatus.Failed)
                                SetResult(ResultState.Failure, "One or more child tests had errors");

                            break;

                        case "Ignored":

                            if (ResultState.Status == TestStatus.Inconclusive || ResultState.Status == TestStatus.Passed)
                                SetResult(ResultState.Ignored, "One or more child tests were ignored");

                            break;

                        default:

                            // Tests skipped for other reasons do not change the outcome
                            // of the containing suite when added.

                            break;
                    }

                    break;

                case TestStatus.Inconclusive:

                    // An inconclusive result does not change the outcome
                    // of the containing suite when added.

                    break;
            }
        }

        /// <summary>
        /// Set the result of the test
        /// </summary>
        /// <param name="resultState">The ResultState to use in the result</param>
        public void SetResult(ResultState resultState)
        {
            SetResult(resultState, null, null);
        }

        /// <summary>
        /// Set the result of the test
        /// </summary>
        /// <param name="resultState">The ResultState to use in the result</param>
        /// <param name="message">A message associated with the result state</param>
        public void SetResult(ResultState resultState, string message)
        {
            SetResult(resultState, message, null);
        }

        /// <summary>
        /// Set the result of the test
        /// </summary>
        /// <param name="resultState">The ResultState to use in the result</param>
        /// <param name="message">A message associated with the result state</param>
        /// <param name="stackTrace">Stack trace giving the location of the command</param>
        public void SetResult(ResultState resultState, string message, string stackTrace)
        {
            ResultState = resultState;
            Message = message;
            StackTrace = stackTrace;
        }

        /// <summary>
        /// Set the test result based on the type of exception thrown
        /// </summary>
        /// <param name="ex">The exception that was thrown</param>
        public void RecordException(Exception ex)
        {
            if (ex is TCLiteException)
                ex = ex.InnerException;

            if (ex is System.Threading.ThreadAbortException)
                SetResult(ResultState.Cancelled, "Test cancelled by user", ex.StackTrace);
            else if (ex is AssertionException)
                SetResult(ResultState.Failure, ex.Message, StackFilter.Filter(ex.StackTrace));
            else if (ex is IgnoreException)
                SetResult(ResultState.Ignored, ex.Message, StackFilter.Filter(ex.StackTrace));
            else if (ex is InconclusiveException)
                SetResult(ResultState.Inconclusive, ex.Message, StackFilter.Filter(ex.StackTrace));
            else if (ex is SuccessException)
                SetResult(ResultState.Success, ex.Message, StackFilter.Filter(ex.StackTrace));
            else
                SetResult(ResultState.Error,
                    ExceptionHelper.BuildMessage(ex),
                    ExceptionHelper.BuildStackTrace(ex));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Adds a reason element to a node and returns it.
        /// </summary>
        /// <param name="targetNode">The target node.</param>
        /// <returns>The new reason element.</returns>
        private XmlNode AddReasonElement(XmlNode targetNode)
        {
            XmlNode reasonNode = targetNode.AddElement("reason");
            reasonNode.AddElement("message").InnerText = Message;
            return reasonNode;
        }

        /// <summary>
        /// Adds a failure element to a node and returns it.
        /// </summary>
        /// <param name="targetNode">The target node.</param>
        /// <returns>The new failure element.</returns>
        private XmlNode AddFailureElement(XmlNode targetNode)
        {
            XmlNode failureNode = targetNode.AddElement("failure");

            if (Message != null)
            {
                failureNode.AddElement("message").InnerText = Message;
            }

            if (StackTrace != null)
            {
                failureNode.AddElement("stack-trace").InnerText = StackTrace;
            }

            return failureNode;
        }

        //private static bool IsTestCase(ITest test)
        //{
        //    return !(test is TestSuite);
        //}

        #endregion
    }
}
