#r @"tools/FAKE.Core/tools/FakeLib.dll"
#load "tools/SourceLink.Fake/tools/SourceLink.fsx"

#r @"tools/FAKE.Core/tools/FakeLib.dll"
#load "tools/SourceLink.Fake/tools/SourceLink.fsx"

open System
open System.IO
open Fake
open Fake.AssemblyInfoFile
open Fake.XUnit2Helper
open SourceLink

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

Target "SourceLink" (fun _ ->
    use repo = new GitRepo(__SOURCE_DIRECTORY__)
    let proj = VsProj.LoadRelease "src/GogoKit/GogoKit.csproj"
    let pdb = new PdbFile(buildDir @@ "GogoKit.pdb")
    let pdbSrcSrvPath = buildDir @@ "GogoKit.srcsrv"

    logfn "source linking %s" pdb.Path
    let files = (proj.Compiles -- "SolutionInfo.cs").SetBaseDirectory __SOURCE_DIRECTORY__
    repo.VerifyChecksums files
    pdb.VerifyChecksums files |> ignore

    // Make sure that we don't hold onto a file lock on the .pdb
    pdb.Dispose()

    let pdbSrcSrvBytes = SrcSrv.create "https://raw.githubusercontent.com/viagogo/gogokit.net/{0}/%var2%" repo.Revision (repo.Paths files)
    File.WriteAllBytes(pdbSrcSrvPath, pdbSrcSrvBytes)
    Pdbstr.exec pdb.Path pdbSrcSrvPath
)

Target "CreatePackage" (fun _ ->
    CopyFiles buildDir ["LICENSE.txt"; "README.md"; "ReleaseNotes.md"]
    
    let tags = "viagogo API HAL tickets concerts"
    let dependencies = [
        ("HalKit", GetPackageVersion "./packages/" "HalKit")
    ]
    let libPortableDir = "lib/portable-net45+win+wpa81+wp80+MonoAndroid10+xamarinios10+MonoTouch10/"
    let files = [
        ("GogoKit.dll", Some libPortableDir, None)
        ("GogoKit.pdb", Some libPortableDir, None)
        ("GogoKit.xml", Some libPortableDir, None)
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

Target "Default" DoNothing

Target "CreatePackages" DoNothing

"Clean"
    ==> "AssemblyInfo"
    ==> "BuildApp"

"UnitTests"
    ==> "Default"

"SourceLink"
    ==> "Default"

"CreatePackage"
    ==> "CreatePackages"

RunTargetOrDefault "BuildApp"