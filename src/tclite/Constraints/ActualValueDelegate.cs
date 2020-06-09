// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System.Collections;
//using TCLite.Framework.Internal;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Delegate used to delay evaluation of the actual value
    /// to be used in evaluating a constraint
    /// </summary>
    public delegate T ActualValueDelegate<T>();
}