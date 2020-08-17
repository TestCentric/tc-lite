Title: Assert.Ignore
Description: Causes a test to terminate immediately with an ignored result.
Order: 34
---

The `Assert.Ignore` method provides you with the ability to cause a test to be ignored at runtime. In most cases, use of the `IgnoreAttribute` is preferable but `Assert.Ignore` allows you to ignore the test dynamically based on some runtime logic.

```c#
Assert.Ignore(string message=null, params object[] args);
```
