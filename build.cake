// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";

var SUPPORTED_TEST_PLATFORMS = new [] { "netcoreapp3.1" };
var TEST_ASSEMBLY = "tclite.tests.dll";

// Deletes the contents of the Artifacts folder if it contains anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(BIN_DIR);
    });

// Run dotnet restore to restore all package references.
Task("Restore")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

// Build using the build configuration specified as an argument.
 Task("Build")
    .Does(() =>
    {
        DotNetCoreBuild(".",
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });

Task("Test")
    .Does(() =>
    {
        foreach (var platform in SUPPORTED_TEST_PLATFORMS)
            StartProcess("dotnet", System.IO.Path.Combine(BIN_DIR + platform, TEST_ASSEMBLY));
    });

// A meta-task that runs all the steps to Build and Test the app
Task("BuildAndTest")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("AppVeyor")
    .IsDependentOn("BuildAndTest");

Task("Travis")
    .IsDependentOn("BuildAndTest");

// The default task to run if none is explicitly specified. In this case, we want
// to run everything starting from Clean, all the way up to Publish.
Task("Default")
    .IsDependentOn("BuildAndTest");

// Executes the task specified in the target argument.
RunTarget(target);
