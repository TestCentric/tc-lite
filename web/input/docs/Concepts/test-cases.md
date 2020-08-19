Title: Test Cases
Description: Describes how test cases are represented in TC-Lite.
Order: 2
---

Individual tests in **TC-Lite** are called "Test Cases" and are represented in the code
by methods annotated with one or more special attributes used to tell **TC-Lite** to
run them as tests.

# Test Methods

A method intended for use as a test must be public. It may be an instance method or a
static method. It may not be abstract.

If the test method returns a value, the attribute must provide an expected return value
That value will be checked against the return value of the test method when it is run.

If the test method takes arguments, you must provide the required number and type of
arguments through the attribute. If it takes none, you must not provide any.

The various attributes that indicate test cases each have their own way of supplying
both arguments and expected return values. Consult the documentation for the specific 
attribute you are using.

If any of the above requirements are violated, a test case is still generated but
is marked as non-runnable, causing your run to fail. This is done to avoid silent
errors, which might lead you to think the test passed.

<!-- TODO: Add info about async tests -->

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

|     Location     |   Complete Test Cases    | Data for One Argument |
|------------------|--------------------------|-----------------------|
| **Inline**       | [`[TestCase]`][1]        | [`[Random]`][4]       |
|                  |                          | [`[Range]`][5]        |
|                  |                          | [`[Values]`][6]       |
| **Separate**     | [`[TestCaseData]`][2]    | [`[ValueSource]`][7]  |
|                  | [`[TestCaseFactory]`][3] |                       |

When data is specified for individual arguments, the [Combinatorial][8] attribute may be added to
indicate that **TC-Lite** is generating all combinations of the supplied values. Adding the
attribute is optional, since **TC-Lite** sees the parameter attributes and knows how to combine
them, but its use may be desireable for its documentation value.

# Multiple Attributes

For parameterized tests, these attributes may be applied more than once, generating a
different test case each time. They may even be mixed. For example, some data might
be provided through `[Random]` with important corner cases provided using `[TestCase]`.

Of course, when multiple sources of data are used on a method, they must all be compatible
with that method's signature. That means they will each provide an expected result where
required or omit it if the method is void. Similarly, they would normally each supply
the same number of arguments, with exception made only for optional arguments to the method
and for any final argument in the form of a `params` array.

No matter how the data is supplied, the individual test cases are executed in an
indeterminate order, which may vary between different compilers or different .NET platforms.
In keeping with **TC-Lite**'s design as a microtest framework, there is no facility for 
controlling the order of tests.

Note that `[TestCase]` without arguments may appear only once, and only only on a method
that takes no arguments,

[1]: /tc-lite/docs/Features/Attributes/testcase-attribute.html
[2]: /tc-lite/docs/Features/Attributes/testcasedata-attribute.html
[3]: /tc-lite/docs/Features/Attributes/testcasefactory-attribute.html
[4]: /tc-lite/docs/Features/Attributes/random-attribute.html
[5]: /tc-lite/docs/Features/Attributes/range-attribute.html
[6]: /tc-lite/docs/Features/Attributes/values-attribute.html
[7]: /tc-lite/docs/Features/Attributes/valuesource-attribute.html
[8]: /tc-lite/docs/Features/Attributes/combinatorial-attribute.html
