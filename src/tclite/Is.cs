// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using TCLite.Framework.Constraints;

namespace TCLite.Framework
{
    /// <summary>
    /// Helper class with properties and methods that supply
    /// a number of constraints used in Asserts.
    /// </summary>
    public partial class Is
    {
        #region Not

        /// <summary>
        /// Returns a ConstraintExpression that negates any
        /// following constraint.
        /// </summary>
        public static ConstraintExpression Not
        {
            get { return new ConstraintExpression().Not; }
        }

        #endregion

#if NYI // All
        #region All

        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding if all of them succeed.
        /// </summary>
        public static ConstraintExpression All
        {
            get { return new ConstraintExpression().All; }
        }

        #endregion
#endif

        #region Null

        /// <summary>
        /// Returns a constraint that tests for null
        /// </summary>
        public static NullConstraint Null
        {
            get { return new NullConstraint(); }
        }

        #endregion

        #region True

        /// <summary>
        /// Returns a constraint that tests for True
        /// </summary>
        public static TrueConstraint True
        {
            get { return new TrueConstraint(); }
        }

        #endregion

        #region False

        /// <summary>
        /// Returns a constraint that tests for False
        /// </summary>
        public static FalseConstraint False
        {
            get { return new FalseConstraint(); }
        }

        #endregion

#if NYI // Positive, Negative
        #region Positive
 
        /// <summary>
        /// Returns a constraint that tests for a positive value
        /// </summary>
        public static GreaterThanConstraint Positive
        {
            get { return new GreaterThanConstraint(0); }
        }
 
        #endregion
 
        #region Negative
 
        /// <summary>
        /// Returns a constraint that tests for a negative value
        /// </summary>
        public static LessThanConstraint Negative
        {
            get { return new LessThanConstraint(0); }
        }
 
        #endregion
#endif

        #region NaN

        /// <summary>
        /// Returns a constraint that tests for NaN
        /// </summary>
        public static NaNConstraint NaN
        {
            get { return new NaNConstraint(); }
        }

        #endregion

#if NYI // Empty, Unique, BinarySerializable, XmlSerializable
        #region Empty

        /// <summary>
        /// Returns a constraint that tests for empty
        /// </summary>
        public static EmptyConstraint Empty
        {
            get { return new EmptyConstraint(); }
        }

        #endregion

        #region Unique

        /// <summary>
        /// Returns a constraint that tests whether a collection 
        /// contains all unique items.
        /// </summary>
        public static UniqueItemsConstraint Unique
        {
            get { return new UniqueItemsConstraint(); }
        }

        #endregion

        #region BinarySerializable

        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in binary format.
        /// </summary>
        public static BinarySerializableConstraint BinarySerializable
        {
            get { return new BinarySerializableConstraint(); }
        }

        #endregion

        #region XmlSerializable

        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in xml format.
        /// </summary>
        public static XmlSerializableConstraint XmlSerializable
        {
            get { return new XmlSerializableConstraint(); }
        }

        #endregion
#endif

        #region EqualTo


        #endregion

        #region SameAs

        /// <summary>
        /// Returns a constraint that tests that two references are the same object
        /// </summary>
        public static SameAsConstraint SameAs(object expected)
        {
            return new SameAsConstraint(expected);
        }

        #endregion

        #region GreaterThan

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than the suppled argument
        /// </summary>
        public static GreaterThanConstraint GreaterThan(IComparable expected)
        {
            return new GreaterThanConstraint(expected);
        }

        #endregion

