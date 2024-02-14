// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System;

namespace TCLite.Interfaces
{
    public interface IRandomGenerator
    {
        /// <summary>
        /// Returns a random int.
        /// </summary>
        int Next();

        /// <summary>
        /// Returns a random int less than the specified maximum.
        /// </summary>
        int Next(int max);

        /// <summary>
        /// Returns a random int within a specified range.
        /// </summary>
        int Next(int min, int max);

        /// <summary>
        /// Returns a random unsigned int.
        /// </summary>
        uint NextUInt();

        /// <summary>
        /// Returns a random unsigned int less than the specified maximum.
        /// </summary>
        uint NextUInt(uint max);

        /// <summary>
        /// Returns a random unsigned int within a specified range.
        /// </summary>
        uint NextUInt(uint min, uint max);

        /// <summary>
        /// Returns a random short.
        /// </summary>
        short NextShort();

        /// <summary>
        /// Returns a random short less than the specified maximum.
        /// </summary>
        short NextShort(short max);

        /// <summary>
        /// Returns a random short within a specified range.
        /// </summary>
        short NextShort(short min, short max);

        /// <summary>
        /// Returns a random unsigned short.
        /// </summary>
        ushort NextUShort();

        /// <summary>
        /// Returns a random unsigned short less than the specified maximum.
        /// </summary>
        ushort NextUShort(ushort max);

        /// <summary>
        /// Returns a random unsigned short within a specified range.
        /// </summary>
        ushort NextUShort(ushort min, ushort max);

        /// <summary>
        /// Returns a random long.
        /// </summary>
        long NextLong();

        /// <summary>
        /// Returns a random long less than the specified maximum.
        /// </summary>
        long NextLong(long max);

        /// <summary>
        /// Returns a random long within a specified range.
        /// </summary>
        long NextLong(long min, long max);

        /// <summary>
        /// Returns a random unsigned long.
        /// </summary>
        ulong NextULong();

        /// <summary>
        /// Returns a random unsigned long less than the specified maximum.
        /// </summary>
        ulong NextULong(ulong max);

        /// <summary>
        /// Returns a random unsigned long within a specified range.
        /// </summary>
        ulong NextULong(ulong min, ulong max);

        /// <summary>
        /// Returns a random byte.
        /// </summary>
        byte NextByte();

        /// <summary>
        /// Returns a random byte less than the specified maximum.
        /// </summary>
        byte NextByte(byte max);

        /// <summary>
        /// Returns a random byte within a specified range.
        /// </summary>
        byte NextByte(byte min, byte max);

        /// <summary>
        /// Returns a random sbyte.
        /// </summary>
        sbyte NextSByte();

        /// <summary>
        /// Returns a random sbyte less than the specified maximum.
        /// </summary>
        sbyte NextSByte(sbyte max);

        /// <summary>
        /// Returns a random sbyte within a specified range.
        /// </summary>
        sbyte NextSByte(sbyte min, sbyte max);

        /// <summary>
        /// Returns a random double.
        /// </summary>
        double NextDouble();

        /// <summary>
        /// Returns a random double less than the specified maximum.
        /// </summary>
        double NextDouble(double max);

        /// <summary>
        /// Returns a random double within a specified range.
        /// </summary>
        double NextDouble(double min, double max);

        /// <summary>
        /// Returns a random float.
        /// </summary>
        float NextFloat();

        /// <summary>
        /// Returns a random float less than the specified maximum.
        /// </summary>
        float NextFloat(float max);

        /// <summary>
        /// Returns a random float within a specified range.
        /// </summary>
        float NextFloat(float min, float max);

        /// <summary>
        /// Returns a random decimal.
        /// </summary>
        decimal NextDecimal();

        /// <summary>
        /// Returns a random decimal less than the specified maximum.
        /// </summary>
        decimal NextDecimal(decimal max);

        /// <summary>
        /// Returns a random decimal within a specified range.
        /// </summary>
        decimal NextDecimal(decimal min, decimal max);

        /// <summary>
        /// Returns a random bool, with 50% probability of being true.
        /// </summary>
        bool NextBool();

        /// <summary>
        /// Returns a random bool, with the specified probablility of being true.
        /// </summary>
        bool NextBool(double probability);

        /// <summary>
        /// Returns a random enum of the specified Type.
        /// </summary>
        object NextEnum(Type type);

        /// <summary>
        /// Returns a random enum of the specified Type.
        /// </summary>
        T NextEnum<T>();

        /// <summary>
        /// Returns a random Guid.
        /// </summary>
        Guid NextGuid();

        // /// <summary>
        // /// Returns a random string.
        // /// </summary>
        // /// <returns></returns>
        // string GetString();

        /// <summary>
        /// Returns a random string of the specified length, made up of upper and lower case letters, digits and the underscore character.
        /// </summary>
        string GetString(int outputLength);

        /// <summary>
        /// Returns a random string of the specified length, made up of the characters specified.
        /// </summary>
        /// <param name="outputlength"></param>
        /// <param name="allowedChars"></param>
        /// <returns></returns>
        string GetString(int outputlength, string allowedChars);
    }
}