Title: Assert.Pass
Description: Causes a test to terminate immediately with a passing result.
Order: 31
---

The `Assert.Pass` method allows you to immediately end the test, recording it as successful. Since it causes an exception to be thrown, it is more efficient to simply allow the test to return. However, `Assert.Pass` allows you to record a message in the test result and may also make the test easier to read in some situations. Additionally, it can be invoked from a nested method call with the result of immediately terminating test execution.

```c#
Assert.Pass(string message=null, params object[] args);
```
