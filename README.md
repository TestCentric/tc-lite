# TC-Lite Test Framework

The _TC-Lite_ framework is currently under early development. This document describes the roots of the project, its current status and my vision for its development. As such, it serves as a very informal "requirements" doc for the new framework, with the difference that it was not written up front but only after a substantial amount of implementation work and experimentation.

> __Note:__ This document is changing constantly as the work proceeds. So be sure to refresh!

## Background

_TC-Lite_ is a lightweight test framework in the spirit of the original _NUnitLite_. I started work on _NUnitLite_ in 2007 or 2008, while also working on the _NUnit_ framework. It was developed from scratch but echoed many features of the larger framework. Its two main distinguishing features were the use of self-executing test assemblies and introduction of the Constraint Model of assertions. The final version was released in 2013 as *NUnitLite 1.0*.

Fluent assertions using the constraint model were ported into _NUnit_ itself with version 2.5 and the concept of self-executing tests was introduced with the release of version 3. _NUnit 3_ now includes a separate package called _NUnitLite_, which is used in conjunction with the framework in order to allow executable test assemblies to run their own tests.

## General Description

_TC-Lite_, like the original _NUnitLite_, aims to provide a very simple approach to testing in which a single test assembly is written in the form of a console application, referencing only the test framework and the software being tested. This allows tests to be deployed very easily by copying a few files.

_TC-Lite_ is intended as a framework for writing and running micro-tests. Its feature set is oriented toward that usage. Hat tip to the folks on the _Lonely Coaches Sodality_ mail list for helping me narrow down the features, which such a framework might need. Of course, the ultimate responsibility for what's included and what's excluded is my own. Since the name stands for _TestCentric Lite_, it's possible there will be a more complete _TestCentric_ framework in the future, suitable for other kinds of testing.

And speaking of the name, I tend to use _TC-Lite_ and _TCLite_ somewhat interchangeably, although I've tried to stick with the first form in this document.

### Platforms

_TC-Lite_ is written in C# and is aimed at supporting managed test assemblies written in any .NET-supported language supported by .NET. This includes the .NET Framework, .NET Core and the soon-to-be-released .NET 5. It is being developed on Linux using .NET Core and will run on Linux, Windows or Mac operating systems. The initial release will run on .NET Framework 4.6.1+, .NET Core 3.1+ and .NET 5.0.

## What's Done

### Self-Execution

The _TC-Lite_ assembly contains both the test framework and a console runner. The executable test assembly invokes the runner from it's `Main()` with a single line of code, as follows:

```c#
public int Main(string[] args)
{
    return TestRunner.Execute(args);
}
```

The runner executes tests contained in the assembly that calls it. There is no provision for passing a different assembly or for having the runner load other test assemblies. All tests must be located in the original calling assembly. Tests in assemblies it references are not found.

### Test Fixtures

A `TestFixture` is a class containing tests. A __separate instance__ of the fixture class is created for each test case that is run and is disposed when the test case completes.

This is a big change from the _NUnit_ model but is actually the way most xUnit-style test frameworks work. Using a separate fixture instance per test tends to discourage inter-test dependencies, which is desirable in most unit testing. It also makes parallel execution a lot simpler and reduces the size of the framework.

To work well within this model, fixtures must be very lightweight. Although the fixture constructor may be used to initialize instance members, this should usually be limited to those needed by every test. In rare instances, disposal of the fixture may be required. _TC-Lite_ will call `Dispose` for any fixture class, which implements `IDisposable`.

Initially, _TC-Lite_ does not  support any setup or teardown methods. In particular, `[OneTimeSetUp]` and `[OneTimeTearDown]` make no sense when each test case uses a separate fixture instance.

### Attributes

_TC-Lite_ uses attributes to identify tests, control various aspects of how they are run, choose which tests should be run and to provide general information about each test. The following attributes are currently supported in the code.

