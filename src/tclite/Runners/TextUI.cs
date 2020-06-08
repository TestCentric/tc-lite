// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Runners
{
    /// <summary>
    /// TextUI is a general purpose class that runs tests and
    /// outputs to a TextWriter.
    /// 
    /// Call it from your Main like this:
    ///   new TextUI(textWriter).Execute(args);
    ///     OR
    ///   new TextUI().Execute(args);
    /// The provided TextWriter is used by default, unless the
    /// arguments to Execute override it using -out. The second
    /// form uses the Console, provided it exists on the platform.
    /// 
    /// NOTE: When running on a platform without a Console, such
    /// as Windows Phone, the results will simply not appear if
    /// you fail to specify a file in the call itself or as an option.
    /// </summary>
    public class TextUI
    {
        private ExtendedTextWriter _writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextUI"/> class.
        /// </summary>
        public TextUI() : this(new ColorConsoleWriter(true), TestListener.NULL) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextUI"/> class.
        /// </summary>
        /// <param name="writer">The TextWriter to use.</param>
        public TextUI(ExtendedTextWriter writer) : this(writer, TestListener.NULL) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextUI"/> class.
        /// </summary>
        /// <param name="writer">The TextWriter to use.</param>
        /// <param name="listener">The Test listener to use.</param>
        public TextUI(ExtendedTextWriter writer, ITestListener listener)
        {
            // Set the default writer - may be overridden by the args specified
            _writer = writer;
        }

        /// <summary>
        /// Write the standard header information to a TextWriter.
        /// </summary>
        public void DisplayHeader()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            string title = "TcLite";
            AssemblyName assemblyName = AssemblyHelper.GetAssemblyName(executingAssembly);
            Version version = assemblyName.Version;
            string copyright = "Copyright (C) 2020, Charlie Poole";
            string build = "";

            var titleAttr = executingAssembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (titleAttr != null)
                title = titleAttr.Title;

            var copyrightAttr = executingAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            if (copyrightAttr != null)
                copyright = copyrightAttr.Copyright;

            var configAttr = executingAssembly.GetCustomAttribute<AssemblyConfigurationAttribute>();
            if (configAttr != null)
                build = $"({configAttr.Configuration})";

            WriteHeader($"{title} {version.ToString(3)} {build}");
            WriteSubHeader(copyright);
        }

        public void DisplayHelp(Mono.Options.OptionSet optionSet)
        {
            using (new ColorConsole(ColorStyle.Help))
                optionSet.WriteOptionDescriptions(_writer);  
        }

        public void DisplayErrorMessages(IEnumerable<string> messages)
        {
            _writer.WriteLine();
            foreach (string msg in messages)
                _writer.WriteLine(ColorStyle.Error, msg);
        }

        /// <summary>
        /// Write information about the current runtime environment
        /// </summary>
        /// <param name="writer">The TextWriter to be used</param>
        public void DisplayRuntimeEnvironment()
        {
            string clrPlatform = Type.GetType("Mono.Runtime", false) == null ? ".NET" : "Mono";

            WriteSectionHeader("Runtime Environment");
            _writer.WriteLabelLine("    OS Version: ", Environment.OSVersion);
            _writer.WriteLabelLine($"  {clrPlatform} Version: ", Environment.Version);
        }

        public void DisplayTestFile(string assemblyPath)
        {
            WriteSectionHeader("Test File");

            _writer.WriteLine(ColorStyle.Default, "    " + assemblyPath);
        }

        public void DisplayTestFilter(string whereClause)
        {
            WriteSectionHeader("Test Filter");
            WriteLabelLine("    Where: ", whereClause.Trim());
            _writer.WriteLine();
        }

        /// <summary>
        /// Prints the Summary Report
        /// </summary>
        public void DisplaySummaryReport(ResultSummary summary)
        {
            var status = summary.ResultState.Status;

            var overallResult = status.ToString();
            if (overallResult == "Skipped")
                overallResult = "Warning";

            ColorStyle overallStyle = status == TestStatus.Passed
                ? ColorStyle.Pass
                : status == TestStatus.Failed
                    ? ColorStyle.Failure
                    : status == TestStatus.Skipped
                        ? ColorStyle.Warning
                        : ColorStyle.Output;

            // if (_testCreatedOutput)
            //     Writer.WriteLine();

            WriteSectionHeader("Test Run Summary");
            WriteLabelLine("  Overall result: ", overallResult, overallStyle);

            WriteSummaryCount("  Test Count: ", summary.TestCount);
            WriteSummaryCount(", Passed: ", summary.PassCount);
            WriteSummaryCount(", Failed: ", summary.FailedCount, ColorStyle.Failure);
            //WriteSummaryCount(", Warnings: ", summary.WarningCount, ColorStyle.Warning);
            WriteSummaryCount(", Inconclusive: ", summary.InconclusiveCount);
            //WriteSummaryCount(", Skipped: ", summary.TotalSkipCount);
            _writer.WriteLine();

            if (summary.FailedCount > 0)
            {
                WriteSummaryCount("    Failed Tests - Failures: ", summary.FailureCount, ColorStyle.Failure);
                WriteSummaryCount(", Errors: ", summary.ErrorCount, ColorStyle.Error);
                WriteSummaryCount(", Invalid: ", summary.InvalidCount, ColorStyle.Error);
                _writer.WriteLine();
            }

            if (summary.NotRunCount > 0)
            {
                WriteSummaryCount("    Skipped Tests - Ignored: ", summary.IgnoreCount, ColorStyle.Warning);
                //WriteSummaryCount(", Explicit: ", summary.ExplicitCount);
                WriteSummaryCount(", Other: ", summary.SkipCount);
                _writer.WriteLine();
            }

            //writer.WriteLabelLine("  Start time: ", summary.StartTime.ToString("u"));
            //writer.WriteLabelLine("    End time: ", summary.EndTime.ToString("u"));
            WriteLabelLine("    Duration: ", string.Format(NumberFormatInfo.InvariantInfo, "{0:0.000} seconds", summary.Duration));
            _writer.WriteLine();
        }

        /// <summary>
        /// Prints the Error Report
        /// </summary>
        public void DisplayErrorsAndFailuresReport(ITestResult result)
        {
            int reportCount = 0;
            WriteSectionHeader("Errors and Failures");
            ListErrorsAndFailures(result, ref reportCount);
        }

        /// <summary>
        /// Prints the Not Run Report
        /// </summary>
        public void DisplayNotRunReport(ITestResult result)
        {
            int reportCount = 0;
            WriteSectionHeader("Tests Not Run");
            ListNotRunResults(result, ref reportCount);
        }

        /// <summary>
        /// Prints a full report of all results
        /// </summary>
        public void DisplayFullReport(ITestResult result)
        {
            WriteSectionHeader("All Test Results");
            ListAllResults(result, " ");
        }

        public void DisplayTestLabel(string testName, string status=null)
        {
            if (status != null)
                _writer.Write(GetColorForResultStatus(status), $"{status} ");

            _writer.WriteLine(ColorStyle.SectionHeader, $"=> {testName}");
        }

        public void DisplaySavedResultMessage(string path, string format)
        {
            _writer.WriteLine(ColorStyle.Output, $"Result saved in {format} format as {path}");
            _writer.WriteLine();
        }

       #region Helper Methods

        private void WriteHeader(string text)
        {
            _writer.WriteLine();
            _writer.WriteLine(ColorStyle.Header, text);
        }

        private void WriteSubHeader(string text)
        {
            _writer.WriteLine(ColorStyle.SubHeader, text);
        }

        private void WriteSectionHeader(string text)
        {
            _writer.WriteLine();
            _writer.WriteLine(ColorStyle.SectionHeader, text);
        }

        private void WriteSummaryCount(string label, int count)
        {
            _writer.WriteLabel(label, count.ToString(CultureInfo.CurrentUICulture));
        }

        private void WriteSummaryCount(string label, int count, ColorStyle color)
        {
            _writer.WriteLabel(label, count.ToString(CultureInfo.CurrentUICulture), count > 0 ? color : ColorStyle.Value);
        }

        private void WriteLabel(string label, object option, ColorStyle color = ColorStyle.Value)
        {
            _writer.WriteLabel(label, option, color);
        }

        private void WriteLabelLine(string label, object option, ColorStyle color = ColorStyle.Value)
        {
            _writer.WriteLabelLine(label, option, color);
        }

        private void ListErrorsAndFailures(ITestResult result, ref int reportCount)
        {
            if (result.ResultState.Status == TestStatus.Failed)
                if (!result.HasChildren)
                    WriteSingleResult(result, ++reportCount);

            if (result.HasChildren)
                foreach (ITestResult childResult in result.Children)
                    ListErrorsAndFailures(childResult, ref reportCount);
        }

        private void ListNotRunResults(ITestResult result, ref int reportCount)
        {
            if (result.HasChildren)
                foreach (ITestResult childResult in result.Children)
                    ListNotRunResults(childResult, ref reportCount);
            else if (result.ResultState.Status == TestStatus.Skipped)
                WriteSingleResult(result, ++reportCount);
        }

        private void PrintTestProperties(ITest test)
        {
            foreach (PropertyEntry entry in test.Properties)
                _writer.WriteLine("  {0}: {1}", entry.Name, entry.Value);
        }

        private void ListAllResults(ITestResult result, string indent)
        {
            string status = null;
            switch (result.ResultState.Status)
            {
                case TestStatus.Failed:
                    status = "FAIL";
                    break;
                case TestStatus.Skipped:
                    status = "SKIP";
                    break;
                case TestStatus.Inconclusive:
                    status = "INC ";
                    break;
                case TestStatus.Passed:
                    status = "OK  ";
                    break;
            }

            _writer.Write(status);
            _writer.Write(indent);
            _writer.WriteLine(result.Name);

            if (result.HasChildren)
                foreach (ITestResult childResult in result.Children)
                    ListAllResults(childResult, indent + "  ");
        }

        private void WriteSingleResult(ITestResult result, int reportCount)
        {
            _writer.WriteLine($"\n{reportCount}) {result.Name} ({result.FullName})");

            if (result.Message != null && result.Message != string.Empty)
                _writer.WriteLine("   {0}", result.Message);

            if (!string.IsNullOrEmpty(result.StackTrace))
            {
                string stackTrace = result.ResultState == ResultState.Failure
                    ? StackFilter.Filter(result.StackTrace)
                    : result.StackTrace;

                _writer.Write(stackTrace);

                if (!stackTrace.EndsWith(Environment.NewLine))
                    _writer.WriteLine();
            }
        }

        private static ColorStyle GetColorForResultStatus(string status)
        {
            switch (status)
            {
                case "Passed":
                    return ColorStyle.Pass;
                case "Failed":
                    return ColorStyle.Failure;
                case "Error":
                case "Invalid":
                case "Cancelled":
                    return ColorStyle.Error;
                case "Warning":
                case "Ignored":
                    return ColorStyle.Warning;
                default:
                    return ColorStyle.Output;
            }
        }

        #endregion
    }
}
