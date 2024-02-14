// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;

namespace TCLite.Constraints
{
    /// <summary>
    /// Delegate used to delay evaluation of the actual value
    /// to be used in evaluating a constraint
    /// </summary>
    public delegate T ActualValueDelegate<T>();
}