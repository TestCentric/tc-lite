// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
	/// <summary>
	/// Adding this attribute to a method creates a TCLite TestMethod. 
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public class TestAttribute : TCLiteAttribute, IApplyToTest
	{
		/// <summary>
		/// Descriptive text for this test
		/// </summary>
		public string Description { get; set; }

        #region IApplyToTest Members

        /// <summary>
        /// Modifies a test by adding a description, if not already set.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(Test test)
        {
            if (!test.Properties.ContainsKey(PropertyNames.Description) && Description != null)
                test.Properties.Set(PropertyNames.Description, Description);
        }

        #endregion
    }
}
