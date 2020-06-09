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
	public class ConditionAssertTests : AssertionTestBase
	{
		private const string ISTRUE_MESSAGE = "  Expected: True\n  But was:  False\n";
		private const string ISFALSE_MESSAGE = "  Expected: False\n  But was:  True\n";
		private const string ISNULL_MESSAGE = "  Expected: null\n  But was:  {0}\n";
		private const string ISNOTNULL_MESSAGE = "  Expected: not null\n  But was:  null\n";

		[Test]
		public void IsTrue()
		{
			Assert.True(true);
		}

		[Test] 
		public void IsTrueFails()
		{
			ThrowsAssertionException(() => Assert.True(false), ISTRUE_MESSAGE);
		}

		[Test]
		public void IsFalse()
		{
			Assert.False(false);
		}

		[Test]
		public void IsFalseFails()
		{
			ThrowsAssertionException(() => Assert.False(true), ISFALSE_MESSAGE);
		}

		[Test]
		public void IsNull()
		{
			Assert.Null(null);
		}

		[Test]
		public void IsNullFails()
		{
			ThrowsAssertionException(() => Assert.Null(42), string.Format(ISNULL_MESSAGE, 42));
		}

		[Test]
		public void IsNotNull()
		{
			String s1 = "S1";
			Assert.NotNull(s1);
		}

		[Test]
		public void IsNotNullFails()
		{
			ThrowsAssertionException(() => Assert.NotNull(null), ISNOTNULL_MESSAGE);
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
