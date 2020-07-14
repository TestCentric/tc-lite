﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Tests;
using TCLite.Framework.Internal;
using TCLite.Framework.WorkItems;

namespace TCLite.Framework.Builders
{
    /// <summary>
    /// Default implementation of ITestAssemblyRunner
    /// </summary>
    public class TCLiteTestAssemblyRunner : ITestAssemblyRunner
    {
        private IDictionary<string, object> _settings;

        TCLiteTestFixtureBuilder _builder = new TCLiteTestFixtureBuilder();

        #region Properties

        /// <summary>
        /// Root of the tree of loaded tests or null if none have been loaded
        /// </summary>
        public TestSuite LoadedTest { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the tests found in an Assembly
        /// </summary>
        /// <param name="assembly">The assembly to load</param>
        /// <param name="settings">Dictionary of option settings for loading the assembly</param>
        /// <returns>True if the load was successful</returns>
        public bool Load(Assembly assembly, IDictionary<string, object> settings)
        {
            _settings = settings;

            var fixtures = GetFixtures(assembly);

            return fixtures.Count > 0
                ? (LoadedTest = BuildTestAssembly(assembly, fixtures)) != null
                : false;
        }

        /// <summary>
        /// Run selected tests and return a test result. The test is run synchronously,
        /// and the listener interface is notified as it progresses.
        /// </summary>
        /// <param name="listener">Interface to receive EventListener notifications.</param>
        /// <param name="filter">A test filter used to select tests to be run</param>
        /// <returns></returns>
        public ITestResult Run(ITestListener listener, ITestFilter filter)
        {
            TestExecutionContext context = new TestExecutionContext();

            if (this._settings.ContainsKey("WorkDirectory"))
                context.WorkDirectory = (string)this._settings["WorkDirectory"];
            else
                context.WorkDirectory = Environment.CurrentDirectory;

            context.Listener = listener;

            WorkItem workItem = new CompositeWorkItem(LoadedTest, filter);
            workItem.Execute(context);

            while (workItem.State != WorkItemState.Complete)
                System.Threading.Thread.Sleep(5);
            return workItem.Result;
        }

        #endregion

        #region Helper Methods

        private IList<TestFixture> GetFixtures(Assembly assembly)
        {
            var fixtures = new List<TestFixture>();

            foreach (Type testType in assembly.GetTypes())
            {
                if (_builder.CanBuildFrom(testType))
                    fixtures.AddRange(_builder.BuildFrom(testType));
            }

            return fixtures;
        }

        private TestSuite BuildTestAssembly(Assembly assembly, IList<TestFixture> fixtures)
        {
            string assemblyPath = AssemblyHelper.GetAssemblyPath(assembly);

            TestSuite testAssembly = new TestAssembly(assembly, assemblyPath);
            testAssembly.Seed = Randomizer.InitialSeed;

            foreach (Test fixture in fixtures)
                testAssembly.Add(fixture);

            if (fixtures.Count == 0)
            {
                testAssembly.RunState = RunState.NotRunnable;
                testAssembly.Properties.Set(PropertyNames.SkipReason, "Has no TestFixtures");
            }

            testAssembly.ApplyAttributesToTest(assembly);

            testAssembly.Properties.Set(PropertyNames.ProcessID, System.Diagnostics.Process.GetCurrentProcess().Id);
            testAssembly.Properties.Set(PropertyNames.AppDomain, AppDomain.CurrentDomain.FriendlyName);

            return testAssembly;
        }

        #endregion
    }
}
