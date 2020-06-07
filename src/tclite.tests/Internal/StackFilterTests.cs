// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using NUnit.Framework;

namespace TCLite.Framework.Internal
{
    [TestFixture]
    public class StackFilterTests
    {
        #region Test Data

        private static readonly string NL = Environment.NewLine;

        // TODO: Replace with realistic stack examples
        
        private static readonly string shortTrace_Assert =
    @"   at TCLite.Framework.Assert.Fail(String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 56" + NL +
    @"   at TCLite.Framework.Assert.That(String label, Object actual, Matcher expectation, String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 50" + NL +
    @"   at TCLite.Framework.Assert.That(Object actual, Matcher expectation) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 19" + NL +
    @"   at TCLite.Tests.GreaterThanMatcherTest.MatchesGoodValue() in D:\Dev\NUnitLite\NUnitLiteTests\GreaterThanMatcherTest.cs:line 12" + NL;

        private static readonly string shortTrace_Assume =
    @"   at TCLite.Framework.Assert.Fail(String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 56" + NL +
    @"   at TCLite.Framework.Assume.That(String label, Object actual, Matcher expectation, String message) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 50" + NL +
    @"   at TCLite.Framework.Assume.That(Object actual, Matcher expectation) in D:\Dev\NUnitLite\NUnitLite\Framework\Assert.cs:line 19" + NL +
    @"   at TCLite.Tests.GreaterThanMatcherTest.MatchesGoodValue() in D:\Dev\NUnitLite\NUnitLiteTests\GreaterThanMatcherTest.cs:line 12" + NL;

        private static readonly string shortTrace_Result =
    @"   at TCLite.Tests.GreaterThanMatcherTest.MatchesGoodValue() in D:\Dev\NUnitLite\NUnitLiteTests\GreaterThanMatcherTest.cs:line 12" + NL;

        // NOTE: In most cases, NUnit does not have to deal with traces
        // like this because the InnerException of a TargetInvocationException
        // only includes the methods called from the point of invocation.
        // However, in the compact framework, such long traces may arise.
        private static readonly string longTrace =
    @"   at TCLite.Framework.Assert.Fail(String message, Object[] args)" + NL +
    @"   at MyNamespace.MyAppsTests.AssertFailTest()" + NL +
    @"   at System.Reflection.RuntimeMethodInfo.InternalInvoke(RuntimeMethodInfo rtmi, Object obj, BindingFlags invokeAttr, Binder binder, Object parameters, CultureInfo culture, Boolean isBinderDefault, Assembly caller, Boolean verifyAccess, StackCrawlMark& stackMark)" + NL +
    @"   at System.Reflection.RuntimeMethodInfo.InternalInvoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture, Boolean verifyAccess, StackCrawlMark& stackMark)" + NL +
    @"   at System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)" + NL +
    @"   at System.Reflection.MethodBase.Invoke(Object obj, Object[] parameters)" + NL +
    @"   at NUnitLite.ProxyTestCase.InvokeMethod(MethodInfo method, Object[] args)" + NL +
    @"   at TCLite.Framework.TestCase.RunTest()" + NL +
    @"   at TCLite.Framework.TestCase.RunBare()" + NL +
    @"   at TCLite.Framework.TestCase.Run(TestResult result, TestListener listener)" + NL +
    @"   at TCLite.Framework.TestCase.Run(TestListener listener)" + NL +
    @"   at TCLite.Framework.TestSuite.Run(TestListener listener)" + NL +
    @"   at TCLite.Framework.TestSuite.Run(TestListener listener)" + NL +
    @"   at NUnitLite.Runner.TestRunner.Run(ITest test)" + NL +
    @"   at NUnitLite.Runner.ConsoleUI.Run(ITest test)" + NL +
    @"   at NUnitLite.Runner.TestRunner.Run(Assembly assembly)" + NL +
    @"   at NUnitLite.Runner.ConsoleUI.Run()" + NL +
    @"   at NUnitLite.Runner.ConsoleUI.Main(String[] args)" + NL +
    @"   at OpenNETCF.Linq.Demo.Program.Main(String[] args)" + NL;

        private static readonly string longTrace_Result =
    @"   at MyNamespace.MyAppsTests.AssertFailTest()" + NL;

        #endregion

        // NOTE: Using individual tests rather than test cases 
        // to make the error messages easier to read.

        [NUnit.Framework.Test]
        public void FilterShortTraceWithAssert()
        {
            Assert.That(StackFilter.Filter(shortTrace_Assert), Is.EqualTo(shortTrace_Result));
        }

        [NUnit.Framework.Test]
        public void FilterShortTraceWithAssume_Trace1()
        {
            Assert.That(StackFilter.Filter(shortTrace_Assume), Is.EqualTo(shortTrace_Result));
        }

        [NUnit.Framework.Test]
        public void FilterLongTrace()
        {
            Assert.That(StackFilter.Filter(longTrace), Is.EqualTo(longTrace_Result));
        }
    }
}
