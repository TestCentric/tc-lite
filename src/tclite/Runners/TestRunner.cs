// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using TCLite.Framework;
using TCLite.Framework.Api;
using TCLite.Framework.Builders;
using TCLite.Framework.Internal;
using TCLite.Runners.TestSelection;

namespace TCLite.Runners
{
    public class TestRunner : ITestListener
    {
        // Runner Return Codes
        public const int OK = 0;
        public const int INVALID_ARG = -1;
        public const int FILE_NOT_FOUND = -2;
        //public const int FIXTURE_NOT_FOUND = -3;
        public const int INVALID_TEST_FIXTURE = -4;
        public const int NO_TESTS_FOUND = -5;
        public const int UNEXPECTED_ERROR = -100;

        private Assembly _testAssembly;
        private CommandLineOptions _options;
        private ExtendedTextWriter _writer;
        private TextUI _textUI;
        private ITestAssemblyRunner _runner;
        private string _workDirectory;

        public TestRunner()
        {
            _testAssembly = Assembly.GetCallingAssembly();
            _runner = new TCLiteTestAssemblyRunner();
        }

        public int Execute(string[] args)
        {
            _options = new CommandLineOptions();
            _options.Parse(args);

            _workDirectory = _options.WorkDirectory ?? Environment.CurrentDirectory;

            _writer = _options.OutFile != null
                ? new ExtendedTextWrapper(new StreamWriter(_options.OutFile))
                : new ColorConsoleWriter(!_options.NoColor);
            _textUI = new TextUI(_writer);

            if (!_options.NoHeader)
                _textUI.DisplayHeader();

            if (_options.ShowHelp)
            {
                _textUI.DisplayHelp(_options._monoOptions);
                return OK;
            }

            if (_options.ShowVersion)
                return OK;

            if (_options.Error)
            {
                _textUI.DisplayErrorMessages(_options.ErrorMessages);
                _textUI.DisplayHelp(_options._monoOptions);
                return INVALID_ARG;
            }

            _textUI.DisplayRuntimeEnvironment();

            _textUI.DisplayTestFile(AssemblyHelper.GetAssemblyPath(_testAssembly));

            if (_options.WaitBeforeExit && _options.OutFile != null)
                _writer.WriteLine("Ignoring /wait option - only valid for Console");

            var runSettings = MakeRunSettings(_options);

            // We display the filters at this point so that any exception message
            // thrown by CreateTestFilter will be understandable.
            if (_options.WhereClauseSpecified)
                _textUI.DisplayTestFilter(_options.WhereClause);

            ITestFilter filter = _options.WhereClauseSpecified
                ? CreateTestFilter(_options.WhereClause)
                : TestFilter.Empty;

            try
            {
                Randomizer.InitialSeed = _options.RandomSeed;

                if (!_runner.Load(_testAssembly, runSettings))
                {
                    AssemblyName assemblyName = AssemblyHelper.GetAssemblyName(_testAssembly);
                    Console.WriteLine("No tests found in assembly {0}", assemblyName.Name);
                    return NO_TESTS_FOUND;
                }

                return _options.Explore
                    ? ExploreTests()
                    : RunTests(filter);
            }
            catch (FileNotFoundException ex)
            {
                _writer.WriteLine(ex.Message);
                return FILE_NOT_FOUND;
            }
            catch (Exception ex)
            {
                _writer.WriteLine(ex.ToString());
                return UNEXPECTED_ERROR;
            }
            finally
            {
                if (_options.OutFile == null)
                {
                    if (_options.WaitBeforeExit)
                    {
                        Console.WriteLine("Press Enter key to continue . . .");
                        Console.ReadLine();
                    }
                }
                else
                {
                    _writer.Close();
                }
            }
        }

        private int RunTests(ITestFilter filter)
        {
            var labelsOption = _options.DisplayTestLabels?.ToUpper(System.Globalization.CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(labelsOption))
            {
                _displayBeforeTest = labelsOption == "BEFORE";
                _displayAfterTest = labelsOption == "AFTER";
                _displayBeforeOutput = _displayBeforeTest || _displayAfterTest || labelsOption == "ON";

                if (_displayBeforeOutput)
                {
                    Console.SetOut(new TextCapture(this, TestOutputType.Out));
                    Console.SetError(new TextCapture(this, TestOutputType.Error));
                }
            }

            DateTime startTime = DateTime.Now;

            ITestResult result = _runner.Run(this, filter);

            var summary = new ResultSummary(result);

            if (summary.FailureCount > 0 || summary.ErrorCount > 0 || summary.WarningCount > 0)
                _textUI.DisplayErrorsFailuresAndWarningsReport(result);

            if (summary.NotRunCount > 0)
                _textUI.DisplayNotRunReport(result);

            //if (commandLineOptions.Full)
            //    PrintFullReport(result);

            _textUI.DisplaySummaryReport(summary);

            if (!_options.NoResult)
            {
                string resultFile = _options.ResultFile ?? "TestResult.xml";
                if (!Path.IsPathRooted(resultFile))
                    resultFile = Path.Combine(_workDirectory, resultFile);
                string resultFormat = _options.ResultFormat ?? "nunit3";
                
                if(resultFormat == "nunit3")
                    new NUnit3XmlOutputWriter(startTime).WriteResultFile(result, resultFile);
                else if (resultFormat == "nunit2")
                    new NUnit2XmlOutputWriter(startTime).WriteResultFile(result, resultFile);
                else
                    throw new InvalidOperationException($"Invalid result format: {resultFormat}");
                    
                _textUI.DisplaySavedResultMessage(resultFile, resultFormat);
            }

            return Math.Min(100, summary.FailedCount);
        }