* `[TestCase]` identifies a test method. Applied to a method with parameters, it is the similar to _NUnit_ `[TestCase]` and must include argument values for each parameter. Applied to a method without parameters, it is the equivalent of _NUnit_ `[Test].

* `[TestCaseSource]` is used only on parameterized test methods and provides a level of indirection. It is similar in concept to _NUnit's_ attribute of the same name but is expected to differ in specific details as implementation proceeds. Specifically, it may require use of a separate class from the fixture class itself as the data source, since the source lifetime may be longer than that of the fixture.

* `[Combinatorial]` is used on parameterized test methods, when the data for the arguments is provided individually rather than as test cases. The attribute generates test cases using every possible combination of the individual argument values. The `CombinatorialAttribute` is actually optional. If __any__ of the parameters for the test method is decorated with an attribute for generating values, then `[Combinatorial]` is assumed.

* For use with `[Combinatorial]` (explicit or implied), the following attributes may be applied to method parameters for generating individual arguments.

  * `[Values]` allows explicitly including argument values.
  * `[Range]` allows specifying a numeric range of values.
  * `[Random]` allows for generating random values.
  * `[ValueSource]` works like `[Values]`, but with a level of indirection similar to `[TestCaseSource]`.

* The `[TestFixture]` attribute is required for generic or parameterized fixtures, for which it provides the constructor and/or Type arguments. It is optional for non-generic, non-parameterized fixtures, which are recognized simply by the fact that they contain tests.

* `[TestFixtureSource]` is only used on parameterized or generic fixtures and provides a level of indirection analogous to what `[TestCaseSource]` does for methods.

* `[Ignore]` indicates that a test should be ignored and causes a warning to be issued. Note that we don't support the `Until` named property as _NUnit_ does. It's not useful for microtests because they should not be ignored for any length of time. Ignored tests are always treated as a warning.

* `[Explicit]` indicates that a test should only be run if directly selected and not by default in a general run. Explicit tests are simply skipped and do not affect the overall run result.

* `[Category]` allows grouping tests under arbitrary tags, which may be used to select the test to be run.

* `[Property]` goes beyond categories, allowing name / value pairs to be applied to tests and used to select them.

* `[Author]` provides the author or authors of the test as a test property.

* `[Description]` provides a readable description of the test as a property.

* `[DefaultTolerance]` sets the default tolerance for floating point equality assertions. If not used the default is zero tolerance.

### Assertions

As with _NUnitLite_ and _NUnit_, the main `Assert` verb is `Assert.That`, which may be used to test a boolean value directly or to apply a `Constraint`. In addition, a limited number of "classic" (non-constraint-based) Asserts continue to be supported:

* Assert.Pass, Assert.Fail, Assert.Warn, Assert.Ignore, Assert.Inconclusive
* Assert.IsTrue,  Assert.IsFalse
* Assert.IsNull, Assert.IsNotNull
* Assert.IsNaN
* Assert.IsEmpty, Assert.IsNotEmpty
* Assert.Throws, Assert.DoesNotThrow, Assert.Catch

### Constraints

The _NUnit_ Constraint syntax has been re-implemented based on Generic methods to avoid boxing and provide greater type safety. Improvements are ongoing work is ongoing in this area. The following table lists constraints, which are currently supported. New constraints are added on an ongoing basis.

The "Expression Syntax" column shows how the constraint is created while parsing a constraint expression. For constraints that may begin an expression, the "Initial Syntax" column shows the syntax used to initialize the expression. Note that some constraints have been developed and tested but do not yet have syntax elements defined.

| Constraint                     | Expression Syntax    | Initial Syntax
| ------------------------------ | -------------------- | --------------
| `AllItemsConstraint`           | `All`                | `Is.All`, `Has.All`
| `AndConstraint`                | `And`
| `CollectionEquivalentConstraint` | `EquivalentTo`     | `Is.EquivalentTo`
| `EmptyCollectionConstraint`    | `Empty`              | `Is.Empty`
| `EmptyConstraint`              | `Empty`              | `Is.Empty`
| `EmptyStringConstraint`        | `Empty`              | `Is.Empty`
| `EndsWithConstraint`
| `EqualConstraint`              | `EqualTo`            | `Is.EqualTo`
|                                | `Zero`               | `Is.Zero`
| `ExactTypeConstraint`          | `TypeOf`             | `Is.TypeOf`
| `ExceptionTypeConstraint`      |                      |
| `FalseConstraint`              | `False`              | `Is.False`
| `GreaterThanConstraint`        | `GreaterThan`        | `Is.GreaterThan`
|                                | `Positive`           | `Is.Positive`
| `GreaterThanOrEqualConstraint` | `GreaterThanOrEqual` | `Is.GreaterThanOrEqual`
|                                | `AtLeast`            | `Is.AtLeast`
| `InstanceOfTypeConstraint`     | `InstanceOf`         | `Is.InstanceOf`
| `LessThanConstraint`           | `LessThan`           | `Is.LessThan`
|                                | `Negative`           | `Is.Negative`
| `LessThanOrEqualConstraint`    | `LessThanOrEqual`    | `Is.LessThanOrEqual`
|                                | `AtMost`             | `Is.AtMost`
| `NaNConstraint`                | `NaN`                | `Is.NaN`
| `NoItemConstraint`             | `None`               | `Has.None`, `Has.No`
| `NotConstraint`                | `Not`                | `Is.Not`
| `NullConstraint`               | `Null`               | `Is.Null`
| `OrConstraint`                 | `Or`
| `PropertyConstraint`
| `PropertyExistsConstraint`
| `RegexConstraint`
| `SameAsConstraint`             | `SameAs`             | `Is.SameAs`
| `SomeItemsConstraint`          | `Some`               | `Has.Some`, `Contains.Item`
| `StartsWithConstraint`
| `SubstringConstraint`          | `Substring`          | `Contains.Substring`
| `ThrowsConstraint`             |                      | `Throws.TypeOf`
| `ThrowsExceptionConstraint`    |                      | `Throws.Exception`
| `ThrowsNothingConstraint`      |                      | `Throws.Nothing`
| `TrueConstraint`               | `True`               | `Is.True`
| `UniqueItemsConstraint`        | `Unique`             | `Is.Unique`

### Test Results

* Test results are saved as a valid _NUnit 3_ result file by default. The content of the file is flatter than the same tests run under _NUnit_ because namespaces are not represented.

* Optionally, results may be saved in NUnitV2 format.

### Command-line Options

When running the test assembly, various options may be specified. Where the feature has not yet been implemented, the option is labeled (NYI).

| Option | Comments |
| ------ | -------- |
| __Test Selection:__
| --where=TSL | TSL expression indicating which tests will be run. If omitted, all tests are run. |
| __How Tests are Run:__
| --params, -p=VALUE     | (NYI) Define test parameters.
| --timeout=MILLISECONDS | (NYI) Set default test case timeout in MILLISECONDS.
| --seed=SEED            | (NYI) Set the random SEED used to generate test data. Used for debugging earlier runs.
| --workers=NUMBER       | (NYI) Specify the NUMBER of worker threads to be used in running tests. If not specified, defaults to 2 or the number of processors, whichever is greater.
| --stoponerror          | (NYI) Stop run immediately upon any test failure or error.
| --wait                 | Wait for input before closing console window.
| __Test Output:__
| --work=PATH            | PATH of the directory to use for output files. If not specified, defaults to the current directory.
| --out=PATH             | (NYI) File PATH to contain text output from the tests.
| --err=PATH             | (NYI) File PATH to contain error output from the tests.
| --explore[=PATH]       | Explore tests rather than running them. The optional PATH is used for the XML report describing the tests. It defaults to 'tests.xml'.
| --result=PATH          | Save test result XML in file at PATH. If not specified, default is TestResult.xml.
| --format=FORMAT        | Specify the FORMAT to be used in saving the test result. May be `nunit3` or `nunit2'.
| --noresult             | Don't save any test results.
| --labels=VALUE         | Specify whether to write test case labels to the output. Values: Off, On, Before, After.
| --teamcity             | (NYI) Turns on use of TeamCity service messages.
| --trace=LEVEL          | (NYI) Set internal trace LEVEL. Values: Off, Error, Warning, Info, Verbose (Debug)
| --noheader, --noh      | Don't display program header at start of run.
| --nocolor, --noc       | Displays console output without color.
| --help, -h             | Display this message and exit.
| --version, -V          | Display the header and exit.

