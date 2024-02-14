// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Reflection;
using TCLite.Constraints;
using TCLite.Interfaces;
using TCLite.Internal;

namespace TCLite
{
    /// <summary>
    /// Supplies a range of values to an individual parameter of a parameterized test.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public class RangeAttribute : DataParamAttribute, IParameterDataSource
    {
        private readonly object _from;
        private readonly object _to;
        private readonly object _step;

        #region Ints

        /// <summary>
        /// Constructs a range of <see cref="int"/> values using the default step of 1.
        /// </summary>
        public RangeAttribute(int from, int to) : this(from, to, 1) { }

        /// <summary>
        /// Constructs a range of <see cref="int"/> values with the specified step size.
        /// </summary>
        public RangeAttribute(int from, int to, int step)
        {
            Guard.ArgumentValid(step > 0, "Step must be greater than zero", nameof(step));
            Guard.ArgumentValid(to >= from, "Value of to must be greater than or equal to from", nameof(to));

            _from = from;
            _to = to;
            _step = step;
        }

        #endregion

        #region Unsigned Ints

        /// <summary>
        /// Constructs a range of <see cref="uint"/> values using the default step of 1.
        /// </summary>
        public RangeAttribute(uint from, uint to) : this(from, to, 1u) { }

        /// <summary>
        /// Constructs a range of <see cref="uint"/> values with the specified step size.
        /// </summary>
        public RangeAttribute(uint from, uint to, uint step)
        {
            Guard.ArgumentValid(step > 0, "Step must be greater than zero", nameof(step));
            Guard.ArgumentValid(to >= from, "Value of to must be greater than or equal to from", nameof(to));

            _from = from;
            _to = to;
            _step = step;
        }

        #endregion

        #region Longs

        /// <summary>
        /// Constructs a range of <see cref="long"/> values using a default step of 1.
        /// </summary>
        public RangeAttribute(long from, long to) : this(from, to, 1L) { }

        /// <summary>
        /// Constructs a range of <see cref="long"/> values with the specified step size.
        /// </summary>
        public RangeAttribute(long from, long to, long step)
        {
            Guard.ArgumentValid(step > 0L, "Step must be greater than zero", nameof(step));
            Guard.ArgumentValid(to >= from, "Value of to must be greater than or equal to from", nameof(to));

            _from = from;
            _to = to;
            _step = step;
        }

        #endregion

        #region Unsigned Longs

        /// <summary>
        /// Constructs a range of <see cref="ulong"/> values using the default step of 1.
        /// </summary>
        public RangeAttribute(ulong from, ulong to) : this(from, to, 1ul) { }

        /// <summary>
        /// Constructs a range of <see cref="ulong"/> values with the specified step size.
        /// </summary>
        public RangeAttribute(ulong from, ulong to, ulong step)
        {
            Guard.ArgumentValid(step > 0, "Step must be greater than zero", nameof(step));
            Guard.ArgumentValid(to >= from, "Value of to must be greater than or equal to from", nameof(to));

            _from = from;
            _to = to;
            _step = step;
        }

        #endregion

        #region Doubles

        /// <summary>
        /// Constructs a range of <see cref="double"/> values with the specified step size.
        /// </summary>
        public RangeAttribute(double from, double to, double step)
        {
            Guard.ArgumentValid(step > 0.0D, "Step must be greater than zero", nameof(step));
            Guard.ArgumentValid(to >= from, "Value of to must be greater than or equal to from", nameof(to));

            _from = from;
            _to = to;
            _step = step;
        }

        #endregion

        #region Floats

        /// <summary>
        /// Constructs a range of <see cref="float"/> values with the specified step size.
        /// </summary>
        public RangeAttribute(float from, float to, float step)
        {
            Guard.ArgumentValid(step > 0.0F, "Step must be greater than zero", nameof(step));
            Guard.ArgumentValid(to >= from, "Value of to must be greater than or equal to from", nameof(to));

            _from = from;
            _to = to;
            _step = step;
        }

        #endregion

        /// <summary>
        /// Retrieves a list of arguments which can be passed to the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter of a parameterized test.</param>
        public IEnumerable GetData(ParameterInfo parameter) 
        {
            var targetType = parameter.ParameterType;
            Guard.OperationValid(Numerics.IsNumericType(targetType), "RangeAttribute is only valid on numeric parameters");

            var from = Convert(_from, targetType);
            var to = Convert(_to, targetType);
            var step = Convert(_step, targetType);

            if (targetType == typeof(double))
                return GenerateRange((double)from, (double)to, (double)step);

            if (targetType == typeof(float))
                return GenerateRange((float)from, (float)to, (float)step);

            if (targetType == typeof(ulong))
                return GenerateRange((ulong)from, (ulong)to, (ulong)step);

            if (targetType == typeof(long))
                return GenerateRange((long)from, (long)to, (long)step);

            if (targetType == typeof(uint))
                return GenerateRange((uint)from, (uint)to, (uint)step);

            return GenerateRange((int)from, (int)to, (int)step);
        }

        IEnumerable GenerateRange(double from, double to, double step)
        {
            for (double val = from; val <= to; val += step)
                yield return val;
        }

        IEnumerable GenerateRange(float from, float to, float step)
        {
            for (float val = from; val <= to; val += step)
                yield return val;
        }

        IEnumerable GenerateRange(ulong from, ulong to, ulong step)
        {
            for (ulong val = from; val <= to; val += step)
                yield return val;
        }

        IEnumerable GenerateRange(long from, long to, long step)
        {
            for (long val = from; val <= to; val += step)
                yield return val;
        }

        IEnumerable GenerateRange(uint from, uint to, uint step)
        {
            for (uint val = from; val <= to; val += step)
                yield return val;
        }

        IEnumerable GenerateRange(int from, int to, int step)
        {
            for (int val = from; val <= to; val += step)
                yield return val;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        public override string ToString()
        {
            return $"{_from} .. {_step} .. {_to}";
        }
    }
}
