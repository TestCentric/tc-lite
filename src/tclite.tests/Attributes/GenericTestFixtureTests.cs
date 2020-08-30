// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TCLite.Internal
{
    [TestFixture(TypeArgs=new Type[] {typeof(List<int>)} )]
    [TestFixture(TypeArgs=new Type[] {typeof(List<object>)} )]
    [TestFixture(TypeArgs=new Type[] {typeof(ArrayList)} )]
    public class GenericTestFixture_IList<T> where T : IList, new()
    {
        [TestCase]
        public void TestCollectionCount()
        {
            IList list = new T();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.AreEqual(3, list.Count);
        }
    }

    [TestFixture(TypeArgs=new Type[] { typeof(double) } )]
    public class GenericTestFixture_Numeric<T>
    {
        [TestCase(5)]
        [TestCase(1.23)]
        public void TestMyArgType(T x)
        {
            Assert.That(x, Is.TypeOf(typeof(T)));
        }
    }
}
