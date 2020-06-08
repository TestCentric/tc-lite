// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Reflection;
using System.Xml;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Runners
{
    /// <summary>
    /// NUnit3XmlOutputWriter is responsible for writing the results
    /// of a test to a file in NUnit 3.0 format.
    /// </summary>
    public class NUnit3XmlOutputWriter : OutputWriter
    {
        private DateTime runStartTime;
        private XmlWriter xmlWriter;

        public NUnit3XmlOutputWriter(DateTime runStartTime)
        {
            this.runStartTime = runStartTime;
        }
        /// <summary>
        /// Writes the test result to the specified TextWriter
        /// </summary>
        /// <param name="result">The result to be written to a file</param>
        /// <param name="writer">A TextWriter to which the result is written</param>
        public override void WriteResultFile(ITestResult result, TextWriter writer)
        {
            // TODO: Fix this
            // NOTE: Under .NET 1.1, XmlTextWriter does not implement IDisposable,
            // but does implement Close(). Hence we cannot use a 'using' clause.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(writer, settings);

            try
            {
                WriteXmlOutput(result, xmlWriter);
            }
            finally
            {
                xmlWriter.Close();
            }
        }

        private void WriteXmlOutput(ITestResult result, XmlWriter xmlWriter)
        {
            this.xmlWriter = xmlWriter;

            InitializeXmlFile(result);

            result.ToXml(true).WriteTo(xmlWriter);

            TerminateXmlFile();
        }

        private void InitializeXmlFile(ITestResult result)
        {
            xmlWriter.WriteStartDocument(false);

            // In order to match the format used by NUnit 3.0, we
            // wrap the entire result from the framework in a 
            // <test-run> element.
            xmlWriter.WriteStartElement("test-run");

            xmlWriter.WriteAttributeString("id", "2"); // TODO: Should not be hard-coded
            xmlWriter.WriteAttributeString("name", result.Name);
            xmlWriter.WriteAttributeString("fullname", result.FullName);
            xmlWriter.WriteAttributeString("testcasecount", result.Test.TestCaseCount.ToString());

            xmlWriter.WriteAttributeString("result", result.ResultState.Status.ToString());
            if (result.ResultState.Label != string.Empty) // && result.ResultState.Label != ResultState.Status.ToString())
                xmlWriter.WriteAttributeString("label", result.ResultState.Label);

            xmlWriter.WriteAttributeString("time", result.Duration.ToString());

            xmlWriter.WriteAttributeString("total", (result.PassCount + result.FailCount + result.SkipCount + result.InconclusiveCount).ToString());
            xmlWriter.WriteAttributeString("passed", result.PassCount.ToString());
            xmlWriter.WriteAttributeString("failed", result.FailCount.ToString());
            xmlWriter.WriteAttributeString("inconclusive", result.InconclusiveCount.ToString());
            xmlWriter.WriteAttributeString("skipped", result.SkipCount.ToString());
            xmlWriter.WriteAttributeString("asserts", result.AssertCount.ToString());

            xmlWriter.WriteAttributeString("run-date", XmlConvert.ToString(runStartTime, "yyyy-MM-dd"));
            xmlWriter.WriteAttributeString("start-time", XmlConvert.ToString(runStartTime, "HH:mm:ss"));

            xmlWriter.WriteAttributeString("random-seed", Randomizer.InitialSeed.ToString());

            WriteEnvironmentElement();
        }

        private void WriteEnvironmentElement()
        {
            xmlWriter.WriteStartElement("environment");

            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = AssemblyHelper.GetAssemblyName(assembly);
            xmlWriter.WriteAttributeString("nunit-version", assemblyName.Version.ToString());

            xmlWriter.WriteAttributeString("clr-version", Environment.Version.ToString());
            xmlWriter.WriteAttributeString("os-version", Environment.OSVersion.ToString());
            xmlWriter.WriteAttributeString("platform", Environment.OSVersion.Platform.ToString());
            xmlWriter.WriteAttributeString("cwd", Environment.CurrentDirectory);
            xmlWriter.WriteAttributeString("machine-name", Environment.MachineName);
            xmlWriter.WriteAttributeString("user", Environment.UserName);
            xmlWriter.WriteAttributeString("user-domain", Environment.UserDomainName);
            xmlWriter.WriteAttributeString("culture", System.Globalization.CultureInfo.CurrentCulture.ToString());
            xmlWriter.WriteAttributeString("uiculture", System.Globalization.CultureInfo.CurrentUICulture.ToString());

            xmlWriter.WriteEndElement();
        }

        private void TerminateXmlFile()
        {
            xmlWriter.WriteEndElement(); // test-run
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            xmlWriter.Close();
        }
    }
}
