// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;

namespace TCLite.Framework.Api
{
    /// <summary>
    /// The IParameterDataSource interface is implemented by types
    /// that can provide data for a test method parameter.
    /// </summary>
    public interface IParameterDataSource
    {
        /// <summary>
        /// Gets an enumeration of data items for use as arguments
        /// for a test method parameter.
        /// </summary>
        /// <param name="parameter">The parameter for which data is needed</param>
        /// <returns>An enumeration containing individual data items</returns>
        IEnumerable GetData(ParameterInfo parameter);
    }
}
