#tool "nuget:?package=GitVersion.CommandLine"

#tool "nuget:?package=xunit.runner.console"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solution = "./src/Cake.OpenCoverToCoberturaConverter.sln";

var appName = "Cake.OpenCoverToCoberturaConverter";

var local = BuildSystem.IsLocalBuild;
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
var buildNumber = AppVeyor.Environment.Build.Number;


var branchName = isRunningOnAppVeyor ? EnvironmentVariable("APPVEYOR_REPO_BRANCH") : "master";
var isMasterBranch = System.String.Equals("master", branchName, System.StringComparison.OrdinalIgnoreCase);

///////////////////////////////////////////////////////////////////////////////
// VERSION
///////////////////////////////////////////////////////////////////////////////

var gitVersion = GitVersion();

string GetDotNetCoreArgsVersions()
{
        string version = string.Concat(gitVersion.Major, ".", gitVersion.Minor);
        string semVersion = string.Concat(version, ".", gitVersion.BuildMetaData, ".", buildNumber);
        string nuget = semVersion;
        // tag non master branches with pre-release 
        // gitversion in the future will support something similar
        if(!isMasterBranch && !local) 
            nuget += "-" + branchName;

    return string.Format(
        @"/p:Version={1} /p:AssemblyVersion={0} /p:FileVersion={0} /p:ProductVersion={0}",
        semVersion, nuget);
}
///////////////////////////////////////////////////////////////////////////////
// PREPARE
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() => {
		CleanDirectory("./nuget");
	});

Task("Restore-Nuget-Packages")
	.IsDependentOn("Clean")
	.Does(() => {
		
		DotNetCoreRestore(solution, new DotNetCoreRestoreSettings()
		{
		});

	});

//////////////////////////////////////////////////////////////////////////////
// Build
//////////////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Restore-Nuget-Packages")
	.Does(() => {

		DotNetCoreBuild(solution,
			new DotNetCoreBuildSettings {
				Configuration = configuration,
				ArgumentCustomization = aggs => aggs
						.Append(GetDotNetCoreArgsVersions())
			});

	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() => {
		DotNetCoreTest("./src/Cake.OpenCoverToCoberturaConverter.Test/Cake.OpenCoverToCoberturaConverter.Test.csproj");
	});

//////////////////////////////////////////////////////////////////////////////
// Nuget
//////////////////////////////////////////////////////////////////////////////

Task("Pack")
	.IsDependentOn("Test")
	.Does(() => {
		
		CleanDirectory("nuget");
		CreateDirectory("nuget");

		DotNetCorePack(
            "./src/Cake.OpenCoverToCoberturaConverter/Cake.OpenCoverToCoberturaConverter.csproj",
            new DotNetCorePackSettings 
            {
                Configuration = configuration,
                OutputDirectory = "./nuget",
                NoBuild = true,
                ArgumentCustomization = aggs => aggs
                    .Append(GetDotNetCoreArgsVersions())
            }
        );
	});

Task("Publish")
	.IsDependentOn("Pack")
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isMasterBranch)
	.Does(() => {
		
		// Resolve the API key.
		var apiKey = EnvironmentVariable("NUGET_API_KEY");
		if(string.IsNullOrEmpty(apiKey)) {
			throw new InvalidOperationException("Could not resolve NuGet API key.");
		}

		// Resolve the API url.
		var apiUrl = EnvironmentVariable("NUGET_URL");
		if(string.IsNullOrEmpty(apiUrl)) {
			throw new InvalidOperationException("Could not resolve NuGet API url.");
		}

		var package = "./nuget/Cake.OpenCoverToCoberturaConverter." + gitVersion.NuGetVersionV2 + ".nupkg";
            
		// Push the package.
		NuGetPush(package, new NuGetPushSettings {
    		Source = apiUrl,
    		ApiKey = apiKey
		});
	});

Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(gitVersion.FullSemVer);
});

	

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("Pack");
Task("AppVeyor")
	.IsDependentOn("Update-AppVeyor-Build-Number")
	.IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
