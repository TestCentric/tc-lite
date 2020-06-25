// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// The EqualConstraintResult class is tailored for formatting
    /// and displaying the result of an EqualConstraint.
    /// </summary>
    public class EqualConstraintResult : ConstraintResult
    {
        private readonly object expectedValue;
        private readonly Tolerance tolerance;
        private readonly bool caseInsensitive;
        private readonly bool clipStrings;
        // NYI? private readonly IList<NUnitEqualityComparer.FailurePoint> failurePoints;

        #region Message Strings
        private static readonly string StringsDiffer_1 =
            "String lengths are both {0}. Strings differ at index {1}.";
        private static readonly string StringsDiffer_2 =
            "Expected string length {0} but was {1}. Strings differ at index {2}.";
#if NYI // Streams
        private static readonly string StreamsDiffer_1 =
            "Stream lengths are both {0}. Streams differ at offset {1}.";
        private static readonly string StreamsDiffer_2 =
            "Expected Stream length {0} but was {1}.";// Streams differ at offset {2}.";
#endif
        private static readonly string CollectionType_1 =
            "Expected and actual are both {0}";
        private static readonly string CollectionType_2 =
            "Expected is {0}, actual is {1}";
#if NYI // ValuesDiffer
        private static readonly string ValuesDiffer_1 =
            "Values differ at index {0}";
        private static readonly string ValuesDiffer_2 =
            "Values differ at expected index {0}, actual index {1}";
#endif
        #endregion

        /// <summary>
        /// Construct an EqualConstraintResult
        /// </summary>
        public EqualConstraintResult(EqualConstraint constraint, object actual, bool hasSucceeded)
            : base(constraint, actual, hasSucceeded)
        {
            this.expectedValue = constraint.ExpectedValue;
            this.tolerance = constraint.Tolerance;
            this.caseInsensitive = constraint.CaseInsensitive;
            this.clipStrings = constraint.ClipStrings;
        }

        /// <summary>
        /// Write a failure message. Overridden to provide custom
        /// failure messages for EqualConstraint.
        /// </summary>
        /// <param name="writer">The MessageWriter to write to</param>
        public override void WriteMessageTo(MessageWriter writer)
        {
            DisplayDifferences(writer, expectedValue, ActualValue, 0);
        }

        private void DisplayDifferences(MessageWriter writer, object expected, object actual, int depth)
        {
            if (expected is string && actual is string)
                DisplayStringDifferences(writer, (string)expected, (string)actual);
            else if (expected is ICollection && actual is ICollection)
                DisplayCollectionDifferences(writer, (ICollection)expected, (ICollection)actual, depth);
            else if (expected is IEnumerable && actual is IEnumerable)
                DisplayEnumerableDifferences(writer, (IEnumerable)expected, (IEnumerable)actual, depth);
#if NYI // Streams
            else if (expected is Stream && actual is Stream)
                DisplayStreamDifferences(writer, (Stream)expected, (Stream)actual, depth);
#endif
            else if (tolerance != null)
                writer.DisplayDifferences(expected, actual, tolerance);
            else
                writer.DisplayDifferences(expected, actual);
        }

        #region DisplayStringDifferences
        private void DisplayStringDifferences(MessageWriter writer, string expected, string actual)
        {
            int mismatch = MsgUtils.FindMismatchPosition(expected, actual, 0, caseInsensitive);

            if (expected.Length == actual.Length)
                writer.WriteMessageLine(StringsDiffer_1, expected.Length, mismatch);
            else
                writer.WriteMessageLine(StringsDiffer_2, expected.Length, actual.Length, mismatch);

            writer.DisplayStringDifferences(expected, actual, mismatch, caseInsensitive, clipStrings);
        }
        #endregion

        #region DisplayStreamDifferences
#if NYI // Streams
        private void DisplayStreamDifferences(MessageWriter writer, Stream expected, Stream actual, int depth)
        {
            if (expected.Length == actual.Length)
            {
                long offset = failurePoints[depth].Position;
                writer.WriteMessageLine(StreamsDiffer_1, expected.Length, offset);
            }
            else
                writer.WriteMessageLine(StreamsDiffer_2, expected.Length, actual.Length);
        }
#endif
        #endregion

        #region DisplayCollectionDifferences
        /// <summary>
        /// Display the failure information for two collections that did not match.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display</param>
        /// <param name="expected">The expected collection.</param>
        /// <param name="actual">The actual collection</param>
        /// <param name="depth">The depth of this failure in a set of nested collections</param>
        private void DisplayCollectionDifferences(MessageWriter writer, ICollection expected, ICollection actual, int depth)
        {
            DisplayTypesAndSizes(writer, expected, actual, depth);

#if NYI // FailurePoints
            if (failurePoints.Count > depth)
            {
                NUnitEqualityComparer.FailurePoint failurePoint = failurePoints[depth];

                DisplayFailurePoint(writer, expected, actual, failurePoint, depth);

                if (failurePoint.ExpectedHasData && failurePoint.ActualHasData)
                    DisplayDifferences(
                        writer,
                        failurePoint.ExpectedValue,
                        failurePoint.ActualValue,
                        ++depth);
                else if (failurePoint.ActualHasData)
                {
                    writer.Write("  Extra:    ");
                    writer.WriteCollectionElements(actual.Skip(failurePoint.Position), 0, 3);
                }
                else
                {
                    writer.Write("  Missing:  ");
                    writer.WriteCollectionElements(expected.Skip(failurePoint.Position), 0, 3);
                }
            }
#endif
        }

        /// <summary>
        /// Displays a single line showing the types and sizes of the expected
        /// and actual collections or arrays. If both are identical, the value is
        /// only shown once.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display</param>
        /// <param name="expected">The expected collection or array</param>
        /// <param name="actual">The actual collection or array</param>
        /// <param name="indent">The indentation level for the message line</param>
        private void DisplayTypesAndSizes(MessageWriter writer, IEnumerable expected, IEnumerable actual, int indent)
        {
            string sExpected = MsgUtils.GetTypeRepresentation(expected);
            if (expected is ICollection && !(expected is Array))
                sExpected += string.Format(" with {0} elements", ((ICollection)expected).Count);

            string sActual = MsgUtils.GetTypeRepresentation(actual);
            if (actual is ICollection && !(actual is Array))
                sActual += string.Format(" with {0} elements", ((ICollection)actual).Count);

            if (sExpected == sActual)
                writer.WriteMessageLine(indent, CollectionType_1, sExpected);
            else
                writer.WriteMessageLine(indent, CollectionType_2, sExpected, sActual);
        }

#if NYI // FailurePoints
        /// <summary>
        /// Displays a single line showing the point in the expected and actual
        /// arrays at which the comparison failed. If the arrays have different
        /// structures or dimensions, both values are shown.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display</param>
        /// <param name="expected">The expected array</param>
        /// <param name="actual">The actual array</param>
        /// <param name="failurePoint">Index of the failure point in the underlying collections</param>
        /// <param name="indent">The indentation level for the message line</param>
        private void DisplayFailurePoint(MessageWriter writer, IEnumerable expected, IEnumerable actual, NUnitEqualityComparer.FailurePoint failurePoint, int indent)
        {
            Array expectedArray = expected as Array;
            Array actualArray = actual as Array;

            int expectedRank = expectedArray != null ? expectedArray.Rank : 1;
            int actualRank = actualArray != null ? actualArray.Rank : 1;

            bool useOneIndex = expectedRank == actualRank;

            if (expectedArray != null && actualArray != null)
                for (int r = 1; r < expectedRank && useOneIndex; r++)
                    if (expectedArray.GetLength(r) != actualArray.GetLength(r))
                        useOneIndex = false;

            int[] expectedIndices = MsgUtils.GetArrayIndicesFromCollectionIndex(expected, failurePoint.Position);
            if (useOneIndex)
            {
                writer.WriteMessageLine(indent, ValuesDiffer_1, MsgUtils.GetArrayIndicesAsString(expectedIndices));
            }
            else
            {
                int[] actualIndices = MsgUtils.GetArrayIndicesFromCollectionIndex(actual, failurePoint.Position);
                writer.WriteMessageLine(indent, ValuesDiffer_2,
                    MsgUtils.GetArrayIndicesAsString(expectedIndices), MsgUtils.GetArrayIndicesAsString(actualIndices));
            }
        }
#endif

        #endregion

        #region DisplayEnumerableDifferences

        /// <summary>
        /// Display the failure information for two IEnumerables that did not match.
        /// </summary>
        /// <param name="writer">The MessageWriter on which to display</param>
        /// <param name="expected">The expected enumeration.</param>
        /// <param name="actual">The actual enumeration</param>
        /// <param name="depth">The depth of this failure in a set of nested collections</param>
        private void DisplayEnumerableDifferences(MessageWriter writer, IEnumerable expected, IEnumerable actual, int depth)
        {
            DisplayTypesAndSizes(writer, expected, actual, depth);

#if NYI // FailurePoints
            if (failurePoints.Count > depth)
            {
                NUnitEqualityComparer.FailurePoint failurePoint = failurePoints[depth];

                DisplayFailurePoint(writer, expected, actual, failurePoint, depth);

                if (failurePoint.ExpectedHasData && failurePoint.ActualHasData)
                    DisplayDifferences(
                        writer,
                        failurePoint.ExpectedValue,
                        failurePoint.ActualValue,
                        ++depth);
                else if (failurePoint.ActualHasData)
                {
                    writer.Write($"  Extra:    < {MsgUtils.FormatValue(failurePoint.ActualValue)}, ... >");
                }
                else
                {
                    writer.Write($"  Missing:  < {MsgUtils.FormatValue(failurePoint.ExpectedValue)}, ... >");
                }
            }
#endif
        }

        #endregion
    }
}