## Coming Soon

### Attributes

* Something like _NUnit_'s `[ValuesSource]` to allow specifying values for individual arguments indirectly.

* `[Include]` allows the developer to indicate when a test will be included, based on environmental factors like the OS platform or the current Culture. A corresponding `[Exclude]` attribute is available when specifying rules for exclusion is more convenient.

## Coming Later (Project Backlog)

### Theories

Continuation and extension of NUnit's _Theories_.

* `[Theory]` is reserved for use in implementing a new take on "theory tests" as originally implemented in _NUnit_. Note that this is __not__ equivalent to the `TheoryAttribute` implemented in _xUnit_, which is no more than a parameterized test, whereas the original academic work on "theories" gave the responsibility of deciding which inputs are appropriate to the test itself. _TC-Lite's_ `[Theory]` will do that but may differ in details from the _NUnit_ implementation, which became somewhat frozen in its development in order to preserve backward compatibility. It's possible that `[Theory]` will not make the cut for the first release of _TC-Lite_.

* `[Datapoint]` and `[DatapointSource]` _or some equivalent_ will be developed along with theories. More research is needed to identify alternative approaches to either filtering or generating data for theories.

### Parallel Test Execution

Attributes for indicating how tests are permitted to run in parallel. The implementation will be a simpler one than that in _NUnit_ since we are using separate fixtures for each test case and are not constrained by backward compatibility.

