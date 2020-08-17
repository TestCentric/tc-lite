Title: RangeAttribute
Description: 
---

The **RangeAttribute** is used to specify a range of values to be provided for an individual
parameter of a parameterized test method. **TC-Lite** combines the data
provided for each parameter into a set of test cases representing all possible combinations
of the data provided. In general, data must be provided for all parameters if it is provided
for any of them.

RangeAttribute supports the following constructors:

```c#
public RangeAttribute(int from, int to);
public RangeAttribute(int from, int to, int step);
public RangeAttribute(long from, long to, long step);
public RangeAttribute(float from, float to, float step);
public RangeAttribute(double from, double to, double step);
```
