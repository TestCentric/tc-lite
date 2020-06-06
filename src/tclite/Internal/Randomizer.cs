// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// Randomizer returns a set of random values in a repeatable
    /// way, to allow re-running of tests if necessary. 
    /// 
    /// This class is an internal framework class used for setting up tests. 
    /// It is used to generate random test parameters, at the time of loading
    /// the tests. It also generates seeds for use at execution time, when
    /// creating a RandomGenerator for use by the test.
    /// </summary>
    public class Randomizer : Random
    {
        #region Static Members

        private static int initialSeed = new Random().Next();

        private static Random seedGenerator;

        private static Dictionary<MemberInfo, Randomizer> randomizers = new Dictionary<MemberInfo, Randomizer>();

        /// <summary>
        /// Initial seed used to create randomizers for this run
        /// </summary>
        public static int InitialSeed
        {
            get { return initialSeed; }
            set { initialSeed = value; }
        }

        /// <summary>
        /// Get a randomizer for a particular member, returning
        /// one that has already been created if it exists.
        /// This ensures that the same values are generated
        /// each time the tests are reloaded.
        /// </summary>
        public static Randomizer GetRandomizer(MemberInfo member)
        {
            if (randomizers.ContainsKey(member))
                return (Randomizer)randomizers[member];
            else
            {
                Randomizer r = CreateRandomizer();
                randomizers[member] = r;
                return (Randomizer)r;
            }
        }

        /// <summary>
        /// Get a randomizer for a particular parameter, returning
        /// one that has already been created if it exists.
        /// This ensures that the same values are generated
        /// each time the tests are reloaded.
        /// </summary>
        public static Randomizer GetRandomizer(ParameterInfo parameter)
        {
            return GetRandomizer(parameter.Member);
        }

        /// <summary>
        /// Create a new Randomizer using the next seed
        /// available to ensure that each randomizer gives
        /// a unique sequence of values.
        /// </summary>
        /// <returns></returns>
        public static Randomizer CreateRandomizer()
        {
            if (seedGenerator == null)
                seedGenerator = new Random(initialSeed);

            return new Randomizer(seedGenerator.Next());
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Construct a randomizer using a specified seed
        /// </summary>
        public Randomizer(int seed) : base(seed) { }

        #endregion

        #region Public Methods
        /// <summary>
        /// Return an array of random doubles between 0.0 and 1.0.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public double[] GetDoubles(int count)
        {
            double[] rvals = new double[count];

            for (int index = 0; index < count; index++)
                rvals[index] = NextDouble();

            return rvals;
        }

        /// <summary>
        /// Return an array of random Enums
        /// </summary>
        /// <param name="count"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public object[] GetEnums(int count, Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException(string.Format("The specified type: {0} was not an enum", enumType));

#if !NETCF && !SILVERLIGHT
            Array values = Enum.GetValues(enumType);
#else
            Array values = TypeHelper.GetEnumValues(enumType);
#endif
            object[] rvals = new Enum[count];

            for (int index = 0; index < count; index++)
                rvals[index] = values.GetValue(Next(values.Length));

            return rvals;
        }

        /// <summary>
        /// Return an array of random doubles with values in a specified range.
        /// </summary>
        public double[] GetDoubles(double min, double max, int count)
        {
            double range = max - min;
            double[] rvals = new double[count];

            for (int index = 0; index < count; index++)
                rvals[index] = NextDouble() * range + min;

            return rvals;
        }

        /// <summary>
        /// Return an array of random ints with values in a specified range.
        /// </summary>
        public int[] GetInts(int min, int max, int count)
        {
            int[] ivals = new int[count];

            for (int index = 0; index < count; index++)
                ivals[index] = Next(min, max);

            return ivals;
        }

        #endregion
    }
}
