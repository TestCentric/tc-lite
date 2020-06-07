// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Extensibility
{
	/// <summary>
	/// The ISuiteBuilder interface is exposed by a class that knows how to
	/// build a suite from one or more Types. 
	/// </summary>
	public interface ISuiteBuilder
	{
		/// <summary>
		/// Examine the type and determine if it is suitable for
		/// this builder to use in building a TestSuite.
        /// 
        /// Note that returning false will cause the type to be ignored 
        /// in loading the tests. If it is desired to load the suite
        /// but label it as non-runnable, ignored, etc., then this
        /// method must return true.
        /// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>True if the type can be used to build a TestSuite</returns>
		bool CanBuildFrom( Type type );

		/// <summary>
		/// Build a TestSuite from type provided.
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>A TestSuite</returns>
		Test BuildFrom( Type type );
	}
}
