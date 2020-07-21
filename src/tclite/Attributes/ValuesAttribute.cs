// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework
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
        protected object[] data;

        // /// <summary>
        // /// Construct with one argument
        // /// </summary>
        // /// <param name="arg1"></param>
        // public ValuesAttribute(object arg1)
        // {
        //     data = new object[] { arg1 };
        // }

        // /// <summary>
        // /// Construct with two arguments
        // /// </summary>
        // /// <param name="arg1"></param>
        // /// <param name="arg2"></param>
        // public ValuesAttribute(object arg1, object arg2)
        // {
        //     data = new object[] { arg1, arg2 };
        // }

        // /// <summary>
        // /// Construct with three arguments
        // /// </summary>
        // /// <param name="arg1"></param>
        // /// <param name="arg2"></param>
        // /// <param name="arg3"></param>
        // public ValuesAttribute(object arg1, object arg2, object arg3)
        // {
        //     data = new object[] { arg1, arg2, arg3 };
        // }

        /// <summary>
        /// Construct with an array of arguments
        /// </summary>
        /// <param name="args"></param>
        public ValuesAttribute(params object[] args)
        {
            data = args ?? new object[] { null };
        }

        /// <summary>
        /// Get the collection of values to be used as arguments
        /// </summary>
        public IEnumerable GetData(ParameterInfo parameter)
        {
            return ConvertData(data, parameter.ParameterType);
            // Type targetType = parameter.ParameterType;

            // for (int i = 0; i < data.Length; i++)
            // {
            //     object arg = data[i];

            //     if (arg == null) 
            //         continue;

            //     if (arg.GetType().FullName == "TCLite.Framework.SpecialValue" &&
            //         arg.ToString() == "Null")
            //     {
            //         data[i] = null;
            //         continue;
            //     }

            //     if (targetType.IsAssignableFrom(arg.GetType()))
            //         continue;

            //     if (arg is DBNull)
            //     {
            //         data[i] = null;
            //         continue;
            //     }

            //     bool convert = false;

            //     if (targetType == typeof(short) || targetType == typeof(byte) || targetType == typeof(sbyte))
            //         convert = arg is int;
            //     else
            //         if (targetType == typeof(decimal))
            //             convert = arg is double || arg is string || arg is int;
            //         else
            //             if (targetType == typeof(DateTime))
            //                 convert = arg is string;

            //     if (convert)
            //     {
            //         data[i] = Convert.ChangeType(arg, targetType, System.Globalization.CultureInfo.InvariantCulture);
            //         continue;
            //     }

            //     var converter = TypeDescriptor.GetConverter(targetType);
            //     if (converter.CanConvertFrom(arg.GetType()))
            //         data[i] = converter.ConvertFrom(null, CultureInfo.InvariantCulture, arg);
            // }

            // return data;
        }
    }
}
