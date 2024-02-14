// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Constraints
{
    /// <summary>
    /// ConstraintExpression represents a compound constraint in the 
    /// process of being constructed from a series of syntactic elements.
    /// 
    /// Individual elements are appended to the expression as they are
    /// recognized. Once an actual Constraint is appended, the expression
    /// returns a resolvable Constraint.
    /// </summary>
    /// <remarks>
    /// This is a partial class, with the individual syntactic elements
    /// like "Not", "All" or "EqualTo" distributed in individual files
    /// for the constraint or operator they generate. For example, "Not"
    /// is defined in NotOperator.cs and "EqualTo" in EqualConstraint.cs.
    /// </remarks>
    public partial class ConstraintExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstraintExpression"/> class.
        /// </summary>
        public ConstraintExpression() 
            : this(new ConstraintBuilder()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ConstraintExpression"/> 
        /// class passing in a ConstraintBuilder, which may be pre-populated.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public ConstraintExpression(ConstraintBuilder builder)
        {
            Builder = builder;
        }

        /// <summary>
        /// The ConstraintBuilder associated with this expression
        /// </summary>
        public ConstraintBuilder Builder { get; set; }

        #region Append Methods

        /// <summary>
        /// Appends an operator to the expression and returns the
        /// resulting expression itself.
        /// </summary>
        public ConstraintExpression Append(ConstraintOperator op)
        {
            Builder.Append(op);
            return this;
        }

        /// <summary>
        /// Appends a self-resolving operator to the expression and
        /// returns a new ResolvableConstraintExpression.
        /// </summary>
        public ResolvableConstraintExpression Append(SelfResolvingOperator op)
        {
            Builder.Append(op);
            return new ResolvableConstraintExpression(Builder);
        }

        /// <summary>
        /// Appends a constraint to the expression and returns that
        /// constraint, which is associated with the current state
        /// of the expression being built.
        /// </summary>
        public Constraint Append(Constraint constraint)
        {
            Builder.Append(constraint);
            return constraint;
        }

        #endregion

        // TODO: Move these to the relevant files once those files are created.

#if NYI // Attribute
        /// <summary>
        /// Returns a new AttributeConstraint checking for the
        /// presence of a particular attribute on an object.
        /// </summary>
        public ResolvableConstraintExpression Attribute(Type expectedType)
        {
            return this.Append(new AttributeOperator(expectedType));
        }

        /// <summary>
        /// Returns a new AttributeConstraint checking for the
        /// presence of a particular attribute on an object.
        /// </summary>
        public ResolvableConstraintExpression Attribute<T>()
        {
            return Attribute(typeof(T));
        }
#endif

#if NYI // With
        /// <summary>
        /// With is currently a NOP - reserved for future use.
        /// </summary>
        public ConstraintExpression With
        {
            get { return this.Append(new WithOperator()); }
        }
#endif

#if NYI // Matches
        /// <summary>
        /// Returns the constraint provided as an argument - used to allow custom
        /// custom constraints to easily participate in the syntax.
        /// </summary>
        public Constraint Matches(Constraint constraint)
        {
            return this.Append(constraint);
        }

        /// <summary>
        /// Returns the constraint provided as an argument - used to allow custom
        /// custom constraints to easily participate in the syntax.
        /// </summary>
        public Constraint Matches<T>(Predicate<T> predicate)
        {
            return this.Append(new PredicateConstraint<T>(predicate));
        }
#endif

#if NYI // AssignableFrom
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public AssignableFromConstraint AssignableFrom(Type expectedType)
        {
            return (AssignableFromConstraint)this.Append(new AssignableFromConstraint(expectedType));
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public AssignableFromConstraint AssignableFrom<T>()
        {
            return (AssignableFromConstraint)this.Append(new AssignableFromConstraint(typeof(T)));
        }
#endif

#if NYI // AssignableTo
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public AssignableToConstraint AssignableTo(Type expectedType)
        {
            return (AssignableToConstraint)this.Append(new AssignableToConstraint(expectedType));
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public AssignableToConstraint AssignableTo<T>()
        {
            return (AssignableToConstraint)this.Append(new AssignableToConstraint(typeof(T)));
        }
#endif

#if NYI // SubsetOf
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a subset of the collection supplied as an argument.
        /// </summary>
        public CollectionSubsetConstraint SubsetOf(IEnumerable expected)
        {
            return (CollectionSubsetConstraint)this.Append(new CollectionSubsetConstraint(expected));
        }
#endif

#if NYI // Ordered
        /// <summary>
        /// Returns a constraint that tests whether a collection is ordered
        /// </summary>
        public CollectionOrderedConstraint Ordered
        {
            get { return (CollectionOrderedConstraint)this.Append(new CollectionOrderedConstraint()); }
        }
#endif

#if NYI // SamePath
        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is the same as an expected path after canonicalization.
        /// </summary>
        public SamePathConstraint SamePath(string expected)
        {
            return (SamePathConstraint)this.Append(new SamePathConstraint(expected));
        }
#endif

#if NYI // SubPath
        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is the same path or under an expected path after canonicalization.
        /// </summary>
        public SubPathConstraint SubPath(string expected)
        {
            return (SubPathConstraint)this.Append(new SubPathConstraint(expected));
        }
#endif

#if NYI // SamePathOrUnder
        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is the same path or under an expected path after canonicalization.
        /// </summary>
        public SamePathOrUnderConstraint SamePathOrUnder(string expected)
        {
            return (SamePathOrUnderConstraint)this.Append(new SamePathOrUnderConstraint(expected));
        }
#endif
    }
}
