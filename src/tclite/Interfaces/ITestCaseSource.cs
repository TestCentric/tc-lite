// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;

namespace TCLite.Interfaces
{
    /// <summary>
    /// ITestCaseSource interface is implemented by Types that know how to 
    /// return a set of ITestCaseData items for use by a test method.
    /// </summary>
    /// <remarks>
    /// This method is defined differently depending on the version of .NET.
    /// </remarks>
    public interface ITestCaseSource
    {
        /// <summary>
        /// Returns a set of ITestCaseDataItems for use as arguments
        /// to a parameterized test method.
        /// </summary>
        /// <param name="method">The method for which data is needed.</param>
        /// <returns></returns>
        IEnumerable<ITestCaseData> GetTestCasesFor(MethodInfo method);
    }
}
