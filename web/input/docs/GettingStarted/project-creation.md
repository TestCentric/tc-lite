Title: Creating a Test Project
Description: How to set up a  test project to contain your TC-Lite tests.
Order: 1
---

You can create your **TC-Lite** test project using various tools. On this page,
I'll describe the use of Visual Studio Code and the dotnet CLI. Other approaches, 
including use of Visual Studio, will be added soon.

The example used here continues in some of the following pages as well. It involves
calculation of the values of a Fibonacci series. The following names are used:

 * `Fibonacci` the name of the solution.
 * `FibLib` the name of the library being tested.
 * `FibTests` the name of the test project.

# Create the Solution

Start by creating a folder called `Fibonacci`. You can do this in VSCode
using the `File | Open Folder` menu item and clicking on the `Create Folder`
button. Alternatively, create your folder from the command-line using the
appropriate command for your operating system. If you use the command-line,
you should enter the new folder (`cd Fibonacci`). If you used VSCode, you're
already in that folder.

At the command-line or in the VSCode terminal window enter

```bash
dotnet new sln
```

**Note:** All the initial steps on this page may be carried out either at the
command-line or in the VSCode Terminal panel. This is handy to know in some cases,
as when you want to create a script to set up your projects.

# Create the Project Under Test

From within the solution folder, enter the following commands:

```bash
dotnet new classlib -o FibLib
dotnet sln add FibLib
```

# Create the Test Project

Still in the solution folder, enter the following commands:

```bash
dotnet new console -o FibTests
dotnet sln add FibTests
```

**Note** A **TC-Lite** test project must be a console application.

# Add a Reference to the Project Under Test

At this point you have a solution with two projects, `FibLib` and `FibTests`.
`FibTests` needs to know about `FibLib` so, still working in the solution folder,
enter the following command:

```bash
dotnet add FibTests reference FibLib
```

# Add a Reference to TCLite

The test project also needs a reference to **TCLite** itself. Enter the following command:

```bash
dotnet add FibTests package TCLite
```

The package reference is added to the project and the package is restored from NuGet.org.

# Solution Structure

You should now have a folder structure like this:

```text
Fibonacci/
|  +--Fibonacci.sln
|
+--MyLibrary/
|  +--Class1.cs
|  +--FibLib.csproj
|
+--MyTests/
   |--Program.cs
   +--FibTests.csproj
```

The project files are ready to use unless you want to change the `TargetFramework`
in either of them. In my case, I performed the above steps using .NET 5.0, so both
my projects target that framework. Other defaults will be used depending on the
latest version of the SDK you have installed.

The source files `Class1.cs` and `Program.cs` are sample files that dotnet CLI
generated for you. You should delete `Class1.cs`.

# Call the TestRunner

The final step in setting up your project is to make sure the **TC-Lite** TestRunner
is called. Edit `Program.cs` so that it contains the following code:

```c#
using TCLite.Runners;

namespace FibTests
{
    class Program
    {
        static int Main(string[] args)
        {
            return new TCLite.Runners.TestRunner().Execute(args);
        }
    }
}
```

This code will run when you execute your test assembly. It passes the arguments to
**TCLite**'s `TestRunner` class, which is then able to examine the test assembly,
discover tests and execute them.

You can verify that by building and running the test project. Enter

```bash
dotnet run -p FibTests
```

TCLite builds your projects and tries to run your test assembly. It gives an error
message indicating that it found no tests, because we haven't created any so far.
We'll do that on the [next page](writing-tests.html).
