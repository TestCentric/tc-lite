// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// Applies a category to a test
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method, AllowMultiple=true, Inherited=true)]
    public class CategoryAttribute : TCLiteAttribute, IApplyToTest
    {
        /// <summary>
        /// Construct attribute for a given category based on
        /// a name. The name may not contain the characters ',',
        /// '+', '-' or '!'. However, this is not checked in the
        /// constructor since it would cause an error to arise at
        /// as the test was loaded without giving a clear indication
        /// of where the problem is located. The error is handled
        /// in NUnitFramework.cs by marking the test as not
        /// runnable.
        /// </summary>
        /// <param name="name">The name of the category</param>
        public CategoryAttribute(string names)
        {
            foreach (string name in names.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                Categories.Add(name.Trim());
        }

        /// <summary>
        /// Protected constructor uses the Type name as the name
        /// of the category.
        /// </summary>
        protected CategoryAttribute()
        {
            var name = GetType().Name;
            if (name.EndsWith( "Attribute" ))
                name = name.Substring( 0, name.Length - 9 );
            Categories.Add(name);
        }

        /// <summary>
        /// The categories specified by this attribute
        /// </summary>
        public List<string> Categories { get; } = new List<string>();

        #region IApplyToTest Members

        /// <summary>
        /// Modifies a test by adding a category to it.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(ITest test)
        {
            foreach (string cat in Categories)
                test.Properties.Add(PropertyNames.Category, cat);
        }

        #endregion
    }
}
