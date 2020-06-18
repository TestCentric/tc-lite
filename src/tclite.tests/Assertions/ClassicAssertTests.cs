// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System;
using System.Collections;
using System.Threading;
using System.Globalization;
using TCLite.Framework;

namespace TCLite.Framework.Assertions
{
	[TestFixture]
	public class ClassicAssertTests : AssertionTestBase
	{
		[Test]
		public void IsTrue()
		{
			Assert.IsTrue(true);
		}

		[Test] 
		public void IsTrueFails()
		{
            var ex = Assert.Throws<AssertionException>(() => Assert.IsTrue(false));
            Assert.AreEqual(StandardErrorMessage(true, false), ex.Message);
		}

		[Test]
		public void IsFalse()
		{
			Assert.IsFalse(false);
		}

		[Test]
		public void IsFalseFails()
		{
            var ex = Assert.Throws<AssertionException>(() => Assert.IsFalse(true));
            Assert.AreEqual(StandardErrorMessage(false, true), ex.Message);
		}

		[Test]
		public void IsNull()
		{
			Assert.IsNull(null);
		}

		[Test]
		public void IsNullFails()
		{
            var ex = Assert.Throws<AssertionException>(() => Assert.IsNull(42));
            Assert.AreEqual(StandardErrorMessage("null", 42) , ex.Message);
		}

		[Test]
		public void IsNotNull()
		{
			String s1 = "S1";
			Assert.IsNotNull(s1);
		}

		[Test]
		public void IsNotNullFails()
		{
            var ex = Assert.Throws<AssertionException>(() => Assert.IsNotNull(null));
            Assert.AreEqual(StandardErrorMessage("not null", "null"), ex.Message);
		}

#if NYI
		[Test]
		public void IsNaN()
		{
			Assert.IsNaN(double.NaN);
		}

		[Test]
		public void IsEmpty()
		{
			Assert.IsEmpty( "", "Failed on empty String" );
			Assert.IsEmpty( new int[0], "Failed on empty Array" );
			Assert.IsEmpty( new ArrayList(), "Failed on empty ArrayList" );
			Assert.IsEmpty( new Hashtable(), "Failed on empty Hashtable" );
			Assert.IsEmpty( (IEnumerable)new int[0], "Failed on empty IEnumerable" );
		}

		[Test]
		public void IsNotEmpty()
		{
			int[] array = new int[] { 1, 2, 3 };
			ArrayList list = new ArrayList( array );
			Hashtable hash = new Hashtable();
			hash.Add( "array", array );

			Assert.IsNotEmpty( "Hi!", "Failed on String" );
			Assert.IsNotEmpty( array, "Failed on Array" );
			Assert.IsNotEmpty( list, "Failed on ArrayList" );
			Assert.IsNotEmpty( hash, "Failed on Hashtable" );
			Assert.IsNotEmpty( (IEnumerable)array, "Failed on IEnumerable" );
		}
#endif
	}
}
