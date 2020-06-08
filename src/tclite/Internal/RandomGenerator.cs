﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// RandomGenerator returns a set of random values in a repeatable
    /// way, to allow re-running of tests if necessary. 
    /// 
    /// This class is internal to the framework but exposed externally to through the TestContext 
    /// the class is used to allow for obtaining repeatable random values during a tests execution.
    /// this class should not be used inside the framework only with a TestMethod.
    /// </summary>
    public class RandomGenerator
    {
        #region Members & Constructor
        /// <summary>
        /// Seed for the wrapped Random
        /// </summary>
        public readonly int seed;

        private Random random;

        /// <summary>
        /// Lazy-loaded Random built on the readonly Seed
        /// </summary>
        private Random Rand
        {
            get
            {
                random = random == null ? new Random(seed) : random;
                return random;
            }
        }

        /// <summary>
        /// Constructor requires Seed value in order to store it for use in Random creation
        /// </summary>
        /// <param name="seed"></param>
        public RandomGenerator(int seed)
        {
            this.seed = seed;
        }
        #endregion

        #region Ints
        /// <summary>
        /// Get Next Integer from Random 
        /// </summary>
        /// <returns> int </returns>
        public int GetInt()
        {
            return Rand.Next();
        }
        /// <summary>
        /// Get Next Integer within the specified min & max from Random 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns> int </returns>
        public int GetInt(int min, int max)
        {
            return Rand.Next(min, max);
        }
        #endregion

        #region Shorts
        /// <summary>
        /// Get Next Short from Random
        /// </summary>
        /// <returns> short </returns>
        public short GetShort()
        {
            return (short)Rand.Next(short.MinValue, short.MaxValue);
        }
        /// <summary>
        /// Get Next Short within the specified min & max from Random 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns> short </returns>
        public short GetShort(short min, short max)
        {
            return (short)Rand.Next(min, max);
        }
        #endregion

        #region Bytes
        /// <summary>
        /// Get Next Byte from Random
        /// </summary>
        /// <returns> byte </returns>
        public byte GetByte()
        {
            return (byte)Rand.Next(Byte.MinValue, Byte.MaxValue);
        }
        /// <summary>
        /// Get Next Byte within the specified min & max from Random
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns> byte </returns>
        public byte GetByte(byte min, byte max)
        {
            return (byte)Rand.Next(min, max);
        }
        #endregion

        #region Bools
        /// <summary>
        /// Get Random Boolean value
        /// </summary>
        /// <returns> bool </returns>
        public bool GetBool()
        {
            return Rand.Next(0, 2) == 0;
        }
        /// <summary>
        /// Get Random Boolean value based on the probability of that value being true
        /// </summary>
        /// <param name="probability"></param>
        /// <returns> bool </returns>
        public bool GetBool(double probability)
        {
            return Rand.NextDouble() < Math.Abs(probability % 1.0);
        }
        #endregion

        #region Double & Float
        /// <summary>
        /// Get Next Double from Random
        /// </summary>
        /// <returns></returns>
        public double GetDouble()
        {
            return Rand.NextDouble();
        }
        /// <summary>
        /// Get Next Float from Random
        /// </summary>
        /// <returns></returns>
        public float GetFloat()
        {
            return (float)Rand.NextDouble();
        }
        #endregion

        #region Enums
        
        /// <summary>
        /// Return a random enum value representation of the specified Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns> T </returns>
        public T GetEnum<T>()
        {
            Array enums = Enum.GetValues(typeof(T));
            return (T)enums.GetValue(Rand.Next(0, enums.Length));
        }

        #endregion

    }
}
