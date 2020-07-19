﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework
{
    /// <summary>
    /// The abstract base class for all custom attributes defined by TCLite.
    /// </summary>
    public abstract class TCLiteAttribute : Attribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TCLiteAttribute() { }
    }
}
