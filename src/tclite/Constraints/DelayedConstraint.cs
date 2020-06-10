// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Threading;

namespace TCLite.Framework.Constraints
{
    ///<summary>
    /// Applies a delay to the match so that a match can be evaluated in the future.
    ///</summary>
    public class DelayedConstraint : PrefixConstraint
    {
        private readonly int _delayInMilliseconds;
        private readonly int _pollingInterval;

        ///<summary>
        /// Creates a new DelayedConstraint
        ///</summary>
        ///<param name="baseConstraint">The inner constraint two decorate</param>
        ///<param name="delayInMilliseconds">The time interval after which the match is performed</param>
        ///<exception cref="InvalidOperationException">If the value of <paramref name="delayInMilliseconds"/> is less than 0</exception>
        public DelayedConstraint(Constraint baseConstraint, int delayInMilliseconds)
            : this(baseConstraint, delayInMilliseconds, 0) { }

        ///<summary>
        /// Creates a new DelayedConstraint
        ///</summary>
        ///<param name="baseConstraint">The inner constraint two decorate</param>
        ///<param name="delayInMilliseconds">The time interval after which the match is performed</param>
        ///<param name="pollingInterval">The time interval used for polling</param>
        ///<exception cref="InvalidOperationException">If the value of <paramref name="delayInMilliseconds"/> is less than 0</exception>
        public DelayedConstraint(Constraint baseConstraint, int delayInMilliseconds, int pollingInterval)
            : base(baseConstraint)
        {
            if (delayInMilliseconds < 0)
                throw new ArgumentException("Cannot check a condition in the past", "delayInMilliseconds");

            _delayInMilliseconds = delayInMilliseconds;
            _pollingInterval = pollingInterval;
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for if the base constraint fails, false if it succeeds</returns>
        public override bool Matches<T>(T actual)
        {
            int remainingDelay = _delayInMilliseconds;

            while (_pollingInterval > 0 && _pollingInterval < remainingDelay)
            {
                remainingDelay -= _pollingInterval;
                Thread.Sleep(_pollingInterval);
                ActualValue = actual;
                if (BaseConstraint.Matches(actual))
                    return true;
            }

            if (remainingDelay > 0)
                Thread.Sleep(remainingDelay);
            ActualValue = actual;
            return BaseConstraint.Matches(actual);
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a delegate
        /// </summary>
        /// <param name="del">The delegate whose value is to be tested</param>
        /// <returns>True for if the base constraint fails, false if it succeeds</returns>
        public override bool Matches<T>(ActualValueDelegate<T> del)
        {
            int remainingDelay = _delayInMilliseconds;

            while (_pollingInterval > 0 && _pollingInterval < remainingDelay)
            {
                remainingDelay -= _pollingInterval;
                Thread.Sleep(_pollingInterval);
                ActualValue = InvokeDelegate(del);
				
				try
				{
	                if (BaseConstraint.Matches(ActualValue))
	                    return true;
				}
				catch
				{
					// Ignore any exceptions when polling
				}
            }

            if (remainingDelay > 0)
                Thread.Sleep(remainingDelay);
            ActualValue = InvokeDelegate(del);
            return BaseConstraint.Matches(ActualValue);
        }

        private static object InvokeDelegate<T>(ActualValueDelegate<T> del)
        {
#if NYI
            if (AsyncInvocationRegion.IsAsyncOperation(del))
                using (AsyncInvocationRegion region = AsyncInvocationRegion.Create(del))
                    return region.WaitForPendingOperationsToComplete(del());
#endif

            return del();
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given reference.
        /// Overridden to wait for the specified delay period before
        /// calling the base constraint with the dereferenced value.
        /// </summary>
        /// <param name="actual">A reference to the value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override bool Matches<T>(ref T actual)
        {
            int remainingDelay = _delayInMilliseconds;

            while (_pollingInterval > 0 && _pollingInterval < remainingDelay)
            {
                remainingDelay -= _pollingInterval;
                Thread.Sleep(_pollingInterval);
                ActualValue = actual;

                try
                {
                    if (BaseConstraint.Matches(actual))
                        return true;
                }
                catch (Exception)
                {
                    // Ignore any exceptions when polling
                }
            }

            if (remainingDelay > 0)
                Thread.Sleep(remainingDelay);
            ActualValue = actual;
            return BaseConstraint.Matches(actual);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            BaseConstraint.WriteDescriptionTo(writer);
            writer.Write(string.Format(" after {0} millisecond delay", _delayInMilliseconds));
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a MessageWriter.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            BaseConstraint.WriteActualValueTo(writer);
        }

        /// <summary>
        /// Returns the string representation of the constraint.
        /// </summary>
        protected override string GetStringRepresentation()
        {
            return string.Format("<after {0} {1}>", _delayInMilliseconds, BaseConstraint);
        }
    }
}
