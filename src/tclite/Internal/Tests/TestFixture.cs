// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Internal
{
	/// <summary>
	/// TestFixture is a surrogate for a user test fixture class,
	/// containing one or more tests.
	/// </summary>
	internal class TestFixture : TestSuite
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixture"/> class.
        /// </summary>
        /// <param name="fixtureType">Type of the fixture.</param>
        /// <param name="arguments">The arguments.</param>
        public TestFixture(Type fixtureType, object[] arguments = null)
            : base(fixtureType.Name) 
        {
            FixtureType = fixtureType;
            FullName = fixtureType.FullName;
#if NYI // DisplayName Adjustment
            string name = TypeHelper.GetDisplayName(fixtureType, arguments);
            this.Name = name;
            
            this.FullName = name;
            string nspace = fixtureType.Namespace;
            if (nspace != null && nspace != "")
                this.FullName = nspace + "." + name;
#endif
            Arguments = arguments;
        }

    }
}
