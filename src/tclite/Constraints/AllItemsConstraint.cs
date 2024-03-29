// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.Collections;
using TCLite.Internal;

namespace TCLite.Constraints
{
    /// <summary>
    /// AllItemsConstraint applies another constraint to each
    /// item in a collection, succeeding if they all succeed.
    /// </summary>
    public class AllItemsConstraint : PrefixConstraint
    {
        /// <summary>
        /// Construct an AllItemsConstraint on top of an existing constraint
        /// </summary>
        /// <param name="itemConstraint"></param>
        public AllItemsConstraint(IConstraint itemConstraint)
            : base(itemConstraint)
        {
            DescriptionPrefix = "all items";
        }

        /// <summary> 
        /// The display name of this Constraint for use by ToString().
        /// The default value is the name of the constraint with
        /// trailing "Constraint" removed. Derived classes may set
        /// this to another name in their constructors.
        /// </summary>
        public override string DisplayName { get { return "All"; } }

        /// <summary>
        /// Apply the item constraint to each item in the collection,
        /// failing if any item fails.
        /// </summary>
        /// <param name="actual"></param>
        /// <returns></returns>
        protected override ConstraintResult ApplyConstraint<TActual>(TActual actual)
        {
            var enumerable = ConstraintUtils.RequireActual<IEnumerable>(actual, nameof(actual));

            int index = 0;
            foreach (object item in enumerable)
            {
                if (!BaseConstraint.ApplyTo(item).IsSuccess)
                {
                    return new EachItemConstraintResult(this, actual, item, index);
                }

                index++;
            }

            return new ConstraintResult(this, actual, ConstraintStatus.Success);
        }
    }
}
