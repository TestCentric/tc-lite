// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TCLite.Constraints;
using TCLite.Interfaces;
using TCLite.Internal;
using TCLite.WorkItems;

namespace TCLite
{
    /// <summary>
    /// Provide the context information of the current test.
    /// This is an adapter for the internal ExecutionContext
    /// class, hiding the internals from the user test.
    /// </summary>
    public class TestContext
    {
        private readonly TestExecutionContext _testExecutionContext;
        private TestAdapter _test;
        private ResultAdapter _result;

        #region Constructor

        /// <summary>
        /// Construct a TestContext for an ExecutionContext
        /// </summary>
        /// <param name="testExecutionContext">The ExecutionContext to adapt</param>
        public TestContext(TestExecutionContext testExecutionContext)
        {
            _testExecutionContext = testExecutionContext;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the current test context. This is created
        /// as needed. The user may save the context for
        /// use within a test, but it should not be used
        /// outside the test for which it is created.
        /// </summary>
        public static TestContext CurrentContext => new TestContext(TestExecutionContext.CurrentContext);

        /// <summary>
        /// Get a representation of the current test.
        /// </summary>
        public static TestAdapter CurrentTest => CurrentContext.Test;

        /// <summary>
        /// Gets a TextWriter that will send output to the current test result.
        /// </summary>
        public static TextWriter Out => TestExecutionContext.CurrentContext.Out;

#if NYI
        /// <summary>
        /// Gets a TextWriter that will send output directly to Console.Error
        /// </summary>
        public static TextWriter Error = new EventListenerTextWriter("Error", Console.Error);

        /// <summary>
        /// Gets a TextWriter for use in displaying immediate progress messages
        /// </summary>
        public static readonly TextWriter Progress = new EventListenerTextWriter("Progress", Console.Error);
#endif

        /// <summary>
        /// TestParameters object holds parameters for the test run, if any are specified
        /// </summary>
        public static readonly TestParameters Parameters = new TestParameters();

        /// <summary>
        /// Static DefaultWorkDirectory is now used as the source
        /// of the public instance property WorkDirectory. This is
        /// a bit odd but necessary to avoid breaking user tests.
        /// </summary>
        internal static string DefaultWorkDirectory;

        /// <summary>
        /// Get a representation of the current test.
        /// </summary>
        public TestAdapter Test
        {
            get { return _test ?? (_test = new TestAdapter(_testExecutionContext.CurrentTest)); }
        }

        /// <summary>
        /// Gets a Representation of the TestResult for the current test.
        /// </summary>
        public ResultAdapter Result
        {
            get { return _result ?? (_result = new ResultAdapter(_testExecutionContext.CurrentResult)); }
        }

#if NYI
        /// <summary>
        /// Gets the unique name of the Worker that is executing this test.
        /// </summary>
        public string WorkerId
        {
            get { return _testExecutionContext.TestWorker?.Name; }
        }

        /// <summary>
        /// Gets the directory containing the current test assembly.
        /// </summary>
        public string TestDirectory
        {
            get
            {
                Assembly assembly = _testExecutionContext?.CurrentTest?.TypeInfo?.Assembly;

                if (assembly != null)
                    return AssemblyHelper.GetDirectoryName(assembly);

                // Test is null, we may be loading tests rather than executing.
                // Assume that calling assembly is the test assembly.
                return AssemblyHelper.GetDirectoryName(Assembly.GetCallingAssembly());
            }
        }
#endif

        /// <summary>
        /// Gets the directory to be used for outputting files created
        /// by this test run.
        /// </summary>
        public string WorkDirectory => DefaultWorkDirectory
            ?? throw new InvalidOperationException("TestContext.WorkDirectory must not be accessed before DefaultTestAssemblyBuilder.Build runs.");

        /// <summary>
        /// Gets the random generator.
        /// </summary>
        /// <value>
        /// The random generator.
        /// </value>
        public IRandomGenerator Random
        {
            get { return _testExecutionContext.RandomGenerator; }
        }

        /// <summary>
        /// Gets the number of assertions executed
        /// up to this point in the test.
        /// </summary>
        public int AssertCount
        {
            get { return _testExecutionContext.AssertCount; }
        }

#if NYI
        /// <summary>
        /// Get the number of times the current Test has been repeated. This is currently only
        /// set when using the <see cref="RetryAttribute"/>.
        /// TODO: add this to the RepeatAttribute as well
        /// </summary>
        public int CurrentRepeatCount
        {
            get { return _testExecutionContext.CurrentRepeatCount; }
        }
#endif

        #endregion

        #region Static Methods

        /// <summary>Write the string representation of a boolean value to the current result</summary>
        public static void Write(bool value) { Out.Write(value); }

        /// <summary>Write a char to the current result</summary>
        public static void Write(char value) { Out.Write(value); }

        /// <summary>Write a char array to the current result</summary>
        public static void Write(char[] value) { Out.Write(value); }

        /// <summary>Write the string representation of a double to the current result</summary>
        public static void Write(double value) { Out.Write(value); }

        /// <summary>Write the string representation of an Int32 value to the current result</summary>
        public static void Write(Int32 value) { Out.Write(value); }

        /// <summary>Write the string representation of an Int64 value to the current result</summary>
        public static void Write(Int64 value) { Out.Write(value); }

        /// <summary>Write the string representation of a decimal value to the current result</summary>
        public static void Write(decimal value) { Out.Write(value); }

        /// <summary>Write the string representation of an object to the current result</summary>
        public static void Write(object value) { Out.Write(value); }

        /// <summary>Write the string representation of a Single value to the current result</summary>
        public static void Write(Single value) { Out.Write(value); }

        /// <summary>Write a string to the current result</summary>
        public static void Write(string value) { Out.Write(value); }

        /// <summary>Write the string representation of a UInt32 value to the current result</summary>
        public static void Write(UInt32 value) { Out.Write(value); }

        /// <summary>Write the string representation of a UInt64 value to the current result</summary>
        public static void Write(UInt64 value) { Out.Write(value); }

        /// <summary>Write a formatted string to the current result</summary>
        public static void Write(string format, object arg1) { Out.Write(format, arg1); }

        /// <summary>Write a formatted string to the current result</summary>
        public static void Write(string format, object arg1, object arg2) { Out.Write(format, arg1, arg2); }

        /// <summary>Write a formatted string to the current result</summary>
        public static void Write(string format, object arg1, object arg2, object arg3) { Out.Write(format, arg1, arg2, arg3); }

        /// <summary>Write a formatted string to the current result</summary>
        public static void Write(string format, params object[] args) { Out.Write(format, args); }

        /// <summary>Write a line terminator to the current result</summary>
        public static void WriteLine() { Out.WriteLine(); }

        /// <summary>Write the string representation of a boolean value to the current result followed by a line terminator</summary>
        public static void WriteLine(bool value) { Out.WriteLine(value); }

        /// <summary>Write a char to the current result followed by a line terminator</summary>
        public static void WriteLine(char value) { Out.WriteLine(value); }

        /// <summary>Write a char array to the current result followed by a line terminator</summary>
        public static void WriteLine(char[] value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of a double to the current result followed by a line terminator</summary>
        public static void WriteLine(double value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of an Int32 value to the current result followed by a line terminator</summary>
        public static void WriteLine(Int32 value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of an Int64 value to the current result followed by a line terminator</summary>
        public static void WriteLine(Int64 value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of a decimal value to the current result followed by a line terminator</summary>
        public static void WriteLine(decimal value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of an object to the current result followed by a line terminator</summary>
        public static void WriteLine(object value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of a Single value to the current result followed by a line terminator</summary>
        public static void WriteLine(Single value) { Out.WriteLine(value); }

        /// <summary>Write a string to the current result followed by a line terminator</summary>
        public static void WriteLine(string value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of a UInt32 value to the current result followed by a line terminator</summary>
        public static void WriteLine(UInt32 value) { Out.WriteLine(value); }

        /// <summary>Write the string representation of a UInt64 value to the current result followed by a line terminator</summary>
        public static void WriteLine(UInt64 value) { Out.WriteLine(value); }

        /// <summary>Write a formatted string to the current result followed by a line terminator</summary>
        public static void WriteLine(string format, object arg1) { Out.WriteLine(format, arg1); }

        /// <summary>Write a formatted string to the current result followed by a line terminator</summary>
        public static void WriteLine(string format, object arg1, object arg2) { Out.WriteLine(format, arg1, arg2); }

        /// <summary>Write a formatted string to the current result followed by a line terminator</summary>
        public static void WriteLine(string format, object arg1, object arg2, object arg3) { Out.WriteLine(format, arg1, arg2, arg3); }

        /// <summary>Write a formatted string to the current result followed by a line terminator</summary>
        public static void WriteLine(string format, params object[] args) { Out.WriteLine(format, args); }

#if NYI
        /// <summary>
        /// This method adds the a new ValueFormatterFactory to the
        /// chain of responsibility used for formatting values in messages.
        /// The scope of the change is the current TestContext.
        /// </summary>
        /// <param name="formatterFactory">The factory delegate</param>
        public static void AddFormatter(ValueFormatterFactory formatterFactory)
        {
            TestExecutionContext.CurrentContext.AddFormatter(formatterFactory);
        }

        /// <summary>
        /// Attach a file to the current test result
        /// </summary>
        /// <param name="filePath">Relative or absolute file path to attachment</param>
        /// <param name="description">Optional description of attachment</param>
        public static void AddTestAttachment(string filePath, string? description = null)
        {
            Guard.ArgumentNotNull(filePath, nameof(filePath));
            Guard.ArgumentValid(filePath.IndexOfAny(Path.GetInvalidPathChars()) == -1,
                $"Test attachment file path contains invalid path characters. {filePath}", nameof(filePath));

            if (!Path.IsPathRooted(filePath))
                filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Test attachment file path could not be found.", filePath);

            var result = TestExecutionContext.CurrentContext.CurrentResult;
            result.AddTestAttachment(new TestAttachment(filePath, description));
        }

        /// <summary>
        /// This method provides a simplified way to add a ValueFormatter
        /// delegate to the chain of responsibility, creating the factory
        /// delegate internally. It is useful when the Type of the object
        /// is the only criterion for selection of the formatter, since
        /// it can be used without getting involved with a compound function.
        /// </summary>
        /// <typeparam name="TSupported">The type supported by this formatter</typeparam>
        /// <param name="formatter">The ValueFormatter delegate</param>
        public static void AddFormatter<TSupported>(ValueFormatter formatter)
        {
            AddFormatter(next => val => (val is TSupported) ? formatter(val) : next(val));
        }
#endif

#endregion

#region Nested TestAdapter Class

        /// <summary>
        /// TestAdapter adapts a Test for consumption by
        /// the user test code.
        /// </summary>
        public class TestAdapter
        {
            private readonly Test _test;

#region Constructor

            /// <summary>
            /// Construct a TestAdapter for a Test
            /// </summary>
            /// <param name="test">The Test to be adapted</param>
            public TestAdapter(Test test)
            {
                _test = test;
            }

#endregion

#region Properties

            public TestAdapter Parent => _test.Parent != null ? new TestAdapter(_test.Parent) : null;

            /// <summary>
            /// Gets the unique Id of a test
            /// </summary>
            public String ID => _test.Id;

            /// <summary>
            /// The name of the test, which may or may not be
            /// the same as the method name.
            /// </summary>
            public string Name => _test.Name;

            /// <summary>
            /// The name of the method representing the test.
            /// </summary>
            public string MethodName => (_test as TestMethod)?.Method.Name;

            /// <summary>
            /// The FullName of the test
            /// </summary>
            public string FullName => _test.FullName;

            /// <summary>
            /// The ClassName of the test
            /// </summary>
            public string ClassName => _test.ClassName;

            public string Description => (string)Properties.Get(PropertyNames.Description);

            public IEnumerable<string> Categories
            {
                get
                {
                    var categories = new List<string>();
                    foreach (string cat in Properties[PropertyNames.Category])
                        categories.Add(cat);
                    return categories;
                }
            }

            /// <summary>
            /// A shallow copy of the properties of the test.
            /// </summary>
            public PropertyBagAdapter Properties => new PropertyBagAdapter(_test.Properties);

            /// <summary>
            /// The arguments to use in creating the test or empty array if none are required.
            /// </summary>
            public object[] Arguments => _test.Arguments;

            #endregion
        }

#endregion

#region Nested ResultAdapter Class

        /// <summary>
        /// ResultAdapter adapts a TestResult for consumption by
        /// the user test code.
        /// </summary>
        public class ResultAdapter
        {
            private readonly TestResult _result;

#region Constructor

            /// <summary>
            /// Construct a ResultAdapter for a TestResult
            /// </summary>
            /// <param name="result">The TestResult to be adapted</param>
            public ResultAdapter(TestResult result)
            {
                _result = result;
            }

#endregion

#region Properties

            /// <summary>
            /// Gets a ResultState representing the outcome of the test
            /// up to this point in its execution.
            /// </summary>
            public ResultState Outcome => _result.ResultState;

            /// <summary>
            /// Gets a list of the assertion results generated
            /// up to this point in the test.
            /// </summary>
            public IEnumerable<AssertionResult> Assertions => _result.AssertionResults;

            /// <summary>
            /// Gets the message associated with a test
            /// failure or with not running the test
            /// </summary>
            public string Message => _result.Message;

            /// <summary>
            /// Gets any stack trace associated with an
            /// error or failure.
            /// </summary>
            public virtual string StackTrace => _result.StackTrace;

            /// <summary>
            /// Gets the number of test cases that failed
            /// when running the test and all its children.
            /// </summary>
            public int FailCount => _result.FailCount;

            /// <summary>
            /// Gets the number of test cases that had warnings
            /// when running the test and all its children.
            /// </summary>
            public int WarningCount => _result.WarningCount;

            /// <summary>
            /// Gets the number of test cases that passed
            /// when running the test and all its children.
            /// </summary>
            public int PassCount => _result.PassCount;

            /// <summary>
            /// Gets the number of test cases that were skipped
            /// when running the test and all its children.
            /// </summary>
            public int SkipCount => _result.SkipCount;

            /// <summary>
            /// Gets the number of test cases that were inconclusive
            /// when running the test and all its children.
            /// </summary>
            public int InconclusiveCount => _result.InconclusiveCount;

#endregion
        }

        #endregion

        #region Nested PropertyBagAdapter Class

        /// <summary>
        /// <see cref="PropertyBagAdapter"/> adapts an <see cref="IPropertyBag"/>
        /// for consumption by the user.
        /// </summary>
        public class PropertyBagAdapter
        {
            private readonly IPropertyBag _source;

            /// <summary>
            /// Construct a <see cref="PropertyBagAdapter"/> from a source
            /// <see cref="IPropertyBag"/>.
            /// </summary>
            public PropertyBagAdapter(IPropertyBag source)
            {
                _source = source;
            }

            /// <summary>
            /// Get the first property with the given <paramref name="key"/>, if it can be found, otherwise
            /// returns null.
            /// </summary>
            public object Get(string key) => _source.Get(key);

            /// <summary>
            /// Indicates whether <paramref name="key"/> is found in this
            /// <see cref="PropertyBagAdapter"/>.
            /// </summary>
            public bool ContainsKey(string key) => _source.ContainsKey(key);

            /// <summary>
            /// Returns a collection of properties
            /// with the given <paramref name="key"/>.
            /// </summary>
            public IEnumerable<object> this[string key]
            {
                get
                {
                    var list = new List<object>();
                    foreach(var item in _source[key])
                    {
                        list.Add(item);
                    }

                    return list;
                }
            }

            /// <summary>
            /// Returns the count of elements with the given <paramref name="key"/>.
            /// </summary>
            public int Count(string key) => _source[key].Count;

            /// <summary>
            /// Returns a collection of the property keys.
            /// </summary>
            public ICollection<string> Keys => _source.Keys;
        }

        #endregion
    }
}
