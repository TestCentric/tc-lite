// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.ObjectModel;
using TCLite.Internal;

namespace TCLite.Constraints
{
    /// <summary>
    /// ExactCountConstraint applies another constraint to each
    /// item in a collection, succeeding only if a specified
    /// number of items succeed.
    /// </summary>
    public class ExactCountConstraint : Constraint
    {
        private readonly int _expectedCount;
        private readonly IConstraint _itemConstraint;

        /// <summary>
        /// Construct a standalone ExactCountConstraint
        /// </summary>
        /// <param name="expectedCount"></param>
        public ExactCountConstraint(int expectedCount)
        {
            _expectedCount = expectedCount;
        }

        /// <summary>
        /// Construct an ExactCountConstraint on top of an existing constraint
        /// </summary>
        /// <param name="expectedCount"></param>
        /// <param name="itemConstraint"></param>
        public ExactCountConstraint(int expectedCount, IConstraint itemConstraint)
            : base(itemConstraint)
        {
            Guard.ArgumentNotNull(itemConstraint, nameof(itemConstraint));

            _itemConstraint = itemConstraint;
            _expectedCount = expectedCount;
        }

        /// <summary>
        /// Apply the item constraint to each item in the collection,
        /// succeeding only if the expected number of items pass.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        protected override ConstraintResult ApplyConstraint<TActual>(TActual actual)
        {
            var enumerable = ConstraintUtils.RequireActual<IEnumerable>(actual, nameof(actual));
            var itemList = new Collection<object>();
            var matchCount = 0;

            foreach (var item in enumerable)
            {
                if (_itemConstraint != null)
                {
                    if (_itemConstraint.ApplyTo(item).IsSuccess)
                        matchCount++;
                }
                else
                {
                    matchCount++;
                }

                // We intentionally add one item too many because we use it to trigger
                // the ellipsis when we call "MsgUtils.FormatCollection" later on.
                if (itemList.Count <= MsgUtils.DefaultMaxItems )
                    itemList.Add(item);
            }

            return new ExactCountConstraintResult(this, actual, matchCount == _expectedCount, matchCount, itemList);
        }

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get
            {
                var descriptionPrefix =
                    _expectedCount == 0 ? "no item" :
                    _expectedCount == 1 ? "exactly one item" :
                    string.Format("exactly {0} items", _expectedCount);

                return _itemConstraint != null ? PrefixConstraint.FormatDescription(descriptionPrefix, _itemConstraint) : descriptionPrefix;
            }
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a ConstraintExpression, which will apply
        /// the following constraint to all members of a collection,
        /// succeeding only if a specified number of them succeed.
        /// </summary>
        public ItemsConstraintExpression Exactly(int expectedCount)
        {
            Builder.Append(new ExactCountOperator(expectedCount));
            return new ItemsConstraintExpression(Builder);
        }

        /// <summary>
        /// Returns a <see cref="ItemsConstraintExpression"/> which will apply
        /// the following constraint to only one member of the collection,
        /// and fail if none or more than one match occurs.
        /// </summary>
        public ItemsConstraintExpression One => Exactly(1);
    }

    public partial class Has_Syntax
    {
        /// <summary>
        /// Returns a <see cref="TCLite.ItemsConstraintExpression", which will apply
        /// the following constraint to all members of a collection,
        /// succeeding only if a specified number of them succeed.
        /// </summary>
        public static ItemsConstraintExpression Exactly(int expectedCount)
        {
            return new ConstraintExpression().Exactly(expectedCount);
        }

        /// <summary>
        /// Returns a <see cref="TCLite.ItemsConstraintExpression"/> which will apply
        /// the following constraint to only one member of the collection,
        /// and fail if none or more than one match occurs.
        /// </summary>
        public static ItemsConstraintExpression One => Exactly(1);
    }
}

