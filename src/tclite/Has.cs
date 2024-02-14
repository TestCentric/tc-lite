// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

namespace TCLite
{
    /// <summary>
    /// Helper class exposing properties and methods that form
    /// part of the Constraint syntax in the primary namespace.
    /// </summary>
    /// <remarks>
    /// The actual syntactic elements are defined in the base class,
    /// Has_Syntax, a partial class, with individual elements
    /// distributed across multiple files.
    /// </remarks>
    public partial class Has : Constraints.Has_Syntax
    {
        // TODO: Move these to the relevant files once those files are created.

#if NYI // Attribute
        #region Attribute

        /// <summary>
        /// Returns a new AttributeConstraint checking for the
        /// presence of a particular attribute on an object.
        /// </summary>
        public static ResolvableConstraintExpression Attribute(Type expectedType)
        {
            return new ConstraintExpression().Attribute(expectedType);
        }

        /// <summary>
        /// Returns a new AttributeConstraint checking for the
        /// presence of a particular attribute on an object.
        /// </summary>
        public static ResolvableConstraintExpression Attribute<T>()
        {
            return Attribute(typeof(T));
        }

        #endregion
#endif
    }
}
