// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;
using System.IO;
using System.Text;

namespace TCLite.Runners
{
    public class ColorConsoleWriter : ExtendedTextWrapper
    {
        // ReSharper disable once InconsistentNaming
        // Disregarding naming convention for back-compat
        public bool _colorEnabled;

        /// <summary>
        /// Construct a ColorConsoleWriter.
        /// </summary>
        public ColorConsoleWriter() : this(true) { }

        /// <summary>
        /// Construct a ColorConsoleWriter.
        /// </summary>
        /// <param name="colorEnabled">Flag indicating whether color should be enabled</param>
        public ColorConsoleWriter(bool colorEnabled)
            : base(Console.Out)
        {
            _colorEnabled = colorEnabled;
        }

        #region Extended Methods
        
        /// <summary>
        /// Writes the value with the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="value">The value.</param>
        public override void Write(ColorStyle style, string value)
        {
            if (_colorEnabled)
                using (new ColorConsole(style))
                {
                    Write(value);
                }
            else
                Write(value);
        }

        /// <summary>
        /// Writes the value with the specified style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="value">The value.</param>
        public override void WriteLine(ColorStyle style, string value)
        {
            if (_colorEnabled)
                using (new ColorConsole(style))
                {
                    WriteLine(value);
                }
            else
                WriteLine(value);
        }

        /// <summary>
        /// Writes the label and the option that goes with it and optionally writes a new line.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="option">The option.</param>
        /// <param name="valueStyle">The color to display the value with</param>
        public override void WriteLabel(string label, object option, ColorStyle valueStyle = ColorStyle.Value)
        {
            Write(ColorStyle.Label, label);
            Write(valueStyle, option.ToString());
        }

        /// <summary>
        /// Writes the label and the option that goes with it followed by a new line.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="option">The option.</param>
        /// <param name="valueStyle">The color to display the value with</param>
        public override void WriteLabelLine(string label, object option, ColorStyle valueStyle = ColorStyle.Value)
        {
            WriteLabel(label, option, valueStyle);
            WriteLine();
        }

        #endregion
    }
}
