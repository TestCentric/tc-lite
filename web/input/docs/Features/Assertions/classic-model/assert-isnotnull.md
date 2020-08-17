Title: Assert.IsNotNull
Description: Tests that an object is not null.
Order: 4
---

**Assert.IsNotNull** tests that an object is not null.

```c#
Assert.IsNotNull(object obj, string message=null, params object[] args);
```

You may use **Assert.That** to achieve the
same result. The two statements below are equivalent:

```c#
Assert.IsNotNull(anObject);
Assert.That(anObject, Is.Not.Null);
```
