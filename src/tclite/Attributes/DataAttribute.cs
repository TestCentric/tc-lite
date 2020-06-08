// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework
{
    /// <summary>
    /// The abstract base class for all data-providing attributes 
    /// defined by TCLite. Used to select all data sources for a 
    /// method, class or parameter.
    /// </summary>
    public abstract class DataAttribute : TCLiteAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DataAttribute() { }
    }
}
