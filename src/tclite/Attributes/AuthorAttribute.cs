// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using TCLite.Framework.Internal;

namespace TCLite.Framework
{
    /// <summary>
    /// Provides the author of a test or test fixture. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true, Inherited=false)]
    public sealed class AuthorAttribute : PropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the author.</param>
        public AuthorAttribute(string name) : base(PropertyNames.Author, name) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the author.</param>
        /// <param name="email">The email address of the author.</param>
        public AuthorAttribute(string name, string email)
            : base(PropertyNames.Author, string.Format($"{name} <{email}>"))
        {
        }
    }
}
