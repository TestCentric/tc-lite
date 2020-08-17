Title: Assert.IsEmpty
Description: Tests that a string or collection is empty.
Order: 5
---

**Assert.IsEmpty** tests that a string or collection is empty.

```c#
Assert.IsEmpty(string aString, string message=null, params object[] args);
Assert.IsEmpty(IEnumerable aCollection, string message=null, params object[] args);
```

You may use **Assert.That** to achieve the
same result. The two statements below are equivalent:

```c#
Assert.IsEmpty("");
Assert.That("", Is.Empty);
```
