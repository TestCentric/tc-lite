Title: Command-Line Options
Description: Lists options that may be used on the command-line.
Order: 8
---

When running the test assembly, various options may be specified and are passed on to the TC-Lite runner.

__Note:__ Where the feature has not yet been implemented, the option is labeled (NYI).

| Option | Comments |
| ------ | -------- |
| __Test Selection:__
| --where=TSL | TSL expression indicating which tests will be run. If omitted, all tests are run. |
| __How Tests are Run:__
| --params, -p=VALUE     | (NYI) Define test parameters.
| --timeout=MILLISECONDS | (NYI) Set default test case timeout in MILLISECONDS.
| --seed=SEED            | (NYI) Set the random SEED used to generate test data. Used for debugging earlier runs.
| --workers=NUMBER       | (NYI) Specify the NUMBER of worker threads to be used in running tests. If not specified, defaults to 2 or the number of processors, whichever is greater.
| --stoponerror          | (NYI) Stop run immediately upon any test failure or error.
| --wait                 | Wait for input before closing console window.
| __Test Output:__
| --work=PATH            | PATH of the directory to use for output files. If not specified, defaults to the current directory.
| --out=PATH             | (NYI) File PATH to contain text output from the tests.
| --err=PATH             | (NYI) File PATH to contain error output from the tests.
| --explore[=PATH]       | Explore tests rather than running them. The optional PATH is used for the XML report describing the tests. It defaults to 'tests.xml'.
| --result=PATH          | Save test result XML in file at PATH. If not specified, default is TestResult.xml.
| --format=FORMAT        | Specify the FORMAT to be used in saving the test result. May be `nunit3` or `nunit2'.
| --noresult             | Don't save any test results.
| --labels=VALUE         | Specify whether to write test case labels to the output. Values: Off, On, Before, After.
| --teamcity             | (NYI) Turns on use of TeamCity service messages.
| --trace=LEVEL          | (NYI) Set internal trace LEVEL. Values: Off, Error, Warning, Info, Verbose (Debug)
| --noheader, --noh      | Don't display program header at start of run.
| --nocolor, --noc       | Displays console output without color.
| --help, -h             | Display this message and exit.
| --version, -V          | Display the header and exit.
