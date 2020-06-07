// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Threading.Tasks;
using TCLite.Framework.Api;
using TCLite.Framework.Internal;
using TCLite.Framework.Internal.Tests;
using TCLite.Framework.Extensibility;

namespace TCLite.Framework.Builders
{
    /// <summary>
    /// Class to build ether a parameterized or a normal NUnitTestMethod.
    /// There are four cases that the builder must deal with:
    ///   1. The method needs no params and none are provided
    ///   2. The method needs params and they are provided
    ///   3. The method needs no params but they are provided in error
    ///   4. The method needs params but they are not provided
    /// This could have been done using two different builders, but it
    /// turned out to be simpler to have just one. The BuildFrom method
    /// takes a different branch depending on whether any parameters are
    /// provided, but all four cases are dealt with in lower-level methods
    /// </summary>
    public class TCLiteTestCaseBuilder : ITestCaseBuilder2
	{
        private Randomizer _randomizer;

        // NYI: private ITestCaseProvider _testCaseProvider = new TestCaseProviders();

        /// <summary>
        /// Default no argument constructor for NUnitTestCaseBuilder
        /// </summary>
        public TCLiteTestCaseBuilder()
        {
            _randomizer = Randomizer.CreateRandomizer();
        }

        #region ITestCaseBuilder Methods
        /// <summary>
        /// Determines if the method can be used to build an NUnit test
        /// test method of some kind. The method must normally be marked
        /// with an identifying attribute for this to be true.
        /// 
        /// Note that this method does not check that the signature
        /// of the method for validity. If we did that here, any
        /// test methods with invalid signatures would be passed
        /// over in silence in the test run. Since we want such
        /// methods to be reported, the check for validity is made
        /// in BuildFrom rather than here.
        /// </summary>
        /// <param name="method">A MethodInfo for the method being used as a test method</param>
        /// <returns>True if the builder can create a test case from this method</returns>
        public bool CanBuildFrom(MethodInfo method)
        {
            return method.IsDefined(typeof(IImplyFixture), false);
        }

        /// <summary>
        /// Build a Test from the provided MethodInfo. Depending on
        /// whether the method takes arguments and on the availability
        /// of test case data, this method may return a single test
        /// or a group of tests contained in a ParameterizedMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <returns>A Test representing one or more method invocations</returns>
        public Test BuildFrom(MethodInfo method)
		{
            return BuildFrom(method, null);
        }

        #endregion

        #region ITestCaseBuilder2 Members

        /// <summary>
        /// Determines if the method can be used to build an NUnit test
        /// test method of some kind. The method must normally be marked
        /// with an identifying attribute for this to be true.
        /// 
        /// Note that this method does not check that the signature
        /// of the method for validity. If we did that here, any
        /// test methods with invalid signatures would be passed
        /// over in silence in the test run. Since we want such
        /// methods to be reported, the check for validity is made
        /// in BuildFrom rather than here.
        /// </summary>
        /// <param name="method">A MethodInfo for the method being used as a test method</param>
        /// <param name="parentSuite">The test suite being built, to which the new test would be added</param>
        /// <returns>True if the builder can create a test case from this method</returns>
        public bool CanBuildFrom(MethodInfo method, ITest parentSuite)
        {
            return CanBuildFrom(method);
        }

        /// <summary>
        /// Build a Test from the provided MethodInfo. Depending on
        /// whether the method takes arguments and on the availability
        /// of test case data, this method may return a single test
        /// or a group of tests contained in a ParameterizedMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <param name="parentSuite">The test fixture being populated, or null</param>
        /// <returns>A Test representing one or more method invocations</returns>
        public Test BuildFrom(MethodInfo method, ITest parentSuite)
        {
            return BuildSingleTestMethod(method, parentSuite, null);
#if NYI
            return _testCaseProvider.HasTestCasesFor(method)
                ? BuildParameterizedMethodSuite(method, parentSuite)
                : BuildSingleTestMethod(method, parentSuite, null);
#endif
        }

        #endregion

