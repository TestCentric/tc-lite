// Load the recipe
#load nuget:?package=TestCentric.Cake.Recipe&version=1.1.0-dev00084
// Comment out above line and uncomment below for local tests of recipe changes
//#load ../TestCentric.Cake.Recipe/recipe/*.cake

BuildSettings.Initialize
(
	context: Context,
	title: "TC Lite",
	solutionFile: "tc-lite.sln",
	unitTests: "**/*.tests.exe",
	githubOwner: "TestCentric",
	githubRepository: "tc-lite",
	exemptFiles: new [] { "Options.cs" } 
);

Task("AppVeyor")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Package");

Task("Travis")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Default")
    .IsDependentOn("Test");

// Executes the task specified in the target argument.
RunTarget(CommandLineOptions.Target.Value);
