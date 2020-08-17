Title: Assert.Fail
Description: Causes a test to terminate immediately with a failing result.
Order: 32
---

The `Assert.Fail` method allows you to immediately end the test, recording it as failed.
Use of `Assert.Fail` allows you to generate failure messages for tests that are not encapsulated by the other `Assert` methods. It is also useful in developing your own project-specific assertions.

```c#
Assert.Fail(string message=null, params object[] args);
```

Here's an example of its use to create a private assertion that tests whether a string contains an expected value.

```c#
public void AssertStringContains(string expected, string actual)
{
    AssertStringContains(expected, actual, string.Empty);
}

public void AssertStringContains(string expected, string actual, string message)
{
    if (actual.IndexOf(expected) < 0)
        Assert.Fail(message);
}
```
