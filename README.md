## TC-Lite Test Framework

The _TC-Lite_ framework is currently under early development. This document describes the roots of the project and my vision for its development. As such, it serves as a very informal "requirements" doc for the new framework, with the difference that it was not written up front but only after a substantial amount of implementation work and experimentation. It's intended to change further as the work proceeds.

### Background

_TC-Lite_ will be a lightweight test framework in the spirit of the original _NUnitLite_. I started work on _NUnitLite_ in 2007 or 2008, while also working on the _NUnit_ framework. It was developed from scratch but echoed many features of the larger framework. Its two main distinguishing features were the use of self-executing test assemblies and introduction of the Constraint Model of assertions. The final version was released in 2013 as *NUnitLite 1.0*.

Fluent assertions using the constraint model were ported into _NUnit_ itself with version 2.5 and the concept of self-executing tests was introduced with the release of version 3. _NUnit 3_ now includes a separate package called _NUnitLite_, which is used in conjunction with the framework in order to allow executable test assemblies to run their own tests.

### General Description

_TC-Lite_, like the original _NUnitLite_, aims to provide a very simple approach to testing in which a single test assembly is written in the form of a console application, referencing only the test framework and the software being tested. This allows tests to be deployed very easily by copying a few files.

_TC-Lite_ is intended as a framework for writing and running micro-tests. Its feature set will be oriented toward that usage. Hat tip to the folks on the _Lonely Coaches Sodality_ mail list for helping me narrow down the features such a framework needs. Of course, the ultimate responsibility for what's included and what's excluded is my own. Since the name stands for _TestCentric Lite_, it's possible there will be a more complete _TestCentric_ framework in the future, suitable for other kinds of testing.

And speaking of the name, I tend to use _TC-Lite_ and _TCLite_ somewhat interchangeably, although I've tried to stick with the first form in this document.

### Features

The rest of this document provides more detail on some specific features and design choices. Since _TC-Lite_ is a work in progress, the details are subject to change. Note that even though _TC-Lite_ is being developed independently, it draws on my experience working on _NUnit_ over the past 18 years. Consequently, I've taken the shortcut of describing some aspects of it in terms of _NUnit_. 

In lieu of a detailed roadmap, I have designated features with the labels __Tier1__, __Tier2__ and __Tier3__.

* __Tier1__ features are those I feel are needed for initial release of a useful framework.
* __Tier2__ represents features that I expect to add in subsequent releases.
* __Tier3__ are "maybes" - features that may or may not be implemented based on needs of users.

Major features listed without any label are all __Tier1__.

#### Platforms

_TC-Lite_ is being written in C# and is aimed at supporting managed test assemblies under written in any .NET-supported language supported by .NET. This includes the .NET Framework, .NET Core and the soon-to-be-released .NET 5. It is being developed on Linux using .NET Core and will run on Linux, Windows or Mac operating systems. The initial release will run on .NET Framework 4.6.1 and later, .NET Core 3.1 and later and .NET 5.0.

__Tier3__: Support for legacy platforms will be considered for future enhancements if there is a demand.

#### Self-Execution

The _TC-Lite_ assembly will contain both the test framework and a console runner. The executable
test assembly will invoke the runner from it's `Main()` with a single line of code, as follows:

```c#
public int Main(string[] args)
{
    return TestRunner.Execute(args);
}
```

The runner will execute tests contained in the assembly that calls it. There is no provision for passing a different assembly or for having the runner load other test assemblies. All tests must be located in the original calling assembly. Any tests in assemblies it references will not be found, although that may be considered as a future (__Tier3__) enhancement.

#### Test Fixtures

A `TestFixture` is a class containing tests. A __separate instance__ of the fixture class is created for each test case that is run and is disposed when the test case completes.

This is a big change from the _NUnit_ model but is actually the way most xUnit-style test frameworks work. Using a separate fixture instance per test tends to discourage inter-test dependencies, which is desirable in most unit testing. It also makes parallel execution a lot simpler and reduces the size of the framework.

To work well within this model, fixtures must be very lightweight. Although the fixture constructor may be used to initialize instance members, this should usually be limited to those needed by every test. In rare instances, disposal of the fixture may be required. _TC-Lite_ will call `Dispose` for any fixture class, which implements `IDisposable`.

At least initially, _TC-Lite_ will  support any setup or teardown methods. In particular, `[OneTimeSetUp]` and `[OneTimeTearDown]` make no sense when each test case uses a separate fixture instance.

__Tier3__: `[SetUp]` and `[TearDown]` as a notational convenience if there seems to be a demand for them. They would not provide any new functionality beyond what is provided through construction and disposal.

#### Attributes

_TC-Lite_ uses attributes to identify tests, control various aspects of how they are run, choose which tests should be run and to provide general information about each test. The following is a somewhat approximate list of attributes that will be supported.

