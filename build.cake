// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";

var BIN_DIR_3_1 = BIN_DIR + "netcoreapp3.1/";
var BIN_DIR_5_0 = BIN_DIR + "net5.0";
    
var TEST_ASSEMBLY = "tclite.tests.dll";
var TEST_EXECUTABLE = "tclite.tests.exe";

// Deletes the contents of the Artifacts folder if it contains anything from a previous build.
Task("Clean")
    .Does(() =>
    {
        CleanDirectory(BIN_DIR);
    });

// Run dotnet restore to restore all package references.
Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

// Build using the build configuration specified as an argument.
 Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetCoreBuild(".",
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    });

Task("TestNetCoreApp31")
    .IsDependentOn("Build")
    .Does(() =>
    {
        StartProcess("dotnet", System.IO.Path.Combine(BIN_DIR_3_1, TEST_ASSEMBLY));
    });

Task("TestNet50")
    .IsDependentOn("Build")
    .Does(() =>
    {
        StartProcess("dotnet", System.IO.Path.Combine(BIN_DIR_5_0, TEST_ASSEMBLY));
    });

Task("Test")
    .IsDependentOn("TestNetCoreApp31")
    .IsDependentOn("TestNet50");

Task("AppVeyor")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("All")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Default")
    .IsDependentOn("Test");

// Executes the task specified in the target argument.
RunTarget(target);
