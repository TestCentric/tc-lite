// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Api
{
    /// <summary>
    /// Immutable class that stores a property entry as a Name/Value pair.
    /// </summary>
    public class PropertyEntry
    {
        private readonly string name;
        private readonly object value;

        /// <summary>
        /// Initializes a new immutable instance of the <see cref="PropertyEntry"/> class.  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public PropertyEntry(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>Name of the PropertyEntry.</summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>Value of the PropertyEntry.</summary>
        public object Value
        {
            get { return value; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}={1}", name, value);
        }
    }
}
