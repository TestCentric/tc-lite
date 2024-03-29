// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Reflection;

namespace TCLite.Constraints
{
    /// <summary>
    /// ThrowsConstraint is used to test the exception thrown by 
    /// a delegate by applying a constraint to it.
    /// </summary>
    public class ThrowsConstraint : PrefixConstraint
    {
        private Exception _caughtException;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsConstraint"/> class,
        /// using a constraint to be applied to the exception.
        /// </summary>
        /// <param name="baseConstraint">A constraint to apply to the caught exception.</param>
        public ThrowsConstraint(IConstraint baseConstraint)
            : base(baseConstraint) { }

        /// <summary>
        /// Get the actual exception thrown - used by Assert.Throws.
        /// </summary>
        public Exception ActualException
        {
            get { return _caughtException; }
        }

        #region Constraint Overrides

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            Guard.ArgumentOfType<Delegate>(actual, nameof(actual));
        }

        /// <summary>
        /// Executes the code of the delegate and captures any exception.
        /// If a non-null base constraint was provided, it applies that
        /// constraint to the exception.
        /// </summary>
        /// <param name="actual">A delegate representing the code to be tested</param>
        /// <returns>True if an exception is thrown and the constraint succeeds, otherwise false</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            try
            {
                (actual as Delegate).DynamicInvoke();
            }
            catch(TargetInvocationException ex)
            {
                _caughtException = ex.InnerException;
            }
            catch(Exception ex)
            {
                _caughtException = ex;
            }

            return new ThrowsConstraintResult(
                this,
                _caughtException,
                _caughtException != null
                    ? BaseConstraint.ApplyTo(_caughtException)
                    : null);
        }

        /// <summary>
        /// Converts an ActualValueDelegate to a TestDelegate
        /// before calling the primary overload.
        /// </summary>
        /// <param name="del"></param>
        /// <returns></returns>
        protected override ConstraintResult ApplyConstraint<T>(ActualValueDelegate<T> del)
        {
            return ApplyConstraint((Delegate)del);
        }

        #endregion

        /// <summary>
        /// Returns the string representation of this constraint
        /// </summary>
        protected override string GetStringRepresentation()
        {
            if (BaseConstraint == null)
                return "<throws>";

            return base.GetStringRepresentation();
        }
    }

    #region ExceptionInterceptor

    internal class ExceptionInterceptor
    {
        private ExceptionInterceptor() { }

        internal static Exception Intercept(object invocation)
        {
            IInvocationDescriptor invocationDescriptor = GetInvocationDescriptor(invocation);

#if NYI // async
            if (AsyncInvocationRegion.IsAsyncOperation(invocationDescriptor.Delegate))
            {
                using (AsyncInvocationRegion region = AsyncInvocationRegion.Create(invocationDescriptor.Delegate))
                {
                    object result = invocationDescriptor.Invoke();

                    try
                    {
                        region.WaitForPendingOperationsToComplete(result);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        return ex;
                    }
                }
            }
            else
#endif
            {
                try
                {
                    invocationDescriptor.Invoke();
                    return null;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
        }

        private static IInvocationDescriptor GetInvocationDescriptor(object actual)
        {
            IInvocationDescriptor invocationDescriptor = actual as IInvocationDescriptor;

            if (invocationDescriptor == null)
            {
                TestDelegate testDelegate = actual as TestDelegate;

                if (testDelegate == null)
                    throw new ArgumentException(
                        String.Format("The actual value must be a TestDelegate or ActualValueDelegate but was {0}", actual.GetType().Name),
                        "actual");

                invocationDescriptor = new VoidInvocationDescriptor(testDelegate);
            }

            return invocationDescriptor;
        }
    }

    #endregion

    #region InvocationDescriptor

    internal class VoidInvocationDescriptor : IInvocationDescriptor
    {
        private readonly TestDelegate _del;

        public VoidInvocationDescriptor(TestDelegate del)
        {
            _del = del;
        }

        public object Invoke()
        {
            _del();
            return null;
        }

        public Delegate Delegate
        {
            get { return _del; }
        }
    }

    internal class GenericInvocationDescriptor<T> : IInvocationDescriptor
    {
        private readonly ActualValueDelegate<T> _del;

        public GenericInvocationDescriptor(ActualValueDelegate<T> del)
        {
            _del = del;
        }

        public object Invoke()
        {
            return _del();
        }

        public Delegate Delegate
        {
            get { return _del; }
        }
    }

    internal interface IInvocationDescriptor
    {
        object Invoke();
        Delegate Delegate { get; }
    }

    #endregion
}
