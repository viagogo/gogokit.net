#r @"tools/FAKE.Core/tools/FakeLib.dll"

open System
open System.IO
open Fake
open Fake.AssemblyInfoFile
open Fake.XUnit2Helper

// Project information used to generate AssemblyInfo and .nuspec
let projectName = "GogoKit"
let projectDescription = "A lightweight async viagogo API client library for .NET"
let authors = ["viagogo"]
let copyright = @"Copyright © viagogo " + DateTime.UtcNow.ToString("yyyy");

// Directories
let buildDir = @"./artifacts/"
let packagingDir = buildDir @@ "packages"
let testResultsDir = @"./testresults/"

// Read Release Notes and version from ReleaseNotes.md
let releaseNotes = 
    ReadFile "ReleaseNotes.md"
    |> ReleaseNotesHelper.parseReleaseNotes


// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testResultsDir; packagingDir]
)

Target "AssemblyInfo" (fun _ ->
    CreateCSharpAssemblyInfo "./SolutionInfo.cs"
      [ Attribute.Product projectName
        Attribute.Company authors.[0]
        Attribute.Copyright copyright
        Attribute.Version releaseNotes.AssemblyVersion
        Attribute.FileVersion releaseNotes.AssemblyVersion
        Attribute.InformationalVersion releaseNotes.NugetVersion
        Attribute.ComVisible false ]
)

Target "BuildApp" (fun _ ->
    let buildMode = getBuildParamOrDefault "buildMode" "Release"

    RestorePackages()

    MSBuild buildDir "Build" ["Configuration", buildMode] ["./GogoKit.sln"]
    |> Log "AppBuild-Output: "
)


Target "UnitTests" (fun _ ->
    !! (buildDir + @"\GogoKit*.Tests.dll")
    |> NUnitParallel (fun p ->
        {p with
            OutputFile = testResultsDir + @"TestResults.xml"
        }
    )
)

Target "CreatePackage" (fun _ ->
    let tags = "viagogo API HAL tickets concerts"
    let dependencies = [
        ("HalKit", GetPackageVersion "./packages/" "HalKit")
    ]
    
    let inline nugetFriendlyPath (path : string) = if path.StartsWith("./") then path.Remove(0, 2) else path

    let libPortableDir = "lib/portable-net45+win+wpa81+wp80+MonoAndroid10+xamarinios10+MonoTouch10/"
    let files = [
        (nugetFriendlyPath buildDir @@ "GogoKit.dll", Some libPortableDir, None)
        (nugetFriendlyPath buildDir @@ "GogoKit.pdb", Some libPortableDir, None)
        (nugetFriendlyPath buildDir @@ "GogoKit.xml", Some libPortableDir, None)
        ("LICENSE.txt", None, None)
        ("README.md", None, None)
        ("ReleaseNotes.md", None, None)
        ("src\GogoKit\**\*.cs", Some "src", None)
    ]

    NuGet (fun p ->
        {p with
            Project = projectName
            Description = projectDescription
            Copyright = copyright
            Authors = authors
            OutputPath = packagingDir
            WorkingDir = @"."
            SymbolPackage = NugetSymbolPackage.Nuspec
            Version = releaseNotes.NugetVersion
            ReleaseNotes = toLines releaseNotes.Notes
            Dependencies = dependencies
            Files = files}) "GogoKit.nuspec"
)

Target "CreatePackages" DoNothing

"Clean"
    ==> "AssemblyInfo"
    ==> "BuildApp"
    ==> "UnitTests"
    ==> "CreatePackages"

"CreatePackages"
    ==> "CreatePackage"

RunTargetOrDefault "BuildApp"