__Tier1__: `[TestCase]` identifies a test method. Applied to a method with parameters, it is the similar to _NUnit_ `[TestCase]` and must include argument values for each parameter. Applied to a method without parameters, it is the equivalent of _NUnit_ `[Test].

__Tier1__: `[TestCaseSource]` is used only on parameterized test methods and provides a level of indirection. It is similar in concept to _NUnit's_ attribute of the same name but is expected to differ in specific details as implementation proceeds. Specifically, it may require use of a separate class from the fixture class itself as the data source, since the source lifetime may be longer than that of the fixture.

__Tier1__: The `[TestFixture]` is required for generic or parameterized fixtures, for which it provides the constructor and/or Type arguments. It is optional for non-generic, non-parameterized fixtures, which are recognized simply by the fact that they contain tests.

__Tier1__: `[TestFixtureSource]` is only used on parameterized or generic fixtures and provides a level of indirection analogous to what `[TestCaseSource]` does for methods.

__Tier1__: `[Category]` allows grouping tests under arbitrary tags, which may be used to select the test to be run.

__Tier1__: `[Property]` goes beyond categories, allowing name / value pairs to be applied to tests and used to select them.

__Tier1__: `[Include]` allows the developer to indicate when a test will be included, based on environmental factors like the OS platform or the current Culture. A corresponding `[Exclude]` attribute is available when specifying rules for exclusion is more convenient.

__Tier1__: `[Explicit]` indicates that a test should only be run if directly selected and not by default in a general run.

__Tier1__: `[Ignore]` indicates that a test should be ignored and causes a warning to be issued.

__Tier1__: `[DefaultFloatingPointTolerance]` sets the default tolerance for floating point equality assertions. If not used the default is zero tolerance.

__Tier1__: `[MaxTime]` indicates the maximum time a test should take and causes either a failure or a warning if the time is exceeded.

__Tier1__: `[Repeat]` causes a test to be repeated some number of times with each execution reported separately.

__Tier1__: `[Retry]` causes a test to be repeated up to a maximum number of times in case of an error, failure or warning.

__Tier1__: `[Timeout]` specifies the maximum time a test case may run before being cancelled.

__Tier1__: `[DefaultTimeout]` is used on fixtures or assemblies to indicate the default timeout for individual tests.

__Tier2__: `[Combinatorial]`, `[Pairwise]` and (possibly) `[Sequential]` are used on parameterized test methods, when the data for the arguments are provided individually rather than as test cases. They provide alternate "recipes" for combining the individual arguments. For the moment, I envision these as working like _NUnit's_ attributes of the same names, although that could change.

__Tier2__: Various attributes may be applied to method parameters for generating individual arguments. The following is a possible selection, subject to change: `[Values]`. `[ValuesSource]`, `[Range]`, `[Random]`.

__Tier2__: `[Theory]` is reserved for use in implementing a new take on "theory tests" as originally implemented in _NUnit_. Note that this is __not__ equivalent to the `TheoryAttribute` implemented in _xUnit_, which is no more than a parameterized test, whereas the original academic work on "theories" gave the responsibility of deciding which inputs are appropriate to the test itself. _TC-Lite's_ `[Theory]` will do that but may differ in details from the _NUnit_ implementation, which became somewhat frozen in its development in order to preserve backward compatibility. It's possible that `[Theory]` will not make the cut for the first release of _TC-Lite_.

__Tier2__: `[Datapoint]` and `[DatapointSource]` _or some equivalent_ will be developed along with theories. More research is needed to identify alternative approaches to either filtering or generating data for theories.

__Tier2__: `[LevelOfParallelism]`, `[Parallelizable]` and `[NonParallelizable]` are used to specify how tests may be run in parallel. The attribute names are taken from _NUnit_ but the precise implementation is expected to be somewhat simpler due to the greater independence of tests in _TC-Lite_.

__Tier2__: `[SingleThreaded]` indicates that all the tests in a fixture must run on the same thread. Although it's different from parallel execution, it's not needed until we actually have multiple parallel threads.

__Tier2__: `[Author]` stores the author of the test as a test property.

__Tier2__: `[Description]` provides a readable description of the test as a property.

#### Assertions

__Tier1__ The _NUnit_ Fluent Assertion syntax is being re-implemented based on Generic methods to avoid boxing and provide greater type safety.

__Tier1__ Assertions will be made available for separate use outside the test framework.

__Tier2__ A new syntax will be added alongside the _NUnit_ syntax. It will allow combining the actual value and the constraint in a single expression for easier identification of types. The new syntax is not yet designed but I'm thinking of something like `Assert.That(actual.Is.GreaterThan(200));`

#### Test Results

__Tier1__ Test results will be stored as a valid nunit result file. However, the content of the file will flatter than the same tests run under NUnit because namespaces are not represented.

__Tier3__ NUnit2 format may also be provided.

For more information, see the [TC-Lite Wiki](https://github.com/TestCentric/tc-lite/wiki), which is being used to document the design.
