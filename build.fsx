// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r @"tools\FAKE\tools\FakeLib.dll"
#r "System.Management.Automation"

open Fake
open Fake.AssemblyInfoFile
open Fake.Testing 
open System.Management.Automation

// --------------------------------------------------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------

let nugetProject = "kraken-net"
let project = "Kraken"
let product = "kraken.NET 4.5"
let authors = ["Kraken.io, Kevin Bronsdijk"]
let summary = "The official Kraken.io .Net client"
let version = "0.2.0.1"
let description = "The official kraken-net client interacts with the Kraken.io REST API allowing you to utilize Krakens features using a .NET interface."
let notes = "form data fix kraken-net/pull/22"
let nugetVersion = "2.0.1"
let tags = "kraken.io C# API image optimization official"
let gitHome = "https://github.com/kraken-io"
let gitName = "kraken-net"

// --------------------------------------------------------------------------------------
// Build script 
// --------------------------------------------------------------------------------------

let buildDir = "./output/"
let buildDirV2 = "./outputV2/"
let packagingOutputPath = "./nuGet/"
let packagingWorkingDir = "./inputNuget/"
let packagingWorkingDirV2 = "./inputNuget/v2/"
let nugetDependencies = getDependencies "./src/kraken-net/packages.config"

// --------------------------------------------------------------------------------------

Target "Clean" (fun _ ->
 CleanDir buildDir
 CleanDir buildDirV2
)

// --------------------------------------------------------------------------------------

Target "AssemblyInfo" (fun _ ->
    let attributes =
        [ 
            Attribute.Title project
            Attribute.Product product
            Attribute.Description summary
            Attribute.Version version
            Attribute.FileVersion version
            Attribute.Copyright "2020"
        ]

    CreateCSharpAssemblyInfo "src/kraken-net/Properties/AssemblyInfo.cs" attributes
)

// --------------------------------------------------------------------------------------

Target "RestorePackages" (fun _ -> 
     "src/Tests/packages.config"
     |> RestorePackage (fun p ->
         { p with
             Sources = "https://www.nuget.org/api/v2" :: p.Sources
             OutputPath = "src/packages"
             Retries = 4 })
 )
 
// --------------------------------------------------------------------------------------

Target "Build" (fun _ ->
 EnvironmentHelper.setBuildParam "VisualStudioVersion" "16.0"
 !! "src/kraken-net.sln"
 |> MSBuildRelease buildDir "Build"
 |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------

Target "BuildV2" (fun _ ->
 !! "src/kraken-net-v2.sln"
 |> MSBuildRelease buildDirV2 "Build"
 |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------

let nunitRunnerPath = "src/packages/NUnit.ConsoleRunner.3.7.0/tools/nunit3-console.exe"

Target "TestNunit" (fun _ ->
    !! (buildDir + @"\Tests.dll") 
    |> NUnit3 (fun p ->
        {p with 
             ToolPath = nunitRunnerPath
        }))

// --------------------------------------------------------------------------------------

Target "CreatePackage" (fun _ ->

    CreateDir packagingWorkingDir
    CleanDir packagingWorkingDir
    CreateDir packagingWorkingDirV2
    CleanDir packagingWorkingDirV2
    CopyFile packagingWorkingDir "./output/kraken.dll"
    CopyFile packagingWorkingDirV2 "./outputV2/kraken.dll"
    CreateDir packagingOutputPath

    NuGet (fun p -> 
        {p with
            Authors = authors    
            Files = [ (@"kraken.dll", Some @"lib/net452", None);
                        (@"kraken.dll", Some @"lib/net45", None);
                         (@"v2/kraken.dll", Some @"lib/netstandard1.6", None) ] 
            Project = nugetProject
            Description = description
            OutputPath = packagingOutputPath
            Summary = summary
            WorkingDir = packagingWorkingDir
            Version = nugetVersion
            ReleaseNotes = notes
            Publish = false }) 
            "kraken.nuspec"

)

// --------------------------------------------------------------------------------------

Target "RunSomePowerShell" <| fun _ ->
    PowerShell.Create()
      .AddScript("(Get-Content 'src/kraken-net-v2/kraken-net-v2.csproj') | Foreach-Object { $_ -replace '1.0.0.1', '" + version + "'} | Set-Content 'src/kraken-net-v2/kraken-net-v2.csproj'")
      .Invoke()
      |> Seq.iter (printfn "%O")
      
// --------------------------------------------------------------------------------------

Target "All" DoNothing

"Clean"
  ==> "RunSomePowerShell"
  ==> "AssemblyInfo"
  ==> "RestorePackages"
  ==> "Build"
  ==> "BuildV2"
  ==> "TestNunit"
  ==> "CreatePackage"
  ==> "All"

RunTargetOrDefault "All"