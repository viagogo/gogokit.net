#r @"packages/FAKE/tools/FakeLib.dll"

open System
open Fake
open Fake.AssemblyInfoFile

RestorePackages()

let buildMode = getBuildParamOrDefault "buildMode" "Release"

// Directories
let buildDir = @"./build/"
let testResultsDir = @"./testresults/"

// TODO: Grab the VersionNumber from the latest release notes
let version = "0.0.1"


// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; testResultsDir]
)

Target "BuildApp" (fun _ ->
    MSBuild buildDir "Build" ["Configuration", "Release"] ["./Gogokit.sln"]
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

"Clean"
    ==> "BuildApp"
    ==> "UnitTests"

RunTargetOrDefault "UnitTests"