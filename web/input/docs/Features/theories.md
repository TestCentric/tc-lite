Title: Theories
Description: Describes the planned Theories feature.
Order: 7
---

__Note:__ The feature described on this page is not yet implemented.

Continuation and extension of NUnit's _Theories_.

* `[Theory]` is reserved for use in implementing a new take on "theory tests" as originally implemented in _NUnit_. Note that this is __not__ equivalent to the `TheoryAttribute` implemented in _xUnit_, which is no more than a parameterized test, whereas the original academic work on "theories" gave the responsibility of deciding which inputs are appropriate to the test itself. _TC-Lite's_ `[Theory]` will do that but may differ in details from the _NUnit_ implementation, which became somewhat frozen in its development in order to preserve backward compatibility. It's possible that `[Theory]` will not make the cut for the first release of **TC-Lite**.

* `[Datapoint]` and `[DatapointSource]` _or some equivalent_ will be developed along with theories. More research is needed to identify alternative approaches to either filtering or generating data for theories.
