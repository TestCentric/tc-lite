Title: TestCaseAttribute
Description: Identifies a test method and optionally provides a set of arguments.
---

`TestCaseAttribute` identifies a method as a test. In **TC-Lite** it is used for both
simple and parameterized tests. In the case of parameterized tests, the arguments
are provided directly by the attribute constructor. See [Test Cases][1] for a general
introduction to test cases.

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
* **Description** sets the description property of the individual test case.
* **ExpectedResult** sets the expected result to be returned from the method, which must have a compatible return type.
* **Ignore** causes the test case to be ignored and specifies the reason.
* **IgnoreReason** causes this test case to be ignored and specifies the reason.
* **Reason** specifies the reason for not running this test case.

# See Also

* [Overview of Test Cases][1]
* [TestCaseData Attribute][2]
* [TestCaseFactory Attribute][3]

[1]: /tc-lite/docs/Concepts/test-cases.html
[2]: /tc-lite/docs/Features/Attributes/testcasedata-attribute.html
[3]: /tc-lite/docs/Features/Attributes/testcasefactory-attribute.html
