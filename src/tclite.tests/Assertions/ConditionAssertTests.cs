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
            var ex = Assert.Throws<AssertionException>(() => Assert.True(false));
            Assert.AreEqual(ISTRUE_MESSAGE, ex.Message);
		}

		[Test]
		public void IsFalse()
		{
			Assert.False(false);
		}

		[Test]
		public void IsFalseFails()
		{
            var ex = Assert.Throws<AssertionException>(() => Assert.False(true));
            Assert.AreEqual(ISFALSE_MESSAGE, ex.Message);
		}

		[Test]
		public void IsNull()
		{
			Assert.Null(null);
		}

		[Test]
		public void IsNullFails()
		{
            var ex = Assert.Throws<AssertionException>(() => Assert.Null(42));
            Assert.AreEqual(string.Format(ISNULL_MESSAGE, 42) , ex.Message);
		}

		[Test]
		public void IsNotNull()
		{
			String s1 = "S1";
			Assert.NotNull(s1);
		}

		[Test]
		public void WIPTest()
		{
			var NullConstraint = new Constraints.NullConstraint();
			Assert.That(NullConstraint.ApplyTo(null).IsSuccess);
			Assert.True(NullConstraint.ApplyTo(null).IsSuccess);
			Assert.True(!NullConstraint.ApplyTo(42).IsSuccess);
			Assert.False(NullConstraint.ApplyTo(42).IsSuccess);
			Assert.True(Is.Null.ApplyTo(null).IsSuccess);
			Assert.False(Is.Null.ApplyTo(42).IsSuccess);

			var NotConstraint = new Constraints.NotConstraint(NullConstraint);
			Assert.False(NotConstraint.ApplyTo(null).IsSuccess);
			Assert.False(NotConstraint.ApplyTo(null).IsSuccess);
			Assert.False(!NotConstraint.ApplyTo(42).IsSuccess);
			Assert.True(NotConstraint.ApplyTo(42).IsSuccess);

			Assert.AreEqual("<null>", Is.Null.ToString());
			Assert.AreEqual("<unresolved <null>>", Is.Not.Null.ToString());
			Assert.AreEqual("<not <null>>", ((Constraints.IResolveConstraint)Is.Not.Null).Resolve().ToString());
			
			Assert.False(((Constraints.IResolveConstraint)Is.Not.Null).Resolve().ApplyTo(null).IsSuccess);
			Assert.True(((Constraints.IResolveConstraint)Is.Not.Null).Resolve().ApplyTo(42).IsSuccess);

			Assert.That(null, Is.Null, "Assert.That(null, Is.Null)");
			Assert.That(42, Is.Not.Null, "Assert.That(null, Is.Not.Null)");
		}

		[Test]
		public void IsNotNullFails()
		{
            var ex = Assert.Throws<AssertionException>(() => Assert.NotNull(null));
            Assert.AreEqual(ISNOTNULL_MESSAGE, ex.Message);
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
