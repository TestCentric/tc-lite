Title: Assert.Inconclusive
Description: Causes a test to terminate immediately with an warning result.
Order: 35
---

The `Assert.Inconclusive` method causes a test to terminate immediately with an `Inconclusive` result. This indicates neither success nor failure but that the test could not be completed with the data available. It should be used in situations where another run with different data would allow the test to determine success or failure.

```c#
Assert.Inconclusive(string message=null, params object[] args);
```
