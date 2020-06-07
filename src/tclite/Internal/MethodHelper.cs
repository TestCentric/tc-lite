// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Reflection;
using System.Text;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// MethodHelper provides static methods for working with methods.
    /// </summary>
    public class MethodHelper
    {
        /// <summary>
        /// Gets the display name for a method as used by TCLite.
        /// </summary>
        /// <param name="method">The method for which a display name is needed.</param>
        /// <param name="arglist">The arguments provided.</param>
        /// <returns>The display name for the method</returns>
        public static string GetDisplayName(MethodInfo method, object[] arglist)
        {
            StringBuilder sb = new StringBuilder(method.Name);

            if (method.IsGenericMethod)
            {
                sb.Append("<");
                int cnt = 0;
                foreach (Type t in method.GetGenericArguments())
                {
                    if (cnt++ > 0) sb.Append(",");
                    sb.Append(t.Name);
                }
                sb.Append(">");
            }

            if (arglist != null)
            {
                sb.Append("(");

                for (int i = 0; i < arglist.Length; i++)
                {
                    if (i > 0) sb.Append(",");
                    sb.Append(GetDisplayString(arglist[i]));
                }

                sb.Append(")");
            }

            return sb.ToString();
        }

        private static string GetDisplayString(object arg)
        {
            string display = arg == null 
                ? "null"
                : Convert.ToString(arg, System.Globalization.CultureInfo.InvariantCulture);

            if (arg is double)
            {
                double d = (double)arg;

                if (double.IsNaN(d))
                    display = "double.NaN";
                else if (double.IsPositiveInfinity(d))
                    display = "double.PositiveInfinity";
                else if (double.IsNegativeInfinity(d))
                    display = "double.NegativeInfinity";
                else if (d == double.MaxValue)
                    display = "double.MaxValue";
                else if (d == double.MinValue)
                    display = "double.MinValue";
                else
                {
                    if (display.IndexOf('.') == -1)
                        display += ".0";
                    display += "d";
                }
            }
            else if (arg is float)
            {
                float f = (float)arg;

                if (float.IsNaN(f))
                    display = "float.NaN";
                else if (float.IsPositiveInfinity(f))
                    display = "float.PositiveInfinity";
                else if (float.IsNegativeInfinity(f))
                    display = "float.NegativeInfinity";
                else if (f == float.MaxValue)
                    display = "float.MaxValue";
                else if (f == float.MinValue)
                    display = "float.MinValue";
                else
                {
                    if (display.IndexOf('.') == -1)
                        display += ".0";
                    display += "f";
                }
            }
            else if (arg is decimal)
            {
                decimal d = (decimal)arg;
                if (d == decimal.MinValue)
                    display = "decimal.MinValue";
                else if (d == decimal.MaxValue)
                    display = "decimal.MaxValue";
                else
                    display += "m";
            }
            else if (arg is long)
            {
                long l = (long)arg;
                if (l == long.MinValue)
                    display = "long.MinValue";
                else if (l == long.MinValue)
                    display = "long.MaxValue";
                else
                    display += "L";
            }
            else if (arg is ulong)
            {
                ulong ul = (ulong)arg;
                if (ul == ulong.MinValue)
                    display = "ulong.MinValue";
                else if (ul == ulong.MinValue)
                    display = "ulong.MaxValue";
                else
                    display += "UL";
            }
            else if (arg is string)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char c in (string)arg)
                    sb.Append(EscapeControlChar(c));
                sb.Append("\"");
                display = sb.ToString();
            }
            else if (arg is char)
            {
                display = "\'" + EscapeControlChar((char)arg) + "\'";
            }
            else if (arg is int)
            {
                int ival = (int)arg;
                if (ival == int.MaxValue)
                    display = "int.MaxValue";
                else if (ival == int.MinValue)
                    display = "int.MinValue";
            }

            return display;
        }

        private static string EscapeControlChar(char c)
        {
            switch (c)
            {
                case '\'':
                    return "\\\'";
                case '\"':
                    return "\\\"";
                case '\\':
                    return "\\\\";
                case '\0':
                    return "\\0";
                case '\a':
                    return "\\a";
                case '\b':
                    return "\\b";
                case '\f':
                    return "\\f";
                case '\n':
                    return "\\n";
                case '\r':
                    return "\\r";
                case '\t':
                    return "\\t";
                case '\v':
                    return "\\v";

                case '\x0085':
                case '\x2028':
                case '\x2029':
                    return string.Format("\\x{0:X4}", (int)c);

                default:
                    return c.ToString();
            }
        }

#if NYI
        /// <summary>
        /// Returns true if the method specified by the argument
        /// is an async method.
        /// </summary>
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return method.IsDefined(typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute));
        }
#endif
    }
}
