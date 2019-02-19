// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.MSBuild
nuget Fake.DotNet
nuget Fake.DotNet.Testing.NUnit
nuget Fake.Core.Target //"

open Fake.IO
open Fake.IO.Globbing.Operators //enables !! and globbing
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.Core

// --------------------------------------------------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------

let nugetProject = "kraken-net"
let project = "Kraken"
let product = "kraken.NET"
let authors = ["Kraken.io, Kevin Bronsdijk"]
let summary = "The official Kraken.io .Net client"
let version = "0.2.1.0"
let description = "The official kraken-net client interacts with the Kraken.io REST API allowing you to utilize Krakens features using a .NET interface."
let notes = "Removed net45 support, only netstandard1.6 will be supported. For more information and documentation, please visit the project site on GitHub."
let nugetVersion = "2.1.0"
let tags = "kraken.io C# API image optimization official"
let gitHome = "https://github.com/kraken-io"
let gitName = "kraken-net"

// --------------------------------------------------------------------------------------
// Build script 
// --------------------------------------------------------------------------------------

let buildDir = "./output/"
let packagingOutputPath = "./nuGet/"
let packagingWorkingDir = "./inputNuget/"

// --------------------------------------------------------------------------------------

// Targets
Target.create "Clean" (fun _ ->
  Shell.cleanDir buildDir
)

Target.create "BuildApp" (fun _ ->
  !! "src/kraken-net-v2.sln"
    |> MSBuild.runRelease id buildDir "Build"
    |> Trace.logItems "AppBuild-Output: "
)

Target.create "Test" (fun _ ->
    !! (buildDir + "/*tests.dll")
      |> NUnit3.run (fun p ->
          {p with
                ShadowCopy = false })
)

Target.create "Default" (fun _ ->
  Trace.trace "End of Default"
)

open Fake.Core.TargetOperators

"Clean"
  ==> "BuildApp"
  ==> "Test"
  ==> "Default"

// start build
Target.runOrDefault "Default"