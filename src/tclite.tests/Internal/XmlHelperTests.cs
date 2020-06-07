﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;

namespace TCLite.Framework.Internal
{
    public class XmlHelperTests
    {
        [NUnit.Framework.Test]
        public void SingleElement()
        {
            XmlNode node = XmlHelper.CreateTopLevelElement("myelement");

            Assert.That(node.Name, Is.EqualTo("myelement"));
            Assert.That(node.Attributes.Count, Is.EqualTo(0));
            Assert.That(node.ChildNodes.Count, Is.EqualTo(0));
        }

        [NUnit.Framework.Test]
        public void SingleElementWithAttributes()
        {
            XmlNode node = XmlHelper.CreateTopLevelElement("person");
            XmlHelper.AddAttribute(node, "name", "Fred");
            XmlHelper.AddAttribute(node, "age", "42");
            XmlHelper.AddAttribute(node, "quotes", "'c' is a char but \"c\" is a string");

            Assert.That(node.Name, Is.EqualTo("person"));
            Assert.That(node.Attributes.Count, Is.EqualTo(3));
            Assert.That(node.ChildNodes.Count, Is.EqualTo(0));
            Assert.That(node.Attributes["name"].Value, Is.EqualTo("Fred"));
            Assert.That(node.Attributes["age"].Value, Is.EqualTo("42"));
            Assert.That(node.Attributes["quotes"].Value, Is.EqualTo("'c' is a char but \"c\" is a string"));
        }

        [NUnit.Framework.Test]
        public void ElementContainsElementWithInnerText()
        {
            XmlNode top = XmlHelper.CreateTopLevelElement("top");
            XmlNode message = top.AddElement("message");
            message.InnerText = "This is my message";

            Assert.That(top.SelectSingleNode("message").InnerText, Is.EqualTo("This is my message"));
        }

        [NUnit.Framework.Test]
        public void ElementContainsElementWithCData()
        {
            XmlNode top = XmlHelper.CreateTopLevelElement("top");
            top.AddElementWithCDataSection("message", "x > 5 && x < 7");

            Assert.That(top.SelectSingleNode("message").InnerText, Is.EqualTo("x > 5 && x < 7"));
        }

        [NUnit.Framework.Test]
        public void SafeAttributeAccess()
        {
            XmlNode node = XmlHelper.CreateTopLevelElement("top");

            Assert.That(XmlHelper.GetAttribute(node, "junk"), Is.Null);
        }

        [NUnit.Framework.Test]
        public void SafeAttributeAccessWithIntDefaultValue()
        {
            XmlNode node = XmlHelper.CreateTopLevelElement("top");
            Assert.That(XmlHelper.GetAttribute(node, "junk", 42), Is.EqualTo(42));
        }

        [NUnit.Framework.Test]
        public void SafeAttributeAccessWithDoubleDefaultValue()
        {
            XmlNode node = XmlHelper.CreateTopLevelElement("top");
            Assert.That(node.GetAttribute("junk", 1.234), Is.EqualTo(1.234));
        }
    }
}
