// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Internal;

namespace TCLite.Framework.Builders
{
	/// <summary>
	/// Built-in SuiteBuilder for NUnit TestFixture
	/// </summary>
	public class TCLiteTestFixtureBuilder : ITestFixtureBuilder
    {
        static readonly string NO_TYPE_ARGS_MSG = 
            "Fixture type contains generic parameters. You must either provide " +
            "Type arguments or specify constructor arguments that allow NUnit " +
            "to deduce the Type arguments.";

        /// <summary>
		/// The TestFixture being constructed;
		/// </summary>
		private TestFixture _fixture;

        private ITestCaseBuilder _testCaseBuilder;

        public TCLiteTestFixtureBuilder()
        {
            _testCaseBuilder = new TCLiteTestCaseBuilder();
        }

        /// <summary>
        /// Examine the type and determine if it is suitable for
        /// this builder to use to create one or more TestFixtures.
		/// </summary>
		/// <param name="type">The fixture type to check</param>
		/// <returns>True if the fixture can be built, false if not</returns>
		public bool CanBuildFrom(Type type)
		{
            // TODO: Should we allow static Types as fixtures?
            if ( type.IsAbstract && !type.IsSealed )
                return false;

            if (type.IsDefined(typeof(TestFixtureAttribute), true))
                return true;

            // Generics must have a TestFixtureAttribute
            if (type.IsGenericTypeDefinition)
                return false;

            return Reflect.HasMethodWithAttribute(type, typeof(IImplyFixture));
		}

		/// <summary>
		/// Build one or more TestFixtures from the Type provided.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IEnumerable<TestFixture> BuildFrom(Type type)
		{
            int count = 0;
            foreach(var attr in GetTestFixtureAttributesWithArguments(type))
            {
                yield return BuildSingleFixture(type, attr);
                count++;
            }

            if (count == 0)
                yield return BuildSingleFixture(type, null);
        }
        
		#region Helper Methods

        private TestFixture BuildSingleFixture(Type type, TestFixtureAttribute attr)
        {
            object[] arguments = null;

            if (attr != null)
            {
                arguments = (object[])attr.Arguments;

#if NYI // Generic Fixtures                
                if (type.ContainsGenericParameters)
                {
                    Type[] typeArgs = (Type[])attr.TypeArgs;
                    if( typeArgs.Length > 0 || 
                        TypeHelper.CanDeduceTypeArgsFromArgs(type, arguments, ref typeArgs))
                    {
                        type = TypeHelper.MakeGenericType(type, typeArgs);
                    }
                }
#endif                
            }

            this._fixture = new TestFixture(type, arguments);
            CheckTestFixtureIsValid(_fixture);

            _fixture.ApplyAttributesToTest(type);

            if (_fixture.RunState == RunState.Runnable && attr != null)
            {
                if (attr.Ignore)
                {
                    _fixture.RunState = RunState.Ignored;
                    _fixture.Properties.Set(PropertyNames.SkipReason, attr.IgnoreReason);
                }
            }

            AddTestCases(type);

            return this._fixture;
        }

        /// <summary>
		/// Method to add test cases to the newly constructed fixture.
		/// </summary>
		/// <param name="fixtureType"></param>
		private void AddTestCases( Type fixtureType )
		{
			var methods = fixtureType.GetMethods( 
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );

			foreach(MethodInfo method in methods)
                if (_testCaseBuilder.CanBuildFrom(method))
                    foreach (var test in _testCaseBuilder.BuildFrom(method, _fixture))
				        _fixture.Add(test);
		}

        private void CheckTestFixtureIsValid(TestFixture fixture)
        {
            Type fixtureType = fixture.FixtureType;

            if (fixtureType.ContainsGenericParameters)
            {
                SetNotRunnable(fixture, NO_TYPE_ARGS_MSG);
                return;
            }

            if( !IsStaticClass(fixtureType)  && !HasValidConstructor(fixtureType, fixture.Arguments) )
            {
                SetNotRunnable(fixture, "No suitable constructor was found");
                return;
            }
        }

        private static bool HasValidConstructor(Type fixtureType, object[] args)
        {
            Type[] argTypes;

            // Note: This could be done more simply using
            // Type.EmptyTypes and Type.GetTypeArray() but
            // they don't exist in all runtimes we support.
            if (args == null)
                argTypes = new Type[0];
            else
            {
                argTypes = new Type[args.Length];

                int index = 0;
                foreach (object arg in args)
                    if (arg != null)
                        argTypes[index++] = arg.GetType();
                    else
                        return false;
            }

            return fixtureType.GetConstructor(argTypes) != null;
        }

        private void SetNotRunnable(TestFixture fixture, string reason)
        {
            fixture.RunState = RunState.NotRunnable;
            fixture.Properties.Set(PropertyNames.SkipReason, reason);
        }

        private static bool IsStaticClass(Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        /// <summary>
        /// Get all TestFixtureAttributes attached to a Type, which
        /// specify arguments or Type arguments. Any attributes without
        /// args, constructed with the default constructor, are ignored.
        /// </summary>
        private IEnumerable<TestFixtureAttribute> GetTestFixtureAttributesWithArguments(Type type)
        {
            foreach (var attr in type.GetCustomAttributes(typeof(TestFixtureAttribute), true))
            {
                var fixtureAttr = (TestFixtureAttribute)attr;
                if (fixtureAttr.Arguments.Length > 0)
#if NYI // Generic Fixtures                
                    || fixtureAttr.TypeArgs.Length > 0)
#endif                    
                    yield return fixtureAttr;
            }
        }

#endregion
	}
}
