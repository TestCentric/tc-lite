// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// The Constraint class is the base of all built-in constraints
    /// within TCLite. The class models a constraint, which puts no
    /// limitations on the type of actual value it is able to handle.
    /// </summary>
    public abstract class Constraint : IConstraint, IResolveConstraint
    {
        #region Constructor

        /// <summary>
        /// Construct a constraint with optional arguments
        /// </summary>
        /// <param name="args">Arguments to be saved</param>
        protected Constraint(params object[] args)
        {
            Arguments = args;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The display name of this Constraint for use by ToString().
        /// The default value is the name of the constraint with
        /// trailing "Constraint" removed. Derived classes may set
        /// this to another name in their constructors.
        /// </summary>
        public virtual string DisplayName
        {
            get
            {
                if (_displayName == null)
                {
                    _displayName = GetType().Name.ToLower();
                    if (_displayName.EndsWith("`1") || _displayName.EndsWith("`2"))
                        _displayName = _displayName.Substring(0, _displayName.Length - 2);
                    if (_displayName.EndsWith("constraint"))
                        _displayName = _displayName.Substring(0, _displayName.Length - 10);
                }

                return _displayName;
            }

            set { _displayName = value; }
        }
        private string _displayName;

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public abstract string Description { get; } 

        /// <summary>
        /// Arguments provided to this Constraint, for use in
        /// formatting the description.
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// A ConstraintBuilder holding this constraint. If not null
        /// the constraint is under construction.
        /// </summary>
        public ConstraintBuilder Builder { get; set; }

        #endregion

        #region IResolveConstraint

        bool IResolveConstraint.IsResolvable => Builder == null || Builder.IsResolvable;

        Constraint IResolveConstraint.Resolve()
        {
            if (Builder == null)
                return this;

            if (!Builder.IsResolvable)
                throw new InvalidOperationException("Attempted to resolve a Constraint under construction");

            return Builder.Resolve();
        }

        #endregion
        
        #region Abstract and Virtual Methods

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public abstract ConstraintResult ApplyTo<T>(T actual);

        /// <summary>
        /// Applies the constraint to an ActualValueDelegate that returns
        /// the value to be tested. The default implementation simply evaluates
        /// the delegate but derived classes may override it to provide for
        /// delayed processing.
        /// </summary>
        /// <param name="del">An ActualValueDelegate</param>
        /// <returns>A ConstraintResult</returns>
        public virtual ConstraintResult ApplyTo<T>(ActualValueDelegate<T> del)
        {
#if NYI // async
            if (AsyncToSyncAdapter.IsAsyncOperation(del))
                return ApplyTo(AsyncToSyncAdapter.Await(() => del.Invoke()));
#endif

            return ApplyTo(del());
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given reference.
        /// The default implementation simply dereferences the value but
        /// derived classes may override it to provide for delayed processing.
        /// </summary>
        /// <param name="actual">A reference to the value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public virtual ConstraintResult ApplyTo<T>(ref T actual)
        {
            return ApplyTo(actual);
        }

        #endregion

        #region ToString Override

        /// <summary>
        /// Default override of ToString returns the constraint DisplayName
        /// followed by any arguments within angle brackets.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string rep = GetStringRepresentation();

            return Builder == null ? rep : $"<unresolved {rep}>";
        }

        /// <summary>
        /// Returns the string representation of this constraint
        /// </summary>
        protected virtual string GetStringRepresentation()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<");
            sb.Append(DisplayName.ToLower());

            foreach (object arg in Arguments)
            {
                sb.Append(" ");
                sb.Append(_displayable(arg));
            }

            sb.Append(">");

            return sb.ToString();
        }

        private static string _displayable(object o)
        {
            if (o == null) return "null";

            string fmt = o is string ? "\"{0}\"" : "{0}";
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, fmt, o);
        }

        #endregion

        #region Binary Operators

        /// <summary>
        /// Returns a ConstraintExpression by appending And
        /// to the current constraint.
        /// </summary>
        public ConstraintExpression And
        {
            get
            {
                if (Builder == null)
                {
                    Builder = new ConstraintBuilder();
                    Builder.Append(this);
                }

                Builder.Append(new AndOperator());

                return new ConstraintExpression(Builder);
            }
        }

        /// <summary>
        /// Returns a ConstraintExpression by appending And
        /// to the current constraint.
        /// </summary>
        public ConstraintExpression With
        {
            get { return And; }
        }

        /// <summary>
        /// Returns a ConstraintExpression by appending Or
        /// to the current constraint.
        /// </summary>
        public ConstraintExpression Or
        {
            get
            {
                if (Builder == null)
                {
                    Builder = new ConstraintBuilder();
                    Builder.Append(this);
                }

                Builder.Append(new OrOperator());

                return new ConstraintExpression(Builder);
            }
        }

        #endregion

        #region After Modifier
