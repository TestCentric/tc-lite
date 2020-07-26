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
    public partial class Is : Constraints.Is_Syntax
    {
#if NYI // BinarySerializable
        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in binary format.
        /// </summary>
        public static BinarySerializableConstraint BinarySerializable
        {
            get { return new BinarySerializableConstraint(); }
        }
#endif

#if NYI // XmlSerializable
        /// <summary>
        /// Returns a constraint that tests whether an object graph is serializable in xml format.
        /// </summary>
        public static XmlSerializableConstraint XmlSerializable
        {
            get { return new XmlSerializableConstraint(); }
        }
#endif

#if NYI // AssignableFrom
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
#endif

#if NYI // AssignableTo
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
#endif

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

#if NYI // Ordered
        /// <summary>
        /// Returns a constraint that tests whether a collection is ordered
        /// </summary>
        public static CollectionOrderedConstraint Ordered
        {
            get { return new CollectionOrderedConstraint(); }
        }
#endif

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
    }
}
