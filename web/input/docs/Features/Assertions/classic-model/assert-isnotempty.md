Title: Assert.IsNotEmpty
Description: Tests that a string or collection is not empty.
Order: 6
---

**Assert.IsEmpty** tests that a string or collection is not empty.

```c#
Assert.IsNotEmpty(string aString, string message=null, params object[] args);
Assert.IsNotEmpty(IEnumerable aCollection, string message=null, params object[] args);
```

You may use **Assert.That** to achieve the
same result. The two statements below are equivalent:

```c#
Assert.IsNotEmpty(someCollection);
Assert.That(someCollection, Is.Not.Empty);
```
