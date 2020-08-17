Title: Writing Tests
Description: How to write your tests.
Order: 2
---

# Class Under Test

Normally, I'd write the tests first. **TC-Lite** is very suitable for doing **Test-Driven Development**. However, on this page, we just want to to illustrate the mechanics of writing
tests, so we'll start by building the class to be tested.

 In the `FibLib` directory create a file named
`Fibonacci.cs` with the following content:

```c#
namespace FibLib
{
    public class Fibonacci
    {
        public int Number(int n)
        {  
            return n == 0 ? 0
                 : n == 1 ? 1
                 : Number(n-2) + Number(n-1);
        }
    }
}
```

This class implements a method to return elements of a Fibonacci sequence. It uses recursion
and is not very efficient, but will serve as a basis for writing some tests.

The observant reader may notice that both the class and the method could have been static.
Although that would work quite well it wouldn't allow me to show you something that I want
to demonstrate later.

# First Tests

Create a file `FibonacciTests.cs` in the `FibTests` directory, with the following content:

```c#
using TCLite;
using FibLib

namespace FibTests
{
    public class FibonacciTests
    {
        readonly Fibonacci fib = new Fibonacci();

        [TestCase]
        public void ElementZeroIsZero()
        {
            Assert.That(fib.Number(0), Is.EqualTo(0));
        }

        [TestCase]
        public void ElementOneIsOne()
        {
            Assert.That(fib.Number(1), Is.EqualTo(1));
        }

        [TestCase]
        public void ElementEightIsTwentyOne()
        {
            Assert.That(fib.Number(8), Is.EqualTo(21));
        }
    }
}
```

Notice the initialization of the field `fib` as part of the construction
of an instance of `Fibonacci`. **TC-Lite** constructs a new instance of
the class for use by each test case. That instance is generally known as
a "Test Fixture" in the lingo of unit testing. The "fixture" is basically
the set of data operated on by the tests.

If you came to **TC-Lite** from **NUnit** you may think of the fixture as
being _shared_ across all the tests. In **TC-Lite** that's not so. Each
test case gets a completely new fixture. This makes inline initialization
of each field an elegant and simple replacement for `SetUp` methods.

Note too that I made the field `readonly`, because I don't want to change it.
Of course, in such simple tests, it's obvious that I am not changing it but
using `readonly` is a good practice if you have no intention of changing a field.

Our tests are rather verbose, but we'll fix that later. For now, run them...

```bash
dotnet run -p FibTests
```

The output shows that our three tests passed:

```text
Test Run Summary
  Overall result: Passed
  Test Count: 3, Passed: 3, Failed: 0, Warnings: 0, Inconclusive: 0
    Duration: 0.000 seconds
```

# Parameterized Tests

Our three test methods basically do the same thing but use different values. We can 
simplify the test code by use of a single method that takes arguments.

Modify `FibonacciTests.cs` to contain just one method:

```c#
using TCLite;
using FibLib

namespace FibTests
{
    public class FibonacciTests
    {
        readonly Fibonacci fib = new Fibonacci();

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(8, 21)]
        public void ElementsAreCorrect(int index, int value)
        {
            Assert.That(fib.Number(index), Is.EqualTo(value));
        }
    }
}
```

Since three sets of arguments are provided, three test cases are executed and the
output remains the same.

At this point, it's trivial to add more test values. The Fibonacci sequence is endless,
but we'll have to be satisfied with a finite number of cases. Go ahead and try this
yourself. Use some of the first few elements of the sequence...

```text
0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 56, 90, 146, 236, 382, 618...
```
