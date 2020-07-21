// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Interfaces;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// The ParameterDataProvider class implements IParameterDataProvider
    /// and provides data for the parameters of a single method.
    /// </summary>
    public class ParameterDataProvider : IParameterDataProvider
    {
        private readonly List<ParameterDataSource> _sources = new List<ParameterDataSource>();

        public ParameterDataProvider(MethodInfo method)
        {
            foreach(var parameter in method.GetParameters())
                _sources.Add(new ParameterDataSource(parameter));
        }

        /// <summary>
        /// Determines whether any data is available for a parameter.
        /// </summary>
        /// <param name="parameter">The parameter of a parameterized test</param>
        public bool HasDataFor(ParameterInfo parameter)
        {
            foreach (var source in _sources)
                if (source.Parameter == parameter)
                    return true;

            return false;
        }

        /// <summary>
        /// Retrieves data for use with the supplied parameter.
        /// </summary>
        /// <param name="parameter">The parameter of a parameterized test</param>
        public IEnumerable GetDataFor(ParameterInfo parameter)
        {
            foreach (var source in _sources)
                foreach (object data in source.GetData(parameter))
                    yield return data;
        }
    }
}
