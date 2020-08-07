// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Reflection;

namespace TCLite.Interfaces
{
    /// <summary>
    /// Provides data for a single test parameter.
    /// </summary>
    public interface IParameterDataProvider
    {
        /// <summary>
        /// Determines whether any data is available for a parameter.
        /// </summary>
        /// <param name="parameter">The parameter of a parameterized test.</param>
        bool HasDataFor(ParameterInfo parameter);

        /// <summary>
        /// Retrieves a list of arguments which can be passed to the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter of a parameterized test.</param>
        IEnumerable GetDataFor(ParameterInfo parameter);
    }
}
