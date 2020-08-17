#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.3.1"
#addin nuget:?package=Cake.Git&version=0.22.0

// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information($"Running target {target} in configuration {configuration}");

var PROJECT_DIR = Context.Environment.WorkingDirectory.FullPath + "/";
var BIN_DIR = PROJECT_DIR + "bin/" + configuration + "/";
var OUTPUT_DIR = PROJECT_DIR + "output/";
var WEB_DIR = PROJECT_DIR + "web/";
var WEB_OUTPUT_DIR = WEB_DIR + "output/";
var WEB_DEPLOY_DIR = System.IO.Path.GetFullPath("../tc-lite.deploy/");

var NUGET_ID = "TCLite";
var VERSION = "0.1.1";

const string WYAM = "wyam";
const string PROJECT_URI = "https://github.com/TestCentric/tc-lite";
const string DEPLOY_BRANCH = "gh-pages";

const string USER_ID = "USER_ID";
const string USER_EMAIL = "USER_EMAIL";
const string GITHUB_PASSWORD = "GITHUB_PASSWORD";

string UserId;
string UserEmail;
string GitHubPassword;

Setup((context) =>
{
    UserId = context.EnvironmentVariable(USER_ID);
    UserEmail = context.EnvironmentVariable(USER_EMAIL);
    GitHubPassword = context.EnvironmentVariable(GITHUB_PASSWORD);

});

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

Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
    {
        CreateDirectory(OUTPUT_DIR);

        NuGetPack(PROJECT_DIR + "nuspec/tclite.nuspec",
            new NuGetPackSettings()
            {
                Id = NUGET_ID,
                Version = VERSION,
                OutputDirectory = OUTPUT_DIR,
                Files = new [] {
                    new NuSpecContent { Source = PROJECT_DIR + "LICENSE" },
                    new NuSpecContent { Source = PROJECT_DIR + "testcentric_128x128.png"},
                    new NuSpecContent { Source = BIN_DIR + "netstandard2.0/tclite.dll", Target = "lib/netstandard2.0" },
                }
            });
    });

Task("BuildWebsite")
    .Does(() => StartProcess(WYAM, new ProcessSettings()
    {
        Arguments = "build",
        WorkingDirectory = WEB_DIR
    }));

Task("PreviewWebsite")
    .IsDependentOn("BuildWebsite")
    .Does(() => StartProcess(WYAM, $"preview {WEB_OUTPUT_DIR} --virtual-dir /tc-lite"));

Task("DeployWebsite")
    .IsDependentOn("BuildWebsite")
    .Does(() => 
    {
        if(FileExists("./CNAME"))
            CopyFileToDirectory("./CNAME", WEB_OUTPUT_DIR);

        if (DirectoryExists(WEB_DEPLOY_DIR))
            DeleteDirectory(WEB_DEPLOY_DIR, new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });

        GitClone(PROJECT_URI, WEB_DEPLOY_DIR, new GitCloneSettings()
        {
            Checkout = true,
            BranchName = DEPLOY_BRANCH
        });

        CopyDirectory(WEB_OUTPUT_DIR, WEB_DEPLOY_DIR);

        GitAddAll(WEB_DEPLOY_DIR);
        GitCommit(WEB_DEPLOY_DIR, UserId, UserEmail, "Deploy site to GitHub Pages");
        GitPush(WEB_DEPLOY_DIR, UserId, GitHubPassword);
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
    .IsDependentOn("Test");

Task("All")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Default")
    .IsDependentOn("Test");

// Executes the task specified in the target argument.
RunTarget(target);
