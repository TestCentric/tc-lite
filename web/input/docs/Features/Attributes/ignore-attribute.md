Title: IgnoreAttribute
Description: 
---

IgnoreAttribute is used to indicate that a test should not be executed for some reason,
which must be specified as an argument. Ignored tests are displayed as warnings in order to
provide a reminder that the test needs to be corrected or otherwise changed and re-instated.

The IgnoreAttribute is attached to a method. If that method produces multiple test cases,
all the cases will be ignored. Individual test cases must be ignored through the `Ignore`
property of the `TestCase` attribute.
