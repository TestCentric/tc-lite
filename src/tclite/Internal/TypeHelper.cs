// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TCLite.Internal
{
    /// <summary>
    /// TypeHelper provides static methods that operate on Types.
    /// </summary>
    public class TypeHelper
    {
        /// <summary>
        /// Gets the display name for a Type as used by TCLite.
        /// </summary>
        /// <param name="type">The Type for which a display name is needed.</param>
        /// <returns>The display name for the Type</returns>
        public static string GetDisplayName(Type type)
        {
            if (type.IsGenericParameter)
                return type.Name;

            if (type.IsGenericType)
            {
                string name = type.FullName;
                int index = name.IndexOf('[');
                if (index >= 0) name = name.Substring(0, index);

                index = name.LastIndexOf('.');
                if (index >= 0) name = name.Substring(index+1);

                index = name.IndexOf('`');
                //if (index >= 0) name = name.Substring(0, index);

                var genericArguments = type.GetGenericArguments();
                var currentArgument = 0;

                StringBuilder sb = new StringBuilder();

                bool needPlus = false;
                foreach(string nestedClass in name.Split('+'))
                {
                    if (needPlus)
                        sb.Append("+");
                    needPlus = true;

                    index = nestedClass.IndexOf('`');
                    if (index >= 0)
                    {
                        var nestedClassName = nestedClass.Substring(0, index);
                        sb.Append(nestedClassName);
                        sb.Append("<");

                        var argumentCount = Int32.Parse(nestedClass.Substring(index + 1));
                        for (int i = 0; i < argumentCount; i++)
                        {
                            if (i > 0)
                                sb.Append(",");

                            sb.Append(GetDisplayName(genericArguments[currentArgument++]));
                        }

                        sb.Append(">");
                    }
                    else
                        sb.Append(nestedClass);
                }
                // sb.Append("<");
                // int cnt = 0;
                // foreach (Type t in type.GetGenericArguments())
                // {
                //     if (cnt++ > 0) sb.Append(",");
                //     sb.Append(GetDisplayName(t));
                // }
                // sb.Append(">");

                return sb.ToString();
            }

            int lastdot = type.FullName.LastIndexOf('.');
            return lastdot >= 0 
                ? type.FullName.Substring(lastdot+1)
                : type.FullName;
        }

        /// <summary>
        /// Gets the display name for a Type as used by TCLite.
        /// </summary>
        /// <param name="type">The Type for which a display name is needed.</param>
        /// <param name="arglist">The arglist provided.</param>
        /// <returns>The display name for the Type</returns>
        public static string GetDisplayName(Type type, object[] arglist)
        {
            string baseName = GetDisplayName(type);
            if (arglist == null || arglist.Length == 0)
                return baseName;

            StringBuilder sb = new StringBuilder( baseName );

            sb.Append("(");
            for (int i = 0; i < arglist.Length; i++)
            {
                if (i > 0) sb.Append(",");

                object arg = arglist[i];
                string display = arg == null ? "null" : arg.ToString();

                if (arg is double || arg is float)
                {
                    if (display.IndexOf('.') == -1)
                        display += ".0";
                    display += arg is double ? "d" : "f";
                }
                else if (arg is decimal) display += "m";
                else if (arg is long) display += "L";
                else if (arg is ulong) display += "UL";
                else if (arg is string) display = "\"" + display + "\"";

                sb.Append(display);
            }
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the best fit for a common type to be used in
        /// matching actual arguments to a methods Type parameters.
        /// </summary>
        /// <param name="type1">The first type.</param>
        /// <param name="type2">The second type.</param>
        /// <returns>Either type1 or type2, depending on which is more general.</returns>
        public static Type BestCommonType(Type type1, Type type2)
        {
            if (type1 == type2) return type1;
            if (type1 == null) return type2;
            if (type2 == null) return type1;

            if (TypeHelper.IsNumeric(type1) && TypeHelper.IsNumeric(type2))
            {
                if (type1 == typeof(double)) return type1;
                if (type2 == typeof(double)) return type2;

                if (type1 == typeof(float)) return type1;
                if (type2 == typeof(float)) return type2;

                if (type1 == typeof(decimal)) return type1;
                if (type2 == typeof(decimal)) return type2;

                if (type1 == typeof(UInt64)) return type1;
                if (type2 == typeof(UInt64)) return type2;

                if (type1 == typeof(Int64)) return type1;
                if (type2 == typeof(Int64)) return type2;

                if (type1 == typeof(UInt32)) return type1;
                if (type2 == typeof(UInt32)) return type2;

                if (type1 == typeof(Int32)) return type1;
                if (type2 == typeof(Int32)) return type2;

                if (type1 == typeof(UInt16)) return type1;
                if (type2 == typeof(UInt16)) return type2;

                if (type1 == typeof(Int16)) return type1;
                if (type2 == typeof(Int16)) return type2;

                if (type1 == typeof(byte)) return type1;
                if (type2 == typeof(byte)) return type2;

                if (type1 == typeof(sbyte)) return type1;
                if (type2 == typeof(sbyte)) return type2;
            }

            if (type1.IsAssignableFrom(type2))
                return type1;

            if (type2.IsAssignableFrom(type1))
                return type2;

            return null;
        }

        /// <summary>
        /// Determines whether the specified type is numeric.
        /// </summary>
        /// <param name="type">The type to be examined.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(Type type)
        {
            return type == typeof(double) ||
                    type == typeof(float) ||
                    type == typeof(decimal) ||
                    type == typeof(Int64) ||
                    type == typeof(Int32) ||
                    type == typeof(Int16) ||
                    type == typeof(UInt64) ||
                    type == typeof(UInt32) ||
                    type == typeof(UInt16) ||
                    type == typeof(byte) ||
                    type == typeof(sbyte);
        }

        /// <summary>
        /// Convert an argument list to the required paramter types.
        /// Currently, only widening numeric conversions are performed.
        /// </summary>
        /// <param name="arglist">An array of args to be converted</param>
        /// <param name="parameters">A ParamterInfo[] whose types will be used as targets</param>
        public static void ConvertArgumentList(object[] arglist, ParameterInfo[] parameters)
        {
            System.Diagnostics.Debug.Assert(arglist.Length == parameters.Length);

            for (int i = 0; i < parameters.Length; i++)
            {
                object arg = arglist[i];

                if (arg != null && arg is IConvertible)
                {
                    Type argType = arg.GetType();
                    Type targetType = parameters[i].ParameterType;
                    bool convert = false;

                    if (argType != targetType && !argType.IsAssignableFrom(targetType))
                    {
                        if (IsNumeric(argType) && IsNumeric(targetType))
                        {
                            if (targetType == typeof(double) || targetType == typeof(float))
                                convert = arg is int || arg is long || arg is short || arg is byte || arg is sbyte;
                            else
                                if (targetType == typeof(long))
                                    convert = arg is int || arg is short || arg is byte || arg is sbyte;
                                else
                                    if (targetType == typeof(short))
                                        convert = arg is byte || arg is sbyte;
                        }
                    }

                    if (convert)
                        arglist[i] = Convert.ChangeType(arg, targetType,
                            System.Globalization.CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// Creates an instance of a generic Type using the supplied Type arguments
        /// </summary>
        /// <param name="type">The generic type to be specialized.</param>
        /// <param name="typeArgs">The type args.</param>
        /// <returns>An instance of the generic type.</returns>
        public static Type MakeGenericType(Type type, Type[] typeArgs)
        {
            // TODO: Add error handling
            return type.MakeGenericType(typeArgs);
        }

        /// <summary>
        /// Determines whether this instance can deduce type args for a generic type from the supplied arguments.
        /// </summary>
        /// <param name="type">The type to be examined.</param>
        /// <param name="arglist">The arglist.</param>
        /// <param name="typeArgsOut">The type args to be used.</param>
        /// <returns>
        /// 	<c>true</c> if this the provided args give sufficient information to determine the type args to be used; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanDeduceTypeArgsFromArgs(Type type, object[] arglist, ref Type[] typeArgsOut)
        {
            Type[] typeParameters = type.GetGenericArguments();

            foreach (ConstructorInfo ctor in type.GetConstructors())
            {
                ParameterInfo[] parameters = ctor.GetParameters();
                if (parameters.Length != arglist.Length)
                    continue;

                Type[] typeArgs = new Type[typeParameters.Length];
                for (int i = 0; i < typeArgs.Length; i++)
                {
                    for (int j = 0; j < arglist.Length; j++)
			        {
                        if (parameters[j].ParameterType.Equals(typeParameters[i]))
                            typeArgs[i] = TypeHelper.BestCommonType(
                                typeArgs[i],
                                arglist[j].GetType());
			        }

                    if (typeArgs[i] == null)
                    {
                        typeArgs = null;
                        break;
                    }
                }

                if (typeArgs != null)
                {
                    typeArgsOut = typeArgs;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return the interfaces implemented by a Type.
        /// </summary>
        /// <param name="type">The Type to be examined.</param>
        /// <returns>An array of Types for the interfaces.</returns>
        public static Type[] GetDeclaredInterfaces(Type type)
        {
            List<Type> interfaces = new List<Type>(type.GetInterfaces());

            if (type.GetTypeInfo().BaseType == typeof(object))
                return interfaces.ToArray();

            List<Type> baseInterfaces = new List<Type>(type.GetTypeInfo().BaseType.GetInterfaces());
            List<Type> declaredInterfaces = new List<Type>();

            foreach (Type interfaceType in interfaces)
            {
                if (!baseInterfaces.Contains(interfaceType))
                    declaredInterfaces.Add(interfaceType);
            }

            return declaredInterfaces.ToArray();
        }

        /// <summary>
        /// Return whether or not the given type is a ValueTuple.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Whether or not the given type is a ValueTuple.</returns>
        public static bool IsValueTuple(Type type)
        {
            return IsTupleInternal(type, "System.ValueTuple");
        }

        /// <summary>
        /// Return whether or not the given type is a Tuple.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Whether or not the given type is a Tuple.</returns>
        public static bool IsTuple(Type type)
        {
            return IsTupleInternal(type, "System.Tuple");
        }

        private static bool IsTupleInternal(Type type, string tupleName)
        {
            string typeName = type.FullName;

            if (typeName.EndsWith("[]"))
                return false;

            string typeNameWithoutGenerics = GetTypeNameWithoutGenerics(typeName);
            return typeNameWithoutGenerics == tupleName;
        }

        private static string GetTypeNameWithoutGenerics(string fullTypeName)
        {
            int index = fullTypeName.IndexOf('`');
            return index == -1 ? fullTypeName : fullTypeName.Substring(0, index);
        }

        /// <summary>
        /// Determines whether the cast to the given type would succeed.
        /// If <paramref name="obj"/> is <see langword="null"/> and <typeparamref name="T"/>
        /// can be <see langword="null"/>, the cast succeeds just like the C# language feature.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        internal static bool CanCast<T>(object obj)
        {
            return obj is T || (obj == null && default(T) == null);
        }

        /// <summary>
        /// Casts to a value of the given type if possible.
        /// If <paramref name="obj"/> is <see langword="null"/> and <typeparamref name="T"/>
        /// can be <see langword="null"/>, the cast succeeds just like the C# language feature.
        /// </summary>
        /// <param name="obj">The object to cast.</param>
        /// <param name="value">The value of the object, if the cast succeeded.</param>
        internal static bool TryCast<T>(object obj, out T value)
        {
            if (obj is T)
            {
                value = (T)obj;
                return true;
            }

            value = default(T);
            return obj == null && default(T) == null;
        }
    }
}
