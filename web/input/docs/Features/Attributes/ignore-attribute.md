Title: IgnoreAttribute
Description: Indicates that a test should not be run and specifies the reason.
---

IgnoreAttribute is used to indicate that a test should not be executed for some reason,
which must be specified as an argument. Ignored tests are displayed as warnings in order to
provide a reminder that the test needs to be corrected or otherwise changed and re-instated.

The IgnoreAttribute is attached to a method. If that method produces multiple test cases,
all the cases will be ignored. Individual test cases must be ignored through the `Ignore`
property of the `TestCase` attribute.

# Example

```csharp
namespace Tests
{
  using TCLite;

  [Ignore("Ignores all the tests in this class")]
  public class SomeTests
  {
    [TestCase]
    public void Test1() { }

    [TestCase]
    public void Test2() { }
  }

  public class MoreTests
  {
    [TestCase, Ignore("Just Test3 will be ignored")]
    public void Test3() { }

    [TestCase]
    public void Test4() { }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [Ignore("All three cases are ignored")]
    public void Test5(int x) { }
  }
}
```

In the above example, only Test4 will run. All others are ignored, including the three
separate test cases created for Test5.
