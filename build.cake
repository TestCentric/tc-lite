#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.3.1"

// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";
var OUTPUT_DIR = PROJECT_DIR + "output/";

var NUGET_ID = "TCLite";
var VERSION = "0.1.0";

// Metadata used in the nuget and chocolatey packages
var TITLE = "TCLite Test Framework";
var AUTHORS = new [] { "Charlie Poole" };
var OWNERS = new [] { "Charlie Poole" };
var DESCRIPTION = "A simple .NET framework for writing and running microtests. It features self-executing test assembiles, isolation of tests, fluent assertions and close easy conversion from NUnit.";
var SUMMARY = "A simple .NET framework for writing and running microtests.";
var COPYRIGHT = "Copyright (c) 2020 Charlie Poole";
var RELEASE_NOTES = new [] { "See https://github.com/testcentric/tc-lite/blob/master/README.md" };
var TAGS = new [] { "tc-lite", "tclite", "nunit", "test", "testing", "tdd", "runner" };

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
        StartProcess("dotnet", BIN_DIR + "netcoreapp3.1/tclite.tests.dll");
    });

Task("TestNet50")
    .IsDependentOn("Build")
    .Does(() =>
    {
        StartProcess("dotnet", BIN_DIR + "net5.0/tclite.tests.dll");
    });

// Additional package metadata
var GITHUB_SITE = "https://github.com/testcentric/tc-lite";
var PROJECT_URL = new Uri("https://test-centric.org");
var ICON = "testcentric_128x128";
var ICON_URL = new Uri($"https://github.com/testcentric/tc-lite/blob/master/{ICON}");
var LICENSE_URL = new Uri("https://github.com/testcentric/tc-lite/blob/master/LICENSE");

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CreateDirectory(OUTPUT_DIR);

        NuGetPack(
            new NuGetPackSettings()
            {
                Id = NUGET_ID,
                Version = VERSION,
                Title = TITLE,
                Authors = AUTHORS,
                Owners = OWNERS,
                Description = DESCRIPTION,
                Summary = SUMMARY,
                ProjectUrl = PROJECT_URL,
                //Icon = ICON,
                IconUrl = ICON_URL,
                //License = "LICENSE",
                LicenseUrl = LICENSE_URL,
                RequireLicenseAcceptance = false,
                Copyright = COPYRIGHT,
                ReleaseNotes = RELEASE_NOTES,
                Tags = TAGS,
                //Language = "en-US",
                OutputDirectory = OUTPUT_DIR,
                Repository = new NuGetRepository {
                    Type = "git",
                    Url = GITHUB_SITE
                },
                Files = new [] {
                    new NuSpecContent { Source = PROJECT_DIR + "LICENSE" },
                    new NuSpecContent { Source = PROJECT_DIR + "testcentric_128x128.png"},
                    new NuSpecContent { Source = BIN_DIR + "netstandard2.0/tclite.dll", Target = "tools/netstandard2.0" },
                }
            });
    });

Task("Test")
    .IsDependentOn("TestNetCoreApp31")
    .IsDependentOn("TestNet50");

Task("AppVeyor")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("All")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Test");

// Executes the task specified in the target argument.
RunTarget(target);
