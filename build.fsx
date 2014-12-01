#r @"packages/FAKE/tools/FakeLib.dll"

open System
open Fake
open Fake.AssemblyInfoFile

// Project information used to generate AssemblyInfo and .nuspec
let projectName = "GogoKit"
let projectDescription = "A lightweight async viagogo API client library for .NET"
let authors = ["viagogo"]
let copyright = @"Copyright © viagogo 2014"

// Directories
let buildDir = @"./build/"
let packagingDir = buildDir @@ "nupkgs"
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
    CopyFiles buildDir ["LICENSE.txt"; "README.md"; "ReleaseNotes.md"]

    let tags = "GogoKit viagogo API"
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
        ("ReleaseNotes.md", None, None)
    ]

    NuGet (fun p ->
        {p with
            Project = projectName
            Description = projectDescription
            Copyright = copyright
            Authors = authors
            OutputPath = packagingDir
            WorkingDir = buildDir
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
    ==> "CreateGogoKitPackage"

"CreateGogoKitPackage"
    ==> "CreatePackages"

RunTargetOrDefault "UnitTests"