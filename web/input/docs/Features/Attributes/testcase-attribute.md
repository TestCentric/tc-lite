Title: TestCaseAttribute
Description: Identifies a test method and optionally provides a set of arguments.
---

`TestCaseAttribute` identifies a method as a test. In **TC-Lite** it is used for both
simple and parameterized tests.

# Test Methods

A test method must be public and may be an instance method or a static method. It may
not be abstract, however.

If the test method returns a value, you must provide the expected return value using the
`ExpectedResult` named parameter. This expected return value will be checked for equality with the return value of the test method when it is run.

If the test method takes arguments, you must provide the required number and type of arguments
on the attribute. If it takes none, you must not provide any.

If any of the above requirements are violated, a non-runnable test is generated and reported,
causing your test run to fail.

<!-- TODO: Add info about async tests -->

# Examples

Here are some non-parameterized tests.

```c#
namespace MyTests
{
  using System;
  using TCLite;

  public class SimpleTests
  {
    // A simple test
    [TestCase]
    public void Add()
    {
        Assert.That(2 + 2, Is.EqualTo(4));
    }

    // Test with an expected result
    [Test(ExpectedResult = 4)]
    public int TestAdd()
    {
        return 2 + 2;
    }
  }
}
```

The following example uses methods that take arguments. Three cases are provided to each method,
so each of them is run three times, with different arguments.

```c#
namespace MyTests
{
  using System;
  using TCLite;

  public class ParameterizedTests
  {
    // Expected value provided as an argument
    [TestCase(2, 2, 4)]
    [TestCase(5, 3, 8)]
    [TestCase(0, 0, 0)]
    public void Add(int x, int y, int answer)
    {
        Assert.That(x + y, Is.EqualTo(answer));
    }

    // Expected value provided as ExpectedResult
    [TestCase(2, 2, ExpectedResult = 4)]
    [TestCase(5, 3, ExpectedResult = 8)]
    [TestCase(0, 0, ExpectedResult = 0)]
    public int TestAdd(int x, int y)
    {
        return x + y;
    }
  }
}
```

# Multiple Attributes

As the example shows, `[TestCase]` may be applied to a method more than once, generating a
different test case each time. Normally, all the attributes will have the same number of
arguments provided. The only exception would be for methods whose last parameter is a
`params` array.

Individual test cases are executed in an indeterminate order, which may
vary between different compilers or different .NET platforms. In keeping with **TC-Lite**'s
design as a microtest framework, there is no facility for controlling the order of tests.

In the case of a method that takes no arguments, `[TestCase]` may appear only once.

# Argument Types

In general, **TC-Lite** doesn't do argument conversion but relies on .NET to match any
provided arguments to parameters. However, because direct arguments to attributes are
limited in terms of the Types that may be used, it does do some special conversions
for the `TestCase` attribute before supplying it to the test.

* A provided `int` will be converted to any targeted numeric Type, ignoring overflows.
* A provided `int`, `double` or `string` will be converted to targeted `decimal`.
* A provided `string` will be converted to targeted `DateTime`.
* Any conversions supported by the target Type through `TypeDescriptor.GetConverter` will be used.

# Named Parameters

TestCaseAttribute supports a number of named parameters:

* **Category** provides a comma-delimited list of categories for this test case.
* **Description** sets the description property of the test.
run.
* **ExpectedResult** sets the expected result to be returned from the method, which must have a compatible return type.
* **Ignore** causes the test case to be ignored and specifies the reason.
* **IgnoreReason** causes this test case to be ignored and specifies the reason.
* **Reason** specifies the reason for not running this test case.

# See Also

* [Overview of Test Cases][1]
* [TestCaseData Attribute][2]
* [TestCaseSource Attribute][3]

[1]: /docs/tclite/Concepts/test-cases.html
[2]: /docs/tclite/Features/Attributes/testcasedata-attribute.html
[3]: /docs/tclite/Features/Attributes/testcasesource-attribute.html
