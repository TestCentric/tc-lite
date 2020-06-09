﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE in root directory.
// ***********************************************************************

using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace TCLite.Framework.Internal
{
    public class AssemblyHelperTests
    {
        [NUnit.Framework.Test]
        public void GetPathForAssembly()
        {
            string path = AssemblyHelper.GetAssemblyPath(this.GetType().Assembly);
            Assert.That(Path.GetFileName(path), Is.EqualTo("tclite.tests.dll"));
            Assert.That(File.Exists(path));
        }
#if NYI
        //[NUnit.Framework.Test]
        //public void GetPathForType()
        //{
        //    string path = AssemblyHelper.GetAssemblyPath(this.GetType());
        //    Assert.That(Path.GetFileName(path), Is.EqualTo("nunitlite.tests.exe").IgnoreCase);
        //    Assert.That(File.Exists(path));
        //}
		
        // The following tests are only useful to the extent that the test cases
        // match what will actually be provided to the method in production.
        // As currently used, NUnit's codebase can only use the file: schema,
        // since we don't load assemblies from anything but files. The uri's
        // provided can be absolute file paths or UNC paths.

        // Local paths - Windows Drive
        // [NUnit.Framework.TestCase(@"file:///C:/path/to/assembly.dll", @"C:\path\to\assembly.dll")]
        // [NUnit.Framework.TestCase(@"file:///C:/my path/to my/assembly.dll", @"C:/my path/to my/assembly.dll")]
        // [NUnit.Framework.TestCase(@"file:///C:/dev/C#/assembly.dll", @"C:\dev\C#\assembly.dll")]
        // [NUnit.Framework.TestCase(@"file:///C:/dev/funnychars?:=/assembly.dll", @"C:\dev\funnychars?:=\assembly.dll")]
        // Local paths - Linux or Windows absolute without a drive
        [NUnit.Framework.TestCase(@"file:///path/to/assembly.dll", @"/path/to/assembly.dll")]
        [NUnit.Framework.TestCase(@"file:///my path/to my/assembly.dll", @"/my path/to my/assembly.dll")]
        [NUnit.Framework.TestCase(@"file:///dev/C#/assembly.dll", @"/dev/C#/assembly.dll")]
        [NUnit.Framework.TestCase(@"file:///dev/funnychars?:=/assembly.dll", @"/dev/funnychars?:=/assembly.dll")]
        // Windows drive specified as if it were a server - odd case, sometimes seen
        // [NUnit.Framework.TestCase(@"file://C:/path/to/assembly.dll", @"C:\path\to\assembly.dll")]
        // [NUnit.Framework.TestCase(@"file://C:/my path/to my/assembly.dll", @"C:\my path\to my\assembly.dll")]
        // [NUnit.Framework.TestCase(@"file://C:/dev/C#/assembly.dll", @"C:\dev\C#\assembly.dll")]
        // [NUnit.Framework.TestCase(@"file://C:/dev/funnychars?:=/assembly.dll", @"C:\dev\funnychars?:=\assembly.dll")]
        // UNC format with server and path
        [NUnit.Framework.TestCase(@"file://server/path/to/assembly.dll", @"//server/path/to/assembly.dll")]
        [NUnit.Framework.TestCase(@"file://server/my path/to my/assembly.dll", @"//server/my path/to my/assembly.dll")]
        [NUnit.Framework.TestCase(@"file://server/dev/C#/assembly.dll", @"//server/dev/C#/assembly.dll")]
        [NUnit.Framework.TestCase(@"file://server/dev/funnychars?:=/assembly.dll", @"//server/dev/funnychars?:=/assembly.dll")]
        //[NUnit.Framework.TestCase(@"http://server/path/to/assembly.dll", "//server/path/to/assembly.dll")]
        public void GetAssemblyPathFromCodeBase(string uri, string expectedPath)
        {
            string localPath = AssemblyHelper.GetAssemblyPathFromCodeBase(uri);
            Assert.That(localPath, Is.SamePath(expectedPath));
        }
#endif
    }
}
