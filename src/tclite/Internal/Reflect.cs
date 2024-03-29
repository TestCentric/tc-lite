// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TCLite.Internal
{
	/// <summary>
	/// Helper methods for inspecting a type by reflection. 
	/// 
	/// Many of these methods take ICustomAttributeProvider as an 
	/// argument to avoid duplication, even though certain attributes can 
	/// only appear on specific types of members, like MethodInfo or Type.
	/// 
	/// In the case where a type is being examined for the presence of
	/// an attribute, interface or named member, the Reflect methods
	/// operate with the full name of the member being sought. This
	/// removes the necessity of the caller having a reference to the
	/// assembly that defines the item being sought and allows the
	/// NUnit core to inspect assemblies that reference an older
	/// version of the NUnit framework.
	/// </summary>
	internal static class Reflect
	{
        private static readonly BindingFlags AllMembers = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        // A zero-length Type array - not provided by System.Type for all CLR versions we support.
        private static readonly Type[] EmptyTypes = new Type[0];

        #region Get Methods of a type

        /// <summary>
        /// Examine a fixture type and return an array of methods having a 
        /// particular attribute. The array is order with base methods first.
        /// </summary>
        /// <param name="fixtureType">The type to examine</param>
        /// <param name="attributeType">The attribute Type to look for</param>
        /// <param name="inherit">Specifies whether to search the fixture type inheritance chain</param>
        /// <returns>The array of methods found</returns>
        public static MethodInfo[] GetMethodsWithAttribute(Type fixtureType, Type attributeType, bool inherit)
        {
            MethodInfoList list = new MethodInfoList();

            foreach (MethodInfo method in fixtureType.GetMethods(AllMembers))
            {
                if (method.IsDefined(attributeType, inherit))
                    list.Add(method);
            }

            list.Sort(new BaseTypesFirstComparer());

            return list.ToArray();
        }

        private class BaseTypesFirstComparer : IComparer<MethodInfo>
        {
            public int Compare(MethodInfo m1, MethodInfo m2)
            {
                if (m1 == null || m2 == null) return 0;

                Type m1Type = m1.DeclaringType;
                Type m2Type = m2.DeclaringType;

                if ( m1Type == m2Type ) return 0;
                if ( m1Type.IsAssignableFrom(m2Type) ) return -1;

                return 1;
            }
        }

        /// <summary>
        /// Examine a fixture type and return true if it has a method with
        /// a particular attribute. 
        /// </summary>
        /// <param name="fixtureType">The type to examine</param>
        /// <param name="attributeType">The attribute Type to look for</param>
        /// <param name="inherit">Specifies whether to search the fixture type inheritance chain</param>
        /// <returns>True if found, otherwise false</returns>
        public static bool HasMethodWithAttribute(Type fixtureType, Type attributeType)
        {
            foreach (MethodInfo method in fixtureType.GetMethods(AllMembers))
            {
                if (method.IsDefined(attributeType, false))
                    return true;
            }

            return false;
        }

        #endregion

        #region Invoke Constructors

        /// <summary>
        /// Invoke the default constructor on a Type
        /// </summary>
        /// <param name="type">The Type to be constructed</param>
        /// <returns>An instance of the Type</returns>
        public static object Construct(Type type)
        {
            ConstructorInfo ctor = type.GetConstructor(EmptyTypes);
            if (ctor == null)
                throw new InvalidTestFixtureException(type.FullName + " does not have a default constructor");

            return ctor.Invoke(null);
        }

        /// <summary>
        /// Invoke a constructor on a Type with arguments
        /// </summary>
        /// <param name="type">The Type to be constructed</param>
        /// <param name="arguments">Arguments to the constructor</param>
        /// <returns>An instance of the Type</returns>
        public static object Construct(Type type, object[] arguments)
        {
            if (arguments == null) return Construct(type);

            Type[] argTypes = GetTypeArray(arguments);
            ConstructorInfo ctor = GetConstructors(type, argTypes).FirstOrDefault();
            if (ctor == null)
                throw new InvalidTestFixtureException(type.FullName + " does not have a suitable constructor");

            return ctor.Invoke(arguments);
        }

        /// <summary>
        /// Returns an array of types from an array of objects.
        /// Used because the compact framework doesn't support
        /// Type.GetTypeArray()
        /// </summary>
        /// <param name="objects">An array of objects</param>
        /// <returns>An array of Types</returns>
        private static Type[] GetTypeArray(object[] objects)
        {
            Type[] types = new Type[objects.Length];
            int index = 0;
            foreach (object o in objects)
                types[index++] = o?.GetType();
            return types;
        }

        /// <summary>
        /// Gets the constructors to which the specified argument types can be coerced.
        /// </summary>
        internal static IEnumerable<ConstructorInfo> GetConstructors(Type type, Type[] matchingTypes)
        {
            return type
                .GetConstructors()
                .Where(c => c.GetParameters().ParametersMatch(matchingTypes));
        }

        /// <summary>
        /// Determines if the given types can be coerced to match the given parameters.
        /// </summary>
        internal static bool ParametersMatch(this ParameterInfo[] pinfos, Type[] ptypes)
        {
            if (pinfos.Length != ptypes.Length)
                return false;

            for (int i = 0; i < pinfos.Length; i++)
            {
                if (!ptypes[i].CanImplicitlyConvertTo(pinfos[i].ParameterType))
                    return false;
            }
            return true;
        }

        // §6.1.2 (Implicit numeric conversions) of the specification
        private static readonly Dictionary<Type, List<Type>> convertibleValueTypes = new Dictionary<Type, List<Type>>() {
            { typeof(decimal), new List<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char) } },
            { typeof(double), new List<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float) } },
            { typeof(float), new List<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float) } },
            { typeof(ulong), new List<Type> { typeof(byte), typeof(ushort), typeof(uint), typeof(char) } },
            { typeof(long), new List<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(char) } },
            { typeof(uint), new List<Type> { typeof(byte), typeof(ushort), typeof(char) } },
            { typeof(int), new List<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(char) } },
            { typeof(ushort), new List<Type> { typeof(byte), typeof(char) } },
            { typeof(short), new List<Type> { typeof(byte) } }
        };

        /// <summary>
        /// Determines whether the current type can be implicitly converted to the specified type.
        /// </summary>
        internal static bool CanImplicitlyConvertTo(this Type from, Type to)
        {
            if (to.IsAssignableFrom(from))
                return true;

            // Look for the marker that indicates from was null
            if (from == null)
            {
                // Look for the marker that indicates from was null
                return to.GetTypeInfo().IsClass || to.FullName.StartsWith("System.Nullable");
            }

            if (convertibleValueTypes.ContainsKey(to) && convertibleValueTypes[to].Contains(from))
                return true;

            return from
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Any(m => m.ReturnType == to && m.Name == "op_Implicit");
        }

        #endregion

        #region Invoke Methods

        /// <summary>
		/// Invoke a parameterless method returning void on an object.
		/// </summary>
		/// <param name="method">A MethodInfo for the method to be invoked</param>
		/// <param name="fixture">The object on which to invoke the method</param>
		public static object InvokeMethod( MethodInfo method, object fixture ) 
		{
			return InvokeMethod( method, fixture, null );
		}

		/// <summary>
		/// Invoke a method, converting any TargetInvocationException to an NUnitException.
		/// </summary>
		/// <param name="method">A MethodInfo for the method to be invoked</param>
		/// <param name="fixture">The object on which to invoke the method</param>
        /// <param name="args">The argument list for the method</param>
        /// <returns>The return value from the invoked method</returns>
		public static object InvokeMethod( MethodInfo method, object fixture, params object[] args )
		{
			if(method != null)
			{
				try
				{
					return method.Invoke( fixture, args );
				}
				catch(Exception e)
				{
                    if (e is TargetInvocationException)
                        throw new TCLiteException("Rethrown", e.InnerException);
                    else
                        throw new TCLiteException("Rethrown", e);
                }
			}

		    return null;
		}

		#endregion

        class MethodInfoList : List<MethodInfo> { }
	}
}
