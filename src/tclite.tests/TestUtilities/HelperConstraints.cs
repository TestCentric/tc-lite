// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using TCLite.Constraints;
using TCLite.Internal;

namespace TCLite
{
    public sealed class HelperConstraints
    {
        public static Constraint HasMaxTime(int milliseconds)
        {
            return new HasMaxTimeConstraint(milliseconds);
        }

        private sealed class HasMaxTimeConstraint : Constraint
        {
            private readonly int _milliseconds;

            public HasMaxTimeConstraint(int milliseconds)
            {
                _milliseconds = milliseconds;
            }

            public override string Description => "max time";

            public override void ValidateActualValue(object actual)
            {
                Guard.ArgumentNotNull(actual, nameof(actual));
                Guard.ArgumentOfType<Delegate>(actual, nameof(actual));
            }

            protected override ConstraintResult ApplyConstraint<TActual>(TActual actual)
            {
                var del = actual as Delegate;
                // var invokeMethod = del.GetType().GetTypeInfo().GetMethod("Invoke");
                // if (invokeMethod.GetParameters().Length != 0)
                //     throw new ArgumentException("Delegate must be parameterless.", nameof(actual));

                var stopwatch = new System.Diagnostics.Stopwatch();

#if NYI // async
                if (AsyncToSyncAdapter.IsAsyncOperation(@delegate))
                {
                    stopwatch.Start();
                    AsyncToSyncAdapter.Await(() => @delegate.DynamicInvoke());
                    stopwatch.Stop();
                }
                else
                {
#endif
                    stopwatch.Start();
                    del.DynamicInvoke();
                    stopwatch.Stop();
                //}

                return new Result(this, stopwatch.ElapsedMilliseconds);
            }

            private sealed class Result : ConstraintResult
            {
                private readonly HasMaxTimeConstraint _constraint;

                public Result(HasMaxTimeConstraint constraint, long actualMilliseconds)
                    : base(constraint, actualMilliseconds, actualMilliseconds <= constraint._milliseconds)
                {
                    _constraint = constraint;
                }

                public override void WriteMessageTo(MessageWriter writer)
                {
                    writer.Write($"Elapsed time of {ActualValue}ms exceeds maximum of {_constraint._milliseconds}ms");
                }
            }
        }
    }
}
