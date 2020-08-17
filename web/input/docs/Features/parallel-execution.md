Title: Parallel Test Execution
Description: Describes the planned parallel execution feature.
Order: 6
---

__Note:__ The feature described on this page is not yet implemented.

Attributes for indicating how tests are permitted to run in parallel. The implementation will be a simpler one than that in _NUnit_ since we are using separate fixtures for each test case and are not constrained by backward compatibility.

The following is a starting set of features:

* Attributes should indicate what __may__ or __may not__ run in Parallel. It's up to the __TC-Lite__ framework to decide whether to actually use parallel execution in a given environment.

* An assembly-level attribute should allow specifying the maximum number of tests allowed to execute at one time.

* A pair of class- and method-level attributes should allow specifying whether a given test may or may not be run in parallel with other tests.

* (Possibly) an assembly level attribute to set the default level of parallelism. This requires some thought since it can requires __every__ test to be written in a way that allows it to run in parallel. However, it is a great convenience to be able to specify the most common setting once and then override it at the class or method level.
