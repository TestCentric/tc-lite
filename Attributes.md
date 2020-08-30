# Attributes

Like NUnit, TC-Lite uses custom attributes to identify tests. This table lists all NUnit attributes and their TC-Lite equivalents, where available.

|   NUnit                         |   TC-Lite          |
| ------------------------------- | ------------------ |
| [Apartment]                     | Not Supported      |
| [Author]                        | Not Supported      |
| [Category]                      | [Category]         |
| [Combinatorial]                 | [Combinatorial]    |
| [Culture]                       | [Include]          |
| [Datapoint]                     | Not Supported      |
| [DatapointSource]               | Not Supported      |
| [DefaultFloatingPointTolerance] | [DefaultTolerance] |
| [Description]                   | [Description]      |
| [Explicit]                      | Not Supported      |
| [Ignore]                        | [Ignore]           |
| [LevelOfParallelism]            | NYI                |
| [MaxTime]                       | NYI                |
| [NonParallelizable]             | NYI                |
| [NonTestAssembly]               | Not Supported      |
| [OneTimeSetUp]                  | Not Supported      |
| [OneTimeTearDown]               | Not Supported      |
| [Order]                         | NYI                |
| [Pairwise]                      | NYI                |
| [Parallelizable]                | NYI                |
| [Platform]                      | [Include]          |
| [Property]                      | [Property]         |
| [Random]                        | [Random]           |
| [Range]                         | [Range]            |
| [Repeat]                        | NYI                |
| [RequiresThread]                | Not Supported      |
| [Retry]                         | NYI                |
| [Sequential]                    | Not Supported      |
| [SetCulture]                    | NYI                |
| [SetUICulture]                  | NYI                |
| [SetUp]                         | Not Supported      |
| [SetUpFixture]                  | Not Supported      |
| [SingleThreaded]                | NYI                |
| [TearDown]                      | Not Supported      |
| [Test]                          | [TestCase]         |
| [TestCase]                      | [TestCase]         |
| [TestCaseSource]                | ?                  |
| [TestFixture]                   | [TestFixture]      |
| [TestFixtureSource]             | ?                  |
| [TestOf]                        | Not Supported      |
| [Theory]                        | NYI                |
| [Timeout]                       | NYI                |
| [Values]                        | [Values]           |
| [ValueSource]                   | [ValueSource]      |
