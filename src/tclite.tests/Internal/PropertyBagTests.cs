// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Xml;

namespace TCLite.Framework.Internal
{
    [TestFixture]
    public class PropertyBagTests
    {
        private PropertyBag bag = CreatePropertyBag();

        [Test]
        public void IndexGetsListOfValues()
        {
            Assert.That(bag["Answer"].Count, Is.EqualTo(1));
            Assert.That(bag["Answer"], Contains.Item.EqualTo(42));

            Assert.That(bag["Tag"].Count, Is.EqualTo(2));
            Assert.That(bag["Tag"], Contains.Item.EqualTo("bug"));
            Assert.That(bag["Tag"], Contains.Item.EqualTo("easy"));
        }

        [Test]
        public void IndexGetsEmptyListIfNameIsNotPresent()
        {
            Assert.That(bag["Level"].Count, Is.EqualTo(0));
        }

        [Test]
        public void IndexSetsListOfValues()
        {
            bag["Zip"] = new string[] {"junk", "more junk"};
            Assert.That(bag["Zip"].Count, Is.EqualTo(2));
            Assert.That(bag["Zip"], Contains.Item.EqualTo("junk"));
            Assert.That(bag["Zip"], Contains.Item.EqualTo("more junk"));
        }

        [Test]
        public void AllKeysAreListed()
        {
            Assert.That(bag.Keys.Count, Is.EqualTo(2));
            Assert.That(bag.Keys, Has.Member.EqualTo("Answer"));
            Assert.That(bag.Keys, Has.Member.EqualTo("Tag"));
        }

        [Test]
        public void ContainsKey()
        {
            Assert.That(bag.ContainsKey("Answer"));
            Assert.That(bag.ContainsKey("Tag"));
            Assert.IsFalse(bag.ContainsKey("Target"));
        }

        [Test]
        public void GetReturnsSingleValue()
        {
            Assert.That(bag.Get("Answer"), Is.EqualTo(42));
            Assert.That(bag.Get("Tag"), Is.EqualTo("bug"));
        }

        [Test]
        public void SetAddsNewSingleValue()
        {
            bag.Set("Zip", "ZAP");
            Assert.That(bag["Zip"].Count, Is.EqualTo(1));
            Assert.That(bag["Zip"], Has.Member.EqualTo("ZAP"));
            Assert.That(bag.Get("Zip"), Is.EqualTo("ZAP"));
        }

        [Test]
        public void SetReplacesOldValues()
        {
            bag.Set("Tag", "ZAPPED");
            Assert.That(bag["Tag"].Count, Is.EqualTo(1));
            Assert.That(bag.Get("Tag"), Is.EqualTo("ZAPPED"));
        }

        [Test]
        public void XmlIsProducedCorrectly()
        {
            var topNode = bag.ToXml(true);
            Assert.That(topNode.Name, Is.EqualTo("properties"));

            string[] props = new string[topNode.ChildNodes.Count];
            for (int i = 0; i < topNode.ChildNodes.Count; i++)
            {
                XmlNode node = topNode.ChildNodes[i];

                Assert.That(node.Name, Is.EqualTo("property"));
                
                var name = node.GetAttribute("name");
                var value = node.GetAttribute("value");
                props[i] = $"{name}={value}";
            }
            Assert.That(props,
                Is.EqualTo(new string[] { "Answer=42", "Tag=bug", "Tag=easy" }));
        }

        private static PropertyBag CreatePropertyBag()
        {
            var bag = new PropertyBag();
            bag.Add("Answer", 42);
            bag.Add("Tag", "bug");
            bag.Add("Tag", "easy");

            return bag;
        }

        // TODO: Do we need this feature?
        // [Test]
        // public void TestNullPropertyValueIsntAdded()
        // {
        //     Assert.Throws<ArgumentNullException>(() => bag.Add("dontAddMe", null));
        //     Assert.IsFalse(bag.ContainsKey("dontAddMe"));
        // }
    }
}
