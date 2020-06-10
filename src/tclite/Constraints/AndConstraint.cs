// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// AndConstraint succeeds only if both members succeed.
    /// </summary>
    public class AndConstraint : BinaryConstraint
    {
        private enum FailurePoint
        {
            None,
            Left,
            Right
        };

        private FailurePoint failurePoint;

        /// <summary>
        /// Create an AndConstraint from two other constraints
        /// </summary>
        /// <param name="left">The first constraint</param>
        /// <param name="right">The second constraint</param>
        public AndConstraint(Constraint left, Constraint right) : base(left, right) { }

        /// <summary>
        /// Apply both member constraints to an actual value, succeeding 
        /// succeeding only if both of them succeed.
        /// </summary>
        /// <param name="actual">The actual value</param>
        /// <returns>True if the constraints both succeeded</returns>
        public override bool Matches(object actual)
        {
            ActualValue = actual;

            failurePoint = _left.Matches(actual)
                ? _right.Matches(actual)
                    ? FailurePoint.None
                    : FailurePoint.Right
                : FailurePoint.Left;

            return failurePoint == FailurePoint.None;
        }

        /// <summary>
        /// Write a description for this contraint to a MessageWriter
        /// </summary>
        /// <param name="writer">The MessageWriter to receive the description</param>
        public override void WriteDescriptionTo(MessageWriter writer)
        {
            _left.WriteDescriptionTo(writer);
            writer.WriteConnector("and");
            _right.WriteDescriptionTo(writer);
        }

        /// <summary>
        /// Write the actual value for a failing constraint test to a
        /// MessageWriter. The default implementation simply writes
        /// the raw value of actual, leaving it to the writer to
        /// perform any formatting.
        /// </summary>
        /// <param name="writer">The writer on which the actual value is displayed</param>
        public override void WriteActualValueTo(MessageWriter writer)
        {
            switch (failurePoint)
            {
                case FailurePoint.Left:
                    _left.WriteActualValueTo(writer);
                    break;
                case FailurePoint.Right:
                    _right.WriteActualValueTo(writer);
                    break;
                default:
                    base.WriteActualValueTo(writer);
                    break;
            }
        }
    }
}