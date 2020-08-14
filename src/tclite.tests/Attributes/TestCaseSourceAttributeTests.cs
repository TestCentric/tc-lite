// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;
using TCLite.Internal;

namespace TCLite.Attributes
{
    public class TestCaseSourceAttributeTests
    {
        [TestCaseSource(typeof(DataSourceClass))]
        public void SourceCanBeInstanceOfIEnumerable(string source)
        {
            Assert.AreEqual("DataSourceClass", source);
        }

        class DataSourceClass : IEnumerable
        {
            public DataSourceClass()
            {
            }

            public IEnumerator GetEnumerator()
            {
                yield return "DataSourceClass";
            }
        }
    }
}
