// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Commands
{
    /// <summary>
    /// The CommandStage enumeration represents the defined stages
    /// of execution for a series of TestCommands. The int values
    /// of the enum are used to apply decorators in the proper 
    /// order. Lower values are applied first and are therefore
    /// "closer" to the actual test execution.
    /// </summary>
    /// <remarks>
    /// No CommandStage is defined for actual invocation of the test or
    /// for creation of the context. Execution may be imagined as 
    /// proceeding from the bottom of the list upwards, with cleanup
    /// after the test running in the opposite order.
    /// </remarks>
    public enum CommandStage
    {
        /// <summary>
        /// Use an application-defined default value.
        /// </summary>
        Default,

        // NOTE: The test is actually invoked here.

        /// <summary>
        /// Make adjustments needed before and after running
        /// the raw test - that is, after any SetUp has run
        /// and before TearDown.
        /// </summary>
        BelowSetUpTearDown,

        /// <summary>
        /// Run SetUp and TearDown for the test.  This stage is used
        /// internally by NUnit and should not normally appear
        /// in user-defined decorators.
        /// </summary>
        SetUpTearDown,

        /// <summary>
        /// Make adjustments needed before and after running 
        /// the entire test - including SetUp and TearDown.
        /// </summary>
        AboveSetUpTearDown

        // Note: The context is created here and destroyed
        // after the test has run.

        // Command Stages
        //   Create/Destroy Context
        //   Modify/Restore Context
        //   Create/Destroy fixture object
        //   Repeat test
        //   Create/Destroy thread
        //   Modify overall result
        //   SetUp/TearDown
        //   Modify raw result
    }
}
