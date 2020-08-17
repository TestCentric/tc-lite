Title: Test Cases
Description: Describes how test cases are represented in TC-Lite.
Order: 2
---

Individual tests in **TC-Lite** are called "Test Cases" and are represented in the code
by methods annotated with the special attributes.

# Simple Test Cases

A simple test case is one that takes no arguments. It is represented by a method annotated
using `[TestCase]`. Here is an example:

```c#
[TestCase]
public void TestAddition()
{
    Assert.That(2 + 2, Is.EqualTo(4));
}
```

As you can see, the method representing the test is `public void` and takes no arguments.
This is the simplest use of `[TestCase]`.

# Test Cases With Arguments

We might want to test addition using various values passed to the same method. Here is a test method that generates three test cases:

```c#
[TestCase(2, 2, 4)]
[TestCase(0, 99, 99)]
[TestCase(21, 21, 42)]
public void TestAddition(int x, int y, int answer)
{
    Assert.That(x + y, Is.EqualTo(answer));
}
```

In this case, the test method takes three arguments of Type `int`. The three `TestCase` attributes
supply all of them for three different cases, which are executed independently.

# Other Sources of Arguments

Use of `[TestCase]` for parameterized tests is convenient because the intended arguments
are easily visible. There are, however, some inconveniences:

* .NET itself limits the kinds of arguments that may be used to construct an attribute, so some arguments cannot be specified in this way.
* If several methods need the same arguments, they must be repeated for each method.
* The test cases must be specified by the programmer at the time the code is written.

To overcome these limitations, **TC-Lite** provides a variety of attributes for use with
parameterized tests. Some attributes allow you to specify arguments inline - directly on the
attribute. Others use a separate method, property or field to hold the arguments. Still others
use an entirely separate class.

An additional distinction is that some attributes identify complete test cases, with all the
necessary arguments, while others only provide data for a single argument. **TC-Lite** then
combines the individual arguments to form test cases.

|     Location       | Complete Test Cases  | Data for One Argument |
|--------------------|----------------------|-----------------------|
| **Inline**         | [TestCase][1]        | [Random][2], [Range][3],[Values][4] |
| **Same Class**     | [TestCaseSource][5]  | [ValueSource][6]      |
| **Separate Class** | [TestCaseFactory][7] |                       |

When data is specified for individual arguments, the [Combinatorial][8] attribute may be added to
indicate that **TC-Lite** is generating all combinations of the supplied values. Adding the
attribute is optional, since **TC-Lite** sees the parameter attributes and knows how to combine
them, but its use may be desireable for its documentation value.

[1]: /docs/tclite/Features/Attributes/testcase.html
[2]: /docs/tclite/Features/Attributes/random.html
[3]: /docs/tclite/Features/Attributes/range.html
[4]: /docs/tclite/Features/Attributes/values.html
[5]: /docs/tclite/Features/Attributes/testcasesource.html
[6]: /docs/tclite/Features/Attributes/valuesource.html
[7]: /docs/tclite/Features/Attributes/testcasefactory.html
[8]: /docs/tclite/Features/Attributes/combinatorial.html
