Title: Constraint Model
Description: All Assertions use the same static method, Assert.That, with a Constraint argument that determines the particular test to be made.
Order: 2
---

Constraint-based assertions all use the `Assert.That` method, which has the following overloads:

```c#
Assert.That(bool condition, string message=null, params object[] args);
Assert.That(Func<bool> condition, string message=null, params object[] params);
Assert.That<TActual>(TActual actual, IResolveConstraint constraint, string message=null,;
    params object[] params);
Assert.That<TActual>(ActualValueDelegate<TActual> del, IResolveConstraint constraint,
    string message=null, object[] params);
Assert.That(TestDelegate code, IResolveConstraint constraint, string message=null,
    params object[] args);
```
