Title: TestCaseFactoryAttribute
Description: Identifies a test method together with a test case factory to provide arguments. Use of a factory provides the greatest level of control in generating arguments dynamically.
---

**TestCaseFactoryAttribute** is used on a parameterized test method to identify the factory Type,
which will provide the required arguments. The attribute additionally identifies the method as a
test method. By use of a factory, the data is kept separate from the test itself and may be used
by multiple test methods.

# Example

Consider a test of the divide operation, taking three arguments: the numerator, the denominator and the expected result. We can specify the test and its data using **TestCaseFactoryAttribute**,
as follows:

```c#
public class MyTestClass
{
    [TestCaseFactory(typeof(DivideCases))]
    public void DivideTest(int n, int d, int q)
    {
        Assert.AreEqual(q, n / d);
    }
}

class DivideCases : ITestCaseFactory
{
    public IEnumerator<ITestCaseData> GetEnumerator()
    {
        yield return new TestCaseData( 12, 3, 4 );
        yield return new TestCaseData( 12, 2, 6 );
        yield return new TestCaseData( 12, 4, 3 );
    }
}
```

The Type argument in this form represents the class that provides test cases.
It must have a default constructor and implement `ITestCaseFactory`. The enumerator
must return `ITestCaseData` items compatible with the signature of the test on which
the attribute appears.

See the **Test Case Construction** section below for details.

Note that it is not possible to pass parameters to the source, even if the source is a method.

# Named Parameters

**TestCaseFactoryAttribute** supports one named parameter:

* **Category** is used to assign one or more categories to every test case returned from this source.

# See Also

* [Overview of Test Cases][1]
* [TestCaseData Attribute][2]
* [TestCase Attribute][3]

[1]: /tc-lite/docs/Concepts/test-cases.html
[2]: /tc-lite/docs/Features/Attributes/testcasedata-attribute.html
[3]: /tc-lite/docs/Features/Attributes/testcase-attribute.html
