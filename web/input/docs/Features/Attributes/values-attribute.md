Title: ValuesAttribute
Description: 
---

The ValuesAttribute is used to specify a set of values to be provided for an individual parameter of a parameterized test method. Since NUnit combines the data provided for each parameter into a set of test cases, data must be provided for all parameters if it is provided for any of them.

By default, NUnit creates test cases from all possible combinations of the data values provided on parameters - the combinatorial approach. This default may be modified by use of specific attributes on the test method itself.