Title: Assert.IsNull
Description: Tests that an object is null.
Order: 3
---

**Assert.IsNull** tests that an object is null.

```c#
Assert.IsNull(object obj, string message=null, params object[] args);
```

You may use **Assert.That** to achieve the
same result. The two statements below are equivalent:

```c#
Assert.IsNull(anObject);
Assert.That(anObject, Is.Null);
```
