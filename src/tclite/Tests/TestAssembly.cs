// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.IO;
using System.Reflection;

namespace TCLite.Framework.Tests
{
    /// <summary>
    /// TestAssembly is a TestSuite that represents the execution
    /// of tests in a managed assembly.
    /// </summary>
    public class TestAssembly : TestSuite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAssembly"/> class.
        /// </summary>
        /// <param name="assembly">The assembly containing the tests.</param>
        /// <param name="path">The path used to load the assembly.</param>
        public TestAssembly(Assembly assembly, string path) : base(path) 
        {
            this.Name = Path.GetFileName(path);
        }

        /// <summary>
        /// Gets the name used for the top-level element in the
        /// XML representation of this test
        /// </summary>
        public override string TestType => "Assembly";
    }
}
