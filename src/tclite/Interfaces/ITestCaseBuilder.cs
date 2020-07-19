// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Interfaces
{
	/// <summary>
	/// The ITestCaseBuilder interface is exposed by a class that knows how to
	/// build a test case from certain methods. 
	/// </summary>
	public interface ITestCaseBuilder
	{
        /// <summary>
        /// Examine the method and determine if it is suitable for
        /// this builder to use in building a TestCase.
        /// 
        /// Note that returning false will cause the method to be ignored 
        /// in loading the tests. If it is desired to load the method
        /// but label it as non-runnable, ignored, etc., then this
        /// method must return true.
        /// </summary>
        /// <param name="method">The test method to examine</param>
        /// <returns>True is the builder can use this method</returns>
        bool CanBuildFrom(MethodInfo method);

        /// <summary>
        /// Build a TestCase from the provided MethodInfo for
        /// inclusion in the suite being constructed.
        /// </summary>
        /// <param name="method">The method to be used as a test case</param>
        /// <param name="suite">The test suite being populated, or null</param>
        /// <returns>A TestCase or null</returns>
        IEnumerable<Test> BuildFrom(MethodInfo method, ITest suite);
    }
}
