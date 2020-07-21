// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections.Generic;
using System.Reflection;
using TCLite.Framework.Internal; // TODO: We shouldn't reference this in the interface

namespace TCLite.Framework.Interfaces
{
    /// <summary>
    /// The ITestBuilder interface is exposed by a class that knows how to
    /// build tests from a specified method. In general, it is exposed
    /// by an attribute which has additional information available to provide
    /// the necessary test parameters to distinguish the test cases built.
    /// </summary>
    public interface ITestBuilder
    {
        /// <summary>
        /// Builds any number of tests from the specified method and context.
        /// </summary>
        /// <param name="method">The method to be used as a test</param>
        /// <param name="suite">The TestSuite to which the method will be added</param>
        IEnumerable<Test> BuildFrom(MethodInfo method, ITest suite);
    }
}
