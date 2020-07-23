// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// RangeConstraint tests whether two values are within a 
    /// specified range.
    /// </summary>
    public class RangeConstraint<TExpected> : Constraint
    {
        private readonly TExpected _from;
        private readonly TExpected _to;

        private ComparisonAdapter comparer = ComparisonAdapter.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeConstraint"/> class.
        /// </summary>
        /// <param name="from">Inclusive beginning of the range.</param>
        /// <param name="to">Inclusive end of the range.</param>
        public RangeConstraint(TExpected from, TExpected to) : base(from, to)
        {
            Guard.ArgumentNotNull(from, nameof(from));
            Guard.ArgumentNotNull(to, nameof(to));

            _from = from;
            _to = to;
        }

        /// <summary>
        /// Gets text describing a constraint
        /// </summary>
        public override string Description
        {
            get { return string.Format("in range ({0},{1})", _from, _to); }
        }

        public override void ValidateActualValue(object actual)
        {
            Guard.ArgumentNotNull(actual, nameof(actual));
            if (!(actual is TExpected))
                Guard.ArgumentValid(
                    Numerics.IsNumericType(actual) && Numerics.IsNumericType(typeof(TExpected)),
                    "Range comparisons are only supported for numeric types.",
                    nameof(actual));
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override ConstraintResult ApplyConstraint<TActual>(TActual actual)
        {
            if ( _from == null || _to == null || actual == null)
                throw new ArgumentException( "Cannot compare using a null reference", nameof(actual) );
            CompareFromAndTo();
            bool isInsideRange = comparer.Compare(_from, actual) <= 0 && comparer.Compare(_to, actual) >= 0;
            return new ConstraintResult(this, actual, isInsideRange);
        }

        private void CompareFromAndTo()
        {
            if (comparer.Compare(_from, _to) > 0)
                throw new ArgumentException("The from value must be less than or equal to the to value.");
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests whether the actual value falls 
        /// within a specified range.
        /// </summary>
        public RangeConstraint<T> InRange<T>(T from, T to) where T : IComparable<T>
        {
            return (RangeConstraint<T>)this.Append(new RangeConstraint<T>(from, to));
        }
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a constraint that tests whether the actual value falls 
        /// within a specified range.
        /// </summary>
        public static RangeConstraint<T> InRange<T>(T from, T to) where T : IComparable<T>
        {
            return new RangeConstraint<T>(from, to);
        }
    }
}
