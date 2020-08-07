// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TCLite.Internal;

namespace TCLite.Interfaces
{
	/// <summary>
	/// The ITestFixtureBuilder interface is exposed by a class that knows how to
	/// build a suite from one or more Types. 
	/// </summary>
	public interface ITestFixtureBuilder
	{
		/// <summary>
		/// Examine the type and determine if it is suitable for
		/// this builder to use in building one or more TestFixtures.
        /// 
        /// Since returning false will cause the Type to be ignored, 
        /// the method must return true if the Type is annotated as
		/// a fixture.static It may then be labeled as non-runnable,
		/// ignored, etc., so it is visible to the user.
        /// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>True if the type can be used to build a TestSuite</returns>
		bool CanBuildFrom( Type type );

		/// <summary>
		/// Build a TestSuite from type provided. The resulting suite
		/// will either be a single fixture or a suite containing multiple
		/// fixture instances, depending on the Type and its annotation.
		/// </summary>
		/// <param name="type">The type of the fixture to be used</param>
		/// <returns>A TestSuite</returns>
		IEnumerable<TestFixture> BuildFrom( Type type );
	}
}
