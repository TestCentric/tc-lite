// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
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
        public CategoryAttribute(string name)
        {
            Name = name.Trim();
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
            Name = name;
        }

        /// <summary>
        /// The name of the category
        /// </summary>
        public string Name { get; }

        #region IApplyToTest Members

        /// <summary>
        /// Modifies a test by adding a category to it.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(ITest test)
        {
            test.Properties.Add(PropertyNames.Category, this.Name);
        }

        #endregion
    }
}