        #region Implementation

#if NYI
        /// <summary>
        /// Builds a ParameterizedMethodSuite containing individual
        /// test cases for each set of parameters provided for
        /// this method.
        /// </summary>
        /// <param name="method">The MethodInfo for which a test is to be built</param>
        /// <param name="parentSuite">The test suite for which the method is being built</param>
        /// <returns>A ParameterizedMethodSuite populated with test cases</returns>
        public Test BuildParameterizedMethodSuite(MethodInfo method, ITest parentSuite)
        {
            ParameterizedMethodSuite methodSuite = new ParameterizedMethodSuite(method);
            methodSuite.ApplyAttributesToTest(method);

            foreach (ITestCaseData testcase in _testCaseProvider.GetTestCasesFor(method))
            {
                ParameterSet parameterSet = testcase as ParameterSet;
                if (parameterSet == null)
                    parameterSet = new ParameterSet(testcase);

                TestMethod test = BuildSingleTestMethod(method, parentSuite, parameterSet);

                methodSuite.Add(test);
            }

            return methodSuite;
        }
#endif

        /// <summary>
        /// Builds a single NUnitTestMethod, either as a child of the fixture 
        /// or as one of a set of test cases under a ParameterizedTestMethodSuite.
        /// </summary>
        /// <param name="method">The MethodInfo from which to construct the TestMethod</param>
        /// <param name="parentSuite">The suite or fixture to which the new test will be added</param>
        /// <param name="parameterSet">The ParameterSet to be used, or null</param>
        /// <returns></returns>
        private TestMethod BuildSingleTestMethod(MethodInfo method, ITest parentSuite, ParameterSet parameterSet)
        {
            TestMethod testMethod = new TestMethod(method, parentSuite);

            testMethod.Seed = _randomizer.Next();

            string prefix = method.ReflectedType.FullName;

            // Needed to give proper fullname to test in a parameterized fixture.
            // Without this, the arguments to the fixture are not included.
            if (parentSuite != null)
            {
                prefix = parentSuite.FullName;
                //testMethod.FullName = prefix + "." + testMethod.Name;
            }

            if (CheckTestMethodSignature(testMethod, parameterSet))
            {
#if NYI
                if (parameterSet == null)
                    testMethod.ApplyAttributesToTest(method);

                foreach (ICommandDecorator decorator in method.GetCustomAttributes(typeof(ICommandDecorator), true))
                    testMethod.CustomDecorators.Add(decorator);
#endif
            }

            if (parameterSet != null)
            {
                // NOTE: After the call to CheckTestMethodSignature, the Method
                // property of testMethod may no longer be the same as the
                // original MethodInfo, so we reassign it here.
                method = testMethod.Method;

                if (parameterSet.TestName != null)
                {
                    testMethod.Name = parameterSet.TestName;
                    testMethod.FullName = prefix + "." + parameterSet.TestName;
                }
                else if (parameterSet.OriginalArguments != null)
                {
                    string name = MethodHelper.GetDisplayName(method, parameterSet.OriginalArguments);
                    testMethod.Name = name;
                    testMethod.FullName = prefix + "." + name;
                }

                parameterSet.ApplyToTest(testMethod);
            }

            return testMethod;
        }

		#endregion

        #region Helper Methods

