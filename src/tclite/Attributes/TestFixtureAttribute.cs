// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
	/// <example>
	/// [TestFixture]
	/// public class ExampleClass 
	/// {}
	/// </example>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    public class TestFixtureAttribute : TCLiteAttribute, IApplyToTest
	{
        private object[] constructorArgs;

        /// <summary>
        /// Construct a TestFixture with no arguments.
        /// </summary>
        public TestFixtureAttribute() { }

        /// <summary>
        /// Construct with a object[] representing a set of arguments. 
        /// In .NET 2.0, the arguments may later be separated into
        /// type arguments and constructor arguments.
        /// </summary>
        /// <param name="arguments"></param>
        public TestFixtureAttribute(params object[] arguments)
        {
            Arguments = arguments ?? new object[] { null };
        }

        /// <summary>
        /// Descriptive text for this fixture
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The arguments originally provided to the attribute
        /// </summary>
        public object[] Arguments { get; } = new object[0];

        /// <summary>
        /// Get or set the type arguments.
        /// </summary>
        public Type[] TypeArgs { get; set; } = Type.EmptyTypes;


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TestFixtureAttribute"/> should be ignored.
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Gets or sets the ignore reason.
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// Gets and sets the category for this fixture.
        /// May be a comma-separated list of categories.
        /// </summary>
        public string Category { get; set; }
 
        /// <summary>
        /// Gets a list of categories for this fixture
        /// </summary>
        public IList Categories
        {
            get { return Category == null ? null : Category.Split(','); }
        }

#region IApplyToTest Members

        /// <summary>
        /// Modifies a test by adding a description, if not already set.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(ITest test)
        {
            if (!test.Properties.ContainsKey(PropertyNames.Description) && Description != null)
                test.Properties.Set(PropertyNames.Description, Description);
			
			if (Category != null)
				foreach (string cat in Category.Split(new char[] { ',' }) )
					test.Properties.Add(PropertyNames.Category, cat);
        }

#endregion
    }
}