        private int ExploreTests()
        {
            XmlNode testNode = _runner.LoadedTest.ToXml(true);

            string exploreFile = _options.ExploreFile ?? "tests.xml";
            if (!Path.IsPathRooted(exploreFile))
                exploreFile = Path.Combine(_workDirectory, exploreFile);

            XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;

            XmlWriter testWriter = System.Xml.XmlWriter.Create(new StreamWriter(exploreFile), settings);

            testNode.WriteTo(testWriter);
            testWriter.Close();

            Console.WriteLine();
            Console.WriteLine("Test info saved as {0}.", exploreFile);

            return OK;
        }

        /// <summary>
        /// Make the settings for this run - this is public for testing
        /// </summary>
        public static Dictionary<string, object> MakeRunSettings(CommandLineOptions options)
        {
            // Transfer command line options to run settings
            var runSettings = new Dictionary<string, object>();

            if (options.NumberOfTestWorkers >= 0)
                runSettings[FrameworkPackageSettings.NumberOfTestWorkers] = options.NumberOfTestWorkers;

            if (options.InternalTraceLevel != null)
                runSettings[FrameworkPackageSettings.InternalTraceLevel] = options.InternalTraceLevel;

            if (options.RandomSeed >= 0)
                runSettings[FrameworkPackageSettings.RandomSeed] = options.RandomSeed;

            if (options.WorkDirectory != null)
                runSettings[FrameworkPackageSettings.WorkDirectory] = Path.GetFullPath(options.WorkDirectory);

            if (options.DefaultTimeout >= 0)
                runSettings[FrameworkPackageSettings.DefaultTimeout] = options.DefaultTimeout;

            if (options.StopOnError)
                runSettings[FrameworkPackageSettings.StopOnError] = true;

            // if (options.DefaultTestNamePattern != null)
            //     runSettings[FrameworkPackageSettings.DefaultTestNamePattern] = options.DefaultTestNamePattern;

            if (options.TestParameters.Count != 0)
                runSettings[FrameworkPackageSettings.TestParametersDictionary] = options.TestParameters;

            return runSettings;
        }

        public ITestFilter CreateTestFilter(string whereClause)
        {
            Guard.ArgumentNotNull(whereClause, nameof(whereClause));

            string xmlText =  new TestSelectionParser().Parse(whereClause);
            return TestFilter.FromXml($"<filter>{xmlText}</filter>");
        }

        #region ITestListener Members

        private string _currentTestName;
        private bool _displayBeforeTest;
        private bool _displayAfterTest;
        private bool _displayBeforeOutput;

        /// <summary>
        /// A test has just started
        /// </summary>
        /// <param name="test">The test</param>
        public void TestStarted(ITest test)
        {
            _currentTestName = test.Name;

            if (_displayBeforeTest)
                _textUI.DisplayTestLabel(_currentTestName);
        }

        /// <summary>
        /// A test has just finished
        /// </summary>
        /// <param name="result">The result of the test</param>
        public void TestFinished(ITestResult result)
        {
            if (_displayAfterTest)
            {
                string status = result.ResultState.Label;
                if (string.IsNullOrEmpty(status))
                    status = result.ResultState.Status.ToString();

                _textUI.DisplayTestLabel(result.Test.Name, status);
            }
        }

        /// <summary>
        /// A test has produced some text output
        /// </summary>
        /// <param name="testOutput">A TestOutput object holding the text that was written</param>
        public void TestOutput(TestOutput testOutput)
        {
            if (_displayBeforeOutput)
                _textUI.DisplayTestLabel(_currentTestName);

            var style = testOutput.Type == TestOutputType.Error
                ? ColorStyle.Error
                : ColorStyle.Output;
            _writer.Write(style, testOutput.Text);
        }

        #endregion
    }
}
