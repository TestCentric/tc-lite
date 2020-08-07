// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Constraints
{
    /// <summary>
    /// The Tolerance class generalizes the notion of a tolerance
    /// within which an equality test succeeds. Normally, it is
    /// used with numeric types, but it can be used with any
    /// type that supports taking a difference between two 
    /// objects and comparing that difference to a value.
    /// </summary>
    public class Tolerance
    {
        private const string ModeMustFollowTolerance = "Tolerance amount must be specified before setting mode";
        private const string MultipleToleranceModes = "Tried to use multiple tolerance modes at the same time";
        private const string NumericToleranceRequired = "A numeric tolerance is required";

        /// <summary>
        /// Returns an empty Tolerance object, equivalent to
        /// specifying no tolerance. In most cases, it results
        /// in an exact match but for floats and doubles a
        /// default tolerance may be used.
        /// </summary>
        public static Tolerance Default
        {
            get { return new Tolerance(0, ToleranceMode.Default); }
        }

        /// <summary>
        /// Returns a default Tolerance object, equivalent to an exact match.
        /// </summary>
        public static Tolerance Exact
        {
            get { return new Tolerance(0, ToleranceMode.Linear); }
        }

        /// <summary>
        /// Constructs a linear tolerance of a specified amount
        /// </summary>
        public Tolerance(object amount) : this(amount, ToleranceMode.Linear) { }

        /// <summary>
        /// Constructs a tolerance given an amount and ToleranceMode
        /// </summary>
        private Tolerance(object amount, ToleranceMode mode)
        {
            Amount = amount;
            Mode = mode;
        }

        /// <summary>
        /// Gets the magnitude of the current Tolerance instance.
        /// </summary>
        public object Amount { get; }

        /// <summary>
        /// the ToleranceMode for the current Tolerance
        /// </summary>
        public ToleranceMode Mode { get; }
        
        /// <summary>
        /// Returns true if the current tolerance is empty.
        /// </summary>
        public bool IsDefault
        {
            get { return Mode == ToleranceMode.Default; }
        }

        #region Modifier Properties

        /// <summary>
        /// Returns a new tolerance, using the current amount as a percentage.
        /// </summary>
        public Tolerance Percent
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(Amount, ToleranceMode.Percent);
            }
        }

        /// <summary>
        /// Returns a new tolerance, using the current amount in Ulps.
        /// </summary>
        public Tolerance Ulps
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(Amount, ToleranceMode.Ulps);
            }
        }

        /// <summary>
        /// Returns a new tolerance with a TimeSpan as the amount, using 
        /// the current amount as a number of days.
        /// </summary>
        public Tolerance Days
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromDays(Convert.ToDouble(Amount)));
            }
        }

        /// <summary>
        /// Returns a new tolerance with a TimeSpan as the amount, using 
        /// the current amount as a number of hours.
        /// </summary>
        public Tolerance Hours
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromHours(Convert.ToDouble(Amount)));
            }
        }

        /// <summary>
        /// Returns a new tolerance with a TimeSpan as the amount, using 
        /// the current amount as a number of minutes.
        /// </summary>
        public Tolerance Minutes
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromMinutes(Convert.ToDouble(Amount)));
            }
        }

        /// <summary>
        /// Returns a new tolerance with a TimeSpan as the amount, using 
        /// the current amount as a number of seconds.
        /// </summary>
        public Tolerance Seconds
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromSeconds(Convert.ToDouble(Amount)));
            }
        }

        /// <summary>
        /// Returns a new tolerance with a TimeSpan as the amount, using 
        /// the current amount as a number of milliseconds.
        /// </summary>
        public Tolerance Milliseconds
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromMilliseconds(Convert.ToDouble(Amount)));
            }
        }

        /// <summary>
        /// Returns a new tolerance with a TimeSpan as the amount, using 
        /// the current amount as a number of clock ticks.
        /// </summary>
        public Tolerance Ticks
        {
            get
            {
                CheckLinearAndNumeric();
                return new Tolerance(TimeSpan.FromTicks(Convert.ToInt64(Amount)));
            }
        }

        #endregion

        /// <summary>
        /// Tests that the current Tolerance is linear with a 
        /// numeric value, throwing an exception if it is not.
        /// </summary>
        private void CheckLinearAndNumeric()
        {
            if (Mode != ToleranceMode.Linear)
                throw new InvalidOperationException(Mode == ToleranceMode.Default
                    ? ModeMustFollowTolerance
                    : MultipleToleranceModes);

            if (!Numerics.IsNumericType(Amount))
                throw new InvalidOperationException(NumericToleranceRequired);
        }
    }
}