        #region GreaterThanOrEqualTo

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public static GreaterThanOrEqualConstraint GreaterThanOrEqualTo(IComparable expected)
        {
            return new GreaterThanOrEqualConstraint(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is greater than or equal to the suppled argument
        /// </summary>
        public static GreaterThanOrEqualConstraint AtLeast(IComparable expected)
        {
            return new GreaterThanOrEqualConstraint(expected);
        }

        #endregion

        #region LessThan

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than the suppled argument
        /// </summary>
        public static LessThanConstraint LessThan(IComparable expected)
        {
            return new LessThanConstraint(expected);
        }

        #endregion

        #region LessThanOrEqualTo

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public static LessThanOrEqualConstraint LessThanOrEqualTo(IComparable expected)
        {
            return new LessThanOrEqualConstraint(expected);
        }

        /// <summary>
        /// Returns a constraint that tests whether the
        /// actual value is less than or equal to the suppled argument
        /// </summary>
        public static LessThanOrEqualConstraint AtMost(IComparable expected)
        {
            return new LessThanOrEqualConstraint(expected);
        }

        #endregion

        #region TypeOf

        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public static ExactTypeConstraint TypeOf(Type expectedType)
        {
            return new ExactTypeConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual
        /// value is of the exact type supplied as an argument.
        /// </summary>
        public static ExactTypeConstraint TypeOf<T>()
        {
            return new ExactTypeConstraint(typeof(T));
        }

        #endregion

        #region InstanceOf

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public static InstanceOfTypeConstraint InstanceOf(Type expectedType)
        {
            return new InstanceOfTypeConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is of the type supplied as an argument or a derived type.
        /// </summary>
        public static InstanceOfTypeConstraint InstanceOf<T>()
        {
            return new InstanceOfTypeConstraint(typeof(T));
        }

        #endregion

#if NYI // AssignableFrom, AssignableTo
        #region AssignableFrom

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public static AssignableFromConstraint AssignableFrom(Type expectedType)
        {
            return new AssignableFromConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public static AssignableFromConstraint AssignableFrom<T>()
        {
            return new AssignableFromConstraint(typeof(T));
        }

        #endregion

        #region AssignableTo

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public static AssignableToConstraint AssignableTo(Type expectedType)
        {
            return new AssignableToConstraint(expectedType);
        }

        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is assignable from the type supplied as an argument.
        /// </summary>
        public static AssignableToConstraint AssignableTo<T>()
        {
            return new AssignableToConstraint(typeof(T));
        }

        #endregion
#endif

        #region EquivalentTo

#if NYI // EquivalentTo
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a collection containing the same elements as the 
        /// collection supplied as an argument.
        /// </summary>
        public static CollectionEquivalentConstraint EquivalentTo(IEnumerable expected)
        {
            return new CollectionEquivalentConstraint(expected);
        }
#endif

        #endregion

        #region SubsetOf

#if NYI // SubsetOf
        /// <summary>
        /// Returns a constraint that tests whether the actual value
        /// is a subset of the collection supplied as an argument.
        /// </summary>
        public static CollectionSubsetConstraint SubsetOf(IEnumerable expected)
        {
            return new CollectionSubsetConstraint(expected);
        }
#endif

        #endregion

        #region Ordered
#if NYI // Ordered
        /// <summary>
        /// Returns a constraint that tests whether a collection is ordered
        /// </summary>
        public static CollectionOrderedConstraint Ordered
        {
            get { return new CollectionOrderedConstraint(); }
        }
#endif
        #endregion

#if NYI // String Constraints
        #region StringContaining

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value contains the substring supplied as an argument.
        /// </summary>
        public static SubstringConstraint StringContaining(string expected)
        {
            return new SubstringConstraint(expected);
        }

        #endregion

        #region StringStarting

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value starts with the substring supplied as an argument.
        /// </summary>
        public static StartsWithConstraint StringStarting(string expected)
        {
            return new StartsWithConstraint(expected);
        }

        #endregion

        #region StringEnding

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value ends with the substring supplied as an argument.
        /// </summary>
        public static EndsWithConstraint StringEnding(string expected)
        {
            return new EndsWithConstraint(expected);
        }

        #endregion

        #region StringMatching

        /// <summary>
        /// Returns a constraint that succeeds if the actual
        /// value matches the regular expression supplied as an argument.
        /// </summary>
        public static RegexConstraint StringMatching(string pattern)
        {
            return new RegexConstraint(pattern);
        }

        #endregion
#endif

        #region SamePath

#if NYI // SamePath

        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is the same as an expected path after canonicalization.
        /// </summary>
        public static SamePathConstraint SamePath(string expected)
        {
            return new SamePathConstraint(expected);
        }
#endif

        #endregion

        #region SubPath

#if NYI // SubPath
        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is under an expected path after canonicalization.
        /// </summary>
        public static SubPathConstraint SubPath(string expected)
        {
            return new SubPathConstraint(expected);
        }
#endif

        #endregion

        #region SamePathOrUnder

#if NYI // SamePathOrUnder
        /// <summary>
        /// Returns a constraint that tests whether the path provided 
        /// is the same path or under an expected path after canonicalization.
        /// </summary>
        public static SamePathOrUnderConstraint SamePathOrUnder(string expected)
        {
            return new SamePathOrUnderConstraint(expected);
        }
#endif

        #endregion

#if NYI // InRange
        #region InRange

        /// <summary>
        /// Returns a constraint that tests whether the actual value falls 
        /// within a specified range.
        /// </summary>
        public static RangeConstraint<T> InRange<T>(T from, T to) where T : IComparable<T>
        {
            return new RangeConstraint<T>(from, to);
        }

        #endregion
#endif
    }
}
