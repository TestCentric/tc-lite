// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace TCLite
{
    /// <summary>
    /// <para>
    /// Base class for attributes, which provide test arguments directly in the attribute constructor.
    /// Since .NET severely restricts the data Types, which may be used in this context, the attribute 
    /// tries to simulate what that might have been provided in a direct method call. 
    /// </para>
    /// <para>
    /// For example, since you can’t apply attributes using <see cref="decimal"/> arguments, we allow an
    /// <see cref="int"/> or a <see cref="double"/> value to be specified and then convert it to <see cref="decimal"/>.
    /// </para>
    /// <para>
    /// This class is unsealed and may be inherited by custom user attributes that take test arguments
    /// directly.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class DataParamAttribute : TCLiteAttribute
    {
        /// <summary>
        /// Converts an array of objects to the <paramref name="targetType"/>, if it is supported.
        /// </summary>
        public static IEnumerable ConvertData(object[] data, Type targetType)
        {
            Guard.ArgumentNotNull(data, nameof(data));
            Guard.ArgumentNotNull(targetType, nameof(targetType));           
            return GetData(data, targetType);
        }

        private static IEnumerable GetData(object[] data, Type targetType)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (TryConvert(data[i], targetType, out var convertedValue))
                    data[i] = convertedValue;
            }

            return data;
        }

        /// <summary>
        /// Converts a single value to the <paramref name="targetType"/>, if it is supported.
        /// </summary>
        public static object Convert(object value, Type targetType)
        {
            if (TryConvert(value, targetType, out var convertedValue))
                return convertedValue;

            throw new InvalidOperationException(
                (value == null ? "Null" : $"A value of type {value.GetType()} ({value})")
                + $" cannot be passed to a parameter of type {targetType}.");
        }

        /// <summary>
        /// Performs several special conversions allowed by NUnit in order to
        /// permit arguments with types that cannot be used in the constructor
        /// of an Attribute such as TestCaseAttribute or to simplify their use.
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The target <see cref="Type"/> in which the <paramref name="value"/> should be converted</param>
        /// <param name="convertedValue">If conversion was successfully applied, the <paramref name="value"/> converted into <paramref name="targetType"/></param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was converted and <paramref name="convertedValue"/> should be used;
        /// <see langword="false"/> is no conversion was applied and <paramref name="convertedValue"/> should be ignored
        /// </returns>
        public static bool TryConvert(object value, Type targetType, out object convertedValue)
        {
            // No conversion needed?
            if (targetType.IsInstanceOfType(value))
            {
                convertedValue = value;
                return true;
            }

            // Null is OK for reference types, not for value types
            if (value == null || value is System.DBNull)
            {
                convertedValue = null;
                return !targetType.IsValueType || IsNullable(targetType);
            }

            // flag indicating we can convert using System.Convert
            bool convert = false;

            var underlyingTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingTargetType == typeof(short) || underlyingTargetType == typeof(byte) || underlyingTargetType == typeof(sbyte)
                || underlyingTargetType == typeof(long) || underlyingTargetType == typeof(double) || underlyingTargetType == typeof(float))
            {
                // We allow converting int to any numeric (overflow may occur)
                convert = value is int;
            }
            else if (underlyingTargetType == typeof(decimal))
            {
                // Allow decimal to be expressed as a double, string or int.
                convert = value is double || value is string || value is int;
            }
            else if (underlyingTargetType == typeof(DateTime))
            {
                convert = value is string;
            }

            if (convert)
            {
                convertedValue = System.Convert.ChangeType(value, underlyingTargetType, CultureInfo.InvariantCulture);
                return true;
            }

            // We could not use system.Convert, try for a specific converter.
            var converter = TypeDescriptor.GetConverter(underlyingTargetType);
            if (converter.CanConvertFrom(value.GetType()))
            {
                convertedValue = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
                return true;
            }

            convertedValue = null;
            return false;
        }

        private static bool IsNullable(Type type)
        {
            return type.IsGenericType
                && !type.IsGenericTypeDefinition
                && ReferenceEquals(type.GetGenericTypeDefinition(), typeof(Nullable<>));
        }
    }
}
