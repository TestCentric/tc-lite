// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using System.Text;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// ExceptionHelper provides static methods for working with exceptions
    /// </summary>
    public class ExceptionHelper
    {
        // TODO: Move to a utility class
        /// <summary>
        /// Builds up a message, using the Message field of the specified exception
        /// as well as any InnerExceptions.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A combined message string.</returns>
        public static string BuildMessage(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.CurrentCulture, "{0} : {1}", exception.GetType().ToString(), exception.Message);

            Exception inner = exception.InnerException;
            while (inner != null)
            {
                sb.Append(Environment.NewLine);
                sb.AppendFormat(CultureInfo.CurrentCulture, "  ----> {0} : {1}", inner.GetType().ToString(), inner.Message);
                inner = inner.InnerException;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Builds up a message, using the Message field of the specified exception
        /// as well as any InnerExceptions.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A combined stack trace.</returns>
        public static string BuildStackTrace(Exception exception)
        {
            StringBuilder sb = new StringBuilder(GetStackTrace(exception));

            Exception inner = exception.InnerException;
            while (inner != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append("--");
                sb.Append(inner.GetType().Name);
                sb.Append(Environment.NewLine);
                sb.Append(GetStackTrace(inner));

                inner = inner.InnerException;
            }

            return sb.ToString();
        }

        private static string GetStackTrace(Exception exception)
        {
            try
            {
                return exception.StackTrace;
            }
            catch (Exception)
            {
                return "No stack trace available";
            }
        }
    }
}
