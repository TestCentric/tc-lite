// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Attaches information to a test assembly, fixture or method as a name/value pair.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method|AttributeTargets.Assembly, AllowMultiple=true, Inherited=true)]
    public class PropertyAttribute : TCLiteAttribute, IApplyToTest
    {
        /// <summary>
        /// Construct a PropertyAttribute with a name and string value
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyValue">The property value</param>
        public PropertyAttribute(string propertyName, string propertyValue)
        {
            Property = new Property(propertyName, propertyValue);
        }

        /// <summary>
        /// Construct a PropertyAttribute with a name and int value
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyValue">The property value</param>
        public PropertyAttribute(string propertyName, int propertyValue)
        {
            Property = new Property(propertyName, propertyValue);
        }

        /// <summary>
        /// Construct a PropertyAttribute with a name and double value
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyValue">The property value</param>
        public PropertyAttribute(string propertyName, double propertyValue)
        {
            Property = new Property(propertyName, propertyValue);
        }

        /// <summary>
        /// Constructor for derived classes that set the
        /// property dictionary directly.
        /// </summary>
        protected PropertyAttribute() { }

        /// <summary>
        /// Constructor for use by derived classes that use the
        /// name of the type as the property name. Derived classes
        /// must ensure that the Type of the property value is
        /// a standard type supported by the BCL. Any custom
        /// types will cause a serialization Exception when
        /// in the client.
        /// </summary>
        protected PropertyAttribute( object propertyValue )
        {
            string propertyName = this.GetType().Name;
            if ( propertyName.EndsWith( "Attribute" ) )
                propertyName = propertyName.Substring( 0, propertyName.Length - 9 );

            Property = new Property(propertyName, propertyValue);
        }

        public Property Property { get; }

        #region IApplyToTest Members

        /// <summary>
        /// Modifies a test by adding properties to it.
        /// </summary>
        /// <param name="test">The test to modify</param>
        public virtual void ApplyToTest(ITest test)
        {
            test.Properties.Add(Property.Name, Property.Value);
        }

        #endregion
    }

    #region Property Class

    // TODO: Temporary class until PropertyBag is rewritten

    public class Property
    {
        public Property(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public object Value { get; }
    }

    #endregion
}
