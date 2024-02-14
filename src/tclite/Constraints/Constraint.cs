// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Constraints
{
    /// <summary>
    /// The Constraint class is the base of all built-in constraints
    /// within TCLite. The class models a constraint, which puts no
    /// limitations on the type of actual value it is able to handle.
    /// </summary>
    public abstract partial class Constraint : IConstraint, IResolveConstraint
    {
        /// <summary>
        /// Construct a constraint with optional arguments
        /// </summary>
        /// <param name="args">Arguments to be saved</param>
        protected Constraint(params object[] args)
        {
            Arguments = args;
        }

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

        #region Public Methods

        /// <summary>
        /// Validate the actual value argument based on what the
        /// particular constraint allows. The default 
        /// implementation does nothing, implying that the constraint
        /// can handle any Type as well as null values.
        /// </summary>
        /// <param name="actual"></param>
        public virtual void ValidateActualValue(object actual) { }

        /// <summary>
        /// Validates the actual value argument and applies the constraint 
        /// to it, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public ConstraintResult ApplyTo<T>(T actual)
        {
            ValidateActualValue(actual);
            return ApplyConstraint(actual);
        }

        /// <summary>
        /// Validates the ActualValueDelegate and applies the constraint
        /// to the value it returns.
        /// </summary>
        /// <param name="del">The delegate to be evaluated and tested</param>
        /// <returns>A ConstraintResult</returns>
        public ConstraintResult ApplyTo<T>(ActualValueDelegate<T> del)
        {
            ValidateActualValue(del);
            return ApplyConstraint(del);
        }

        /// <summary>
        /// Validates the actual value argument and applies the constraint 
        /// to it, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">A reference to the value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public ConstraintResult ApplyTo<T>(ref T actual)
        {
            ValidateActualValue(actual);
            return ApplyConstraint(ref actual);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Applies the constraint to the value to be tested and returns
        /// a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        /// <remarks>
        /// The value has already been validated when this method is called.
        /// </remarks>
        protected abstract ConstraintResult ApplyConstraint<T>(T actual);

        /// <summary>
        /// Applies the constraint to an ActualValueDelegate that returns
        /// the value to be tested. The default implementation simply evaluates
        /// the delegate but derived classes may override it to provide for
        /// delayed processing.
        /// </summary>
        /// <param name="del">An ActualValueDelegate</param>
        /// <returns>A ConstraintResult</returns>
        /// <remarks>
        /// The value has already been validated when this method is called.
        /// </remarks>
        protected virtual ConstraintResult ApplyConstraint<T>(ActualValueDelegate<T> del)
        {
#if NYI // async
            if (AsyncToSyncAdapter.IsAsyncOperation(del))
                return ApplyConstraint(AsyncToSyncAdapter.Await(() => del.Invoke()));
#endif

            return ApplyConstraint(del());
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given reference.
        /// The default implementation simply dereferences the value but
        /// derived classes may override it to provide for delayed processing.
        /// </summary>
        /// <param name="actual">A reference to the value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        /// <remarks>
        /// The value has already been validated when this method is called.
        /// </remarks>
        protected virtual ConstraintResult ApplyConstraint<T>(ref T actual)
        {
            return ApplyConstraint(actual);
        }

        #endregion

        #region IResolveConstraint Implementation

        bool IResolveConstraint.IsResolvable => Builder == null || Builder.IsResolvable;

        IConstraint IResolveConstraint.Resolve()
        {
            if (Builder == null)
                return this;

            if (!Builder.IsResolvable)
                throw new InvalidOperationException("Attempted to resolve a Constraint under construction");

            return Builder.Resolve();
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
    }
}