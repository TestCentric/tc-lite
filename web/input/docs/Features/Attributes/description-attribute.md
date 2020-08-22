Title: DescriptionAttribute
Description: 
---

The Description attribute is used to apply descriptive text to a Test, TestFixture or Assembly.
The text appears in the XML output file.

# Example

```csharp
[assembly: Description("Assembly description here")]

namespace Tests
{
  using System;
  using TCLite;

  [TestFixture, Description("Fixture description here")]
  public class SomeTests
  {
    [TestCase, Description("Test description here")]
    public void OneTest() { }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [Description("This applies to all three test cases")]
    public void ThreeTests() { }
  }
}
```

# Notes

1. As seen above, when there are multiple test cases, `DescriptionAttribute` applies to
   **each** of them. If you want to specify a separate description to each case, then
   use the `Description` **property** of the `TestCaseAttribute`, like this:

   ```csharp
   [TestCase(1, Description="Description1")]
   [TestCase(2, Description="Description2")]
   ...
   ```

2. The C# syntax can sometimes be a bit confusing. To make it easier for your code
   to be read, consider placing any `DescriptionAttribute` on a separate line either
   before or after all the test cases. In particular, novice programmers are often
   confused when the attribute is interleaved with the test cases.
