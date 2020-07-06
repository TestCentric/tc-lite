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

        #region Not

        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public ConstraintExpression Not
        {
            get { return this.Append(new NotOperator()); }
        }

        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public ConstraintExpression No
        {
            get { return this.Append(new NotOperator()); }
        }

        #endregion

#if NYI // All
        #region All

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them succeed.
        /// </summary>
        public ConstraintExpression All
        {
            get { return this.Append(new AllOperator()); }
        }

        #endregion
#endif

        #region Some

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if at least one of them succeeds.
        /// </summary>
        public ConstraintExpression Some
        {
            get { return this.Append(new SomeOperator()); }
        }

#endregion

#if NYI // None, Exactly, Property, Length, Count, Message, InnerException, With, Attribute, Matches
#region None

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them fail.
        /// </summary>
        public ConstraintExpression None
        {
            get { return this.Append(new NoneOperator()); }
        }

#endregion

#region Exactly(n)
		
        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding only if a specified number of them succeed.
        /// </summary>
        public ConstraintExpression Exactly(int expectedCount)
        {
            return this.Append(new ExactCountOperator(expectedCount));
        }
		
#endregion
		
#region Property

        /// <summary>
        /// Returns a new PropertyConstraintExpression, which will either
        /// test for the existence of the named property on the object
        /// being tested or apply any following constraint to that property.
        /// </summary>
        public ResolvableConstraintExpression Property(string name)
        {
            return this.Append(new PropOperator(name));
        }

#endregion

#region Length

        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Length property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Length
        {
            get { return Property("Length"); }
        }

#endregion

#region Count

        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Count property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Count
        {
            get { return Property("Count"); }
        }

#endregion

#region Message

        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the Message property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression Message
        {
            get { return Property("Message"); }
        }

#endregion

#region InnerException

        /// <summary>
        /// Returns a new ConstraintExpression, which will apply the following
        /// constraint to the InnerException property of the object being tested.
        /// </summary>
        public ResolvableConstraintExpression InnerException
        {
            get { return Property("InnerException"); }
        }

#endregion

#region Attribute

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

#endregion

#region With

        /// <summary>
        /// With is currently a NOP - reserved for future use.
        /// </summary>
        public ConstraintExpression With
        {
            get { return this.Append(new WithOperator()); }
        }

#endregion

#region Matches

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

#endregion
#endif

#region Null

        /// <summary>
        /// Returns a constraint that tests for null
        /// </summary>
        public NullConstraint Null
        {
            get { return (NullConstraint)this.Append(new NullConstraint()); }
        }

#endregion

#region True

        /// <summary>
        /// Returns a constraint that tests for True
        /// </summary>
        public TrueConstraint True
        {
            get { return (TrueConstraint)this.Append(new TrueConstraint()); }
        }

#endregion

#region False

        /// <summary>
        /// Returns a constraint that tests for False
        /// </summary>
        public FalseConstraint False
        {
            get { return (FalseConstraint)this.Append(new FalseConstraint()); }
        }

#endregion

#if NYI // Positive, Negative
#region Positive
 
        /// <summary>
        /// Returns a constraint that tests for a positive value
        /// </summary>
        public GreaterThanConstraint Positive
        {
            get { return (GreaterThanConstraint)this.Append(new GreaterThanConstraint(0)); }
        }
 
#endregion
 
#region Negative
 
        /// <summary>
        /// Returns a constraint that tests for a negative value
        /// </summary>
        public LessThanConstraint Negative
        {
            get { return (LessThanConstraint)this.Append(new LessThanConstraint(0)); }
        }
 
#endregion
#endif

#region NaN

        /// <summary>
        /// Returns a constraint that tests for NaN
        /// </summary>
        public NaNConstraint NaN
        {
            get { return (NaNConstraint)this.Append(new NaNConstraint()); }
        }

#endregion

#if NYI // Unique, BinarySerializable, XmlSerializable
#region Unique

        /// <summary>
        /// Returns a constraint that tests whether a collection 
        /// contains all unique items.
        /// </summary>
        public UniqueItemsConstraint Unique
        {
            get { return (UniqueItemsConstraint)this.Append(new UniqueItemsConstraint()); }
        }

#endregion

#region BinarySerializable

        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in binary format.
        /// </summary>
        public BinarySerializableConstraint BinarySerializable
        {
            get { return (BinarySerializableConstraint)this.Append(new BinarySerializableConstraint()); }
        }