The following is a starting set of features:

* Attributes should indicate what __may__ or __may not__ run in Parallel. It's up to the __TC-Lite__ framework to decide whether to actually use parallel execution in a given environment.

* An assembly-level attribute should allow specifying the maximum number of tests allowed to execute at one time.

* A pair of class- and method-level attributes should allow specifying whether a given test may or may not be run in parallel with other tests.

* (Possibly) an assembly level attribute to set the default level of parallelism. This requires some thought since it can requires __every__ test to be written in a way that allows it to run in parallel. However, it is a great convenience to be able to specify the most common setting once and then override it at the class or method level.

## Possible Future Additions (Ideas)

### Platforms

* Support for legacy platforms will be considered for future enhancements if there is a demand.

### Self-Execution

* Detect tests in assemblies referenced by the main test assembly

### Attributes

* Additional attributes to supplement `[Combinatorial]` and provide other ways of generating cases from sets of argument valeus. `[Pairwise]` and `[Sequential]` could be considered, although they may not be appropriate for a framework that focuses on microtests.

* `[Before]` and `[After]` as a notational convenience if there seems to be a demand for them. They would not provide any new functionality beyond what is provided through construction and disposal.

* `[MaxTime]` indicates the maximum time a test should take and causes either a failure or a warning if the time is exceeded.

* `[Repeat]` causes a test to be repeated some number of times with each execution reported separately.

* `[Retry]` causes a test to be repeated up to a maximum number of times in case of an error, failure or warning.

* `[Timeout]` specifies the maximum time a test case may run before being cancelled.

* `[DefaultTimeout]` is used on fixtures or assemblies to indicate the default timeout for individual tests.

### Assertions

* Make Assertions available for separate use outside the test framework.

* A new syntax alongside the _NUnit_ syntax. It would allow combining the actual value and the constraint in a single expression for easier identification of types. The new syntax is not yet designed but I'm thinking of something like `Assert.That(actual.Is.GreaterThan(200));`
