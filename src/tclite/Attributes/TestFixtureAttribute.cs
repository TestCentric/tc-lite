// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Internal;
using TCLite.Framework.Tests;

namespace TCLite.Framework
{
	/// <example>
	/// [TestFixture]
	/// public class ExampleClass 
	/// {}
	/// </example>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    public class TestFixtureAttribute : TCLiteAttribute, IApplyToTest
	{
		private string description;

        private object[] originalArgs;
        private object[] constructorArgs;
        private Type[] typeArgs;
        private bool argsInitialized;

        private bool isIgnored;
        private string ignoreReason;
		private string category;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestFixtureAttribute() : this( null ) { }
        
        /// <summary>
        /// Construct with a object[] representing a set of arguments. 
        /// In .NET 2.0, the arguments may later be separated into
        /// type arguments and constructor arguments.
        /// </summary>
        /// <param name="arguments"></param>
        public TestFixtureAttribute(params object[] arguments)
        {
            this.originalArgs = arguments == null
                ? new object[0]
                : arguments;
            this.constructorArgs = this.originalArgs;
            this.typeArgs = new Type[0];
        }

		/// <summary>
		/// Descriptive text for this fixture
		/// </summary>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

        /// <summary>
        /// The arguments originally provided to the attribute
        /// </summary>
        public object[] Arguments
        {
            get 
            {
                if (!argsInitialized)
                    InitializeArgs();
                return constructorArgs; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TestFixtureAttribute"/> should be ignored.
        /// </summary>
        /// <value><c>true</c> if ignore; otherwise, <c>false</c>.</value>
        public bool Ignore
        {
            get { return isIgnored; }
            set { isIgnored = value; }
        }

        /// <summary>
        /// Gets or sets the ignore reason. May set Ignored as a side effect.
        /// </summary>
        /// <value>The ignore reason.</value>
        public string IgnoreReason
        {
            get { return ignoreReason; }
            set
            {
                ignoreReason = value;
                isIgnored = ignoreReason != null && ignoreReason != string.Empty;
            }
        }

        /// <summary>
        /// Get or set the type arguments. If not set
        /// explicitly, any leading arguments that are
        /// Types are taken as type arguments.
        /// </summary>
        public Type[] TypeArgs
        {
            get
            {
                if (!argsInitialized)
                    InitializeArgs();
                return typeArgs;
            }
            set 
            { 
                typeArgs = value;
                argsInitialized = true;
            }
        }

        /// <summary>
        /// Gets and sets the category for this fixture.
        /// May be a comma-separated list of categories.
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
 
        /// <summary>
        /// Gets a list of categories for this fixture
        /// </summary>
        public IList Categories
        {
            get { return category == null ? null : category.Split(','); }
        }
 
        /// <summary>
        /// Helper method to split the original argument list
        /// into type arguments and constructor arguments.
        /// This action has to be delayed rather than done in
        /// the constructor, since TypeArgs may be set by
        /// menas of a named parameter.
        /// </summary>
        private void InitializeArgs()
        {
            int typeArgCount = 0;

            if (this.originalArgs != null)
            {
                foreach (object o in this.originalArgs)
                    if (o is Type) typeArgCount++;
                    else break;
            }

            this.typeArgs = new Type[typeArgCount];
            for (int i = 0; i < typeArgCount; i++)
                this.typeArgs[i] = (Type)this.originalArgs[i];

            int constructorArgCount = originalArgs.Length - typeArgCount;
            this.constructorArgs = new object[constructorArgCount];
                for (int i = 0; i < constructorArgCount; i++)
                    this.constructorArgs[i] = this.originalArgs[typeArgCount + i];
                
            argsInitialized = true;
        }

        #region IApplyToTest Members

        /// <summary>
        /// Modifies a test by adding a description, if not already set.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public void ApplyToTest(Test test)
        {
            if (!test.Properties.ContainsKey(PropertyNames.Description) && description != null)
                test.Properties.Set(PropertyNames.Description, description);
			
			if (category != null)
				foreach (string cat in category.Split(new char[] { ',' }) )
					test.Properties.Add(PropertyNames.Category, cat);
        }

        #endregion
    }
}
