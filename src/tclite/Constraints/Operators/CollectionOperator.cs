// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Abstract base for operators that indicate how to
    /// apply a constraint to items in a collection.
    /// </summary>
    public abstract class CollectionOperator : PrefixOperator
    {
        /// <summary>
        /// Constructs a CollectionOperator
        /// </summary>
        protected CollectionOperator()
        {
            // Collection Operators stack on everything
            // and allow all other ops to stack on them
            this.left_precedence = 1;
            this.right_precedence = 10;
        }
    }
 }