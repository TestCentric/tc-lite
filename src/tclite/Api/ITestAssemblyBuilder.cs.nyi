// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Api
{
    /// <summary>
    /// The ITestAssemblyBuilder interface is implemented by a class
    /// that is able to build a suite of tests given an assembly or 
    /// an assembly filename.
    /// </summary>
    public interface ITestAssemblyBuilder
    {
        // TODO: Remove use of TestSuite after tests are not self-running

        /// <summary>
        /// Build a suite of tests from a provided assembly
        /// </summary>
        /// <param name="assembly">The assembly from which tests are to be built</param>
        /// <param name="options">A dictionary of options to use in building the suite</param>
        /// <returns>A TestSuite containing the tests found in the assembly</returns>
        TestSuite Build(Assembly assembly, IDictionary options);

        /// <summary>
        /// Build a suite of tests given the filename of an assembly
        /// </summary>
        /// <param name="assemblyName">The filename of the assembly from which tests are to be built</param>
        /// <param name="options">A dictionary of options to use in building the suite</param>
        /// <returns>A TestSuite containing the tests found in the assembly</returns>
        TestSuite Build(string assemblyName, IDictionary options);
    }
}
