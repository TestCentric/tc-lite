Title: Test Fixtures
Description: Describes the role played by the classes that contain tests.
Order: 3
---

In **TC-Lite**, a `TestFixture` is simply a class containing tests. A __separate instance__ of
the fixture class is created for each test case that is run and is disposed when the test case
completes. That shortened life span means that the state of the fixture does not persist 
beyond a single test.

If you come to **TC-Lite** from **NUnit** this is a big change but it's actually the way most
xUnit-style test frameworks work. Using a separate fixture instance for each test case, makes
it harder for tests to interfere with one another. It also makes parallel execution a lot 
simpler and reduces the size of the framework.

To work well within this model, fixtures must be very lightweight. Although the fixture
constructor may be used to initialize instance members, this should usually be limited to those
needed by every test. In rare instances, disposal of the fixture may be required. **TC-Lite**
will call `Dispose` for any fixture class, which implements `IDisposable`.

A `TestFixture` is normally recognized by the fact that the class contains `TestCases`. It may
optionally be indicated by use of the `TestFixtureAttribute` on the class.
