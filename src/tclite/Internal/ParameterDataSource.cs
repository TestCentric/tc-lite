// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Reflection;
using TCLite.Interfaces;

namespace TCLite.Internal
{
    /// <summary>
    /// ParameterDataSource wraps a single ParameterInfo and
    /// provides access to any data available for that parameter
    /// from attributes implementing IParameterDataSource.
    /// It provides a single point for accessing the data
    /// from potentially multiple attributes.
    /// </summary>
    public class ParameterDataSource : IParameterDataSource
    {
        public ParameterDataSource(ParameterInfo parameter)
        {
            Parameter = parameter;
        }

        /// <summary>
        /// The parameter for which this ParameterDataSource was created.
        /// </summary>
        public ParameterInfo Parameter { get; }

        /// <summary>
        /// Get values associated with the parameter
        /// </summary>
        public IEnumerable GetData(ParameterInfo parameter)
        {
            Guard.ArgumentValid(parameter == Parameter, "GetData called for the wrong parameter", nameof(parameter));

            var sources = parameter.GetCustomAttributes(typeof(IParameterDataSource), false);
            foreach (IParameterDataSource source in sources)
                foreach (object value in source.GetData(parameter))
                    yield return value;
        }
    }
}