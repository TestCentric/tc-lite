Title: Assert.AreNotSame
Description: Tests that the two arguments do not reference the same object.
Order: 24
---

`Assert.AreNotSame` tests that the two arguments do not reference the same object.
The arguments must be reference Types.

```c#
Assert.AreNotSame<T>(T expected, T actual,
    string message=null, params object[] args) where T : class;
```
