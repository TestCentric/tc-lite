// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// IImplyFixture is an empty marker interface used by attributes like
    /// TestAttribute that cause the class where they are used to be treated
    /// as a TestFixture even without a TestFixtureAttribute.
    /// 
    /// Marker interfaces are not usually considered a good practice, but
    /// we use it here to avoid cluttering the attribute hierarchy with 
    /// classes that don't contain any extra implementation.
    /// </summary>
    public interface IImplyFixture
    {
    }
}
