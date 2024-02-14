// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// ValuesAttribute is used to provide literal arguments for
    /// an individual parameter of a test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class ValuesAttribute : DataParamAttribute, IParameterDataSource
    {
        /// <summary>
        /// The collection of data to be returned. Must
        /// be set by any derived attribute classes.
        /// We use an object[] so that the individual
        /// elements may have their type changed in GetData
        /// if necessary
        /// </summary>
        // TODO: This causes a lot of boxing so we should eliminate it
        protected object[] _dataValues;

        /// <summary>
        /// Construct with an array of arguments
        /// </summary>
        /// <param name="args"></param>
        public ValuesAttribute(params object[] args)
        {
            _dataValues = args ?? new object[] { null };
        }

        /// <summary>
        /// Get the collection of values to be used as arguments
        /// </summary>
        public IEnumerable GetData(ParameterInfo parameter)
        {
            return ConvertData(_dataValues, parameter.ParameterType);
        }
    }
}
