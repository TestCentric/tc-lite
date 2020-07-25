// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;

namespace TCLite.Framework.Constraints
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

#if NYI // Property
        /// <summary>
        /// Returns a new PropertyConstraintExpression, which will either
        /// test for the existence of the named property on the object
        /// being tested or apply any following constraint to that property.
        /// </summary>
        public ResolvableConstraintExpression Property(string name)
        {
            return this.Append(new PropOperator(name));
        }
#endif

#if NYI // Length
        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Length property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Length
        {
            get { return Property("Length"); }
        }
#endif

#if NYI // Count
        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Count property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Count
        {
            get { return Property("Count"); }
        }
#endif

#if NYI // Message
        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Message property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Message
        {
            get { return Property("Message"); }
        }
#endif

#if NYI // InnerException
        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the InnerException property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression InnerException
        {
            get { return Property("InnerException"); }
        }
#endif

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

#if NYI //BinarySerializable
        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in binary format.
        /// </summary>
        public BinarySerializableConstraint BinarySerializable
        {
            get { return (BinarySerializableConstraint)this.Append(new BinarySerializableConstraint()); }
        }
#endif

#if NYI // XmlSerializable
        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in xml format.
        /// </summary>
        public XmlSerializableConstraint XmlSerializable
        {
            get { return (XmlSerializableConstraint)this.Append(new XmlSerializableConstraint()); }
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

#if NYI // Member
        /// <summary>
        /// Returns a new CollectionContainsConstraint checking for the
        /// presence of a particular object in the collection.
        /// </summary>
        public CollectionContainsConstraint Member(object expected)
        {
            return (CollectionContainsConstraint)this.Append(new CollectionContainsConstraint(expected));
        }

        /// <summary>
        /// Returns a new CollectionContainsConstraint checking for the
        /// presence of a particular object in the collection.
        /// </summary>
        public CollectionContainsConstraint Contains(object expected)
        {
            return (CollectionContainsConstraint)this.Append(new CollectionContainsConstraint(expected));
        }
#endif

#if NYI // Contains
        /// <summary>
        /// Returns a new ContainsConstraint. This constraint
        /// will, in turn, make use of the appropriate second-level
        /// constraint, depending on the type of the actual argument. 
        /// This overload is only used if the item sought is a string,
        /// since any other type implies that we are looking for a 
        /// collection member.
        /// </summary>
        public ContainsConstraint Contains(string expected)
        {
            return (ContainsConstraint)this.Append(new ContainsConstraint(expected));
        }
#endif

#if NYI // StringContaining
        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public SubstringConstraint StringContaining(string expected)
        {
            return (SubstringConstraint)this.Append(new SubstringConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public SubstringConstraint ContainsSubstring(string expected)
        {
            return (SubstringConstraint)this.Append(new SubstringConstraint(expected));
        }
#endif

#if NYI // StartsWith
        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public StartsWithConstraint StartsWith(string expected)
        {
            return (StartsWithConstraint)this.Append(new StartsWithConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public StartsWithConstraint StringStarting(string expected)
        {
            return (StartsWithConstraint)this.Append(new StartsWithConstraint(expected));
        }
#endif

#if NYI // EndsWith
        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value ends with the substring supplied as an argument.
        /// </summary>
        public EndsWithConstraint EndsWith(string expected)
        {
            return (EndsWithConstraint)this.Append(new EndsWithConstraint(expected));
        }

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value ends with the substring supplied as an argument.
        /// </summary>
        public EndsWithConstraint StringEnding(string expected)
        {
            return (EndsWithConstraint)this.Append(new EndsWithConstraint(expected));
        }
#endif

#if NYI // Matches
        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value matches the regular expression supplied as an argument.
        /// </summary>
        public RegexConstraint Matches(string pattern)
        {
            return (RegexConstraint)this.Append(new RegexConstraint(pattern));
        }

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value matches the regular expression supplied as an argument.
        /// </summary>
        public RegexConstraint StringMatching(string pattern)
        {
            return (RegexConstraint)this.Append(new RegexConstraint(pattern));
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
