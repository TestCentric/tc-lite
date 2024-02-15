﻿// ***********************************************************************
// Copyright (c) Charlie Poole and TestCentric contributors.
// Licensed under the MIT License. See LICENSE file in root directory.
// ***********************************************************************

using System.IO;
using System.Reflection;

namespace TCLite.Internal
{
    public class AssemblyHelperTests
    {
        [TestCase]
        public void GetPathForAssembly()
        {
            string path = AssemblyHelper.GetAssemblyPath(this.GetType().Assembly);
            Assert.That(Path.GetFileName(path), Is.EqualTo("tclite.tests.dll"));
            Assert.That(File.Exists(path));
        }

        [TestCase]
        public void GetPathForType()
        {
           string path = AssemblyHelper.GetAssemblyPath(this.GetType());
           Assert.That(Path.GetFileName(path), Is.EqualTo("tclite.tests.dll").IgnoreCase);
           Assert.That(File.Exists(path));
        }
		
        // The following tests are only useful to the extent that the test cases
        // match what will actually be provided to the method in production.
        // As currently used, NUnit's codebase can only use the file: schema,
        // since we don't load assemblies from anything but files. The uri's
        // provided can be absolute file paths or UNC paths.

#if NYI // Windows path
        // Local paths - Windows Drive
        [TestCase(@"file:///C:/path/to/assembly.dll", @"C:\path\to\assembly.dll")]
        [TestCase(@"file:///C:/my path/to my/assembly.dll", @"C:/my path/to my/assembly.dll")]
        [TestCase(@"file:///C:/dev/C#/assembly.dll", @"C:\dev\C#\assembly.dll")]
        [TestCase(@"file:///C:/dev/funnychars?:=/assembly.dll", @"C:\dev\funnychars?:=\assembly.dll")]
        // Windows drive specified as if it were a server - odd case, sometimes seen
        [TestCase(@"file://C:/path/to/assembly.dll", @"C:\path\to\assembly.dll")]
        [TestCase(@"file://C:/my path/to my/assembly.dll", @"C:\my path\to my\assembly.dll")]
        [TestCase(@"file://C:/dev/C#/assembly.dll", @"C:\dev\C#\assembly.dll")]
        [TestCase(@"file://C:/dev/funnychars?:=/assembly.dll", @"C:\dev\funnychars?:=\assembly.dll")]
#endif
        // Local paths - Linux or Windows absolute without a drive
        [TestCase(@"file:///path/to/assembly.dll", @"/path/to/assembly.dll")]
        [TestCase(@"file:///my path/to my/assembly.dll", @"/my path/to my/assembly.dll")]
        [TestCase(@"file:///dev/C#/assembly.dll", @"/dev/C#/assembly.dll")]
        [TestCase(@"file:///dev/funnychars?:=/assembly.dll", @"/dev/funnychars?:=/assembly.dll")]
        // UNC format with server and path
        [TestCase(@"file://server/path/to/assembly.dll", @"//server/path/to/assembly.dll")]
        [TestCase(@"file://server/my path/to my/assembly.dll", @"//server/my path/to my/assembly.dll")]
        [TestCase(@"file://server/dev/C#/assembly.dll", @"//server/dev/C#/assembly.dll")]
        [TestCase(@"file://server/dev/funnychars?:=/assembly.dll", @"//server/dev/funnychars?:=/assembly.dll")]
        //[TestCase(@"http://server/path/to/assembly.dll", "//server/path/to/assembly.dll")]
        public void GetAssemblyPathFromCodeBase(string uri, string expectedPath)
        {
            string localPath = AssemblyHelper.GetAssemblyPathFromCodeBase(uri);
            Assert.That(localPath, Is.EqualTo(expectedPath));
        }
    }
}
