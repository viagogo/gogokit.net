#r @"packages/FAKE/tools/FakeLib.dll"

open System
open Fake
open Fake.AssemblyInfoFile


// Directories
let buildDir = @"./build/"
let packagingDir = buildDir @@ "nupkgs"
let testResultsDir = @"./testresults/"

// TODO: Grab the VersionNumber from the latest release notes
let version = "0.0.1"


// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testResultsDir; packagingDir]
)

Target "BuildApp" (fun _ ->
    let buildMode = getBuildParamOrDefault "buildMode" "Release"

    RestorePackages()

    MSBuild buildDir "Build" ["Configuration", buildMode] ["./Gogokit.sln"]
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

Target "CreateGogoKitPackage" (fun _ ->
    CopyFiles buildDir ["LICENSE.txt"; "README.md"]

    let authors = ["viagogo"]
    let projectName = "GogoKit"
    let projectDescription = "A lightweight async viagogo API client library for .NET"
    let dependencies = [
        ("Microsoft.Net.Http", GetPackageVersion "./packages/" "Microsoft.Net.Http")
        ("Newtonsoft.Json", GetPackageVersion "./packages/" "Newtonsoft.Json")
    ]
    let libPortableDir = "lib/portable-net45+win+wpa81+wp80+MonoAndroid10+xamarinios10+MonoTouch10/"
    let files = [
        ("GogoKit.dll", Some libPortableDir, None)
        ("GogoKit.pdb", Some libPortableDir, None)
        ("LICENSE.txt", None, None)
        ("README.md", None, None)
    ]

    NuGet (fun p -> 
        {p with
            Authors = authors
            Project = projectName
            Description = projectDescription
            OutputPath = packagingDir
            WorkingDir = buildDir
            SymbolPackage = NugetSymbolPackage.Nuspec
            Version = version
            Dependencies = dependencies
            Files = files}) "GogoKit.nuspec"
)

"Clean"
    ==> "BuildApp"
    ==> "UnitTests"
    ==> "CreateGogoKitPackage"

RunTargetOrDefault "UnitTests"