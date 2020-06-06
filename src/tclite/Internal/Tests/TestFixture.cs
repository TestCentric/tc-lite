// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Framework.Api;

namespace TCLite.Framework.Internal
{
	/// <summary>
	/// TestFixture is a surrogate for a user test fixture class,
	/// containing one or more tests.
	/// </summary>
	public class TestFixture : TestSuite
	{
		#region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixture"/> class.
        /// </summary>
        /// <param name="fixtureType">Type of the fixture.</param>
        public TestFixture(Type fixtureType)
            : this(fixtureType, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixture"/> class.
        /// </summary>
        /// <param name="fixtureType">Type of the fixture.</param>
        /// <param name="arguments">The arguments.</param>
        public TestFixture(Type fixtureType, object[] arguments)
            : base(fixtureType, arguments) 
        {
        }

        #endregion
    }
}
