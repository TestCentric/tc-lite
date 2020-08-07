// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Interfaces
{
    /// <summary>
    /// The ITestCaseData interface is implemented by a class
    /// that is able to return the data required to create an
    /// instance of a parameterized test fixture.
    /// </summary>
    public interface ITestFixtureData : ITestData
    {
        /// <summary>
        /// Get the TypeArgs if separately set
        /// </summary>
        Type[] TypeArgs { get;  }
    }
}
