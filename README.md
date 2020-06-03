## TC-Lite Test Framework

The TC-Lite framework is currently under development. This document describes our vision for it.

TC-Lite will be a lightweight test framework in the spirit of the original NUnitLite, which
was released in 2013 and later merged into the larger NUnit framework.

It aims to provide a very simple approach to testing in which a single test assembly is
written in the form of a console application, referencing only the test framework and the
software being tested. This allows tests to be deployed very easily by copying a few files.

The TC-Lite assembly will contain both the test framework and a console runner. The executable
test assembly will invoke the runner from it's `Main()` with a single line of code, as follows:

```
public int Main(string[] args)
{
    return TestRunner.Execute(args);
}
```

### Features

TC-Lite is intended as a framework for writing and running unit tests, more specifically, micro-tests.
Its feature set will be oriented toward that usage. Of course, TC-Lite stands for "TestCentric Lite" and
it's possible there will be a more complete TestCentric framework in the future.

Although subject to change, the following is a list of some planned features:

 * Fluent Assertion syntax based primarily on Generic methods to avoid boxing.
 * Assertions usable separately from the test framework.
 * Single fixture instance per test case.
 * Reliance on construction and disposal rather than setup and teardown.
 * Parallel execution of tests
 * Randomization of test ordering with the ability to repeat ordering for any run.


For more information, see the [TC-Lite Wiki](https://github.com/TestCentric/tc-lite/wiki), which is being used to document the design.
