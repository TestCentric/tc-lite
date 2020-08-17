Title: ValueSourceAttribute
Description: 
---

**ValueSource** is used on individual parameters of a test method to identify a named source for the argument values to be supplied. The attribute has two public constructors.

```c#
ValueSourceAttribute(Type sourceType, string sourceName);
ValueSourceAttribute(string sourceName);
```

If sourceType is specified, it represents the class that provides the data.

If sourceType is not specified, the class containing the test method is used.

The sourceName, represents the name of the source that will provide the arguments. It should have the following characteristics:

* It may be a field, a non-indexed property or a method taking no arguments.
* It must be a static member.
* It must return an IEnumerable or a type that implements IEnumerable.
* The individual items returned from the enumerator must be compatible with the type of the parameter on which the attribute appears.

