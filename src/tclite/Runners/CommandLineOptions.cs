// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Collections.Generic;
using Mono.Options;

namespace TCLite.Runners
{
    /// <summary>
    /// The CommandLineOptions class parses and holds the values of
    /// any options entered at the command line.
    /// </summary>
    public class CommandLineOptions
    {
        internal OptionSet _monoOptions;

#region Constructor

        public CommandLineOptions()
        {
            _monoOptions = new OptionSet()
            {
                "",
                "Usage: TEST_ASSEMBLY [options]",
                "",
                "Runs tests contained in a user TEST_ASSEMBLY, written as a console application",
                "and referencing the tc-lite framework. The following options are supported...",
                "",
                "Select Tests:",
                "",
                { "where=", "{TSL} expression indicating which tests will be run. If omitted, all tests are run.",
                    v => WhereClause = RequiredValue(v, "--where") },
                "",
                "Determine HOW Tests are Run:",
                "",
                { "params|p=", "(NYI) Define test parameters.",
                    v => { } },
//                     {
//                         string parameters = RequiredValue(v, "--params");

//                         // This can be changed without breaking backwards compatibility with frameworks.
//                         foreach (string param in parameters.Split(new[] { ';' }))
//                         {
//                             int eq = param.IndexOf("=");
//                             if (eq == -1 || eq == param.Length - 1)
//                             {
//                                 ErrorMessages.Add("Invalid format for test parameter. Use NAME=VALUE.");
//                             }
//                             else
//                             {
//                                 string name = param.Substring(0, eq);
//                                 string val = param.Substring(eq + 1);

//                                 TestParameters[name] = val;
//                             }
//                         }
//                     }
//                 }

                { "timeout=", "(NYI) Set default test case timeout in {MILLISECONDS}.",
                    v => DefaultTimeout = RequiredInt(v, "--timeout") },

                { "seed=", "(NYI) Set the random {SEED} used to generate test data. Used for debugging earlier runs.",
                    v => RandomSeed = RequiredInt(v, "--seed") },

                 { "workers=", "(NYI) Specify the {NUMBER} of worker threads to be used in running tests. If not specified, defaults to 2 or the number of processors, whichever is greater.",
                     v => NumberOfTestWorkers = RequiredInt(v, "--workers") },

                { "stoponerror", "(NYI) Stop run immediately upon any test failure or error.",
                    v => StopOnError = v != null },

                { "wait", "Wait for input before closing console window.",
                    v => WaitBeforeExit = v != null },
                "",
                "Test Output:",
                "",
                { "work=", "{PATH} of the directory to use for output files. If not specified, defaults to the current directory.",
                    v =>  WorkDirectory = RequiredValue(v, "--work") },

                { "out=", "(NYI) File {PATH} to contain text output from the tests.",
                    v => OutFile = RequiredValue(v, "--output") },

                { "err=", "(NYI) File {PATH} to contain error output from the tests.",
                    v => ErrFile = RequiredValue(v, "--err") },

                // { "result=", "An output {SPEC} for saving the test results. This option may be repeated.",
                //     v => ResolveOutputSpecification(RequiredValue(v, "--resultxml"), resultOutputSpecifications) },

                { "explore:", "Explore tests rather than running them. The optional {PATH} is used for the XML report describing the tests. It defaults to 'tests.xml'.", //Optionally provide an output {SPEC} for saving the test info. This option may be repeated.",
                    v =>
                    {
                        Explore = true;
                        ExploreFile = v;
                        //ResolveOutputSpecification(v, ExploreOutputSpecifications);
                    } },
                
                { "result=", "Save test result XML in file at {PATH}. If not specified, default is TestResult.xml.",
                    v => ResultFile=RequiredValue(v, "--result")},

                { "format=", "Specify the {FORMAT} to be used in saving the test result. May be `nunit3` or `nunit2'.",
                    v => ResultFormat=RequiredValue(v, "--format", "nunit3", "nunit2")},

                { "noresult", "Don't save any test results.",
                    v => NoResult = v != null },

                { "labels=", "Specify whether to write test case labels to the output. Values: Off, On, Before, After.",
                    v => DisplayTestLabels = RequiredValue(v, "--labels", "Off", "On", "Before", "After") },

                // { "test-name-format=", "Non-standard naming pattern to use in generating test names.",
                //     v => DefaultTestNamePattern = RequiredValue(v, "--test-name-format") },

                { "teamcity", "(NYI) Turns on use of TeamCity service messages.",
                    v => TeamCity = v != null },

                { "trace=", "(NYI) Set internal trace {LEVEL}.\nValues: Off, Error, Warning, Info, Verbose (Debug)",
                     v => InternalTraceLevel = RequiredValue(v, "--trace", "Off", "Error", "Warning", "Info", "Verbose", "Debug") },

                { "noheader|noh", "Don't display program header at start of run.",
                    v => NoHeader = v != null },

                { "nocolor|noc", "Displays console output without color.",
                    v => NoColor = v != null },

                { "help|h", "Display this message and exit.",
                    v => ShowHelp = v != null },

                { "version|V", "Display the header and exit.",
                    v => ShowVersion = v != null },

                ""
            };
        }

#endregion

