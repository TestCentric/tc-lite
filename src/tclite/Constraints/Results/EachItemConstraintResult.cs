// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// Provides a <see cref="ConstraintResult"/> for the constraints 
    /// that are applied to each item in the collection
    /// </summary>
    internal sealed class EachItemConstraintResult : ConstraintResult
    {
        private readonly object _nonMatchingItem;
        private readonly int _nonMatchingItemIndex;

        /// <summary>
        /// Constructs a <see cref="EachItemConstraintResult" /> for a particular <see cref="Constraint" />
        /// Only used for Failure
        /// </summary>
        /// <param name="constraint">The Constraint to which this result applies</param>
        /// <param name="actualValue">The actual value to which the Constraint was applied</param>
        /// <param name="nonMatchingItem">Actual item that does not match expected condition</param>
        /// <param name="nonMatchingIndex">Non matching item index</param>
        public EachItemConstraintResult(IConstraint constraint, object actualValue, object nonMatchingItem, int nonMatchingIndex)
            : base(constraint, actualValue, false)
        {
            _nonMatchingItem = nonMatchingItem;
            _nonMatchingItemIndex = nonMatchingIndex;
        }

#if NYI
        /// <summary>
        /// Write constraint description, actual items, and non-matching item
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display the message</param>
        public override void WriteAdditionalLinesTo(MessageWriter writer)
        {
            var nonMatchingStr = $"  First non-matching item at index [{_nonMatchingItemIndex}]:  "
                + MsgUtils.FormatValue(_nonMatchingItem);
            writer.WriteLine(nonMatchingStr);
        }
#endif
    }
}
