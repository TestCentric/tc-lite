// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Builders;

namespace TCLite
{
    /// <summary>
    /// Marks a test to use a combinatorial join of any argument data provided. 
    /// Since this is the default, the attribute is optional.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class CombinatorialAttribute : CombiningStrategyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public CombinatorialAttribute() : base(new CombinatorialStrategy()) { }
    }
}
