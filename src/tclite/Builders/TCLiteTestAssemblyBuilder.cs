using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.Framework.Tests;

namespace TCLite.Framework.Builders
{
    /// <summary>
    /// DefaultTestAssemblyBuilder loads a single assembly and builds a TestSuite
    /// containing test fixtures present in the assembly.
    /// </summary>
    public class TCLiteTestAssemblyBuilder : ITestAssemblyBuilder
    {
        #region Instance Fields

        /// <summary>
        /// The loaded assembly
        /// </summary>
        Assembly assembly;

        TCLiteTestFixtureBuilder _builder;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TCLiteTestAssemblyBuilder"/> class.
        /// </summary>
        public TCLiteTestAssemblyBuilder()
        {
        }

        #endregion

        #region Build Methods
        /// <summary>
        /// Build a suite of tests from a provided assembly
        /// </summary>
        /// <param name="assembly">The assembly from which tests are to be built</param>
        /// <param name="options">A dictionary of options to use in building the suite</param>
        /// <returns>
        /// A TestSuite containing the tests found in the assembly
        /// </returns>
        public TestSuite Build(Assembly assembly, IDictionary options)
        {
            this.assembly = assembly;

            var fixtureNames = options["LOAD"] as IList;

            var fixtures = GetFixtures(assembly, fixtureNames);

            if (fixtures.Count > 0)
            {
                string assemblyPath = AssemblyHelper.GetAssemblyPath(assembly);
                return BuildTestAssembly(assemblyPath, fixtures);
            }

            return null;
        }

        /// <summary>
        /// Build a suite of tests given the filename of an assembly
        /// </summary>
        /// <param name="assemblyName">The filename of the assembly from which tests are to be built</param>
        /// <param name="options">A dictionary of options to use in building the suite</param>
        /// <returns>
        /// A TestSuite containing the tests found in the assembly
        /// </returns>
        public TestSuite Build(string assemblyName, IDictionary options)
        {
            this.assembly = Load(assemblyName);
            if (assembly == null) return null;

            IList fixtureNames = options["LOAD"] as IList;

            IList fixtures = GetFixtures(assembly, fixtureNames);
            if (fixtures.Count > 0)
                return BuildTestAssembly(assemblyName, fixtures);

            return null;
        }
        #endregion

        #region Helper Methods

        private Assembly Load(string path)
        {
            // Throws if this isn't a managed assembly or if it was built
            // with a later version of the same assembly. 
            AssemblyName assemblyName = AssemblyName.GetAssemblyName(Path.GetFileName(path));

            return Assembly.Load(assemblyName);
        }

        private IList GetFixtures(Assembly assembly, IList names)
        {
            var fixtures = new List<object>();

            var testTypes = GetCandidateFixtureTypes(assembly, names);

            foreach (Type testType in testTypes)
            {
                if (_builder.CanBuildFrom(testType))
                    fixtures.Add(_builder.BuildFrom(testType));
            }

            return fixtures;
        }

        private IList GetCandidateFixtureTypes(Assembly assembly, IList names)
        {
            IList types = assembly.GetTypes();

            if (names == null || names.Count == 0)
                return types;

            var result = new List<Type>();

            foreach (string name in names)
            {
                Type fixtureType = assembly.GetType(name);
                if (fixtureType != null)
                    result.Add(fixtureType);
                else
                {
                    string prefix = name + ".";

                    foreach (Type type in types)
                        if (type.FullName.StartsWith(prefix))
                            result.Add(type);
                }
            }

            return result;
        }

        private TestSuite BuildFromFixtureType(string assemblyName, Type testType)
        {
            // TODO: This is the only situation in which we currently
            // recognize and load legacy suites. We need to determine 
            // whether to allow them in more places.
            //if (legacySuiteBuilder.CanBuildFrom(testType))
            //    return (TestSuite)legacySuiteBuilder.BuildFrom(testType);
            //else 
            if (_builder.CanBuildFrom(testType))
                return BuildTestAssembly(assemblyName,
                    new Test[] { _builder.BuildFrom(testType) });
            return null;
        }

        private TestSuite BuildTestAssembly(string assemblyName, IList fixtures)
        {
            TestSuite testAssembly = new TestAssembly(this.assembly, assemblyName);
            testAssembly.Seed = Randomizer.InitialSeed;

            //NamespaceTreeBuilder treeBuilder =
            //    new NamespaceTreeBuilder(testAssembly);
            //treeBuilder.Add(fixtures);
            //testAssembly = treeBuilder.RootSuite;

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


            // TODO: Make this an option? Add Option to sort assemblies as well?
            //testAssembly.Sort();

            return testAssembly;
        }
        #endregion
    }
}
