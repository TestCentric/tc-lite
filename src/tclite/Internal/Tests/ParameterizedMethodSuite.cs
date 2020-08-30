// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Reflection;
using TCLite.Commands;

namespace TCLite.Internal
{
    /// <summary>
    /// ParameterizedMethodSuite holds a collection of individual
    /// TestMethods with their arguments applied.
    /// </summary>
    internal class ParameterizedMethodSuite : TestSuite
    {
#if NYI // Theory
        private bool _isTheory;
#endif
        /// <summary>
        /// Construct from a MethodInfo
        /// </summary>
        /// <param name="method"></param>
        public ParameterizedMethodSuite(MethodInfo method)
            : base(method.ReflectedType.FullName, method.Name)
        {
            Method = method;
#if NYI // Theory
            _isTheory = method.IsDefined(typeof(TheoryAttribute), true);
#endif
        }

        /// <summary>
        /// Gets a string representing the type of test
        /// </summary>
        /// <value></value>
        public override string TestType
        {
            get
            {
#if NYI // Theory
                if (_isTheory)
                    return "Theory";
#endif
                if (Method.ContainsGenericParameters)
                    return "GenericMethod";
                
                return "ParameterizedMethod";
            }
        }
    }
}
