Title: Assert.IsTrue
Description: Tests that a boolean condition is true.
Order: 1
---

**Assert.IsTrue** tests that a boolean condition is true.

```c#
Assert.IsTrue(bool condition, string message=null, params object[] args);
```

You may use **Assert.That** with a boolean argument to achieve the
same result. The two statements below are equivalent:

```c#
Assert.IsTrue(someCondition);
Assert.That(someCondition, Is.True);
```
