Title: TestCaseDataAttribute
Description: Identifies a test method together with a static source from which arguments are retrieved.
---

TestCaseDataAttribute is used on a parameterized test method to identify the source from which
the required arguments will be provided. The attribute additionally identifies the method as a
test method. The data is kept separate from the test itself and may be used by multiple test
methods. See [Test Cases][1] for a general introduction to tests with arguments.

# Syntax

```c#
public TestCaseDataAttribute(string sourceName);
public TestCaseDataAttribute(Type sourceType, string sourceName);
```

The `sourceName` argument gives the name of a static field, property or method, which will
supply the data for test cases. The `sourceType` argument is the `Type` of a class, which
contains that field, property or method. When `sourceType` is not given, the source is
expected to be in the class containing the test.

Wherever it is found, the source of data must have the following characteristics:

* It must be a static field, property or method.
* It must return an `IEnumerable` or a type that implements `IEnumerable`.
* The individual items returned by the enumerator must be compatible with the signature
  of the method on which the attribute appears. See the **Test Case Construction** section
  below for details.

# Examples

Consider a test of the divide operation, taking three arguments: the numerator, the denominator and the expected result. We can specify the test and its data in several ways.

Embedding the data in the test class itself...

```csharp
public class MyTestClass
{
    [TestCaseData(nameof(DivideCases))]
    public void DivideTest(int n, int d, int q)
    {
        Assert.AreEqual(q, n / d);
    }

    static TestCaseData[] DivideCases = new TestCaseData[]
    {
        new TestCaseData(12, 3, 4),
        new TestCaseData(12, 2, 6),
        new TestCaseData(12, 4, 3)
    };
}
```

Placing the data in a separate class.

```csharp
public class MyTestClass
{
    [TestCaseData(typeof(AnotherClass), "DivideCases")]
    public void DivideTest(int n, int d, int q)
    {
        Assert.AreEqual(q, n / d);
    }
}

static class AnotherClass
{
    static TestCaseData[] DivideCases = new TestCaseData[]
    {
        new TestCaseData(12, 3, 4),
        new TestCaseData(12, 2, 6),
        new TestCaseData(12, 4, 3)
    };
}
```

Using a nested class...

```csharp
public class MyTestClass
{
    [TestCaseData(typeof(NestedClass), "DivideCases")]
    public void DivideTest(int n, int d, int q)
    {
        Assert.AreEqual(q, n / d);
    }

    static class NestedClass
    {
        static TestCaseData[] DivideCases = new TestCaseData[]
        {
            new TestCaseData(12, 3, 4),
            new TestCaseData(12, 2, 6),
            new TestCaseData(12, 4, 3)
        };
    }
}
```

# Variations

All three examples above provide the data using a static field of type `TestCaseData[]`,
which is initialized with three `TestCaseData` members containing the actual arguments.
We could have used a property or a method rather than a field and each of them could
be initialized in different ways. Here are some variations.

## Using Fields

We can return an `object` array containing `object` arrays...

```csharp
        static object[] DivideCases = new object[]
        {
            new object[](12, 3, 4),
            new object[](12, 2, 6),
            new object[](12, 4, 3)
        };
```

We can return an `object` array containing `int` arrays in this case, because all the arguments
are `int`...

```csharp
        static object[] DivideCases = new object[]
        {
            new int[](12, 3, 4),
            new int[](12, 2, 6),
            new int[](12, 4, 3)
        };
```

We could even return an object array containing TestCaseData items...

```csharp
        static object[] DivideCases = new object[]
        {
            new TestCaseData(12, 3, 4),
            new TestCaseData(12, 2, 6),
            new TestCaseData(12, 4, 3)
        };
```

## Using Properties

Any of the approaches shown for fields can also be used for properties. This is generally preferred
in a separate class to avoid exposing a field publicly. For example...

```csharp
        static TestCaseData[] DivideCases => new TestCaseData[]
        {
            new TestCaseData(12, 3, 4),
            new TestCaseData(12, 2, 6),
            new TestCaseData(12, 4, 3)
        };
```

With properties, you may also use a `yield` expression...

```csharp
        static IEnumerable<TestCaseData> DivideCases
        {
            get
            {
                yield return new TestCaseData(12, 3, 4);
                yield return new TestCaseData(12, 2, 6);
                yield return new TestCaseData(12, 4, 3);
            }
        };
```

## Using Methods

Methods provide exactly the same capabilities as properties but may be more convenient in
some situations...

```csharp
        static TestCaseData[] DivideCases()
        {
            return new TestCaseData[]
            {
                new TestCaseData(12, 3, 4),
                new TestCaseData(12, 2, 6),
                new TestCaseData(12, 4, 3)
            };
        }
```

and...

```csharp
        static IEnumerable<TestCaseData> DivideCases()
        {
            yield return new TestCaseData(12, 3, 4);
            yield return new TestCaseData(12, 2, 6);
            yield return new TestCaseData(12, 4, 3);
        }
```

## Special cases

If your test method takes only one argument, you may use a single array of the correct type,
which can save a bit of typing...

```csharp
[TestCaseData(nameof(EvenNumbers))]
public void TestMethod(int num)
{
    Assert.IsTrue(num % 2 == 0);
}

static int[] EvenNumbers = new int[] { 2, 4, 6, 8 };
```

If your test method actually takes an array as an argument, some of the techniques using
nested arrays may not work. **TC-Lite** may be confused and give an error. With too many 
levels of nested arrays, the user may be confused as well. Use `TestCaseData` in such cases...

```csharp
[TestCaseData(nameof(ArrayData))]
public void TestMethod(object[][] array)
{
    Assert.That(array[0].Length, Is.EqualTo(5)]);
    Assert.That(array[1].Length, Is.EqualTo(5)]);
    Assert.That(array[2].Length, Is.EqualTo(5)]);
}

static int[][] jaggedArray1 = new int[][]
{
    new int[] { 1, 3, 5, 7, 9 },
    new int[] { 0, 2, 4, 6 },
    new int[] { 11, 22 }
};

static int[][] jaggedArray2 = new int[][]
{
    new int[] { 2, 4, 6, 8, 10 },
    new int[] { 1, 3, 5, 7 },
    new int[] { 17, 34 }
};

static TestCaseData[] ArrayData()
{
    new TestCaseData(jaggedArray1),
    new TestCaseData(jaggedArray2)
};
```

# Notes

1. If the data will be used in more than one test fixture, place the data sources in a separate
class. This should generally be a `static` class, since it will usually only hold static members.

2. If the data is only used in one class, there is some advantage in placing it near it's point of
usage within the test fixture class itself.

3. A generic `IEnumerable` and `IEnumerator` may be used although **TC-Lite** will currently use
the underlying non-generic interfaces. Future releases may be able to detect and use the generic
implementations for greater efficiency.

4. As with all test cases, the order of execution is not specified. It should not be relied upon to remain stable from one release to another or even across different compilers.

# Named Parameters

**TestCaseDataAttribute** supports one named parameter:

* **Category** is used to assign one or more categories to every test case returned.
  If multiple categories are specified, they should be separated by commas.
* **Description** sets the description property of the individual test case.

# See Also

* [Overview of Test Cases][1]
* [TestCaseData API][2]
* [TestCase Attribute][3]
* [TestCaseFactory Attribute][4]

[1]: /tc-lite/docs/Concepts/test-cases.html
[2]: /tc-lite/api/TestCaseData/
[3]: /tc-lite/docs/Features/Attributes/testcase-attribute.html
[4]: /tc-lite/docs/Features/Attributes/testcasefactory-attribute.html