#if NYI // DelayedConstraint
        /// <summary>
        /// Returns a DelayedConstraint with the specified delay time.
        /// </summary>
        /// <param name="delayInMilliseconds">The delay in milliseconds.</param>
        /// <returns></returns>
        public DelayedConstraint After(int delayInMilliseconds)
        {
            return new DelayedConstraint(
                Builder == null ? this : Builder.Resolve(),
                delayInMilliseconds);
        }

        /// <summary>
        /// Returns a DelayedConstraint with the specified delay time
        /// and polling interval.
        /// </summary>
        /// <param name="delayInMilliseconds">The delay in milliseconds.</param>
        /// <param name="pollingInterval">The interval at which to test the constraint.</param>
        /// <returns></returns>
        public DelayedConstraint After(int delayInMilliseconds, int pollingInterval)
        {
            return new DelayedConstraint(
                Builder == null ? this : Builder.Resolve(),
                delayInMilliseconds,
                pollingInterval);
        }
#endif

        #endregion
    }

    /// <summary>
    /// The Constraint<TActual> class is derived from non-generic
    /// Constraint and class models a constraint, which requires
    /// the actual values supplied to be of a certain Type.
    /// </summary>
    public abstract class Constraint<TActual> : Constraint
    {
        public Constraint(params object[] args) : base(args) { }

        /// <summary>
        /// Applies the constraint to an actual value, of the same type as
        /// the constraint expected value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public abstract ConstraintResult ApplyTo(TActual actual);

//         /// <summary>
//         /// Applies the constraint to an ActualValueDelegate that returns
//         /// the value to be tested. The default implementation simply evaluates
//         /// the delegate but derived classes may override it to provide for
//         /// delayed processing.
//         /// </summary>
//         /// <param name="del">An ActualValueDelegate</param>
//         /// <returns>A ConstraintResult</returns>
//         public virtual ConstraintResult ApplyTo(ActualValueDelegate<TActual> del)
//         {
// #if NYI // async
//             if (AsyncToSyncAdapter.IsAsyncOperation(del))
//                 return ApplyTo(AsyncToSyncAdapter.Await(() => del.Invoke()));
// #endif

//             return ApplyTo(del());
//         }

        // /// <summary>
        // /// Test whether the constraint is satisfied by a given reference.
        // /// The default implementation simply dereferences the value but
        // /// derived classes may override it to provide for delayed processing.
        // /// </summary>
        // /// <param name="actual">A reference to the value to be tested</param>
        // /// <returns>A ConstraintResult</returns>
        // public virtual ConstraintResult ApplyTo(ref TActual actual)
        // {
        //     return ApplyTo(actual);
        // }
    }

    public abstract class ExpectedValueConstraint<TExpected> : Constraint
    {
        public TExpected ExpectedValue;

        public ExpectedValueConstraint(TExpected expected)
            : base(expected)
        {
            ExpectedValue = expected;
        }
    }
}