Title: CategoryAttribute
Description: Allows tagging tests with a category.
---

The **Category** attribute allows tests to be grouped arbitrarily, across the entire assembly.
Either individual test cases or fixtures may be identified as belonging to a particular category.
When you run tests under **TC-Lite**, you may specify categories to be included in or excluded
from the test run using the `--where` option.

# Example

```csharp
namespace Tests
{
  using TCLite;

  [Category("DataTests")]
  public class SomeTests
  {
    [TestCase, Category("LongRunning")]
    public void Test1() { }

    [TestCase]
    public void Test2() { }
  }

  public class MoreTests
  {
    [TestCase]
    public void Test3() { }

    [TestCase, Category("Crazy")]
    public void Test4() { }

    [TestCase, Category("Crazy,ReallyCrazy")]
    public void Test5() { }
  }
}
```

In the above example, each the five tests will run if any of the categories is selected.

| Test  | Categories             |
| ----- | ---------------------- |
| Test1 | Datatests, LongRunning |
| Test2 | LongRunning            |
| Test3 | --                     |
| Test4 | Crazy                  |
| Test5 | Crazy, ReallyCrazy     |

**Note:** When the attribute is applied to a class, it is not technically present "on" the
individual cases. That is, if you wee to use reflection, you would not find `CategoryAttribute`
on the methods. However, when **TC-Lite** runs the tests, the result is the same as if the
attribute were placed on every test method in the class.

# Custom Category Attributes

Custom attributes that derive from **CategoryAttribute** will be recognized
by NUnit. The default protected constructor of CategoryAttribute
sets the category name to the name of your class.

Here's an example that creates a category of `Critical` tests. It works
just like any other category, but has a simpler syntax. A test reporting
system might make use of the attribute to provide special reports.

```csharp
[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
public class CriticalAttribute : CategoryAttribute
{ }
```

```csharp
[Test, Critical]
public void MyTest()
{ /*...*/ }
```
