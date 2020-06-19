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
    /// within TCLite. It provides the operator overloads used to combine 
    /// constraints.
    /// </summary>
    public abstract class Constraint : IConstraint
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
        /// The builder holding this constraint
        /// </summary>
        public ConstraintBuilder Builder { get; set; }

        #endregion

        #region IResolveConstraint

        Constraint IResolveConstraint.Resolve()
        {
            return Builder == null ? this : Builder.Resolve();
        }

        #endregion
        
        #region Abstract and Virtual Methods

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public abstract ConstraintResult ApplyTo<TActual>(TActual actual);

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// This overload will be selected when (1) the passed value is cast as an 
        /// object or (2) the argument is null and no Type parameter is specified.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public abstract ConstraintResult ApplyTo(object actual);

        /// <summary>
        /// Applies the constraint to an ActualValueDelegate that returns
        /// the value to be tested. The default implementation simply evaluates
        /// the delegate but derived classes may override it to provide for
        /// delayed processing.
        /// </summary>
        /// <param name="del">An ActualValueDelegate</param>
        /// <returns>A ConstraintResult</returns>
        public virtual ConstraintResult ApplyTo<TActual>(ActualValueDelegate<TActual> del)
        {
#if NYI // async
            if (AsyncToSyncAdapter.IsAsyncOperation(del))
                return ApplyTo(AsyncToSyncAdapter.Await(() => del.Invoke()));
#endif

            return ApplyTo(GetTestObject(del));
        }

#pragma warning disable 3006
        /// <summary>
        /// Test whether the constraint is satisfied by a given reference.
        /// The default implementation simply dereferences the value but
        /// derived classes may override it to provide for delayed processing.
        /// </summary>
        /// <param name="actual">A reference to the value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public virtual ConstraintResult ApplyTo<TActual>(ref TActual actual)
        {
            return ApplyTo(actual);
        }
#pragma warning restore 3006

        /// <summary>
        /// Retrieves the value to be tested from an ActualValueDelegate.
        /// The default implementation simply evaluates the delegate but derived
        /// classes may override it to provide for delayed processing.
        /// </summary>
        /// <param name="del">An ActualValueDelegate</param>
        /// <returns>Delegate evaluation result</returns>
        protected virtual object GetTestObject<TActual>(ActualValueDelegate<TActual> del)
        {
            return del();
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

        #region Operator Overloads
        
        /// <summary>
        /// This operator creates a constraint that is satisfied only if both 
        /// argument constraints are satisfied.
        /// </summary>
        public static Constraint operator &(Constraint left, Constraint right)
        {
            IResolveConstraint l = (IResolveConstraint)left;
            IResolveConstraint r = (IResolveConstraint)right;
            return new AndConstraint(l.Resolve(), r.Resolve());
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if either 
        /// of the argument constraints is satisfied.
        /// </summary>
        public static Constraint operator |(Constraint left, Constraint right)
        {
            IResolveConstraint l = (IResolveConstraint)left;
            IResolveConstraint r = (IResolveConstraint)right;
            return new OrConstraint(l.Resolve(), r.Resolve());
        }

        /// <summary>
        /// This operator creates a constraint that is satisfied if the 
        /// argument constraint is not satisfied.
        /// </summary>
        public static Constraint operator !(Constraint constraint)
        {
            IResolveConstraint r = constraint as IResolveConstraint;
            return new NotConstraint(r == null ? new NullConstraint() : r.Resolve());
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
#if NYI
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

    public abstract class Constraint<TExpected> : Constraint
    {
        public Constraint() { }

        public Constraint(TExpected arg) : base(arg) { }

        public Constraint(TExpected arg1, TExpected arg2) : base(arg1, arg2) { }
    }
}