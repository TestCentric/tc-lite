// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TCLite.Framework.Constraints
{
    /// <summary>
    /// EqualConstraint is able to compare an actual value with the
    /// expected value provided in its constructor. Two objects are 
    /// considered equal if both are null, or if both have the same 
    /// value. NUnit has special semantics for some object types.
    /// </summary>
    public class EqualConstraint : Constraint
    {
        #region Static and Instance Fields

        private Tolerance _tolerance = Tolerance.Default;

        /// <summary>
        /// NUnitEqualityComparer used to test equality.
        /// </summary>
        private TCLiteEqualityComparer _comparer = new TCLiteEqualityComparer();

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="EqualConstraint"/> class.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public EqualConstraint(object expected) : base(expected)
        {
            ExpectedValue = expected;
        }
        #endregion

        public object ExpectedValue { get; }

        public override string Description
        {
            get
            {
                var sb = new StringBuilder(ExpectedValue?.ToString() ?? "null");

                if (_tolerance != null && !_tolerance.IsDefault)
                {
                    sb.Append($" +/- {_tolerance.Amount}");
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


        #region Constraint Modifiers

        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        public EqualConstraint IgnoreCase
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
        public EqualConstraint NoClip
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
        public EqualConstraint AsCollection
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
        public EqualConstraint Within(object amount)
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
        public EqualConstraint Ulps
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
        public EqualConstraint Percent
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
        public EqualConstraint Days
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
        public EqualConstraint Hours
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
        public EqualConstraint Minutes
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
        public EqualConstraint Seconds
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
        public EqualConstraint Milliseconds
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
        public EqualConstraint Ticks
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
        public EqualConstraint Using(IComparer comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint Using<T>(IComparer<T> comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied Comparison object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint Using<T>(Comparison<T> comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint Using(IEqualityComparer comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        /// <returns>Self.</returns>
        public EqualConstraint Using<T>(IEqualityComparer<T> comparer)
        {
            _comparer.ExternalComparers.Add(EqualityAdapter.For(comparer));
            return this;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>True for success, false for failure</returns>
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            return new EqualConstraintResult(this, actual, _comparer.AreEqual(ExpectedValue, actual, ref _tolerance));
        }

        #endregion
	}
}