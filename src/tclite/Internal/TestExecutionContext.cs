// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using TCLite.Framework.Api;
using TCLite.Framework.Tests;
//using System.Runtime.Remoting.Messaging;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// Helper class used to save and restore certain static or
	/// singleton settings in the environment that affect tests 
	/// or which might be changed by the user tests.
	/// 
	/// An internal class is used to hold settings and a stack
	/// of these objects is pushed and popped as Save and Restore
	/// are called.
	/// 
	/// Static methods for each setting forward to the internal 
	/// object on the top of the stack.
	/// </summary>
	public class TestExecutionContext
#if NETFRAMEWORK
        : ILogicalThreadAffinative
#endif
	{
        #region Instance Fields

        /// <summary>
        /// Link to a prior saved context
        /// </summary>
        public TestExecutionContext prior;

        /// <summary>
        /// The currently executing test
        /// </summary>
        private Test currentTest;

        /// <summary>
        /// The time the test began execution
        /// </summary>
        private DateTime startTime;

        /// <summary>
        /// The active TestResult for the current test
        /// </summary>
        private TestResult currentResult;
		
		/// <summary>
		/// The work directory to receive test output
		/// </summary>
		private string workDirectory;
		
        /// <summary>
        /// The object on which tests are currently being executed - i.e. the user fixture object
        /// </summary>
        private object testObject;

        /// <summary>
        /// The event listener currently receiving notifications
        /// </summary>
        private ITestListener listener = TestListener.NULL;

        /// <summary>
        /// The number of assertions for the current test
        /// </summary>
        private int assertCount;

        /// <summary>
        /// Indicates whether execution should terminate after the first error
        /// </summary>
        private bool stopOnError;

        /// <summary>
        /// Default timeout for test cases
        /// </summary>
        private int testCaseTimeout;

        private RandomGenerator randomGenerator;

        /// <summary>
        /// The current culture
        /// </summary>
        private CultureInfo currentCulture;

        /// <summary>
        /// The current UI culture
        /// </summary>
        private CultureInfo currentUICulture;

        /// <summary>
        /// Destination for standard output
        /// </summary>
        private TextWriter outWriter;

        /// <summary>
        /// Destination for standard error
        /// </summary>
        private TextWriter errorWriter;

        /// <summary>
		/// Indicates whether trace is enabled
		/// </summary>
		private bool tracing;

        /// <summary>
        /// Destination for Trace output
        /// </summary>
        private TextWriter traceWriter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestExecutionContext"/> class.
        /// </summary>
        public TestExecutionContext()
		{
			this.prior = null;
            this.testCaseTimeout = 0;

            this.currentCulture = CultureInfo.CurrentCulture;
            this.currentUICulture = CultureInfo.CurrentUICulture;

			this.outWriter = Console.Out;
			this.errorWriter = Console.Error;
            this.traceWriter = null;
            this.tracing = false;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestExecutionContext"/> class.
        /// </summary>
        /// <param name="other">An existing instance of TestExecutionContext.</param>
		public TestExecutionContext( TestExecutionContext other )
		{
			this.prior = other;

            this.currentTest = other.currentTest;
            this.currentResult = other.currentResult;
            this.testObject = other.testObject;
			this.workDirectory = other.workDirectory;
            this.listener = other.listener;
            this.stopOnError = other.stopOnError;
            this.testCaseTimeout = other.testCaseTimeout;

            this.currentCulture = CultureInfo.CurrentCulture;
            this.currentUICulture = CultureInfo.CurrentUICulture;

			this.outWriter = other.outWriter;
			this.errorWriter = other.errorWriter;
            this.traceWriter = other.traceWriter;
            this.tracing = other.tracing;
        }

        #endregion

        #region Static Singleton Instance

        /// <summary>
        /// The current context, head of the list of saved contexts.
        /// </summary>
        [ThreadStatic]
        private static TestExecutionContext current;

        /// <summary>
        /// Gets the current context.
        /// </summary>
        /// <value>The current context.</value>
        public static TestExecutionContext CurrentContext
        {
            get 
            {
                if (current == null)
                    current = new TestExecutionContext();

                return current; 
            }
        }

        #endregion

        #region Static Methods

        internal static void SetCurrentContext(TestExecutionContext ec)
        {
            current = ec;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current test
        /// </summary>
        public Test CurrentTest
        {
            get { return currentTest; }
            set { currentTest = value; }
        }

        /// <summary>
        /// The time the current test started execution
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// Gets or sets the current test result
        /// </summary>
        public TestResult CurrentResult
        {
            get { return currentResult; }
            set { currentResult = value; }
        }

        /// <summary>
        /// The current test object - that is the user fixture
        /// object on which tests are being executed.
        /// </summary>
        public object TestObject
        {
            get { return testObject; }
            set { testObject = value; }
        }
		
        /// <summary>
        /// Get or set the working directory
        /// </summary>
		public string WorkDirectory
		{
			get { return workDirectory; }
			set { workDirectory = value; }
		}

        /// <summary>
        /// Get or set indicator that run should stop on the first error
        /// </summary>
        public bool StopOnError
        {
            get { return stopOnError; }
            set { stopOnError = value; }
        }
		
        /// <summary>
        /// The current test event listener
        /// </summary>
        internal ITestListener Listener
        {
            get { return listener; }
            set { listener = value; }
        }

        /// <summary>
        /// Gets the RandomGenerator specific to this Test
        /// </summary>
        public RandomGenerator RandomGenerator
        {
            get
            {
                if (randomGenerator == null)
                {
                    randomGenerator = new RandomGenerator(currentTest.Seed);
                }
                return randomGenerator;
            }
        }

        /// <summary>
        /// Gets the assert count.
        /// </summary>
        /// <value>The assert count.</value>
        internal int AssertCount
        {
            get { return assertCount; }
            set { assertCount = value; }
        }

        /// <summary>
        /// Gets or sets the test case timeout value
        /// </summary>
        public int TestCaseTimeout
        {
            get { return testCaseTimeout; }
            set { testCaseTimeout = value; }
        }

        /// <summary>
        /// Saves or restores the CurrentCulture
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get { return currentCulture; }
            set
            {
                currentCulture = value;
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        /// <summary>
        /// Saves or restores the CurrentUICulture
        /// </summary>
        public CultureInfo CurrentUICulture
        {
            get { return currentUICulture; }
            set
            {
                currentUICulture = value;
                Thread.CurrentThread.CurrentUICulture = currentUICulture;
            }
        }

        /// <summary>
		/// Controls where Console.Out is directed
		/// </summary>
		internal TextWriter Out
		{
			get { return outWriter; }
			set 
			{
				if ( outWriter != value )
				{
					outWriter = value; 
					Console.Out.Flush();
					Console.SetOut( outWriter );
				}
			}
		}

		/// <summary>
		/// Controls where Console.Error is directed
		/// </summary>
		internal TextWriter Error
		{
			get { return errorWriter; }
			set 
			{
				if ( errorWriter != value )
				{
					errorWriter = value; 
					Console.Error.Flush();
					Console.SetError( errorWriter );
				}
			}
		}

        /// <summary>
        /// Controls whether trace and debug output are written
        /// to the standard output.
        /// </summary>
        internal bool Tracing
        {
            get { return tracing; }
            set
            {
                if (tracing != value)
                {
                    if (traceWriter != null && tracing)
                        StopTracing();

                    tracing = value;

                    if (traceWriter != null && tracing)
                        StartTracing();
                }
            }
        }

        /// <summary>
        /// Controls where Trace output is directed
        /// </summary>
		internal TextWriter TraceWriter
		{
			get { return traceWriter; }
			set
			{
				if ( traceWriter != value )
				{
					if ( traceWriter != null  && tracing )
						StopTracing();

					traceWriter = value;

					if ( traceWriter != null && tracing )
						StartTracing();
				}
			}
		}

		private void StopTracing()
		{
			traceWriter.Close();
			System.Diagnostics.Trace.Listeners.Remove( "NUnit" );
		}

		private void StartTracing()
		{
			System.Diagnostics.Trace.Listeners.Add( new TextWriterTraceListener( traceWriter, "NUnit" ) );
		}

        #endregion

        #region Instance Methods

        /// <summary>
        /// Saves the old context and returns a fresh one 
        /// with the same settings.
        /// </summary>
        public TestExecutionContext Save()
        {
            return new TestExecutionContext(this);
        }

        /// <summary>
        /// Restores the last saved context and puts
        /// any saved settings back into effect.
        /// </summary>
        public TestExecutionContext Restore()
        {
            if (prior == null)
                throw new InvalidOperationException("TestContext: too many Restores");

            this.TestCaseTimeout = prior.TestCaseTimeout;

            this.CurrentCulture = prior.CurrentCulture;
            this.CurrentUICulture = prior.CurrentUICulture;

            this.Out = prior.Out;
            this.Error = prior.Error;
            this.Tracing = prior.Tracing;

            return prior;
        }

        /// <summary>
        /// Record any changes in the environment made by
        /// the test code in the execution context so it
        /// will be passed on to lower level tests.
        /// </summary>
        public void UpdateContext()
        {
            this.currentCulture = CultureInfo.CurrentCulture;
            this.currentUICulture = CultureInfo.CurrentUICulture;
        }

        /// <summary>
        /// Increments the assert count.
        /// </summary>
        public void IncrementAssertCount()
        {
            System.Threading.Interlocked.Increment(ref assertCount);
        }

        #endregion
	}
}
