Title: Assert.AreEqual
Description: Tests that the two arguments are equal.
Order: 21
---

`Assert.AreEqual` tests whether the two arguments are equal. It has two overloads:

```c#
Assert.AreEqual(object expected, object actual,
                string message=null, params object[] args);

Assert.AreEqual(double expected, double actual, double tolerance,
                string message=null, params object[] args);
```

### Comparing Numerics of Different Types

The method overloads that compare two objects make special provision so that numeric values of different types compare as expected. This assert succeeds:

```c#
Assert.AreEqual(5, 5.0);
```

### Comparing Floating Point Values

Values of type float and double may be compared using an additional argument that indicates a tolerance within which they will be considered as equal.

```c#
Assert.AreEqual(5.0, 4.99999, 0.001);
```

Special float and double values are handled so that the following Asserts succeed:

```c#
Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity);
Assert.AreEqual(double.NegativeInfinity, double.NegativeInfinity);
Assert.AreEqual(double.NaN, double.NaN);
```

### Comparing Arrays and Collections

TC-Lite is able to compare single-dimensioned arrays, multi-dimensioned arrays, nested arrays (arrays of arrays), lists, collections and enumerations. Two arrays or collections are considered equal if they have the same dimensions and if each pair of corresponding elements is equal.

### Dictionaries

adds the ability to compare generic collections and dictionaries.

### See also
 * [Assert.AreEqual API](/api/TCLite/Assert)