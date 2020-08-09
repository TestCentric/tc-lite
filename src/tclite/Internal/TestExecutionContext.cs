// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using TCLite.Interfaces;
using TCLite.Constraints;

namespace TCLite.Internal
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
	public class TestExecutionContext : ITestExecutionContext
#if NETFRAMEWORK
        : ILogicalThreadAffinative
#endif
	{
        #region Instance Fields

        /// <summary>
        /// Link to a prior saved context
        /// </summary>
        public TestExecutionContext _priorContext;

        /// <summary>
        /// The number of assertions for the current test
        /// </summary>
        private int _assertCount;

        private Randomizer _randomGenerator;

        /// <summary>
        /// The current culture
        /// </summary>
        private CultureInfo _currentCulture;

        /// <summary>
        /// The current UI culture
        /// </summary>
        private CultureInfo _currentUICulture;

        /// <summary>
        /// Destination for standard output
        /// </summary>
        private TextWriter _outWriter;

        /// <summary>
        /// Destination for standard error
        /// </summary>
        private TextWriter _errorWriter;

        /// <summary>
		/// Indicates whether trace is enabled
		/// </summary>
		private bool _tracing;

        /// <summary>
        /// Destination for Trace output
        /// </summary>
        private TextWriter _traceWriter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestExecutionContext"/> class.
        /// </summary>
        public TestExecutionContext()
		{
			this._priorContext = null;
            this.TestCaseTimeout = 0;

            this._currentCulture = CultureInfo.CurrentCulture;
            this._currentUICulture = CultureInfo.CurrentUICulture;

			this._outWriter = Console.Out;
			this._errorWriter = Console.Error;
            this._traceWriter = null;
            this._tracing = false;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestExecutionContext"/> class.
        /// </summary>
        /// <param name="other">An existing instance of TestExecutionContext.</param>
		public TestExecutionContext( TestExecutionContext other )
		{
			this._priorContext = other;

            this.CurrentTest = other.CurrentTest;
            this.CurrentResult = other.CurrentResult;
            this.TestObject = other.TestObject;
			this.WorkDirectory = other.WorkDirectory;
            this.Listener = other.Listener;
            this.DefaultTolerance = other.DefaultTolerance;
            this.StopOnError = other.StopOnError;
            this.TestCaseTimeout = other.TestCaseTimeout;

            this._currentCulture = CultureInfo.CurrentCulture;
            this._currentUICulture = CultureInfo.CurrentUICulture;

			this._outWriter = other._outWriter;
			this._errorWriter = other._errorWriter;
            this._traceWriter = other._traceWriter;
            this._tracing = other._tracing;
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
            internal set
            {
                current = value;
            }
        }

        #endregion

        #region Static Methods

        internal static void SetCurrentContext(TestExecutionContext ec)
        {
            current = ec;
        }

        #endregion

        #region ITestExecutionContext Explicit Implementation

        // /// <summary>
        // /// Gets or sets the current test
        // /// </summary>
        // ITest ITestExecutionContext.CurrentTest => this.CurrentTest;

        // /// <summary>
        // /// The time the current test started execution
        // /// </summary>
        // DateTime ITestExecutionContext.StartTime => this.StartTime;

        // /// <summary>
        // /// Gets or sets the current test result
        // /// </summary>
        // ITestResult ITestExecutionContext.CurrentResult => this.CurrentResult;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current test
        /// </summary>
        public Test CurrentTest { get; set; }

        /// <summary>
        /// The time the current test started execution
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the current test result
        /// </summary>
        public TestResult CurrentResult { get; set; }

        /// <summary>
        /// The current test object - that is the user fixture
        /// object on which tests are being executed.
        /// </summary>
        public object TestObject { get; set; }
		
        /// <summary>
        /// Get or set the working directory
        /// </summary>
		public string WorkDirectory { get; set; }

        /// <summary>
        /// Get or set indicator that run should stop on the first error
        /// </summary>
        public bool StopOnError { get; set; }
		
        /// <summary>
        /// The current test event listener
        /// </summary>
        internal ITestListener Listener { get; set; } = TestListener.NULL;

        /// <summary>
        /// Default tolerance value used for floating point equality
        /// when no other tolerance is specified.
        /// </summary>
        public Tolerance DefaultTolerance { get; set; }

        /// <summary>
        /// Gets the RandomGenerator specific to this Test
        /// </summary>
        public Randomizer RandomGenerator
        {
            get
            {
                if (_randomGenerator == null)
                    _randomGenerator = new Randomizer(CurrentTest.Seed);
                return _randomGenerator;
            }
        }

        /// <summary>
        /// Gets the assert count.
        /// </summary>
        /// <value>The assert count.</value>
        // TODO: public for tests
        public int AssertCount
        {
            get { return _assertCount; }
            set { _assertCount = value; }
        }

        /// <summary>
        /// Gets or sets the test case timeout value
        /// </summary>
        public int TestCaseTimeout { get; set; }

        /// <summary>
        /// Saves or restores the CurrentCulture
        /// </summary>
        public CultureInfo CurrentCulture
        {
            get { return _currentCulture; }
            set
            {
                _currentCulture = value;
                Thread.CurrentThread.CurrentCulture = _currentCulture;
            }
        }

        /// <summary>
        /// Saves or restores the CurrentUICulture
        /// </summary>
        public CultureInfo CurrentUICulture
        {
            get { return _currentUICulture; }
            set
            {
                _currentUICulture = value;
                Thread.CurrentThread.CurrentUICulture = _currentUICulture;
            }
        }

        /// <summary>
		/// Controls where Console.Out is directed
		/// </summary>
		internal TextWriter Out
        {
			get { return _outWriter; }
			set 
			{
				if ( _outWriter != value )
				{
					_outWriter = value; 
					Console.Out.Flush();
					Console.SetOut( _outWriter );
				}
			}
		}

		/// <summary>
		/// Controls where Console.Error is directed
		/// </summary>
		internal TextWriter Error
		{
			get { return _errorWriter; }
			set 
			{
				if ( _errorWriter != value )
				{
					_errorWriter = value; 
					Console.Error.Flush();
					Console.SetError( _errorWriter );
				}
			}
		}

        /// <summary>
        /// Controls whether trace and debug output are written
        /// to the standard output.
        /// </summary>
        internal bool Tracing
        {
            get { return _tracing; }
            set
            {
                if (_tracing != value)
                {
                    if (_traceWriter != null && _tracing)
                        StopTracing();

                    _tracing = value;

                    if (_traceWriter != null && _tracing)
                        StartTracing();
                }
            }
        }

        /// <summary>
        /// Controls where Trace output is directed
        /// </summary>
		internal TextWriter TraceWriter
		{
			get { return _traceWriter; }
			set
			{
				if ( _traceWriter != value )
				{
					if ( _traceWriter != null  && _tracing )
						StopTracing();

					_traceWriter = value;

					if ( _traceWriter != null && _tracing )
						StartTracing();
				}
			}
		}

		private void StopTracing()
		{
			_traceWriter.Close();
			System.Diagnostics.Trace.Listeners.Remove( "NUnit" );
		}

		private void StartTracing()
		{
			System.Diagnostics.Trace.Listeners.Add( new TextWriterTraceListener( _traceWriter, "NUnit" ) );
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
            if (_priorContext == null)
                throw new InvalidOperationException("TestContext: too many Restores");

            this.TestCaseTimeout = _priorContext.TestCaseTimeout;

            this.CurrentCulture = _priorContext.CurrentCulture;
            this.CurrentUICulture = _priorContext.CurrentUICulture;

            this.Out = _priorContext.Out;
            this.Error = _priorContext.Error;
            this.Tracing = _priorContext.Tracing;

            return _priorContext;
        }

        /// <summary>
        /// Record any changes in the environment made by
        /// the test code in the execution context so it
        /// will be passed on to lower level tests.
        /// </summary>
        public void UpdateContext()
        {
            this._currentCulture = CultureInfo.CurrentCulture;
            this._currentUICulture = CultureInfo.CurrentUICulture;
        }

        /// <summary>
        /// Increments the assert count.
        /// </summary>
        public void IncrementAssertCount()
        {
            System.Threading.Interlocked.Increment(ref _assertCount);
        }

        private TestExecutionContext CreateIsolatedContext()
        {
            var context = new TestExecutionContext(this);

            if (context.CurrentTest != null)
                context.CurrentResult = context.CurrentTest.MakeTestResult();

#if NYI // Parallelism
            context.TestWorker = TestWorker;
#endif

            return context;
        }

        #endregion

        #region Nested IsolatedContext Class

        /// <summary>
        /// An IsolatedContext is used when running code
        /// that may effect the current result in ways that
        /// should not impact the final result of the test.
        /// A new TestExecutionContext is created with an
        /// initially clear result, which is discarded on
        /// exiting the context.
        /// </summary>
        /// <example>
        ///     using (new TestExecutionContext.IsolatedContext())
        ///     {
        ///         // Code that should not impact the result
        ///     }
        /// </example>
        public class IsolatedContext : IDisposable
        {
            private readonly TestExecutionContext _originalContext;

            /// <summary>
            /// Save the original current TestExecutionContext and
            /// make a new isolated context current.
            /// </summary>
            public IsolatedContext()
            {
                _originalContext = CurrentContext;
                CurrentContext = _originalContext.CreateIsolatedContext();
            }

            /// <summary>
            /// Restore the original TestExecutionContext.
            /// </summary>
            public void Dispose()
            {
                //_originalContext.OutWriter.Write(CurrentContext.CurrentResult.Output);
                CurrentContext = _originalContext;
            }
        }

        #endregion
    }
}
