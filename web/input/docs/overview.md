Title: TCLite Overview
Description: Provides a <a href="overview">general overview</a> of the project.
Order: 1
---

# Background

_TC-Lite_ is a lightweight test framework in the spirit of the original _NUnitLite_. I started work on _NUnitLite_ in 2007 or 2008, while also working on the _NUnit_ framework. It was developed from scratch but echoed many features of the larger framework. Its two main distinguishing features were the use of self-executing test assemblies and introduction of the Constraint Model of assertions. The final version was released in 2013 as *NUnitLite 1.0*.

Fluent assertions using the constraint model were ported into _NUnit_ itself with version 2.5 and the concept of self-executing tests was introduced with the release of version 3. _NUnit 3_ now includes a separate package called _NUnitLite_, which is used in conjunction with the framework in order to allow executable test assemblies to run their own tests.

# Intended Usage

_TC-Lite_, like the original _NUnitLite_, aims to provide a very simple approach to testing in which a single test assembly is written in the form of a console application, referencing only the test framework and the software being tested. This allows tests to be deployed very easily by copying a few files.

_TC-Lite_ is intended as a framework for writing and running micro-tests. Its feature set is oriented toward that usage. Hat tip to the folks on the _Lonely Coaches Sodality_ mail list for helping me narrow down the features, which such a framework might need. Of course, the ultimate responsibility for what's included and what's excluded is my own. Since the name stands for _TestCentric Lite_, it's possible there will be a more complete _TestCentric_ framework in the future, suitable for other kinds of testing.

And speaking of the name, I tend to use _TC-Lite_ and _TCLite_ somewhat interchangeably, although I've tried to stick with the first form in this document.

# Self-Execution

The _TC-Lite_ assembly contains both the test framework and a console runner. The executable test assembly invokes the runner from it's `Main()` with a single line of code, as follows:

```c#
public int Main(string[] args)
{
    return TestRunner.Execute(args);
}
```

The runner executes tests contained in the assembly that calls it. Currently there is no
provision for passing a different assembly or for having the runner load other test assemblies,
so all tests must be located in the original calling assembly. As a future enhancement,
we may consider detecting tests in assemblies referenced by the main test assembly.

# Platforms

_TC-Lite_ is written in C# and is aimed at supporting managed test assemblies written in any .NET-supported language supported by .NET. This includes the .NET Framework, .NET Core and the soon-to-be-released .NET 5. It is being developed on Linux using .NET Core and will run on Linux, Windows or Mac operating systems. The initial release will run on .NET Framework 4.6.1+, .NET Core 3.1+ and .NET 5.0.

Support for legacy platforms will be considered for future enhancement if there is a demand.

# Test Results

* Test results are saved as a valid _NUnit 3_ result file by default. The content of the file is flatter than the same tests run under _NUnit_ because namespaces are not represented.

* Optionally, results may be saved in NUnitV2 format.
