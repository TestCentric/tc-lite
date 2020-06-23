// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace TCLite.Framework.Internal
{
    /// <summary>
    /// XmlHelper provides static methods for basic XML operations.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Creates a new top level element node.
        /// </summary>
        /// <param name="name">The element name.</param>
        /// <returns>A new XmlNode</returns>
        public static XmlNode CreateTopLevelElement(string name)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml( "<" + name + "/>" );
            return doc.FirstChild;
        }

        public static XmlNode CreateXmlNode(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.FirstChild;
        }

        /// <summary>
        /// Adds an attribute with a specified name and value to an existing XmlNode.
        /// </summary>
        /// <param name="node">The node to which the attribute should be added.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public static void AddAttribute(this XmlNode node, string name, string value)
        {
            XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
            attr.Value = EscapeInvalidXmlCharacters(value);
            node.Attributes.Append(attr);
        }

        /// <summary>
        /// Adds a new element as a child of an existing XmlNode and returns it.
        /// </summary>
        /// <param name="node">The node to which the element should be added.</param>
        /// <param name="name">The element name.</param>
        /// <returns>The newly created child element</returns>
        public static XmlNode AddElement(this XmlNode node, string name)
        {
            XmlNode childNode = node.OwnerDocument.CreateElement(name);
            node.AppendChild(childNode);
            return childNode;
        }

        /// <summary>
        /// Adds the a new element as a child of an existing node and returns it.
        /// A CDataSection is added to the new element using the data provided.
        /// </summary>
        /// <param name="node">The node to which the element should be added.</param>
        /// <param name="name">The element name.</param>
        /// <param name="data">The data for the CDataSection.</param>
        /// <returns></returns>
        public static XmlNode AddElementWithCDataSection(this XmlNode node, string name, string data)
        {
            XmlNode childNode = node.AddElement(name);
            childNode.AppendChild(node.OwnerDocument.CreateCDataSection(data));
            return childNode;
        }

        /// <summary>
        /// Gets the value of the given attribute.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string GetAttribute(this XmlNode result, string name)
        {
            XmlAttribute attr = result.Attributes[name];

            return attr == null ? null : attr.Value;
        }

        /// <summary>
        /// Gets the value of the given attribute as an int.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int GetAttribute(this XmlNode result, string name, int defaultValue)
        {
            XmlAttribute attr = result.Attributes[name];

            return attr == null
                ? defaultValue
                : int.Parse(attr.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the value of the given attribute as a double.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double GetAttribute(this XmlNode result, string name, double defaultValue)
        {
            XmlAttribute attr = result.Attributes[name];

            return attr == null
                ? defaultValue
                : double.Parse(attr.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the value of the given attribute as a DateTime.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DateTime GetAttribute(this XmlNode result, string name, DateTime defaultValue)
        {
            string dateStr = GetAttribute(result, name);
            if (dateStr == null)
                return defaultValue;

            DateTime date;
            if (!DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AllowWhiteSpaces, out date))
                return defaultValue;

            return date;
        }

        private static string EscapeInvalidXmlCharacters(string str)
        {
            if (str == null) return null;

            StringBuilder builder = null;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if(c > 0x20 && c < 0x7F)
                {
                    // ASCII characters - break quickly for these
                    if (builder != null)
                        builder.Append(c);
                }
                // From the XML specification: https://www.w3.org/TR/xml/#charsets
                // Char ::= #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]
                // Any Unicode character, excluding the surrogate blocks, FFFE, and FFFF.
                else if (!(0x0 <= c && c <= 0x8) &&
                    c != 0xB &&
                    c != 0xC &&
                    !(0xE <= c && c <= 0x1F) &&
                    !(0x7F <= c && c <= 0x84) &&
                    !(0x86 <= c && c <= 0x9F) &&
                    !(0xD800 <= c && c <= 0xDFFF) &&
                    c != 0xFFFE &&
                    c != 0xFFFF)
                {
                    if (builder != null)
                        builder.Append(c);
                }
                // Also check if the char is actually a high/low surrogate pair of two characters.
                // If it is, then it is a valid XML character (from above based on the surrogate blocks).
                else if (char.IsHighSurrogate(c) &&
                    i + 1 != str.Length &&
                    char.IsLowSurrogate(str[i + 1]))
                {
                    if (builder != null)
                    {
                        builder.Append(c);
                        builder.Append(str[i + 1]);
                    }
                    i++;
                }
                else
                {
                    // We keep the builder null so that we don't allocate a string
                    // when doing this conversion until we encounter a unicode character.
                    // Then, we allocate the rest of the string and escape the invalid
                    // character.
                    if (builder == null)
                    {
                        builder = new StringBuilder();
                        for (int index = 0; index < i; index++)
                            builder.Append(str[index]);
                    }
                    builder.Append(CharToUnicodeSequence(c));
                }
            }

            if (builder != null)
                return builder.ToString();
            else
                return str;
        }

        private static string CharToUnicodeSequence(char symbol)
        {
            return string.Format("\\u{0}", ((int)symbol).ToString("x4"));
        }
    }
}
