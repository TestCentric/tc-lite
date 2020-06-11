// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

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
        /// <summary>
        /// Argument fields used by ToString();
        /// </summary>
        private readonly int _argcnt;
        private readonly object _arg1;
        private readonly object _arg2;

        /// <summary>
        /// Construct a constraint with no arguments
        /// </summary>
        protected Constraint()
        {
            _argcnt = 0;
        }

        /// <summary>
        /// Construct a constraint with one argument
        /// </summary>
        protected Constraint(object arg)
        {
            _argcnt = 1;
            _arg1 = arg;
        }

        /// <summary>
        /// Construct a constraint with two arguments
        /// </summary>
        protected Constraint(object arg1, object arg2)
        {
            _argcnt = 2;
            _arg1 = arg1;
            _arg2 = arg2;
        }

#region IConstraint Implementation

        /// <summary>
        /// The display name of this Constraint for use by ToString().
        /// The default value is the name of the constraint with
        /// trailing "Constraint" removed. Derived classes may set
        /// this to another name in their constructors.
        /// </summary>
        public string DisplayName
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
        public string Description { get; protected set; } 

        /// <summary>
        /// The actual value being tested against a constraint
        /// </summary>
        protected object ActualValue { get; set; }

        /// <summary>
        /// The builder holding this constraint
        /// </summary>
        public ConstraintBuilder Builder { get; set; }

        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public virtual ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            return new ConstraintResult(this, ActualValue, Matches(ActualValue));
        }

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

        #region Abstract and Virtual Methods
        /// <summary>
        /// Write the failure message to the MessageWriter provided
        /// as an argument. The default implementation simply passes
        /// the constraint and the actual value to the writer, which
        /// then displays the constraint description and the value.
        /// 
        /// Constraints that need to provide additional details,
        /// such as where the error occurred can override this.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display the message</param>
        public virtual void WriteMessageTo(MessageWriter writer)
        {
            writer.DisplayDifferences(this);
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public abstract bool Matches<TActual>(TActual actual);

        /// <summary>
        /// Test whether the constraint is satisfied by an
        /// ActualValueDelegate that returns the value to be tested.
        /// The default implementation simply evaluates the delegate
        /// but derived classes may override it to provide for delayed 
        /// processing.
        /// </summary>
        /// <param name="del">An <see cref="ActualValueDelegate{T}" /></param>
        /// <returns>True for success, false for failure</returns>
        public virtual bool Matches<TActual>(ActualValueDelegate<TActual> del)
        {
#if NYI
            if (AsyncInvocationRegion.IsAsyncOperation(del))
                using (var region = AsyncInvocationRegion.Create(del))
                    return Matches(region.WaitForPendingOperationsToComplete(del()));
#endif
            return Matches(del());
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given reference.
        /// The default implementation simply dereferences the value but
        /// derived classes may override it to provide for delayed processing.
        /// </summary>
        /// <param name="actual">A reference to the value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public virtual bool Matches<TActual>(ref TActual actual)
        {
            return Matches(actual);
        }

        /// <summary>
        /// Write the constraint description to a MessageWriter
        /// </summary>
        /// <param name="writer">The writer on which the description is displayed</param>
        public abstract void WriteDescriptionTo(MessageWriter writer);

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. The default implementation simply writes
        /// the raw value of actual, leaving it to the writer to
        /// perform any formatting.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public virtual void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteActualValue(ActualValue);
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

            return Builder == null ? rep : string.Format("<unresolved {0}>", rep);
        }

        /// <summary>
        /// Returns the string representation of this constraint
        /// </summary>
        protected virtual string GetStringRepresentation()
        {
            switch (_argcnt)
            {
                default:
                case 0:
                    return string.Format("<{0}>", DisplayName);
                case 1:
                    return string.Format("<{0} {1}>", DisplayName, _displayable(_arg1));
                case 2:
                    return string.Format("<{0} {1} {2}>", DisplayName, _displayable(_arg1), _displayable(_arg2));
            }
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

        #endregion

        #region IResolveConstraint Members
        Constraint IResolveConstraint.Resolve()
        {
            return Builder == null ? this : Builder.Resolve();
        }
        #endregion
    }

    public abstract class Constraint<TExpected> : Constraint
    {
        public Constraint() { }

        public Constraint(TExpected arg) : base(arg) { }

        public Constraint(TExpected arg1, TExpected arg2) : base(arg1, arg2) { }
    }
}