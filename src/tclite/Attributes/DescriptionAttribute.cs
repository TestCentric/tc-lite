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
    /// Provides the descriptive text relating to the assembly, test fixture or test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited=false)]
    public sealed class DescriptionAttribute : PropertyAttribute
    {
        private string _description;

        /// <summary>
        /// Construct a description Attribute
        /// </summary>
        /// <param name="description">The text of the description</param>
        public DescriptionAttribute(string description)
            : base(PropertyNames.Description, description)
        {
            _description = description;
        }

        public override void ApplyToTest(ITest test)
        {
            test.Properties.Set(PropertyNames.Description, _description);
        }
    }

}