#endregion

#region XmlSerializable

        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in xml format.
        /// </summary>
        public XmlSerializableConstraint XmlSerializable
        {
            get { return (XmlSerializableConstraint)this.Append(new XmlSerializableConstraint()); }
        }

#endregion
#endif

#region EqualTo


#endregion

#region SameAs

        /// <summary>
        /// Returns a constraint that tests that two references are the same object
        /// </summary>
        public SameAsConstraint SameAs(object expected)
        {
            return (SameAsConstraint)Append(new SameAsConstraint(expected));
        }

#endregion

#region GreaterThan

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public GreaterThanConstraint<T> GreaterThan<T>(T expected)
        {
            return (GreaterThanConstraint<T>)Append(new GreaterThanConstraint<T>(expected));
        }

#endregion

#region GreaterThanOrEqualTo

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public GreaterThanOrEqualConstraint<T> GreaterThanOrEqualTo<T>(T expected)
        {
            return (GreaterThanOrEqualConstraint<T>)this.Append(new GreaterThanOrEqualConstraint<T>(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public GreaterThanOrEqualConstraint<T> AtLeast<T>(T expected)
        {
            return (GreaterThanOrEqualConstraint<T>)this.Append(new GreaterThanOrEqualConstraint<T>(expected));
        }

#endregion

#region LessThan

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than the suppled argument
        /// </summary>
        public LessThanConstraint<T> LessThan<T>(T expected)
        {
            return (LessThanConstraint<T>)Append(new LessThanConstraint<T>(expected));
        }

#endregion

#region LessThanOrEqualTo

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public LessThanOrEqualConstraint<T> LessThanOrEqualTo<T>(T expected)
        {
            return (LessThanOrEqualConstraint<T>)Append(new LessThanOrEqualConstraint<T>(expected));
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public LessThanOrEqualConstraint<T> AtMost<T>(T expected)
        {
            return (LessThanOrEqualConstraint<T>)Append(new LessThanOrEqualConstraint<T>(expected));
        }

#endregion


#region TypeOf

        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public ExactTypeConstraint TypeOf(Type expectedType)
        {
            return (ExactTypeConstraint)this.Append(new ExactTypeConstraint(expectedType));
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public ExactTypeConstraint TypeOf<T>()
        {
            return (ExactTypeConstraint)this.Append(new ExactTypeConstraint(typeof(T)));
        }

#endregion

#region InstanceOf

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public InstanceOfTypeConstraint InstanceOf(Type expectedType)
        {
            return (InstanceOfTypeConstraint)this.Append(new InstanceOfTypeConstraint(expectedType));
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public InstanceOfTypeConstraint InstanceOf<T>()
        {
            return (InstanceOfTypeConstraint)this.Append(new InstanceOfTypeConstraint(typeof(T)));
        }

#endregion

#if NYI // AssignableFrom, AssignableTo
#region AssignableFrom

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

#endregion

#region AssignableTo

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

#endregion

#region EquivalentTo

#if NYI // EquivalentTo
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a collection containing the same elements as the 
        /// collection supplied as an argument.
        /// </summary>
        public CollectionEquivalentConstraint EquivalentTo(IEnumerable expected)
        {
            return (CollectionEquivalentConstraint)this.Append(new CollectionEquivalentConstraint(expected));
        }
#endif

#endregion

#region SubsetOf

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

#endregion

#region Ordered

        /// <summary>
        /// Returns a constraint that tests whether a collection is ordered
        /// </summary>
        public CollectionOrderedConstraint Ordered
        {
            get { return (CollectionOrderedConstraint)this.Append(new CollectionOrderedConstraint()); }
        }

#endregion

#region Member


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

#endregion

#region Contains

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

#endregion

#region StringContaining

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

#endregion

#region StartsWith

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

#endregion

#region EndsWith

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

#endregion

#region Matches

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

#endregion

#region SamePath

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

#endregion

#region SubPath

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

#endregion

#region SamePathOrUnder

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

#endregion

#region InRange

        /// <summary>
        /// Returns a constraint that tests whether the actual value falls 
        /// within a specified range.
        /// </summary>
        public RangeConstraint<T> InRange<T>(T from, T to) where T : IComparable<T>
        {
            return (RangeConstraint<T>)this.Append(new RangeConstraint<T>(from, to));
        }

#endregion
#endif
    }
}
