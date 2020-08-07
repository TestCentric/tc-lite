// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Text.RegularExpressions;

namespace TCLite
{
    using Interfaces;
    using Commands;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExpectedResultAttribute :TCLiteAttribute, ICommandWrapper
    {
        protected ResultState _expectedResult;
        protected string[] _messageFragments;

        public ExpectedResultAttribute(ResultState resultState, string[] messageFragments)
        {
            _expectedResult = resultState;
            _messageFragments = messageFragments;
        }

        public TestCommand Wrap(TestCommand command)
        {
            return new ExpectedResultCommand(command, _expectedResult, _messageFragments);
        }

        class ExpectedResultCommand : AfterTestCommand
        {
            public ExpectedResultCommand(TestCommand command, ResultState expectedResult, params string[] messageFragments)
                : base(command)
            {
                AfterTest = (context) =>
                {
                    var result = context.CurrentResult;

                    if (result.ResultState != expectedResult)
                    {
                        result.SetResult(ResultState.Failure, 
                            $"Expected {expectedResult} result but was {result.ResultState}.");
                        return;
                    }

                    if (messageFragments.Length >0)
                    {
                        foreach (string fragment in messageFragments)
                            if (!result.Message.Contains(fragment))
                            {
                                result.SetResult(ResultState.Failure,
                                    $"Warning message did not match\nExpected substring: {fragment}\nBut was:  {result.Message}");
                                return;
                            }
                    }

                    result.SetResult(ResultState.Success);
                    result.AssertionResults.Clear();
                };
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExpectFailureAttribute : ExpectedResultAttribute, ICommandWrapper
    {
        public ExpectFailureAttribute(params string[] messageFragments)
            : base(ResultState.Failure, messageFragments) { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExpectWarningAttribute : ExpectedResultAttribute, ICommandWrapper
    {
        public ExpectWarningAttribute(params string[] messageFragments)
            : base(ResultState.Warning, messageFragments) { }
    }
}
