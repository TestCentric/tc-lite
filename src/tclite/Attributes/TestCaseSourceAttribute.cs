// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// TestCaseSourceAttribute indicates the source to be used to
    /// provide test cases for a test method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TestCaseSourceAttribute : TCLiteAttribute, ITestCaseSource, IImplyFixture
    {
        /// <summary>
        /// Construct with the name of the method, property or field that will prvide data
        /// </summary>
        /// <param name="sourceName">The name of the method, property or field that will provide data</param>
        public TestCaseSourceAttribute(string sourceName)
        {
            SourceName = sourceName;
        }

        /// <summary>
        /// Construct with a Type and name
        /// </summary>
        /// <param name="sourceType">The Type that will provide data</param>
        /// <param name="sourceName">The name of the method, property or field that will provide data</param>
        public TestCaseSourceAttribute(Type sourceType, string sourceName)
        {
            SourceType = sourceType;
            SourceName = sourceName;
        }
        
        /// <summary>
        /// Construct with a Type
        /// </summary>
        /// <param name="sourceType">The type that will provide data</param>
        public TestCaseSourceAttribute(Type sourceType)
        {
            SourceType = sourceType;
        }

        /// <summary>
        /// The name of a the method, property or fiend to be used as a source
        /// </summary>
        public string SourceName { get; }

        /// <summary>
        /// A Type to be used as a source
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Gets or sets the category associated with this test.
        /// May be a single category or a comma-separated list.
        /// </summary>
        public string Category { get; set; }

        #region ITestCaseSource Members
        /// <summary>
        /// Returns a set of ITestCaseDataItems for use as arguments
        /// to a parameterized test method.
        /// </summary>
        /// <param name="method">The method for which data is needed.</param>
        /// <returns></returns>
        public IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method)
        {
            List<ITestCaseData> data = new List<ITestCaseData>();
            IEnumerable source = GetTestCaseSource(method);

            if (source != null)
            {
                ParameterInfo[] parameters = method.GetParameters();

                foreach (object item in source)
                {
                    TestCaseParameters parms = new TestCaseParameters();
                    ITestCaseData testCaseData = item as ITestCaseData;
					
					if (testCaseData != null)
						parms = new TestCaseParameters(testCaseData);
                    else if (item is object[])
                    {
                        object[] array = item as object[];
                        parms.Arguments = array.Length == parameters.Length
                            ? array
                            : new object[] { item };
                    }
                    // else if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(item.GetType()))
                    // {
                    //    parms.Arguments = new object[] { item };
                    // }
                    else if (item is Array)
                    {
                        Array array = item as Array;

                        if (array.Rank == 1 && array.Length == parameters.Length)
                        {
                            parms.Arguments = new object[array.Length];
                            for (int i = 0; i < array.Length; i++)
                                parms.Arguments[i] = (object)array.GetValue(i);
                        }
                        else
                        {
                            parms.Arguments = new object[] { item };
                        }
                    }
                    else
                    {
                        parms.Arguments = new object[] { item };
                    }

                    if (this.Category != null)
                        foreach (string cat in this.Category.Split(new char[] { ',' }))
                            parms.Properties.Add(PropertyNames.Category, cat);

                    data.Add(parms);
                }
            }

            return data;
        }

        private IEnumerable GetTestCaseSource(MethodInfo method)
        {
            IEnumerable source = null;

            Type sourceType = SourceType ?? method.ReflectedType;

            if (SourceName == null)
            {
                return Reflect.Construct(sourceType) as IEnumerable;
            }

            MemberInfo[] members = sourceType.GetMember(SourceName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (members.Length >= 1)
            {
                MemberInfo member = members[0];
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        FieldInfo field = member as FieldInfo;
                        return field.IsStatic
                            ? (IEnumerable)field.GetValue(null)
                            : SourceMustBeStaticError();
                    case MemberTypes.Property:
                        PropertyInfo property = member as PropertyInfo;
                        return property.GetMethod.IsStatic
                            ? (IEnumerable)property.GetValue(null, null)
                            : SourceMustBeStaticError();
                    case MemberTypes.Method:
                        MethodInfo m = member as MethodInfo;
                        return m.IsStatic
                            ? (IEnumerable)m.Invoke(null, null)
                            : SourceMustBeStaticError();
                }
            }

            return source;
        }

        private static IEnumerable SourceMustBeStaticError()
        {
            var parms = new TestCaseParameters();
            parms.RunState = RunState.NotRunnable;
            parms.Properties.Set(
                PropertyNames.SkipReason,
                "The sourceName specified on a TestCaseSourceAttribute must refer to a static field, property or method.");
            return new TestCaseParameters[] { parms };
        }

        #endregion

    }
}
