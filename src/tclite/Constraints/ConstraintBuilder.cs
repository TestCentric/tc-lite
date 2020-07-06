// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// ConstraintBuilder maintains the stacks that are used in
    /// processing a ConstraintExpression. An OperatorStack
    /// is used to hold operators that are waiting for their
    /// operands to be recognized. a ConstraintStack holds 
    /// input constraints as well as the results of each
    /// operator applied.
    /// </summary>
    public class ConstraintBuilder
    {
        #region Nested Operator Stack Class
        /// <summary>
        /// OperatorStack is a type-safe stack for holding ConstraintOperators
        /// </summary>
        public class OperatorStack
        {
            private Stack<ConstraintOperator> _stack = new Stack<ConstraintOperator>();

            /// <summary>
            /// Initializes a new instance of the <see cref="T:OperatorStack"/> class.
            /// </summary>
            /// <param name="builder">The builder.</param>
            public OperatorStack(ConstraintBuilder builder)
            {
            }

            /// <summary>
            /// Gets a value indicating whether this <see cref="T:OpStack"/> is empty.
            /// </summary>
            /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
            public bool Empty => _stack.Count == 0;

            /// <summary>
            /// Gets the topmost operator without modifying the stack.
            /// </summary>
            /// <value>The top.</value>
            public ConstraintOperator Top => _stack.Peek();

            /// <summary>
            /// Pushes the specified operator onto the stack.
            /// </summary>
            /// <param name="op">The op.</param>
            public void Push(ConstraintOperator op) => _stack.Push(op);

            /// <summary>
            /// Pops the topmost operator from the stack.
            /// </summary>
            /// <returns></returns>
            public ConstraintOperator Pop() => _stack.Pop();
        }
        #endregion

        #region Nested Constraint Stack Class
        /// <summary>
        /// ConstraintStack is a type-safe stack for holding Constraints
        /// </summary>
        public class ConstraintStack
        {
            private Stack<IConstraint> _stack = new Stack<IConstraint>();

            private ConstraintBuilder _builder;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:ConstraintStack"/> class.
            /// </summary>
            /// <param name="builder">The builder.</param>
            public ConstraintStack(ConstraintBuilder builder)
            {
                _builder = builder;
            }

            /// <summary>
            /// Gets a value indicating whether this <see cref="T:ConstraintStack"/> is empty.
            /// </summary>
            /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
            public bool Empty =>_stack.Count == 0;

            /// <summary>
            /// Gets the topmost constraint without modifying the stack.
            /// </summary>
            /// <value>The topmost constraint</value>
            public IConstraint Top => _stack.Peek();

            /// <summary>
            /// Pushes the specified constraint. As a side effect,
            /// the constraint's builder field is set to the 
            /// ConstraintBuilder owning this stack.
            /// </summary>
            /// <param name="constraint">The constraint.</param>
            public void Push(IConstraint constraint)
            {
                _stack.Push(constraint);
                constraint.Builder = _builder;
            }

            /// <summary>
            /// Pops this topmost constrait from the stack.
            /// As a side effect, the constraint's builder
            /// field is set to null.
            /// </summary>
            /// <returns></returns>
            public IConstraint Pop()
            {
                IConstraint constraint = _stack.Pop();
                constraint.Builder = null;
                return constraint;
            }
        }
        #endregion

        #region Instance Fields
        private readonly OperatorStack ops;

        private readonly ConstraintStack constraints;

        private object lastPushed;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstraintExpression"/> class.
        /// </summary>
        public ConstraintBuilder()
        {
            this.ops = new OperatorStack(this);
            this.constraints = new ConstraintStack(this);
        }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the expression under construction
        /// is currently resolvable, allowing the Resolve() method to be called.
        /// </summary>
        public bool IsResolvable
        {
            get { return lastPushed is IConstraint || lastPushed is SelfResolvingOperator; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Appends the specified operator to the expression by first
        /// reducing the operator stack and then pushing the new
        /// operator on the stack.
        /// </summary>
        /// <param name="op">The operator to push.</param>
        public void Append(ConstraintOperator op)
        {
            op.LeftContext = lastPushed;
            if (lastPushed is ConstraintOperator)
                SetTopOperatorRightContext(op);

            // Reduce any lower precedence operators
            ReduceOperatorStack(op.LeftPrecedence);
            
            ops.Push(op);
            lastPushed = op;
        }

        /// <summary>
        /// Appends the specified constraint to the expresson by pushing
        /// it on the constraint stack.
        /// </summary>
        /// <param name="constraint">The constraint to push.</param>
        public Constraint Append(Constraint constraint)
        {
            if (lastPushed is ConstraintOperator)
                SetTopOperatorRightContext(constraint);

            constraints.Push(constraint);
            lastPushed = constraint;
            constraint.Builder = this;

            return constraint;
        }

        /// <summary>
        /// Sets the top operator right context.
        /// </summary>
        /// <param name="rightContext">The right context.</param>
        private void SetTopOperatorRightContext(object rightContext)
        {
            // Some operators change their precedence based on
            // the right context - save current precedence.
            int oldPrecedence = ops.Top.LeftPrecedence;

            ops.Top.RightContext = rightContext;

            // If the precedence increased, we may be able to
            // reduce the region of the stack below the operator
            if (ops.Top.LeftPrecedence > oldPrecedence)
            {
                ConstraintOperator changedOp = ops.Pop();
                ReduceOperatorStack(changedOp.LeftPrecedence);
                ops.Push(changedOp);
            }
        }

        /// <summary>
        /// Reduces the operator stack until the topmost item
        /// precedence is greater than or equal to the target precedence.
        /// </summary>
        /// <param name="targetPrecedence">The target precedence.</param>
        private void ReduceOperatorStack(int targetPrecedence)
        {
            while (!ops.Empty && ops.Top.RightPrecedence < targetPrecedence)
                ops.Pop().Reduce(constraints);
        }

        /// <summary>
        /// Resolves this instance, returning a Constraint. If the builder
        /// is not currently in a resolvable state, an exception is thrown.
        /// </summary>
        /// <returns>The resolved constraint</returns>
        public IConstraint Resolve()
        {
            if (!IsResolvable)
                throw new InvalidOperationException("A partial expression may not be resolved");

            while (!ops.Empty)
            {
                ConstraintOperator op = ops.Pop();
                op.Reduce(constraints);
            }

            return constraints.Pop();
        }
        
        #endregion
    }
}