        /// <summary>
        /// Helper method that checks the signature of a TestMethod and
        /// any supplied parameters to determine if the test is valid.
        /// 
        /// Currently, NUnitTestMethods are required to be public, 
        /// non-abstract methods, either static or instance,
        /// returning void. They may take arguments but the values must
        /// be provided or the TestMethod is not considered runnable.
        /// 
        /// Methods not meeting these criteria will be marked as
        /// non-runnable and the method will return false in that case.
        /// </summary>
        /// <param name="testMethod">The TestMethod to be checked. If it
        /// is found to be non-runnable, it will be modified.</param>
        /// <param name="parameterSet">Parameters to be used for this test, or null</param>
        /// <returns>True if the method signature is valid, false if not</returns>
        private static bool CheckTestMethodSignature(TestMethod testMethod, ParameterSet parameterSet)
		{
            if (testMethod.Method.IsAbstract)
            {
                return MarkAsNotRunnable(testMethod, "Method is abstract");
            }

            if (!testMethod.Method.IsPublic)
            {
                return MarkAsNotRunnable(testMethod, "Method is not public");
            }

#if NETCF
            // TODO: Get this to work
            if (testMethod.Method.IsGenericMethodDefinition)
            {
                return MarkAsNotRunnable(testMethod, "Generic test methods are not yet supported under .NET CF");
            }
#endif

            ParameterInfo[] parameters = testMethod.Method.GetParameters();
            int argsNeeded = parameters.Length;

            object[] arglist = null;
            int argsProvided = 0;

            if (parameterSet != null)
            {
                testMethod.TestCaseParameters = parameterSet;
                testMethod.RunState = parameterSet.RunState;

                arglist = parameterSet.Arguments;

                if (arglist != null)
                    argsProvided = arglist.Length;

                if (testMethod.RunState != RunState.Runnable)
                    return false;
            }

            Type returnType = testMethod.Method.ReturnType;
            if (returnType.Equals(typeof(void)))
            {
                if (parameterSet != null && parameterSet.HasExpectedResult)
                    return MarkAsNotRunnable(testMethod, "Method returning void cannot have an expected result");
            }
            else
            {
#if NET_4_5
                if (MethodHelper.IsAsyncMethod(testMethod.Method))
                {
                    bool returnsGenericTask = returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>);
                    if (returnsGenericTask && (parms == null|| !parms.HasExpectedResult && !parms.ExceptionExpected))
                        return MarkAsNotRunnable(testMethod, "Async test method must have Task or void return type when no result is expected");
                    else if (!returnsGenericTask && parms != null && parms.HasExpectedResult)
                        return MarkAsNotRunnable(testMethod, "Async test method must have Task<T> return type when a result is expected");
                }
                else 
#endif
                if (parameterSet == null || !parameterSet.HasExpectedResult)
                    return MarkAsNotRunnable(testMethod, "Method has non-void return value, but no result is expected");
            }

            if (argsProvided > 0 && argsNeeded == 0)
            {
                return MarkAsNotRunnable(testMethod, "Arguments provided for method not taking any");
            }

            if (argsProvided == 0 && argsNeeded > 0)
            {
                return MarkAsNotRunnable(testMethod, "No arguments were provided");
            }

            if (argsProvided != argsNeeded)
            {
                return MarkAsNotRunnable(testMethod, "Wrong number of arguments provided");
            }

#if !NETCF
            if (testMethod.Method.IsGenericMethodDefinition)
            {
                Type[] typeArguments = GetTypeArgumentsForMethod(testMethod.Method, arglist);
                foreach (object o in typeArguments)
                    if (o == null)
                    {
                        return MarkAsNotRunnable(testMethod, "Unable to determine type arguments for method");
                    }

                testMethod.Method = testMethod.Method.MakeGenericMethod(typeArguments);
                parameters = testMethod.Method.GetParameters();
           }
#endif

           if (arglist != null && parameters != null)
               TypeHelper.ConvertArgumentList(arglist, parameters);

            return true;
        }

#if !NETCF
        private static Type[] GetTypeArgumentsForMethod(MethodInfo method, object[] arglist)
        {
            Type[] typeParameters = method.GetGenericArguments();
            Type[] typeArguments = new Type[typeParameters.Length];
            ParameterInfo[] parameters = method.GetParameters();

            for (int typeIndex = 0; typeIndex < typeArguments.Length; typeIndex++)
            {
                Type typeParameter = typeParameters[typeIndex];

                for (int argIndex = 0; argIndex < parameters.Length; argIndex++)
                {
                    if (parameters[argIndex].ParameterType.Equals(typeParameter))
                        typeArguments[typeIndex] = TypeHelper.BestCommonType(
                            typeArguments[typeIndex],
                            arglist[argIndex].GetType());
                }
            }

            return typeArguments;
        }
#endif

        private static MethodInfo GetExceptionHandler(Type fixtureType, string name)
        {
            return fixtureType.GetMethod(
                name,
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(System.Exception) },
                null);
        }

        private static bool MarkAsNotRunnable(TestMethod testMethod, string reason)
        {
            testMethod.RunState = RunState.NotRunnable;
            testMethod.Properties.Set(PropertyNames.SkipReason, reason);
            return false;
        }

        #endregion
    }
}
