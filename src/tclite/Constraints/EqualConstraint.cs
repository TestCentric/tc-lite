// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TCLite.Constraints
{
    /// <summary>
    /// EqualConstraint is able to compare an actual value with the
    /// expected value provided in its constructor. Two objects are 
    /// considered equal if both are null, or if both have the same 
    /// value. NUnit has special semantics for some object types.
    /// </summary>
    public class EqualConstraint<TExpected> : ExpectedValueConstraint<TExpected>
    {
        /// <summary>
        /// NUnitEqualityComparer used to test equality.
        /// </summary>
        private TCLiteEqualityComparer _comparer = new TCLiteEqualityComparer();

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public EqualConstraint(TExpected expected) : base(expected) { }

        public override string Description
        {
            get
            {
                var sb = new StringBuilder(MsgUtils.FormatValue(ExpectedValue));

                if (_tolerance != null && !_tolerance.IsDefault)
                {
                    sb.Append($" +/- {MsgUtils.FormatValue(_tolerance.Amount)}");
                    if (_tolerance.Mode != ToleranceMode.Linear)
                        sb.Append($" {_tolerance.Mode}");
                }

                if (_comparer.IgnoreCase)
                    sb.Append(", ignoring case");

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the tolerance for this comparison.
        /// </summary>
        /// <value>
        /// The tolerance.
        /// </value>
        public Tolerance Tolerance
        {
            get { return _tolerance; }
        }
        private Tolerance _tolerance = Tolerance.Default;


        /// <summary>
        /// Gets a value indicating whether to compare case insensitive.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if comparing case insensitive; otherwise, <see langword="false"/>.
        /// </value>
        public bool CaseInsensitive
        {
            get { return _comparer.IgnoreCase; }
        }

        /// <summary>
        /// Gets a value indicating whether or not to clip strings.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if set to clip strings otherwise, <see langword="false"/>.
        /// </value>
        public bool ClipStrings { get; private set; } = true;


        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public EqualConstraint<TExpected> IgnoreCase
        {
            get
            {
                _comparer.IgnoreCase = true;
                return this;
            }
        }

        /// <summary>
        /// Flag the constraint to suppress string clipping 
        /// and return self.
        /// </summary>
        public EqualConstraint<TExpected> NoClip
        {
            get
            {
                ClipStrings = false;
                return this;
            }
        }

        /// <summary>
        /// Flag the constraint to compare arrays as collections
        /// and return self.
        /// </summary>
        public EqualConstraint<TExpected> AsCollection
        {
            get
            {
                _comparer.CompareAsCollection = true;
                return this;
            }
        }

        /// <summary>
        /// Flag the constraint to use a tolerance when determining equality.
        /// </summary>
        /// <param name="amount">Tolerance value to be used</param>
        /// <returns>Self.</returns>
        public EqualConstraint<TExpected> Within(object amount)
        {
            if (!_tolerance.IsDefault)
                throw new InvalidOperationException("Within modifier may appear only once in a constraint expression");

            _tolerance = new Tolerance(amount);
            return this;
        }

        /// <summary>
        /// Switches the .Within() modifier to interpret its tolerance as
        /// a distance in representable values (see remarks).
        /// </summary>
        /// <returns>Self.</returns>
        /// <remarks>
        /// Ulp stands for "unit in the last place" and describes the minimum
        /// amount a given value can change. For any integers, an ulp is 1 whole
        /// digit. For floating point values, the accuracy of which is better
        /// for smaller numbers and worse for larger numbers, an ulp depends
        /// on the size of the number. Using ulps for comparison of floating
        /// point results instead of fixed tolerances is safer because it will
        /// automatically compensate for the added inaccuracy of larger numbers.
        /// </remarks>
        public EqualConstraint<TExpected> Ulps
        {
            get
            {
                _tolerance = _tolerance.Ulps;
                return this;
            }
        }

        /// <summary>
        /// Switches the .Within() modifier to interpret its tolerance as
        /// a percentage that the actual values is allowed to deviate from
        /// the expected value.
        /// </summary>
        /// <returns>Self</returns>
        public EqualConstraint<TExpected> Percent
        {
            get
            {
                _tolerance = _tolerance.Percent;
                return this;
            }
        }

        /// <summary>
        /// Causes the tolerance to be interpreted as a TimeSpan in days.
        /// </summary>
        /// <returns>Self</returns>
        public EqualConstraint<TExpected> Days
        {
            get
            {
                _tolerance = _tolerance.Days;
                return this;
            }
        }

        /// <summary>
        /// Causes the tolerance to be interpreted as a TimeSpan in hours.
        /// </summary>
        /// <returns>Self</returns>
        public EqualConstraint<TExpected> Hours
        {
            get
            {
                _tolerance = _tolerance.Hours;
                return this;
            }
        }

        /// <summary>
        /// Causes the tolerance to be interpreted as a TimeSpan in minutes.
        /// </summary>
        /// <returns>Self</returns>
        public EqualConstraint<TExpected> Minutes
        {
            get
            {
                _tolerance = _tolerance.Minutes;
                return this;
            }
        }

        /// <summary>
        /// Causes the tolerance to be interpreted as a TimeSpan in seconds.
        /// </summary>
        /// <returns>Self</returns>
        public EqualConstraint<TExpected> Seconds
        {
            get
            {
                _tolerance = _tolerance.Seconds;
                return this;
            }
        }

        /// <summary>
        /// Causes the tolerance to be interpreted as a TimeSpan in milliseconds.
        /// </summary>
        /// <returns>Self</returns>
        public EqualConstraint<TExpected> Milliseconds
        {
            get
            {
                _tolerance = _tolerance.Milliseconds;
                return this;
            }
        }

        /// <summary>
        /// Causes the tolerance to be interpreted as a TimeSpan in clock ticks.
        /// </summary>
        /// <returns>Self</returns>
        public EqualConstraint<TExpected> Ticks
        {
            get
            {
                _tolerance = _tolerance.Ticks;
                return this;
            }
        }

        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint<TExpected> Using(IComparer comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint<TExpected> Using<T>(IComparer<T> comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied Comparison object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint<TExpected> Using<T>(Comparison<T> comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint<TExpected> Using(IEqualityComparer comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint<TExpected> Using<T>(IEqualityComparer<T> comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        protected override ConstraintResult ApplyConstraint<T>(T actual)
        {
            return new EqualConstraintResult<TExpected>(this, actual, _comparer.AreEqual(ExpectedValue, actual, ref _tolerance));
        }
    }

    public partial class ConstraintExpression
    {
        /// <summary>
        /// Returns a constraint that tests two items for equality
        /// </summary>
        public EqualConstraint<T> EqualTo<T>(T expected) =>
            (EqualConstraint<T>)Append(new EqualConstraint<T>(expected));

        /// <summary>
        /// Returns a constraint that tests if item is equal to zero
        /// </summary>
        public EqualConstraint<int> Zero =>
            (EqualConstraint<int>)Append(new EqualConstraint<int>(0));
    }

    public partial class Is_Syntax
    {
        /// <summary>
        /// Returns a constraint that tests two items for equality
        /// </summary>
        public static EqualConstraint<T> EqualTo<T>(T expected)
        {
            return new EqualConstraint<T>(expected);
        }

        /// <summary>
        /// Returns a constraint that tests if item is equal to zero
        /// </summary>
        public static EqualConstraint<int> Zero => new EqualConstraint<int>(0);
    }
}