        #region Properties

        // Action to Perform

        public bool Explore { get; private set; }

        public bool ShowHelp { get; private set; }

        public bool ShowVersion { get; private set; }

        // Select tests

        public IDictionary<string, string> TestParameters { get; } = new Dictionary<string, string>();

        public string WhereClause { get; private set; }
        public bool WhereClauseSpecified { get { return WhereClause != null; } }

        public int DefaultTimeout { get; private set; } = -1;
        public bool DefaultTimeoutSpecified { get { return DefaultTimeout >= 0; } }

        public int RandomSeed { get; private set; } = -1;
        public bool RandomSeedSpecified { get { return RandomSeed >= 0; } }

        public string DefaultTestNamePattern { get; private set; }

        public int NumberOfTestWorkers { get; private set; } = -1;
        public bool NumberOfTestWorkersSpecified { get { return NumberOfTestWorkers >= 0; } }

        public bool StopOnError { get; private set; }

        public bool WaitBeforeExit { get; private set; }

        // Output Control
        public bool NoHeader { get; private set; }

        public bool NoColor { get; private set; }

        public bool TeamCity { get; private set; }

        public string OutFile { get; private set; }
        public bool OutFileSpecified { get { return OutFile != null; } }

        public string ErrFile { get; private set; }
        public bool ErrFileSpecified { get { return ErrFile != null; } }

        public string DisplayTestLabels { get; private set; }

        // private string workDirectory = null;
        public string WorkDirectory { get; private set; }

        public string InternalTraceLevel { get; private set; }
        public bool InternalTraceLevelSpecified { get { return InternalTraceLevel != null; } }

        // private readonly List<OutputSpecification> resultOutputSpecifications = new List<OutputSpecification>();
        // public IList<OutputSpecification> ResultOutputSpecifications
        // {
        //     get
        //     {
        //         if (noresult)
        //             return new OutputSpecification[0];

        //         if (resultOutputSpecifications.Count == 0)
        //             resultOutputSpecifications.Add(new OutputSpecification("TestResult.xml"));

        //         return resultOutputSpecifications;
        //     }
        // }

        public string ResultFile { get; private set; }
        public string ResultFormat { get; private set; }
        public bool NoResult { get; private set; }

        // public IList<OutputSpecification> ExploreOutputSpecifications { get; } = new List<OutputSpecification>();

        public string ExploreFile { get; private set; }

        public bool Full { get; private set; }

        // Error Processing

        public IList<string> ErrorMessages { get; } = new List<string>();

        public bool Error => ErrorMessages.Count > 0;

        #endregion

        private string ExpandToFullPath(string path)
        {
            if (path == null) return null;

#if NETCF
            return Path.Combine(TCLite.Env.DocumentFolder, path);
#else
            return Path.GetFullPath(path); 
#endif
        }

        /// <summary>
        /// Parse command arguments and initialize option settings accordingly
        /// </summary>
        /// <param name="args">The argument list</param>
        public void Parse(params string[] args)
        {
            var invalidOptions = new List<string>();

            try
            {
                invalidOptions = _monoOptions.Parse(args);
            }
            catch(OptionException ex)
            {
                ErrorMessages.Add(ex.Message);
            }

            foreach (string msg in invalidOptions)
                ErrorMessages.Add($"Invalid option: {msg}");

        }
    
        /// <summary>
        /// Case is ignored when val is compared to validValues. When a match is found, the
        /// returned value will be in the canonical case from validValues.
        /// </summary>
        protected string RequiredValue(string val, string option, params string[] validValues)
        {
            if (string.IsNullOrEmpty(val))
            {
                ErrorMessages.Add("Missing required value for option '" + option + "'.");
                return null;
            }

            bool isValid = true;

            if (validValues != null && validValues.Length > 0)
            {
                isValid = false;

                foreach (string valid in validValues)
                    if (string.Compare(valid, val, StringComparison.OrdinalIgnoreCase) == 0)
                        return valid;

            }

            if (!isValid)
                ErrorMessages.Add($"The value {val} is not valid for option {option}.");

            return val;
        }

        protected int RequiredInt(string val, string option)
        {
            int result;
            if (int.TryParse(val, out result)) return result;

            ErrorMessages.Add(string.IsNullOrEmpty(val)
                ? $"Missing required value for option {option}."
                : $"The value {val} is not valid for option {option}.");

            // We have to return something even though the value will
            // be ignored if an error is reported. The -1 value seems
            // like a safe bet in case it isn't ignored due to a bug.
            return -1;
        }

    }
}
