#addin nuget:?package=Cake.Git&version=0.22.0

using Path = System.IO.Path;

var target = Argument("target", "Default");

var OUTPUT_DIR = Path.GetFullPath("output/");
var DEPLOY_DIR = Path.GetFullPath("../../tc-lite.deploy/");

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

Task("Build")
    .Does(() => StartProcess(WYAM, "build"));

Task("Preview")
    .IsDependentOn("Build")
    .Does(() => StartProcess(WYAM, "preview --virtual-dir /tc-lite"));

Task("Deploy")
    .IsDependentOn("Build")
    .Does(() => 
    {
        if(FileExists("./CNAME"))
            CopyFileToDirectory("./CNAME", OUTPUT_DIR);

        if (DirectoryExists(DEPLOY_DIR))
            DeleteDirectory(DEPLOY_DIR, new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });

        GitClone(PROJECT_URI, DEPLOY_DIR, new GitCloneSettings()
        {
            Checkout = true,
            BranchName = DEPLOY_BRANCH
        });

        CopyDirectory(OUTPUT_DIR, DEPLOY_DIR);

        GitAddAll(DEPLOY_DIR);
        GitCommit(DEPLOY_DIR, UserId, UserEmail, "Deploy site to GitHub Pages");
        GitPush(DEPLOY_DIR, UserId, GitHubPassword);
    });

Task("Default")
    .IsDependentOn("Build");
    
RunTarget(target);
