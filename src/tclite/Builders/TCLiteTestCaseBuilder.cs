// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Interfaces;
using TCLite.Framework.Commands;
using TCLite.Framework.Internal;

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
    public class TCLiteTestCaseBuilder : ITestCaseBuilder
	{
        private Randomizer _randomizer;

        /// <summary>
        /// Default no argument constructor for NUnitTestCaseBuilder
        /// </summary>
        public TCLiteTestCaseBuilder()
        {
            _randomizer = Randomizer.CreateRandomizer();
        }

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
        /// <param name="parentSuite">The test fixture being populated, or null</param>
        /// <returns>A Test representing one or more method invocations</returns>
        public IEnumerable<Test> BuildFrom(MethodInfo method, ITest parentSuite)
        {
            List<TestMethod> testCases = new List<TestMethod>();
            var name = method.Name; // For Debugging
            foreach (ITestCaseSource source in method.GetCustomAttributes(typeof(ITestCaseSource), false))
                foreach (ITestCaseData testCase in source.GetTestCasesFor(method))
                {
                    var parameterSet = testCase as ParameterSet;
                    if (parameterSet == null)
                        parameterSet = new ParameterSet(testCase);

                    testCases.Add(BuildTestMethod(method, parentSuite, parameterSet));
                }

            if (method.GetParameters().Length == 0)
                return testCases;

            ParameterizedMethodSuite methodSuite = new ParameterizedMethodSuite(method);
            methodSuite.ApplyAttributesToTest(method);

            foreach (TestMethod testCase in testCases)
                methodSuite.Add(testCase);

            return new [] { methodSuite };
        }

        #region Helper Methods

        /// <summary>
        /// Builds a single TestMethod
        /// </summary>
        /// <param name="method">The MethodInfo from which to construct the TestMethod</param>
        /// <param name="parentSuite">The suite or fixture to which the new test will be added</param>
        /// <param name="parameterSet">The ParameterSet to be used, or null</param>
        /// <returns></returns>
        private TestMethod BuildTestMethod(MethodInfo method, ITest parentSuite, ParameterSet parameterSet)
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
                testMethod.ApplyAttributesToTest(method.ReflectedType);
                testMethod.ApplyAttributesToTest(method);

                foreach (ICommandWrapper decorator in method.GetCustomAttributes(typeof(ICommandWrapper), true))
                    testMethod.CustomDecorators.Add(decorator);
            }

            // NOTE: In the case of a generic method, testMethod.Method
            // may be changed in the call to CheckTestMethodSignature.
            // This is just -in case some future change tries to use it.
            method = testMethod.Method;

            if (parameterSet != null)
            {
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

            if (parentSuite.RunState == RunState.NotRunnable || parentSuite.RunState == RunState.Skipped)
            {
                testMethod.RunState = parentSuite.RunState;
                var reasonKey = PropertyNames.SkipReason;
                testMethod.Properties.Set(reasonKey, parentSuite.Properties.Get(reasonKey));
            }

            return testMethod;
        }

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
#if NYI // async
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

        // TODO: Wait to see if we really need this. If not, remove both the call and the method.
        //    if (arglist != null && parameters != null)
        //        TypeHelper.ConvertArgumentList(arglist, parameters);

            return true;
        }

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
