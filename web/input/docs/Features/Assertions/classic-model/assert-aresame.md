Title: Assert.AreSame
Description: Tests that the two arguments reference the same object.
Order: 23
---

`Assert.AreSame` tests that the two arguments reference the same object. The arguments
must be reference Types.

```c#
Assert.AreSame<T>(T expected, T actual,
    string message=null, params object[] args) where T : class;
```
