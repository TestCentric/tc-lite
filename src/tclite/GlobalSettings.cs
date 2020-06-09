// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE.txt in root directory.
// ***********************************************************************

using System;

namespace TCLite.Framework
{
	/// <summary>
	/// GlobalSettings is a place for setting default values used
	/// by the framework in performing asserts.
	/// </summary>
	public class GlobalSettings
	{
		/// <summary>
		/// Default tolerance for floating point equality
		/// </summary>
		public static double DefaultFloatingPointTolerance = 0.0d;
	}
}
